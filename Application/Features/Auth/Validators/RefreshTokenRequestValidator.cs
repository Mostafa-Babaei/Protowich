using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Auth.Validators
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage(localizer["Validation.RefreshToken.Required"]);
        }
    }
}
