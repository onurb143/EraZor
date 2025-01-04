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

        // Kestrel-konfiguration
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5000); // HTTP
            if (!builder.Environment.IsDevelopment())
            {
                options.ListenAnyIP(5002, listenOptions =>
                {
                    listenOptions.UseHttps("https/aspnetapp.pfx", "Test1234!"); // HTTPS for produktion
                });
            }
        });

        // Databasekonfiguration
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // ASP.NET Identity-konfiguration
        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
        })
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();

        // JWT-konfiguration
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
                ValidAudience = jwtSettings.GetSection("Audience").Get<string[]>()[0],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });

        // CORS-konfiguration
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5189" };
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // Swagger-konfiguration
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

            // Tilføj JWT SecurityDefinition
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Indtast 'Bearer' efterfulgt af dit token."
            });

            // Tilføj JWT SecurityRequirement
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
        app.UseCors("AllowSpecificOrigins");

        app.UseAuthentication(); // JWT Authentication skal være før Authorization
        app.UseAuthorization();

        // Swagger UI
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

        // Database migrations
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
