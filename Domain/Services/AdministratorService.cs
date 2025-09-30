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

    public override Administrator Include(Administrator administrator)
    {
        _contexto.Administrators.Add(administrator);
        _contexto.SaveChanges();

        return administrator;
    }

    public override List<Administrator> All(int? pagina)
    {
        var query = _contexto.Administrators.AsQueryable();

        int itensPorPagina = 10;

        if (pagina != null)
        {
            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);
        }
        return query.ToList();
    }
    
    public override Administrator? SearchById(int id)
    {
        return _contexto.Administrators.Where(v => v.Id == id).FirstOrDefault();
    }
}