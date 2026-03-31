using Application.Features.FastFood.Dtos;

namespace Web.Areas.Admin.Models
{
    public class SubscriptionCustomerFormVm
    {
        public int? Id { get; set; }
        public SubscriptionCustomerUpsertDto Dto { get; set; } = new();
    }
}
