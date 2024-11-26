using AdventureWorks.Application.UseCases;
using AdventureWorks.Domain.Interfaces;
using AdventureWorks.Infrastructure.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using DotNetEnv; // Agregar el using para DotNetEnv

var builder = WebApplication.CreateBuilder(args);

// Cargar variables del archivo .env
DotNetEnv.Env.Load();
Console.WriteLine($"DB_CONNECTION_STRING: {Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")}");

// Agregar el proveedor de variables de entorno al builder
builder.Configuration.AddEnvironmentVariables();

// Registrar AdventureWorksContext con la cadena de conexión desde la variable de entorno
builder.Services.AddDbContext<AdventureWorksContext>(options =>
{
    var connectionString = builder.Configuration["DB_CONNECTION_STRING"];
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null);
    });
});

// Configuración de servicios
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de dependencias
builder.Services.AddScoped<PersonUseCase>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddHttpClient();

// Agregar soporte para controladores
builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Cambia esto según sea necesario para tu cliente Angular
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configuración del middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

// Aplicar política de CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
