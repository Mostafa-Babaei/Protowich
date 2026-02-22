namespace Application.Features.Auth.DTOs
{
    public class PermissionCategoryDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = "";
        public string Title { get; set; } = "";
        public string Icon { get; set; } = "";

        public List<PermissionDto> Permissions { get; set; } = new();
    }

}
