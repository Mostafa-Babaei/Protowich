using Application.Common;
using Application.Common.Models;
using Application.Features.FastFood.Dtos;
using Domain.Entities.FastFood;

namespace Application.Interfaces.FastFoodInterface
{
    public interface ISubscriptionCustomerService : IGenericRepository<SubscriptionCustomer>
    {
        Task<ApiResult<PagedResult<SubscriptionCustomerListItemDto>>> GetPagedAsync(
            int page, int pageSize, string? keyword = null, CancellationToken ct = default);

        Task<ApiResult<int>> CreateAsync(SubscriptionCustomerUpsertDto dto, CancellationToken ct = default);
        Task<ApiResult<string>> UpdateAsync(int id, SubscriptionCustomerUpsertDto dto, CancellationToken ct = default);
        Task<ApiResult<string>> SetActiveAsync(int id, bool isActive, CancellationToken ct = default);
    }
}
