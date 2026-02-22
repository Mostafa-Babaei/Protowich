using Domain.Entities;
using Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData
{
    public static class PermissionSeed
    {
        public static void Permissions(this ModelBuilder modelBuilder)
        {
            // =========================================================
            // ================== PERMISSION CATEGORIES =================
            // =========================================================

            modelBuilder.Entity<PermissionCategory>().HasData(

                new PermissionCategory { Id = 1, Key = "visitors", Title = "مراجعین", Icon = "hgi hgi-user-group" },
                new PermissionCategory { Id = 2, Key = "documents", Title = "مدارک و لینک‌ها", Icon = "hgi hgi-file-attachment" },
                new PermissionCategory { Id = 3, Key = "reservation", Title = "رزرواسیون", Icon = "hgi hgi-calendar-03" },
                new PermissionCategory { Id = 4, Key = "visit", Title = "ویزیت", Icon = "hgi hgi-stethoscope" },
                new PermissionCategory { Id = 5, Key = "dashboard", Title = "داشبورد", Icon = "hgi hgi-dashboard-square-01" },
                new PermissionCategory { Id = 6, Key = "data_io", Title = "ورود و خروج اطلاعات", Icon = "hgi hgi-upload-04" },
                new PermissionCategory { Id = 7, Key = "activity_logs", Title = "تاریخچه فعالیت‌ها", Icon = "hgi hgi-clock-04" },
                new PermissionCategory { Id = 8, Key = "personel_management", Title = "مدیریت پرسنل", Icon = "hgi hgi-user-square" },
                new PermissionCategory { Id = 9, Key = "role_management", Title = "مدیریت نقش", Icon = "hgi hgi-shield-user" },
                new PermissionCategory { Id = 10, Key = "user_management", Title = "مدیریت کاربران", Icon = "hgi hgi-user-settings-01" }
            );

            // =========================================================
            // ======================= PERMISSIONS ======================
            // =========================================================

            modelBuilder.Entity<Permission>().HasData(

                // =========================================================
                // ======================= VISITORS ========================
                // =========================================================
                new Permission { Id = 1, Code = "VISITORS_VIEW", Title = "مشاهده مراجعین", CategoryId = 1 },
                new Permission { Id = 2, Code = "VISITORS_CREATE", Title = "ثبت مراجع جدید", CategoryId = 1 },
                new Permission { Id = 3, Code = "VISITORS_EDIT", Title = "ویرایش مراجع", CategoryId = 1 },
                new Permission { Id = 4, Code = "VISITORS_DELETE", Title = "حذف مراجع", CategoryId = 1 },
                new Permission { Id = 5, Code = "VISITORS_EXPORT", Title = "خروجی گرفتن از مراجعین", CategoryId = 1 },

                // =========================================================
                // ====================== DOCUMENTS ========================
                // =========================================================
                new Permission { Id = 6, Code = "DOCUMENTS_VIEW", Title = "مشاهده مدارک", CategoryId = 2 },
                new Permission { Id = 7, Code = "DOCUMENTS_UPLOAD", Title = "بارگذاری مدرک", CategoryId = 2 },
                new Permission { Id = 8, Code = "DOCUMENTS_EDIT", Title = "ویرایش مدرک", CategoryId = 2 },
                new Permission { Id = 9, Code = "DOCUMENTS_DELETE", Title = "حذف مدرک", CategoryId = 2 },
                new Permission { Id = 10, Code = "DOCUMENTS_DOWNLOAD", Title = "دانلود مدرک", CategoryId = 2 },

                // =========================================================
                // ===================== RESERVATION =======================
                // =========================================================
                new Permission { Id = 11, Code = "RESERVATION_VIEW", Title = "مشاهده رزروها", CategoryId = 3 },
                new Permission { Id = 12, Code = "RESERVATION_CREATE", Title = "ثبت رزرو", CategoryId = 3 },
                new Permission { Id = 13, Code = "RESERVATION_EDIT", Title = "ویرایش رزرو", CategoryId = 3 },
                new Permission { Id = 14, Code = "RESERVATION_CANCEL", Title = "لغو رزرو", CategoryId = 3 },
                new Permission { Id = 15, Code = "RESERVATION_DELETE", Title = "حذف رزرو", CategoryId = 3 },

                // =========================================================
                // ========================= VISIT =========================
                // =========================================================
                new Permission { Id = 16, Code = "VISIT_VIEW", Title = "مشاهده ویزیت‌ها", CategoryId = 4 },
                new Permission { Id = 17, Code = "VISIT_CREATE", Title = "ثبت ویزیت", CategoryId = 4 },
                new Permission { Id = 18, Code = "VISIT_EDIT", Title = "ویرایش ویزیت", CategoryId = 4 },
                new Permission { Id = 19, Code = "VISIT_DELETE", Title = "حذف ویزیت", CategoryId = 4 },
                new Permission { Id = 20, Code = "VISIT_COMPLETE", Title = "ثبت نتیجه ویزیت", CategoryId = 4 },

                // =========================================================
                // ======================= DASHBOARD =======================
                // =========================================================
                new Permission { Id = 21, Code = "DASHBOARD_VIEW", Title = "مشاهده داشبورد", CategoryId = 5 },
                new Permission { Id = 22, Code = "DASHBOARD_STATISTICS", Title = "مشاهده آمار و گزارش‌ها", CategoryId = 5 },

                // =========================================================
                // ======================== DATA IO ========================
                // =========================================================
                new Permission { Id = 23, Code = "DATA_IMPORT", Title = "ورود اطلاعات", CategoryId = 6 },
                new Permission { Id = 24, Code = "DATA_EXPORT", Title = "خروج اطلاعات", CategoryId = 6 },
                new Permission { Id = 25, Code = "DATA_SYNC", Title = "همگام‌سازی اطلاعات", CategoryId = 6 },

                // =========================================================
                // ===================== ACTIVITY LOGS =====================
                // =========================================================
                new Permission { Id = 26, Code = "LOGS_VIEW", Title = "مشاهده تاریخچه فعالیت‌ها", CategoryId = 7 },
                new Permission { Id = 27, Code = "LOGS_FILTER", Title = "جستجو و فیلتر لاگ‌ها", CategoryId = 7 },
                new Permission { Id = 28, Code = "LOGS_DELETE", Title = "حذف تاریخچه فعالیت‌ها", CategoryId = 7 },

                // =========================================================
                // =================== STAFF MANAGEMENT ===================
                // =========================================================
                new Permission { Id = 29, Code = "PERSONEL_VIEW", Title = "مشاهده پرسنل", CategoryId = 8 },
                new Permission { Id = 30, Code = "PERSONEL_CREATE", Title = "ثبت پرسنل", CategoryId = 8 },
                new Permission { Id = 31, Code = "PERSONEL_EDIT", Title = "ویرایش پرسنل", CategoryId = 8 },
                new Permission { Id = 32, Code = "PERSONEL_DELETE", Title = "حذف پرسنل", CategoryId = 8 },
                new Permission { Id = 33, Code = "PERSONEL_STATUS", Title = "فعال/غیرفعال کردن پرسنل", CategoryId = 8 },

                // =========================================================
                // =================== ROLE MANAGEMENT ====================
                // =========================================================
                new Permission { Id = 34, Code = "ROLE_VIEW", Title = "مشاهده نقش‌ها", CategoryId = 9 },
                new Permission { Id = 35, Code = "ROLE_CREATE", Title = "ایجاد نقش", CategoryId = 9 },
                new Permission { Id = 36, Code = "ROLE_EDIT", Title = "ویرایش نقش", CategoryId = 9 },
                new Permission { Id = 37, Code = "ROLE_DELETE", Title = "حذف نقش", CategoryId = 9 },
                new Permission { Id = 38, Code = "ROLE_ASSIGN_PERMISSION", Title = "تخصیص دسترسی به نقش", CategoryId = 9 },

                // =========================================================
                // =================== USER MANAGEMENT ====================
                // =========================================================
                new Permission { Id = 39, Code = "USER_VIEW", Title = "مشاهده کاربران", CategoryId = 10 },
                new Permission { Id = 40, Code = "USER_CREATE", Title = "ایجاد کاربر", CategoryId = 10 },
                new Permission { Id = 41, Code = "USER_EDIT", Title = "ویرایش کاربر", CategoryId = 10 },
                new Permission { Id = 42, Code = "USER_DELETE", Title = "حذف کاربر", CategoryId = 10 },
                new Permission { Id = 43, Code = "USER_RESET_PASSWORD", Title = "ریست رمز عبور", CategoryId = 10 },
                new Permission { Id = 44, Code = "USER_ASSIGN_ROLE", Title = "تخصیص نقش به کاربر", CategoryId = 10 }
            );
        }

    }
}
