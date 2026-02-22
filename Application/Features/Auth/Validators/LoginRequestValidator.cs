using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(localizer["Validation.Email.Required"]);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(localizer["Validation.Password.Required"])
            .MinimumLength(6).WithMessage(localizer["Validation.Password.MinLength"]);
    }
}
