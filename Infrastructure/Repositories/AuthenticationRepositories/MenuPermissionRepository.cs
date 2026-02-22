using Application.Interfaces.AuthenticationInterface;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Infrastructure.Repositories.AuthenticationRepositories
{
    public class MenuPermissionRepository : GenericRepository<MenuPermission>, IMenuPermissionRepository
    {
        private readonly AppDbContext _context;
        public MenuPermissionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<MenuPermission>> GetByMenuIdAsync(int menuId)
        {
            throw new NotImplementedException();
        }
    }

}
