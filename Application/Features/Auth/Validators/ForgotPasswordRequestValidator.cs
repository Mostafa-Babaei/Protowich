using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequestDto>
{
    public ForgotPasswordRequestValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["Validation.Email.Required"])
            .EmailAddress().WithMessage(localizer["Validation.Email.Invalid"]);
    }
}
