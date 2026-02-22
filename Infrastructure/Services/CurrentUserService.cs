using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _http;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _http = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var val = _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Guid.TryParse(val, out var id) ? id : null;
            }
        }

        public string? Email =>
            _http.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

    }
}
