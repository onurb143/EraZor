using EraZor.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Konfigurer Kestrel-server (HTTP og HTTPS)
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5002); // HTTP
    if (!builder.Environment.IsDevelopment())
    {
        options.ListenAnyIP(5003, listenOptions => listenOptions.UseHttps()); // HTTPS for produktion
    }
});

// Konfigurer database
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfigurer ASP.NET Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

// Tilføj dynamisk CORS-konfiguration
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

// Tilføj controllers og Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Middleware
app.UseCors("AllowSpecificOrigins");
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseAuthorization();

app.MapControllers();

// Swagger konfiguration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    // Begræns Swagger-adgang i produktion
    app.UseSwagger(c => c.RouteTemplate = "/admin/swagger/{documentName}/swagger.json");
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/admin/swagger/v1/swagger.json", "API Documentation");
    });
}

// Test og migrer database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    try
    {
        // Brug migrationer i stedet for EnsureCreated
        dbContext.Database.Migrate();
        Console.WriteLine("Database connected and migrations applied successfully!");
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Database connection or migration failed: {ex}");
        Environment.Exit(1); // Luk applikationen, hvis databasen ikke er tilgængelig
    }
}

// Start applikationen
app.Run();
