using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData
{
    public static class RoleSeed
    {
        public static void Roles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, DisplayName = "SystemAdmin", Name = "SystemAdmin", Description = "Full system access" },
                new Role { Id = 2, DisplayName = "CompanyAdmin", Name = "CompanyAdmin", Description = "Company-level administrator" },
                new Role { Id = 3, DisplayName = "Dispatcher", Name = "Dispatcher", Description = "Manage missions and schedules" },
                new Role { Id = 4, DisplayName = "Caregiver", Name = "Caregiver", Description = "Performs assigned missions and reports hours" }
            );
        }
    }
}
