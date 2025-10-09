using minimal_api.Domain.Enuns;

namespace minimal_api.Domain.DTOs;

public class AdministratorDTO
{
    public string Email { get; set; } = default!;
    public string Senha { get; set; } = default!;
    public Perfil? Perfil { get; set; } = default!;
}