using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Infrastructure.Db;

namespace minimal_api.Domain.Services;

public class AdministratorService : IAdministratorService
{
    private readonly DbContexto _contexto;

    public AdministratorService(DbContexto contexto)
    {
        _contexto = contexto;
    }
    public override Administrator? Login(LoginDTO loginDto)
    {
        var adm = _contexto.Administrators.Where(a => a.Email == loginDto.Email && a.Senha == loginDto.Senha).FirstOrDefault();
        return adm;
    }
}