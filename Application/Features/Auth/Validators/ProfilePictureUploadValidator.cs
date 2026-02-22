using Application.Features.Auth.DTOs;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Auth.Validators
{
    public class ProfilePictureUploadValidator : AbstractValidator<ProfilePictureUploadDto>
    {
        public ProfilePictureUploadValidator(IStringLocalizer<ValidationMessages> localizer)
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage(localizer["Validation.ProfilePicture.Required"])
                .Must(f => f.Length <= 2 * 1024 * 1024)
                    .WithMessage(localizer["Validation.ProfilePicture.MaxSize"])
                .Must(f => f.ContentType.StartsWith("image/"))
                    .WithMessage(localizer["Validation.ProfilePicture.InvalidType"]);
        }
    }

}
