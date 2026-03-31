using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodCategoryImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                schema: "Restaurant",
                table: "FoodCategory",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                schema: "Restaurant",
                table: "FoodCategory");

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
    }
}
