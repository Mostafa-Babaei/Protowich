using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuThemeSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuThemeSetting",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiveThemeKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuThemeSetting", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 11, 18, 31, 884, DateTimeKind.Local).AddTicks(9126), new DateTime(2026, 3, 31, 11, 18, 31, 884, DateTimeKind.Local).AddTicks(9138) });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 11, 18, 31, 884, DateTimeKind.Local).AddTicks(9139), new DateTime(2026, 3, 31, 11, 18, 31, 884, DateTimeKind.Local).AddTicks(9140) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuThemeSetting",
                schema: "Restaurant");

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 10, 43, 21, 622, DateTimeKind.Local).AddTicks(9651), new DateTime(2026, 3, 31, 10, 43, 21, 622, DateTimeKind.Local).AddTicks(9661) });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 3, 31, 10, 43, 21, 622, DateTimeKind.Local).AddTicks(9663), new DateTime(2026, 3, 31, 10, 43, 21, 622, DateTimeKind.Local).AddTicks(9664) });
        }
    }
}
