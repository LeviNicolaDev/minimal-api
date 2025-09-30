using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;

namespace minimal_api.Domain.Interfaces;

public abstract class IAdministratorService
{
    public abstract Administrator? Login(LoginDTO loginDto);
    public abstract Administrator Include(Administrator administrator);
    public abstract List<Administrator> All(int? pagina);
    public abstract Administrator? SearchById(int id);
}