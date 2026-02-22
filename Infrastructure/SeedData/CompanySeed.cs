using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData
{
    public static class CompanySeed
    {
        public static void Companies(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, IsDeleted = false, Code = "1", CreatedAt = DateTime.Now, Title = "مطب دکتر روشنی", IsActive = true, UpdatedAt = DateTime.Now },
                new Company { Id = 2, IsDeleted = false, Code = "2", CreatedAt = DateTime.Now, Title = "کلینیک ترک اعتیاد", IsActive = true, UpdatedAt = DateTime.Now }
            );
        }
    }
}
