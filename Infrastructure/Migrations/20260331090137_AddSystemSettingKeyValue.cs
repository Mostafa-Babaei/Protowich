using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSystemSettingKeyValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemSetting",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    ValueType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSetting", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6206), new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6241) });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6244), new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6245) });

            migrationBuilder.InsertData(
                schema: "Core",
                table: "SystemSetting",
                columns: new[] { "Id", "Category", "CreatedAt", "CreatedBy", "Description", "DisplayOrder", "IsActive", "IsDeleted", "Key", "UpdatedAt", "UpdatedBy", "Value", "ValueType" },
                values: new object[,]
                {
                    { 1, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6343), null, "عنوان اصلی بنر", 1, true, false, "Hero.Title", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6344), null, "پروتئینی پروتویچ", "string" },
                    { 2, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6346), null, "متن زیر عنوان بنر", 2, true, false, "Hero.Subtitle", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6347), null, "غذای سالم، زندگی انرژی‌بخش", "string" },
                    { 3, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6369), null, "عکس بنر", 3, true, false, "Hero.ImageUrl", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6370), null, "https://picsum.photos/id/108/500/350", "url" },
                    { 4, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6374), null, "تلفن اصلی", 4, true, false, "Contact.PhonePrimary", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6375), null, "021-12345678", "string" },
                    { 5, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6379), null, "تلفن دوم", 5, true, false, "Contact.PhoneSecondary", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6380), null, "09120000000", "string" },
                    { 6, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6382), null, "آدرس شعبه", 6, true, false, "Contact.Address", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6382), null, "تهران، خیابان ولیعصر، نبش خیابان ملاصدرا، پلاک ۱۲۴، طبقه اول", "string" },
                    { 7, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6385), null, "عرض جغرافیایی شعبه", 7, true, false, "Contact.Latitude", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6386), null, "35.7749", "number" },
                    { 8, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6397), null, "طول جغرافیایی شعبه", 8, true, false, "Contact.Longitude", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6397), null, "51.4180", "number" },
                    { 9, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6399), null, "آدرس اینستاگرام", 9, true, false, "Social.InstagramUrl", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6400), null, "https://instagram.com/protovitch_protein", "url" },
                    { 10, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6402), null, "آدرس تلگرام", 10, true, false, "Social.TelegramUrl", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6403), null, "https://t.me/protovitch_support", "url" },
                    { 11, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6404), null, "آدرس واتساپ", 11, true, false, "Social.WhatsappUrl", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6405), null, "https://wa.me/989120000000", "url" },
                    { 12, "Landing", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6407), null, "ایمیل", 12, true, false, "Social.Email", new DateTime(2026, 3, 31, 12, 31, 36, 311, DateTimeKind.Local).AddTicks(6408), null, "info@protovitch.com", "string" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemSetting_Category_Key",
                schema: "Core",
                table: "SystemSetting",
                columns: new[] { "Category", "Key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemSetting",
                schema: "Core");

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 12, 0, 29, 645, DateTimeKind.Local).AddTicks(3497), new DateTime(2026, 3, 31, 12, 0, 29, 645, DateTimeKind.Local).AddTicks(3516) });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 12, 0, 29, 645, DateTimeKind.Local).AddTicks(3520), new DateTime(2026, 3, 31, 12, 0, 29, 645, DateTimeKind.Local).AddTicks(3522) });
        }
    }
}
