using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Auth.Validators
{
    public class UserUpsertDtoValidator : AbstractValidator<UserUpsertDto>
    {
        public UserUpsertDtoValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            // 📌 Email (همیشه الزامی)
            //RuleFor(x => x.UserName)
            //    .NotEmpty().WithMessage(localizer["Validation.Email.Required"]);

            RuleFor(x => x.FirstName)
                   .NotEmpty().WithMessage(localizer["Validation.FirstName.Required"]);

            //RuleFor(x => x.LastName)
            //    .NotEmpty().WithMessage(localizer["Validation.LastName.Required"]);

            //RuleFor(x => x.Gender)
            //    .NotEmpty().WithMessage(localizer["Validation.Gender.Required"]);

            // 📌 نقش‌ها (حداقل یک نقش)
            RuleFor(x => x.RoleIds)
                .NotEmpty().WithMessage(localizer["Validation.Role.Required"]);

            // 📌 حالت افزودن کاربر
            When(x => x.Id == null || x.Id == Guid.Empty, () =>
            {
                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage(localizer["Validation.Password.Required"])
                    .MinimumLength(6).WithMessage(localizer["Validation.Password.MinLength"]);
            });

        }
    }

}
