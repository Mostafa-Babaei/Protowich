namespace Application.Features.FastFood.Dtos
{
    public class SystemSettingItemDto
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string? Value { get; set; }
        public string? ValueType { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
    }
}

