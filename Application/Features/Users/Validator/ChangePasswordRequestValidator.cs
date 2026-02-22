using Application.Features.Users.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Users.Validator
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            // Current password
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage(localizer["Validation.Password.Current.Required"]);

            // New password
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage(localizer["Validation.Password.New.Required"])
                .Must(ContainUppercase)
                    .WithMessage(localizer["Validation.Password.Uppercase"])
                .Must(ContainNumber)
                    .WithMessage(localizer["Validation.Password.Number"])
                .MinimumLength(8)
                    .WithMessage(localizer["Validation.Password.MinLength"]);

            // Confirm password
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage(localizer["Validation.Password.Mismatch"]);
        }

        private bool ContainUppercase(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Any(char.IsUpper);
        }

        private bool ContainNumber(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Any(char.IsDigit);
        }
    }
}
