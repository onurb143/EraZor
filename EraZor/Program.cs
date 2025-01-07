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

        // Konfigurer Kestrel server med HTTP og HTTPS lytning
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5000); // Lyt på HTTP-port 5000 for at håndtere ikke-sikrede forbindelser
            if (!builder.Environment.IsDevelopment()) //  bør aktivere HTTPS i produktionsmiljø
            {
                options.ListenAnyIP(5002, listenOptions =>
                {
                    listenOptions.UseHttps("https/aspnetapp.pfx", "Test1234!"); // Bind HTTPS til port 5002 og brug et certifikat til sikkerhed
                });
            }
        });

        // Konfigurer databaseforbindelse med PostgreSQL
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        // Registrerer databasekonteksten og bruger PostgreSQL som database

        // Konfigurer Identity for brugere og roller
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            // Specificer krav til adgangskoder for bedre sikkerhed
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
        })
        .AddEntityFrameworkStores<DataContext>() // Brug Entity Framework til at gemme brugere og roller
        .AddDefaultTokenProviders(); // Tilføj standard token-generatorer til autentifikation

        // Konfigurer JWT Authentication
        var jwtSettings = builder.Configuration.GetSection("JwtSettings"); // Hent JWT-indstillinger fra konfiguration
        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]); // Konverter hemmelig nøgle til byte-array for token-signering
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Brug JWT som standard autentifikationsmetode
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // Udfordringer bruger også JWT
        })
        .AddJwtBearer(options =>
        {
            // Indstillinger for tokenvalidering
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true, // Kontrollér, at tokenets udsteder er gyldig
                ValidateAudience = true, // Kontrollér, at tokenet er tiltænkt korrekt målgruppe
                ValidateLifetime = true, // Kontrollér, at tokenet ikke er udløbet
                ValidateIssuerSigningKey = true, // Kontrollér, at tokenet er korrekt signeret
                ValidIssuer = jwtSettings["Issuer"], // Angiv gyldig udsteder fra konfiguration
                ValidAudience = jwtSettings.GetSection("Audience").Get<string[]>()[0], // Angiv gyldig målgruppe
                IssuerSigningKey = new SymmetricSecurityKey(key), // Brug symmetrisk nøgle til validering
                ClockSkew = TimeSpan.Zero // Fjern tidsforskydning for nøjagtighed
            };
        });

        // Konfigurer CORS (Cross-Origin Resource Sharing)
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "https://localhost:5199" };
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins(allowedOrigins) // Tillad anmodninger kun fra specifikke domæner
                      .AllowAnyMethod() // Tillad alle HTTP-metoder (GET, POST, PUT, DELETE)
                      .AllowAnyHeader(); // Tillad alle headers i anmodninger
            });
        });

        // Konfigurer Swagger
        builder.Services.AddControllers(); // Registrer controllere til API-håndtering
        builder.Services.AddEndpointsApiExplorer(); // Muliggør automatisk endpoint-dokumentation
        builder.Services.AddSwaggerGen(c =>
        {
            // Opret dokumentation for API med version og beskrivelse
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "EraZor API",
                Version = "v1",
                Description = "JWT Authentication enabled API"
            });

            // Tilføj JWT i Swagger UI for at teste sikrede endpoints
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Indtast 'Bearer' efterfulgt af dit token."
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

        // Middleware rækkefølge
        app.UseHttpsRedirection(); // Omdiriger HTTP-anmodninger til HTTPS for sikkerhed
        app.UseCors("AllowSpecificOrigins"); // Anvend CORS-politikken for at tillade specifikke domæner
        app.UseAuthentication(); // Aktiver autentifikationsmiddleware for brugere
        app.UseAuthorization(); // Aktiver autorisationsmiddleware for adgangskontrol

        // Swagger i udviklingsmiljø
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Aktiverer Swagger i udvikling
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation"); // Angiv JSON-dokumentationens placering
                c.RoutePrefix = string.Empty; // Gør Swagger UI tilgængelig på roden
            });
        }

        app.MapControllers(); // Kortlægger controllere til deres respektive endpoints

        // Database migrations
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            try
            {
                dbContext.Database.Migrate(); // Anvend eventuelle ventende migrations på databasen
                Console.WriteLine("Database connected and migrations applied successfully!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Database connection or migration failed: {ex}"); // Log fejl i forbindelse med databasen
                Environment.Exit(1); // Stop applikationen, hvis migrering fejler
            }
        }

        app.Run(); // Start applikationen og begynd at lytte på de konfigurerede porte
    }
}
