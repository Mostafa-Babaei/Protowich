using Application.Common;
using Application.Common.Models;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Application.Interfaces
{
    public class RolePermissionRepository : GenericRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(AppDbContext context) : base(context)
        {
        }

    }
}
