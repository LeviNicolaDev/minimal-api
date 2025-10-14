using FluentValidation;
using minimal_api.Domain.DTOs;

namespace minimal_api.Domain.Validators;

public class VehicleDtoValidator : AbstractValidator<VehicleDTO>
{
    public VehicleDtoValidator()
    {
        RuleFor(v => v.Nome)
            .NotEmpty().WithMessage("O nome não pode ser vazio.")
            .Length(2, 100).WithMessage("O nome deve ter entre 2 e 100 caracteres.");
        
        RuleFor(v => v.Marca)
            .NotEmpty().WithMessage("A marca não pode estar em branco.");
        
        RuleFor(v => v.Ano)
            .GreaterThan(1950).WithMessage("Veículo muito antigo, somente anos superiores a 1950.");
    }
}