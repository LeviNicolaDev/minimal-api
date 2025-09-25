using minimal_api.Domain.DTOs;

var builder = WebApplication.CreateBuilder(args);
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