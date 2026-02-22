namespace Application.Features.Auth.DTOs
{
    public class UserRolesDto
    {
        public Guid UserId { get; set; }
        public List<RoleDto> Roles { get; set; } = new();
    }
}
