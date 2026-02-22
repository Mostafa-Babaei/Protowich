using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Auth.Validators
{
    public class RoleUpsertValidator : AbstractValidator<RoleUpsertDto>
    {
        public RoleUpsertValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["Validation.Role.Name.Required"])
                .MaximumLength(50).WithMessage(localizer["Validation.Role.Name.MaxLength"]);

            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage(localizer["Validation.Role.DisplayName.Required"])
                .MaximumLength(100).WithMessage(localizer["Validation.Role.DisplayName.MaxLength"]);
        }
    }

}
