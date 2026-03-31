using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubscriptionCustomerV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscriptionCustomer",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SubscriptionCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionCustomer", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionCustomer_SubscriptionCode",
                schema: "Restaurant",
                table: "SubscriptionCustomer",
                column: "SubscriptionCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscriptionCustomer",
                schema: "Restaurant");

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 17, 35, 3, 160, DateTimeKind.Local).AddTicks(5266), new DateTime(2026, 2, 17, 17, 35, 3, 160, DateTimeKind.Local).AddTicks(5291) });

            migrationBuilder.UpdateData(
                schema: "Core",
                table: "Company",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 2, 17, 17, 35, 3, 160, DateTimeKind.Local).AddTicks(5295), new DateTime(2026, 2, 17, 17, 35, 3, 160, DateTimeKind.Local).AddTicks(5297) });
        }
    }
}
