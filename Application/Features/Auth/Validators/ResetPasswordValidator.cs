using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordValidator(IStringLocalizer<ValidationMessages> localizer)
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(localizer["Validation.Email.Required"])
            .EmailAddress().WithMessage(localizer["Validation.Email.Invalid"]);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(localizer["Validation.Code.Required"])
            .Length(4).WithMessage(localizer["Validation.Code.Invalid"]);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(localizer["Validation.Password.Required"])
            .MinimumLength(6).WithMessage(localizer["Validation.Password.MinLength"]);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(localizer["Validation.Password.ConfirmRequired"])
            .Equal(x => x.NewPassword).WithMessage(localizer["Validation.Password.Mismatch"]);
    }
    
}
