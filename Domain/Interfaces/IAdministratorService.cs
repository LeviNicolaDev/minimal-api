using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;

namespace minimal_api.Domain.Interfaces;

public abstract class IAdministratorService
{
    public abstract Administrator? Login(LoginDTO loginDto);
}