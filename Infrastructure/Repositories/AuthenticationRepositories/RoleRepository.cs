using Application.Common.Models;
using Application.Features.Auth.DTOs;
using Application.Resources;
using Domain.Entities;
using Domain.Entities.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Interfaces
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<ValidationMessages> _localizer;
        public RoleRepository(
            AppDbContext context,
            IStringLocalizer<ValidationMessages> localizer) : base(context)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<List<int>> GetAssignedPermissionIdsAsync(int roleId)
        {
            return await _context.Set<RolePermission>()
                .AsNoTracking()
                .Where(x => x.RoleId == roleId)
                .Select(x => x.PermissionId)
                .ToListAsync();
        }

        public async Task<ApiResult<string>> AssignPermissionsAsync(int roleId, List<int> permissionIds)
        {
            var old = _context.RolePermissions.Where(x => x.RoleId == roleId);
            _context.RemoveRange(old);

            foreach (var pid in permissionIds)
                await _context.RolePermissions.AddAsync(new RolePermission { RoleId = roleId, PermissionId = pid });

            await SaveChangesAsync();
            return ApiResult<string>.Success(_localizer["Role.Permissions.Assigned"]);
        }
        public async Task<ApiResult<string>> DeleteRoleAsync(int id)
        {
            var role = await GetByIdAsync(id);
            if (role == null)
                return ApiResult<string>.Error(_localizer["Role.NotFound"]);

            // جلوگیری از حذف اگر به کاربر assign شده
            var hasUsers = await _context.UserRoles.AnyAsync(x => x.RoleId == id);
            if (hasUsers)
                return ApiResult<string>.Error(_localizer["Role.Delete.AssignedToUsers"]);

            // 1️⃣ حذف دسترسی‌های وابسته
            var perms = _context.RolePermissions.Where(x => x.RoleId == id);
            _context.RolePermissions.RemoveRange(perms);

            // 2️⃣ حذف خود نقش
            Remove(role);

            await SaveChangesAsync();

            return ApiResult<string>.Success(_localizer["Role.Delete.Success"]);
        }

        public async Task<ApiResult<string>> CreateFullRoleAsync(RoleUpsertDto dto)
        {
            var role = new Role
            {
                Name = dto.Name,
                DisplayName = dto.DisplayName,
                Description = dto.Description
            };

            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            // ذخیره دسترسی‌ها
            foreach (var pid in dto.PermissionIds)
                await _context.RolePermissions.AddAsync(new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = pid
                });

            await _context.SaveChangesAsync();

            return ApiResult<string>.Success(_localizer["Role.Created"]);
        }

        public async Task<ApiResult<string>> UpdateFullRoleAsync(int id, RoleUpsertDto dto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
                return ApiResult<string>.Error(_localizer["Role.NotFound"]);

            role.Name = dto.Name;
            role.DisplayName = dto.DisplayName;
            role.Description = dto.Description;

            // حذف دسترسی قبلی
            var old = _context.RolePermissions.Where(x => x.RoleId == id);
            _context.RolePermissions.RemoveRange(old);

            // افزودن جدید
            foreach (var pid in dto.PermissionIds)
                await _context.RolePermissions.AddAsync(new RolePermission
                {
                    RoleId = id,
                    PermissionId = pid
                });

            await _context.SaveChangesAsync();
            return ApiResult<string>.Success(_localizer["Role.Updated"]);

        }
        public async Task<ApiResult<RoleForEditDto>> GetRoleForEditAsync(int roleId)
        {
            // نقش
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (role == null)
                return ApiResult<RoleForEditDto>.Error(_localizer["Role.NotFound"]);


            // آی‌دی‌های دسترسی فعال
            var selectedIds = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.PermissionId)
                .ToListAsync();

            // تمام Permission ها + اطلاعات کامل دسته‌بندی
            var permissions = await _context.Permissions
                .Select(p => new PermissionEditItemDto
                {
                    Id = p.Id,
                    Code = p.Code,
                    Title = p.Title,

                    Category = p.Category.Title,
                    Icon = p.Category.Icon,

                    // فعال یا نه
                    IsSelected = selectedIds.Contains(p.Id)
                })
                .ToListAsync();

            var dto = new RoleForEditDto
            {
                Id = role.Id,
                Name = role.Name,
                DisplayName = role.DisplayName,
                Description = role.Description,
                Permissions = permissions
            };

            return ApiResult<RoleForEditDto>.Success(dto);
        }
        public async Task<List<int>> GetAssignedMenuIdsAsync(int roleId)
        {
            return await _context.Set<MenuRole>()
                .Where(x => x.RoleId == roleId)
                .Select(x => x.MenuItemId)
                .ToListAsync();
        }

        public async Task AssignMenusAsync(int roleId, List<int> menuIds)
        {
            var db = _context;

            var existing = await db.Set<MenuRole>()
                .Where(x => x.RoleId == roleId)
                .ToListAsync();

            db.RemoveRange(existing);

            if (menuIds != null && menuIds.Any())
            {
                var rows = menuIds.Distinct().Select(mid => new MenuRole
                {
                    RoleId = roleId,
                    MenuItemId = mid
                });

                await db.AddRangeAsync(rows);
            }
        }


    }
}
