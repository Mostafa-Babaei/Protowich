namespace Application.Features.Auth.DTOs
{
    public class RoleUpsertDto
    {
        public int? Id { get; set; }   // اگر null باشد یعنی ایجاد
        public string Name { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<int> PermissionIds { get; set; } = new();

    }

}
