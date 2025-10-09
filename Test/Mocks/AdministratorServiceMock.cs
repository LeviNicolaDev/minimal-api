using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;

namespace Test.Mocks;

public class AdministratorServiceMock : IAdministratorService
{
    private static List<Administrator> administrators = new List<Administrator>()
    {
        new Administrator
        {
            Id = 1,
            Email = "adm@test.com",
            Senha = "123456",
            Perfil = "Adm"
        },
        new Administrator
        {
            Id = 2,
            Email = "editor@test.com",
            Senha = "123456",
            Perfil = "Editor"
        }
    };
    
    public override Administrator? Login(LoginDTO loginDto)
    {
        return administrators.Find(a => a.Email == loginDto.Email && a.Senha == loginDto.Senha);
    }

    public override Administrator Include(Administrator administrator)
    {
        administrator.Id = administrators.Count() + 1;
        administrators.Add(administrator);
        return administrator;
    }

    public override List<Administrator> All(int? pagina)
    {
        return administrators;
    }

    public override Administrator? SearchById(int id)
    {
        return administrators.Find(a => a.Id == id);
    }
}