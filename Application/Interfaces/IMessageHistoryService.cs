using Application.Common;
using Application.Common.Models;
using Application.Features.SMS.Dtos;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMessageHistoryService : IGenericRepository<MessageHistory>
    {
        Task<ApiResult<PagedResult<MessageHistoryListItemDto>>> SearchAsync(
            MessageHistoryListFilterDto filter,
            CancellationToken ct = default);


    }
}
