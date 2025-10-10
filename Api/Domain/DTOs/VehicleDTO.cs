namespace minimal_api.Domain.DTOs;

public record VehicleDTO
{
    public string Nome { get; set; } = default!;
    public string Marca { get; set; } = default!;
    public int Ano { get; set; } = default!;
}