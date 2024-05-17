using AdventureWorks.Application.UseCases;
using AdventureWorks.Domain.Interfaces;
using AdventureWorks.Infrastructure.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
   policy.WithOrigins("http://localhost:5111") // Reemplazar <SwaggerPort> con el puerto real.
         .AllowAnyHeader()
         .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddControllers();

// A�adir el servicio de EF Core DbContext
builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorksDatabase")));

// A�adir la configuraci�n para el repositorio
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// Registrar casos de uso
builder.Services.AddScoped<GetAllPeopleUseCase>(); 

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting(); // Asegúrate de que UseRouting está llamado antes de UseCors.

app.UseCors("AllowSpecificOrigin"); // Aplica la política de CORS

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
