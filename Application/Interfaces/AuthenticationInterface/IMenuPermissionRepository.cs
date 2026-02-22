using Application.Common;
using Domain.Entities;

namespace Application.Interfaces.AuthenticationInterface
{
    public interface IMenuPermissionRepository : IGenericRepository<MenuPermission>
    {
        Task<List<MenuPermission>> GetByMenuIdAsync(int menuId);
    }

}
