using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Auth.Validators
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            //RuleFor(x => x.CurrentPassword)
            //    .NotEmpty().WithMessage(localizer["Validation.CurrentPassword.Required"]);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(localizer["Validation.NewPassword.Required"])
                .MinimumLength(6).WithMessage(localizer["Validation.NewPassword.MinLength"]);
        }
    }
}
