using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.Services;
using minimal_api.Infrastructure.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();

builder.Services.AddDbContext<DbContexto>(options =>
{
        options.UseMySql(
                builder.Configuration.GetConnectionString("MySql"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
        );
});

var app = builder.Build();



app.MapGet("/", () =>"Hello World!");

app.MapPost("/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) =>
{
        if (administratorService.Login(loginDTO) != null)
        {
                return Results.Ok("Login com sucesso");
        } 
        return Results.Unauthorized();
});

app.Run();