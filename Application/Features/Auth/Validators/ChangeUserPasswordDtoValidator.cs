using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Auth.Validators
{
    public class ChangeUserPasswordDtoValidator : AbstractValidator<ChangeUserPasswordDto>
    {
        public ChangeUserPasswordDtoValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(localizer["User.NotFound"]);

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage(localizer["Validation.NewPassword.Required"])
                .MinimumLength(6).WithMessage(localizer["Validation.NewPassword.MinLength"]);
        }
    }
}
