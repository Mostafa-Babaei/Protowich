using Application.Features.FastFood.Dtos;

namespace Web.Areas.Admin.Models
{
public class SystemSettingIndexVm
{
    public string Category { get; set; } = "Landing";
    public List<SystemSettingItemDto> Items { get; set; } = new();
}

public class SystemSettingBulkUpdateItemVm
{
    public int Id { get; set; }
    public string? Value { get; set; }
    public bool IsActive { get; set; }
}
}
