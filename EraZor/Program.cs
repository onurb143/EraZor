using EraZor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Hent konfiguration
var configuration = builder.Configuration;

// Konfigurer Kestrel til kun HTTP
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5002); // HTTP
});

// Tilføj services til containeren
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(); // Tilføj controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Tilføj Swagger-support

// Konfigurer CORS-politik
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Tilføj logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Middleware
app.UseCors("AllowAll"); // Anvend CORS-politikken
app.UseAuthorization();

// Kortlæg kontroller
app.MapControllers();

// Konfigurer HTTP request pipeline (kun i udvikling)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");
        c.RoutePrefix = string.Empty; // Dette sætter Swagger som default route
    });
}

using (var connection = new Npgsql.NpgsqlConnection("Host=db;Port=5432;Database=DatamatikerDB;Username=postgres;Password=Test1234!"))
{
    try
    {
        connection.Open();
        Console.WriteLine("Database connected successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Failed to connect: {ex.Message}");
    }
}


app.Run();
