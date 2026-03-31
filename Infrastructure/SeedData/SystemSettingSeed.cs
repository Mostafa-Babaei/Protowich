using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData
{
    public static class SystemSettingSeed
    {
        public static void SystemSettings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemSetting>().HasData(
                new SystemSetting
                {
                    Id = 1,
                    Category = "Landing",
                    Key = "Hero.Title",
                    Value = "پروتئینی پروتویچ",
                    ValueType = "string",
                    Description = "عنوان اصلی بنر",
                    DisplayOrder = 1,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 2,
                    Category = "Landing",
                    Key = "Hero.Subtitle",
                    Value = "غذای سالم، زندگی انرژی‌بخش",
                    ValueType = "string",
                    Description = "متن زیر عنوان بنر",
                    DisplayOrder = 2,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 3,
                    Category = "Landing",
                    Key = "Hero.ImageUrl",
                    Value = "https://picsum.photos/id/108/500/350",
                    ValueType = "url",
                    Description = "عکس بنر",
                    DisplayOrder = 3,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 4,
                    Category = "Landing",
                    Key = "Contact.PhonePrimary",
                    Value = "021-12345678",
                    ValueType = "string",
                    Description = "تلفن اصلی",
                    DisplayOrder = 4,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 5,
                    Category = "Landing",
                    Key = "Contact.PhoneSecondary",
                    Value = "09120000000",
                    ValueType = "string",
                    Description = "تلفن دوم",
                    DisplayOrder = 5,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 6,
                    Category = "Landing",
                    Key = "Contact.Address",
                    Value = "تهران، خیابان ولیعصر، نبش خیابان ملاصدرا، پلاک ۱۲۴، طبقه اول",
                    ValueType = "string",
                    Description = "آدرس شعبه",
                    DisplayOrder = 6,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 7,
                    Category = "Landing",
                    Key = "Contact.Latitude",
                    Value = "35.7749",
                    ValueType = "number",
                    Description = "عرض جغرافیایی شعبه",
                    DisplayOrder = 7,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 8,
                    Category = "Landing",
                    Key = "Contact.Longitude",
                    Value = "51.4180",
                    ValueType = "number",
                    Description = "طول جغرافیایی شعبه",
                    DisplayOrder = 8,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 9,
                    Category = "Landing",
                    Key = "Social.InstagramUrl",
                    Value = "https://instagram.com/protovitch_protein",
                    ValueType = "url",
                    Description = "آدرس اینستاگرام",
                    DisplayOrder = 9,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 10,
                    Category = "Landing",
                    Key = "Social.TelegramUrl",
                    Value = "https://t.me/protovitch_support",
                    ValueType = "url",
                    Description = "آدرس تلگرام",
                    DisplayOrder = 10,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 11,
                    Category = "Landing",
                    Key = "Social.WhatsappUrl",
                    Value = "https://wa.me/989120000000",
                    ValueType = "url",
                    Description = "آدرس واتساپ",
                    DisplayOrder = 11,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new SystemSetting
                {
                    Id = 12,
                    Category = "Landing",
                    Key = "Social.Email",
                    Value = "info@protovitch.com",
                    ValueType = "string",
                    Description = "ایمیل",
                    DisplayOrder = 12,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            );
        }
    }
}

