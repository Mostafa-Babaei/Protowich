using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData
{
    public static class MenuSeed
    {
        public static void Menus(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>().HasData(

                // Root Menus
                new MenuItem
                {
                    Id = 1,
                    Title = "داشبورد",
                    Icon = "fas fa-home",
                    Route = "/",
                    Section = "Main",
                    DisplayOrder = 1,
                    IsActive = true,
                    IsDeleted = false
                },
                new MenuItem
                {
                    Id = 2,
                    Title = "مراجعین",
                    Icon = "fas fa-users",
                    Route = "/visitor",
                    Section = "Main",
                    DisplayOrder = 2,
                    IsActive = true,
                    IsDeleted = false
                },

                // Admin
                new MenuItem { Id = 10, Title = "مدیریت کاربران", Icon = "fas fa-user-cog", Route = "/User", Section = "Admin", DisplayOrder = 10, IsActive = true },
                new MenuItem { Id = 11, Title = "ورود اطلاعات", Icon = "fas fa-upload", Route = "/Import", Section = "Admin", DisplayOrder = 11, IsActive = true },
                new MenuItem { Id = 12, Title = "خروجی اکسل مراجعین", Icon = "fas fa-file-excel", Route = "/Export", Section = "Admin", DisplayOrder = 12, IsActive = true },
                new MenuItem { Id = 13, Title = "تاریخچه فعالیت‌ها", Icon = "fas fa-history", Route = "/UserActivityLogs", Section = "Admin", DisplayOrder = 13, IsActive = true },
                new MenuItem { Id = 14, Title = "مدیریت پرسنل", Icon = "fas fa-user-clock", Route = "/Personel", Section = "Admin", DisplayOrder = 14, IsActive = true },
                new MenuItem { Id = 15, Title = "مدیریت نقش‌ها", Icon = "fas fa-user-shield", Route = "/Roles", Section = "Admin", DisplayOrder = 15, IsActive = true },

                // Visit Parent
                new MenuItem
                {
                    Id = 20,
                    Title = "مدیریت ویزیت",
                    Icon = "fas fa-calendar-check",
                    Route = null,
                    Section = "Visit",
                    DisplayOrder = 16,
                    IsActive = true
                },

                // Visit Children
                new MenuItem
                {
                    Id = 21,
                    ParentId = 20,
                    Title = "روزها و شیفت‌های ویزیت",
                    Icon = "far fa-clock",
                    Route = "/Reservation",
                    Section = "Visit",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new MenuItem
                {
                    Id = 22,
                    ParentId = 20,
                    Title = "ثبت نوبت مراجعه",
                    Icon = "far fa-calendar-plus",
                    Route = "/Appointment",
                    Section = "Visit",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new MenuItem
                {
                    Id = 23,
                    Title = "پیامک",
                    Icon = "fas fa-envelope",
                    Route = "/MessageHistory",
                    Section = "Admin",
                    DisplayOrder = 17,
                    IsActive = true
                }
            );
        }
    }
}
