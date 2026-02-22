using Application.Common;
using Application.Common.Models;
using Application.Features.Auth.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<ApiResult<List<PermissionCategoryDto>>> GetGroupedWithIconsAsync(); 
        Task<List<PermissionCategoryDto>> GetPermissionTreeAsync();

    }
}
