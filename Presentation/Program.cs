using AdventureWorks.Application.UseCases;
using AdventureWorks.Domain.Interfaces;
using AdventureWorks.Infrastructure.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
   policy.WithOrigins("http://localhost:5111") // Replace with the real port.
         .AllowAnyHeader()
         .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddControllers();

// Add a database context
builder.Services.AddDbContext<AdventureWorksContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorksDatabase")));

// Add configuration for the repository
builder.Services.AddScoped<IPersonRepository, PersonRepository>();

// Register the use case
builder.Services.AddScoped<PersonUseCase>();

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

app.UseRouting();

app.UseCors("AllowSpecificOrigin"); // Apply CORS

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});

app.Run();
