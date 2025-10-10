namespace minimal_api.Domain.ModelViews;

public record AdministratorLoggedIn
{
    public string Email { get; set; } = default!;
    public string Perfil { get; set; } = default!;
    public string Token { get; set; } = default!;
}