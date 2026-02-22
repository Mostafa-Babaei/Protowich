using Application.Common;
using Domain.Entities;

namespace Application.Interfaces.AuthenticationInterface
{
    public interface IMenuItemRepository : IGenericRepository<MenuItem>
    {
        Task<List<MenuItem>> GetUserMenuAsync(Guid userId);
    }

}
