using EraZor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Hent konfiguration
var configuration = builder.Configuration;

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

var app = builder.Build();

// Konfigurer middleware
app.UseCors("AllowAll"); // Anvend CORS-politikken
app.UseAuthorization();
app.MapControllers();

// Konfigurer HTTP request pipeline (kun i udvikling)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
