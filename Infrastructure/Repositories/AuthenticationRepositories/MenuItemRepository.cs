using Application.Interfaces.AuthenticationInterface;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.AuthenticationRepositories
{
    public class MenuItemRepository : GenericRepository<MenuItem>, IMenuItemRepository
    {
        private readonly AppDbContext _context;
        public MenuItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MenuItem>> GetUserMenuAsync(Guid userId)
        {
            // منوهایی که حداقل یکی از Permissionهای کاربر رو دارند
            var userPermissions = await (
                from ur in _context.UserRoles
                join rp in _context.RolePermissions on ur.RoleId equals rp.RoleId
                join p in _context.Permissions on rp.PermissionId equals p.Id
                where ur.UserId == userId
                select p.Code
            ).Distinct().ToListAsync();

            return await _context.MenuItems
                .Include(m => m.MenuPermissions)
                .ThenInclude(mp => mp.Permission)
                .Where(m => m.MenuPermissions.Any(mp => userPermissions.Contains(mp.Permission.Code)))
                .OrderBy(m => m.ParentId)
                .ThenBy(m => m.Id)
                .ToListAsync();
        }
    }

}
