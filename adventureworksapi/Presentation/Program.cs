using AdventureWorks.Application.UseCases;
using AdventureWorks.Domain.Interfaces;
using AdventureWorks.Infrastructure.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add context
builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorksDatabase")));

// Add configuration for the use case
builder.Services.AddScoped<PersonUseCase>();
// Add configuration for the repository
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// Registro de dependencias
builder.Services.AddHttpClient();

// Agrega soporte para controladores (sin vistas ni Razor Pages)
builder.Services.AddControllers();

// Configure CORS to allow specific origins
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseRouting();

// Apply CORS policy
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
