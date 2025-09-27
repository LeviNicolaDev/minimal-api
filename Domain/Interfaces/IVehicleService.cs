using Microsoft.JSInterop.Infrastructure;
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;

namespace minimal_api.Domain.Interfaces;

public abstract class IVehicleService
{
    public abstract List<Vehicle> All(int? pagina = 1, string? nome=null, string? marca=null);
    public abstract Vehicle? SearchById(int id);
    public abstract void Include(Vehicle vehicle);
    public abstract void Update(Vehicle vehicle);
    public abstract void Delete(Vehicle vehicle);
}