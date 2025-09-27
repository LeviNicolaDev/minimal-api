using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.ModelViews;
using minimal_api.Domain.Services;
using minimal_api.Infrastructure.Db;


#region Builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbContexto>(options =>
{
        options.UseMySql(
                builder.Configuration.GetConnectionString("MySql"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
        );
});

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
#endregion

#region Administradores
app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDto, IAdministratorService administratorService) =>
{
        if (administratorService.Login(loginDto) != null)
        {
                return Results.Ok("Login com sucesso");
        } 
        return Results.Unauthorized();
}).WithTags("Admin");
#endregion

#region Veiculos
app.MapPost("/veiculos", ([FromBody] VehicleDTO vehicleDto, IVehicleService vehicleService) =>
{
        var veiculo = new Vehicle
        {
                Nome = vehicleDto.Nome,
                Marca = vehicleDto.Marca,
                Ano = vehicleDto.Ano
        };
        vehicleService.Include(veiculo);
        return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? page, IVehicleService vehicleService) =>
{
        var veiculos = vehicleService.All(page);

        return Results.Ok(veiculos);
}).WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
        var veiculo = vehicleService.SearchById(id);

        if (veiculo == null)
        {
                return Results.NotFound();
        }
        return Results.Ok(veiculo);
}).WithTags("Veiculos");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion