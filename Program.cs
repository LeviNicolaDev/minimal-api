using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.DTOs;
using minimal_api.Infrastructure.Db;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbContexto>(options =>
{
        options.UseMySql(
                builder.Configuration.GetConnectionString("MySql"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MySql"))
        );
});

var app = builder.Build();



app.MapGet("/", () =>"Hello World!");

app.MapPost("/login", (LoginDTO loginDTO) =>
{
        if (loginDTO.Email == "adm@test.com" && loginDTO.Senha == "1234")
        {
                return Results.Ok("Login com sucesso");
        } 
        return Results.Unauthorized();
});

app.Run();