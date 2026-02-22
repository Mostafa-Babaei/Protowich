using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.DTOs
{
    public class ProfilePictureUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }

}
