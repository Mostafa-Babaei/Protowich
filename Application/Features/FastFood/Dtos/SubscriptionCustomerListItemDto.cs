namespace Application.Features.FastFood.Dtos
{
    public class SubscriptionCustomerListItemDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Address { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string SubscriptionCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
