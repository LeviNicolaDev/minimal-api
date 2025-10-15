using FluentValidation;
using minimal_api.Domain.DTOs;

namespace minimal_api.Domain.Validators;

public class AdministratorDtoValidator : AbstractValidator<AdministratorDTO>
{
    public AdministratorDtoValidator()
    {
        RuleFor(a => a.Email)
            .NotEmpty().WithMessage("Email não pode ser vazio.")
            .EmailAddress().WithMessage("O formato do e-mail é inválido.");

        RuleFor(a => a.Senha)
            .NotEmpty().WithMessage("Senha não pode ser vazia.")
            .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");

        RuleFor(a => a.Perfil)
            .NotNull().WithMessage("Perfil não pode ser vazio.");
    }
}