namespace Application.Features.Auth.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
    }
}
