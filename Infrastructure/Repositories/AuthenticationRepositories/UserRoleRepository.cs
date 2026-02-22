using Application.Common;
using Application.Interfaces.AuthenticationInterface;
using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;

namespace Application.Interfaces
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AppDbContext context) : base(context)
        {
        }
    }
}
