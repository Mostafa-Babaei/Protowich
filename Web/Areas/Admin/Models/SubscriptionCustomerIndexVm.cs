using Application.Common.Models;
using Application.Features.FastFood.Dtos;

namespace Web.Areas.Admin.Models
{
    public class SubscriptionCustomerIndexVm
    {
        public string? Keyword { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public PagedResult<SubscriptionCustomerListItemDto>? Data { get; set; }
    }
}
