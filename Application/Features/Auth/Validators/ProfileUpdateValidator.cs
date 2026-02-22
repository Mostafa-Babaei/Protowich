using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Auth.Validators
{
    public class ProfileUpdateValidator : AbstractValidator<ProfileUpdateDto>
    {
        public ProfileUpdateValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage(localizer["Validation.FirstName.Required"]);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage(localizer["Validation.LastName.Required"]);

            //RuleFor(x => x.Email)
            //    .NotEmpty().WithMessage(localizer["Validation.Email.Required"])
            //    .EmailAddress().WithMessage(localizer["Validation.Email.Invalid"]);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20)
                .WithMessage(localizer["Validation.Phone.MaxLength"]);
        }
    }

}
