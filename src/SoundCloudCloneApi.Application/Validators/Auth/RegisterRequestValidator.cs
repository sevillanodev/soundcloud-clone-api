using FluentValidation;
using SoundCloudCloneApi.Application.DTOs.Auth;

namespace SoundCloudCloneApi.Application.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
            .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres.")
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("El formato del correo no es válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("El nombre para mostrar es obligatorio.")
            .MaximumLength(100);
    }
}