using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

public class VerifyResetCodeValidator : AbstractValidator<VerifyResetCodeDto>
{
    public VerifyResetCodeValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["Validation.Email.Required"])
            .EmailAddress().WithMessage(localizer["Validation.Email.Invalid"]);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(localizer["Validation.Code.Required"])
            .Length(4).WithMessage(localizer["Validation.Code.Invalid"]);
    }
}
