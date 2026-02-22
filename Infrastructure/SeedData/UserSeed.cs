using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.SeedData
{
    public static class UserSeed
    {
        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            // Idهای ثابت
            var adminId = Guid.Parse("4e02c74b-17d1-4a53-bd2d-5fbe17c214b8");

            // هش ثابت برای "123456"
            var password = "123456";
            var passwordHash = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password))
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminId,
                    Email = "admin@digicarepro.com",
                    UserName="admin",
                    Password = passwordHash,
                    FirstName = "System",
                    LastName = "Administrator",
                    CreatedAt = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc),
                    LastLogin = null,
                    IsSystemAdmin = true
                }
            );
        }
    }
}
