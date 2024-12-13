using EraZor.Data;
using EraZor.Interfaces;
using EraZor.Repositories;
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

        // Tilføj services til DI containeren før Build
        builder.Services.AddScoped<IDiskService, DiskService>();
        builder.Services.AddScoped<IWipeMethodService, WipeMethodService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IWipeReportService, WipeReportService>();

        // Konfigurer Kestrel-serveren til at lytte på både HTTP og HTTPS
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5000); // Lyt efter HTTP-anmodninger på port 5000
            if (!builder.Environment.IsDevelopment())
            {
                options.ListenAnyIP(5002, listenOptions =>
                {
                    // Lyt efter HTTPS-anmodninger på port 5002 selvsigneret certifikat til udvikling
                    listenOptions.UseHttps("https/aspnetapp.pfx", "Test1234!");
                });
            }
        });

        // Konfigurer PostgreSQL databaseforbindelsen ved hjælp af connection string
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Konfigurer Identity for brugere og roller
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
        })
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();

        // Konfigurer JWT Authentication
        var jwtSettings = builder.Configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })

        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudiences = jwtSettings.GetSection("Audience").Get<string[]>(),
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["jwtToken"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        // Konfigurer CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins("https://localhost:5002", "https://localhost:5199")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
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
        app.UseHttpsRedirection();
        app.UseCors("AllowSpecificOrigins");
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");
                c.RoutePrefix = string.Empty;
            });
        }

        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            try
            {
                dbContext.Database.Migrate();
                Console.WriteLine("Database connected and migrations applied successfully!");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Database connection or migration failed: {ex}");
                Environment.Exit(1);
            }
        }

        app.Run();
    }
}
