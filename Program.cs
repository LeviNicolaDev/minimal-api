using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Enuns;
using minimal_api.Domain.Interfaces;
using minimal_api.Domain.ModelViews;
using minimal_api.Domain.Services;
using minimal_api.Infrastructure.Db;


#region Builder
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();

if (string.IsNullOrEmpty(key)) key = "123456";

builder.Services.AddAuthentication(option =>
{
        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
        option.TokenValidationParameters = new TokenValidationParameters
        {
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
        };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT: "
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
                {
                        new OpenApiSecurityScheme
                        {
                                Reference = new OpenApiReference
                                {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                }
                        },
                        new List<string>()
                }
        });
});

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
app.MapGet("/", () => Results.Json(new Home()))
        .AllowAnonymous()
        .WithTags("Home")
        .WithOpenApi(operation =>
        {
                operation.Security = new List<OpenApiSecurityRequirement>(); 
                return operation;
        });
#endregion

#region Administradores

string GenerateTokenJwt(Administrator administrator)
{
        if (string.IsNullOrEmpty(key))
        {
                return String.Empty;
        }
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
                new Claim("Email", administrator.Email),
                new Claim("Perfil", administrator.Perfil),
                new Claim(ClaimTypes.Role, administrator.Perfil)
        };
        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
        return new JwtSecurityTokenHandler().WriteToken(token);
}

app.MapPost("/administradores/login", ([FromBody] LoginDTO loginDto, IAdministratorService administratorService) =>
{
        var adm = administratorService.Login(loginDto);
        if (adm != null)
        {
                string token = GenerateTokenJwt(adm);
                return Results.Ok(new AdministratorLogado
                {       
                        Email = adm.Email,
                        Perfil = adm.Perfil,
                        Token = token
                });
        } 
        return Results.Unauthorized();
})
        .AllowAnonymous()
        .WithTags("Admin")
        .WithOpenApi(operation =>
        {
                operation.Security = new List<OpenApiSecurityRequirement>(); 
                return operation;
        });

app.MapPost("/administradores", ([FromBody] AdministratorDTO administratorDTO, IAdministratorService administratorService) =>
{
        var validacao = new ValidationErrors
        {
                Mensagens = new List<string>()
        };

        if (string.IsNullOrEmpty(administratorDTO.Email))
        {
                validacao.Mensagens.Add("Email não pode ser vazio");
        }        
        if (string.IsNullOrEmpty(administratorDTO.Senha))
        {
                validacao.Mensagens.Add("Senha não pode ser vazia");
        }        
        if (administratorDTO.Perfil == null)
        {
                validacao.Mensagens.Add("Perfil não pode ser vazio");
        }
        
        if (validacao.Mensagens.Count > 0)
        {
                return Results.BadRequest(validacao);
        }
        
        var administrator = new Administrator
        {
                Email = administratorDTO.Email,
                Senha = administratorDTO.Senha,
                Perfil = administratorDTO.Perfil.ToString() ?? Perfil.Editor.ToString()
        }; 

        administratorService.Include(administrator);
        return Results.Created($"/veiculo/{administrator.Id}", new AdministratorModelView
        {
                Id = administrator.Id,
                Email = administrator.Email,
                Perfil = administrator.Perfil
        });
})
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
        .WithTags("Admin");   

app.MapGet("/administradores", ([FromQuery] int? pagina, IAdministratorService administratorService) =>
{
        var admins = new List<AdministratorModelView>();
        var adminsVisualizer = administratorService.All(pagina);

        foreach (var adm in adminsVisualizer)
        {
                admins.Add(new AdministratorModelView
                {
                        Id = adm.Id,
                        Email = adm.Email,
                        Perfil = adm.Perfil
                });
        }
        return Results.Ok(admins);
})
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
        .WithTags("Admin");

app.MapGet("/administradores/{id}", ([FromRoute] int id, IAdministratorService administratorService) =>
{
        var administrator = administratorService.SearchById(id);

        if (administrator == null)
        {
                return Results.NotFound();
        }
        return Results.Ok(new AdministratorModelView
        {
                Id = administrator.Id,
                Email = administrator.Email,
                Perfil = administrator.Perfil
        });
})
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
        .WithTags("Admin");
#endregion

#region Veiculos
ValidationErrors ValidaDto(VehicleDTO vehicleDto)
{
        var validacao = new ValidationErrors{ Mensagens = new List<string>() }; // TODO: Utilizar FluentValidation para regras de validação

        if (string.IsNullOrEmpty(vehicleDto.Nome))
        {
                validacao.Mensagens.Add("O nome não pode ser vazio");
        } 
        if (string.IsNullOrEmpty(vehicleDto.Marca))
        {
                validacao.Mensagens.Add("A marca não pode estar em branco");
        }        
        if (vehicleDto.Ano < 1950)
        {
                validacao.Mensagens.Add("Veiculo muito antigo, somente veiculos de anos superiores a 1950");
        }

        return validacao;
}

app.MapPost("/veiculos", ([FromBody] VehicleDTO vehicleDto, IVehicleService vehicleService) =>
{
        var validacao = ValidaDto(vehicleDto);
        if (validacao.Mensagens != null && validacao.Mensagens.Count > 0)
        {
                return Results.BadRequest(validacao);
        }
        
        var veiculo = new Vehicle
        {
                Nome = vehicleDto.Nome,
                Marca = vehicleDto.Marca,
                Ano = vehicleDto.Ano
        };
        vehicleService.Include(veiculo);
        return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
})
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm,Edior" })
        .WithTags("Veiculos");

app.MapGet("/veiculos", ([FromQuery] int? page, IVehicleService vehicleService) =>
{
        var veiculos = vehicleService.All(page);

        return Results.Ok(veiculos);
})
        .RequireAuthorization()
        .WithTags("Veiculos");

app.MapGet("/veiculos/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
        var veiculo = vehicleService.SearchById(id);

        if (veiculo == null)
        {
                return Results.NotFound();
        }
        return Results.Ok(veiculo);
})
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm,Editor" })
        .WithTags("Veiculos");

app.MapPut("/veiculos/{id}", ([FromRoute] int id, VehicleDTO vehicleDto, IVehicleService vehicleService) =>
{
        var veiculo = vehicleService.SearchById(id);
        if (veiculo == null)
        {
                return Results.NotFound();
        }
        
        var validacao = ValidaDto(vehicleDto);
        if (validacao.Mensagens != null && validacao.Mensagens.Count > 0)
        {
                return Results.BadRequest(validacao);
        }

        veiculo.Nome = vehicleDto.Nome;
        veiculo.Marca = vehicleDto.Marca;
        veiculo.Ano = vehicleDto.Ano;
        
        vehicleService.Update(veiculo);
        
        return Results.Ok(veiculo);
})
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
        .WithTags("Veiculos");

app.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{
        var veiculo = vehicleService.SearchById(id);

        if (veiculo == null)
        {
                return Results.NotFound();
        }
        
        vehicleService.Delete(veiculo);
        
        return Results.NoContent();
})
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute{ Roles = "Adm" })
        .WithTags("Veiculos");
#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion