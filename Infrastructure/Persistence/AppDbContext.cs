using System.Linq.Expressions;
using Domain.Common;
using Domain.Entities;
using Domain.Entities.Appointments;
using Domain.Entities.Authentication;
using Domain.Entities.FastFood;
using Domain.Entities.Logging;
using Domain.Entities.VisitorModels;
using Infrastructure.Persistence.SeedData;
using Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<ErrorLog> ErrorLogs { get; set; }

        //Authenticate
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuRole> MenuRoles { get; set; }
        public DbSet<MenuPermission> MenuPermissions { get; set; }
        public DbSet<PermissionCategory> PermissionCategories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }


        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<FoodImage> FoodImages { get; set; }
        public DbSet<SubscriptionCustomer> SubscriptionCustomers { get; set; }
        public DbSet<MenuThemeSetting> MenuThemeSettings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.SeedUsers();
            modelBuilder.Menus();
            modelBuilder.Permissions();
            modelBuilder.Companies();
            modelBuilder.SystemSettings();

            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.Property(x => x.Price).HasPrecision(18, 2);
            });

            modelBuilder.Entity<SubscriptionCustomer>(entity =>
            {
                entity.HasIndex(x => x.SubscriptionCode).IsUnique();
            });

            modelBuilder.Entity<MenuRole>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasIndex(x => new { x.RoleId, x.MenuItemId })
                      .IsUnique();

                entity.HasOne(x => x.Role)
                      .WithMany(r => r.MenuRoles)
                      .HasForeignKey(x => x.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.MenuItem)
                      .WithMany(m => m.MenuRoles)
                      .HasForeignKey(x => x.MenuItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasIndex(x => new { x.Category, x.Key }).IsUnique();
            });

        }

    }
}
