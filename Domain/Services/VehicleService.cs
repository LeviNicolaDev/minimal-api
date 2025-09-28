using Microsoft.EntityFrameworkCore;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Infrastructure.Db;

namespace minimal_api.Domain.Services;

public class VehicleService : IVehicleService
{
    private readonly DbContexto _contexto;

    public VehicleService(DbContexto contexto)
    {
        _contexto = contexto; 
    }

    public override List<Vehicle> All(int? pagina = 1, string? nome = null, string? marca = null)
    {
        var query = _contexto.Vehicles.AsQueryable();
        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{nome.ToLower()}%"));
        }

        int itensPorPagina = 10;

        if (pagina != null)
        {
            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);
        }
        return query.ToList();
    }

    public override Vehicle? SearchById(int id)
    {
        return _contexto.Vehicles.Where(v => v.Id == id).FirstOrDefault();
    }

    public override void Include(Vehicle vehicle)
    {
        _contexto.Vehicles.Add(vehicle);
        _contexto.SaveChanges();
    }

    public override void Update(Vehicle vehicle)
    {
        _contexto.Vehicles.Update(vehicle);
        _contexto.SaveChanges();
    }

    public override void Delete(Vehicle vehicle)
    {
        _contexto.Vehicles.Remove(vehicle);
        _contexto.SaveChanges();
    }
}