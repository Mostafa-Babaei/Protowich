using Application.Common;
using Application.Common.Models;
using Application.Features.Auth.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<ApiResult<string>> DeleteRoleAsync(int id);
        Task<ApiResult<string>> CreateFullRoleAsync(RoleUpsertDto dto);
        Task<ApiResult<string>> UpdateFullRoleAsync(int id, RoleUpsertDto dto);
        Task<ApiResult<RoleForEditDto>> GetRoleForEditAsync(int roleId);
        Task<List<int>> GetAssignedMenuIdsAsync(int roleId);
        Task AssignMenusAsync(int roleId, List<int> menuIds);

        Task<List<int>> GetAssignedPermissionIdsAsync(int roleId);
        Task<ApiResult<string>> AssignPermissionsAsync(int roleId, List<int> permissionIds);


    }
}
