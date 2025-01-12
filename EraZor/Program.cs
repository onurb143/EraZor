using EraZor.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Konfigurer Kestrel-serveren til at lytte p� b�de HTTP og HTTPS
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5000); // Lyt efter HTTP-anmodninger p� port 5000
            if (!builder.Environment.IsDevelopment())
            {
                options.ListenAnyIP(5002, listenOptions =>
                {
                    // Lyt efter HTTPS-anmodninger p� port 5002 selvsigneret certifikat til udvikling
                    listenOptions.UseHttps("https/aspnetapp.pfx", "Test1234!");
                });
            }
        });

        // Konfigurer PostgreSQL databaseforbindelsen ved hj�lp af connection string
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Konfigurer Identity for brugere og roller
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true; // Kr�ver mindst �t tal i password
            options.Password.RequiredLength = 8;  // Minimumsl�ngde for password er 8 tegn
            options.Password.RequireNonAlphanumeric = false; // Tillader ikke n�dvendigvis specialtegn
            options.Password.RequireUppercase = true; // Kr�ver mindst �t stort bogstav
        })
        .AddEntityFrameworkStores<DataContext>() // Brug Entity Framework til at gemme brugere
        .AddDefaultTokenProviders(); // Tilf�jer standard token provider for f.eks. password reset

        // Konfigurer JWT Authentication
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Default autentificering bruger JWT
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Default challenge sker ogs� via JWT
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // Valider om issuer matcher
                ValidateAudience = true, // Valider om audience matcher
                ValidateLifetime = true, // Valider om token er udl�bet
                ValidateIssuerSigningKey = true, // Valider signatur af token
                ValidIssuer = jwtSettings["Issuer"], // Giv gyldigt issuer
                ValidAudiences = jwtSettings.GetSection("Audience").Get<string[]>(), // Gyldige audiences
                IssuerSigningKey = new SymmetricSecurityKey(key), // Signeringsn�gle
                ClockSkew = TimeSpan.Zero // Ingen tidsforskel tilladt ved token validering
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Tjek om token findes som en cookie og s�tte det i requesten
                    var token = context.Request.Cookies["jwtToken"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        // Konfigurer CORS (Cross-Origin Resource Sharing)
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()) // Tillad oprindelser fra config
                      .AllowAnyMethod() // Tillad alle HTTP-metoder
                      .AllowAnyHeader() // Tillad alle headers
                      .AllowCredentials(); // Tillad cookies og credentials i anmodninger
            });
        });

        // Konfigurer Swagger til API-dokumentation
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "EraZor API",
                Version = "v1",
                Description = "JWT Authentication enabled API"
            });

            // Tilf�j sikkerhedsdefinition for Bearer token i Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer' followed by your token."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        var app = builder.Build();

        // Middleware pipeline
        app.UseHttpsRedirection(); // Tving HTTP-anmodninger til HTTPS
        app.UseCors("AllowSpecificOrigins"); // Aktiv�r CORS med specifikke oprindelser
        app.UseAuthentication(); // Aktiv�r JWT-autentifikation
        app.UseAuthorization(); // Aktiv�r autorisation

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Aktiv�r Swagger i udviklingsmilj�
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation"); // Swagger UI endpoint
                c.RoutePrefix = string.Empty; // Root path for Swagger UI
            });
        }

        app.MapControllers(); // Kortl�g controllers til API-ruter

        // Anvend migrationer til databasen
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            try
            {
                dbContext.Database.Migrate(); // Anvend eventuelle database-migrationer
                Console.WriteLine("Database connected and migrations applied successfully!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Database connection or migration failed: {ex}");
                Environment.Exit(1); // Stop applikationen, hvis migrationen mislykkedes
            }
        }

        app.Run(); // K�r applikationen
    }
}
