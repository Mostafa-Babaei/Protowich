using Application.Common.Models;
using Application.Features.Auth.DTOs;
using DocumentFormat.OpenXml.InkML;
using Domain.Entities;
using Domain.Entities.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        private readonly AppDbContext context;

        public PermissionRepository(AppDbContext context) : base(context)
        {
            this.context = context;
        }


        public async Task<ApiResult<List<PermissionCategoryDto>>> GetGroupedWithIconsAsync()
        {
            var list = await context.PermissionCategories
                .Include(c => c.Permissions)
                .ToListAsync();

            var result = list.Select(c => new PermissionCategoryDto
            {
                Id = c.Id,
                Key = c.Key,
                Title = c.Title,
                Icon = c.Icon,
                Permissions = c.Permissions
                    .Select(p => new PermissionDto
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Title = p.Title
                    }).ToList()

            }).ToList();

            return ApiResult<List<PermissionCategoryDto>>.Success(result);
        }
        public async Task<List<PermissionCategoryDto>> GetPermissionTreeAsync()
        {
            var categories = await context.Set<PermissionCategory>()
                .AsNoTracking()
            .OrderBy(x => x.Id)
                .ToListAsync();

            var permissions = await context.Set<Permission>()
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .ToListAsync();

            var map = categories.ToDictionary(
                c => c.Id,
                c => new PermissionCategoryDto
                {
                    Id = c.Id,
                    Key = c.Key,
                    Title = c.Title,
                    Icon = c.Icon,
                    Permissions = new List<PermissionDto>()
                });

            foreach (var p in permissions)
            {
                if (map.TryGetValue(p.CategoryId, out var cat))
                {
                    cat.Permissions.Add(new PermissionDto
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Title = p.Title,
                        CategoryId = p.CategoryId
                    });
                }
            }

            return map.Values.ToList();
        }


    }
}
