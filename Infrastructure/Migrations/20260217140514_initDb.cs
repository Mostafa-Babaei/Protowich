using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.EnsureSchema(
                name: "Restaurant");

            migrationBuilder.EnsureSchema(
                name: "Authentication");

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                schema: "Core",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodCategory",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
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
                    table.PrimaryKey("PK_FoodCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionCategory",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    OtpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtpExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginWithSms = table.Column<bool>(type: "bit", nullable: false),
                    IsSystemAdmin = table.Column<bool>(type: "bit", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodItem",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    FoodCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodItem_FoodCategory_FoodCategoryId",
                        column: x => x.FoodCategoryId,
                        principalSchema: "Restaurant",
                        principalTable: "FoodCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Route = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    FoodCategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItem_FoodCategory_FoodCategoryId",
                        column: x => x.FoodCategoryId,
                        principalSchema: "Restaurant",
                        principalTable: "FoodCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MenuItem_MenuItem_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Authentication",
                        principalTable: "MenuItem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_PermissionCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Authentication",
                        principalTable: "PermissionCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Authentication",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Authentication",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodImage",
                schema: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    FoodItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodImage_FoodItem_FoodItemId",
                        column: x => x.FoodItemId,
                        principalSchema: "Restaurant",
                        principalTable: "FoodItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuRole",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    MenuItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuRole_MenuItem_MenuItemId",
                        column: x => x.MenuItemId,
                        principalSchema: "Authentication",
                        principalTable: "MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Authentication",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuPermission",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuPermission_MenuItem_MenuId",
                        column: x => x.MenuId,
                        principalSchema: "Authentication",
                        principalTable: "MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Authentication",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "Authentication",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Authentication",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Authentication",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Core",
                table: "Company",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Description", "IsActive", "IsDeleted", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "1", new DateTime(2026, 2, 17, 17, 35, 3, 160, DateTimeKind.Local).AddTicks(5266), null, null, true, false, "مطب دکتر روشنی", new DateTime(2026, 2, 17, 17, 35, 3, 160, DateTimeKind.Local).AddTicks(5291), null },
                    { 2, "2", new DateTime(2026, 2, 17, 17, 35, 3, 160, DateTimeKind.Local).AddTicks(5295), null, null, true, false, "کلینیک ترک اعتیاد", new DateTime(2026, 2, 17, 17, 35, 3, 160, DateTimeKind.Local).AddTicks(5297), null }
                });

            migrationBuilder.InsertData(
                schema: "Authentication",
                table: "MenuItem",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DisplayOrder", "FoodCategoryId", "Icon", "IsActive", "IsDeleted", "ParentId", "Route", "Section", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, null, 1, null, "fas fa-home", true, false, null, "/", "Main", "داشبورد", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, null, null, 2, null, "fas fa-users", true, false, null, "/visitor", "Main", "مراجعین", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 10, null, null, 10, null, "fas fa-user-cog", true, false, null, "/User", "Admin", "مدیریت کاربران", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 11, null, null, 11, null, "fas fa-upload", true, false, null, "/Import", "Admin", "ورود اطلاعات", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 12, null, null, 12, null, "fas fa-file-excel", true, false, null, "/Export", "Admin", "خروجی اکسل مراجعین", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 13, null, null, 13, null, "fas fa-history", true, false, null, "/UserActivityLogs", "Admin", "تاریخچه فعالیت‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 14, null, null, 14, null, "fas fa-user-clock", true, false, null, "/Personel", "Admin", "مدیریت پرسنل", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 15, null, null, 15, null, "fas fa-user-shield", true, false, null, "/Roles", "Admin", "مدیریت نقش‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 20, null, null, 16, null, "fas fa-calendar-check", true, false, null, null, "Visit", "مدیریت ویزیت", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 23, null, null, 17, null, "fas fa-envelope", true, false, null, "/MessageHistory", "Admin", "پیامک", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                schema: "Authentication",
                table: "PermissionCategory",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Icon", "IsActive", "IsDeleted", "Key", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, null, null, "hgi hgi-user-group", true, false, "visitors", "مراجعین", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, null, null, "hgi hgi-file-attachment", true, false, "documents", "مدارک و لینک‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, null, null, "hgi hgi-calendar-03", true, false, "reservation", "رزرواسیون", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, null, null, "hgi hgi-stethoscope", true, false, "visit", "ویزیت", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, null, null, "hgi hgi-dashboard-square-01", true, false, "dashboard", "داشبورد", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 6, null, null, "hgi hgi-upload-04", true, false, "data_io", "ورود و خروج اطلاعات", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 7, null, null, "hgi hgi-clock-04", true, false, "activity_logs", "تاریخچه فعالیت‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 8, null, null, "hgi hgi-user-square", true, false, "personel_management", "مدیریت پرسنل", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 9, null, null, "hgi hgi-shield-user", true, false, "role_management", "مدیریت نقش", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 10, null, null, "hgi hgi-user-settings-01", true, false, "user_management", "مدیریت کاربران", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                schema: "Authentication",
                table: "User",
                columns: new[] { "Id", "Avatar", "CompanyId", "CreatedAt", "CreatedBy", "Email", "FirstName", "IsActive", "IsDeleted", "IsSystemAdmin", "LastLogin", "LastName", "LoginWithSms", "Mobile", "OtpCode", "OtpExpiresAt", "Password", "Phone", "RefreshToken", "RefreshTokenExpiry", "ResetCode", "UpdatedAt", "UpdatedBy", "UserName" },
                values: new object[] { new Guid("4e02c74b-17d1-4a53-bd2d-5fbe17c214b8"), null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin@digicarepro.com", "System", true, false, true, null, "Administrator", false, null, null, null, "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=", null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "admin" });

            migrationBuilder.InsertData(
                schema: "Authentication",
                table: "MenuItem",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DisplayOrder", "FoodCategoryId", "Icon", "IsActive", "IsDeleted", "ParentId", "Route", "Section", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 21, null, null, 1, null, "far fa-clock", true, false, 20, "/Reservation", "Visit", "روزها و شیفت‌های ویزیت", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 22, null, null, 2, null, "far fa-calendar-plus", true, false, 20, "/Appointment", "Visit", "ثبت نوبت مراجعه", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                schema: "Authentication",
                table: "Permission",
                columns: new[] { "Id", "CategoryId", "Code", "CreatedAt", "CreatedBy", "IsActive", "IsDeleted", "Title", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, "VISITORS_VIEW", null, null, true, false, "مشاهده مراجعین", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, 1, "VISITORS_CREATE", null, null, true, false, "ثبت مراجع جدید", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, 1, "VISITORS_EDIT", null, null, true, false, "ویرایش مراجع", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, 1, "VISITORS_DELETE", null, null, true, false, "حذف مراجع", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, 1, "VISITORS_EXPORT", null, null, true, false, "خروجی گرفتن از مراجعین", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 6, 2, "DOCUMENTS_VIEW", null, null, true, false, "مشاهده مدارک", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 7, 2, "DOCUMENTS_UPLOAD", null, null, true, false, "بارگذاری مدرک", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 8, 2, "DOCUMENTS_EDIT", null, null, true, false, "ویرایش مدرک", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 9, 2, "DOCUMENTS_DELETE", null, null, true, false, "حذف مدرک", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 10, 2, "DOCUMENTS_DOWNLOAD", null, null, true, false, "دانلود مدرک", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 11, 3, "RESERVATION_VIEW", null, null, true, false, "مشاهده رزروها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 12, 3, "RESERVATION_CREATE", null, null, true, false, "ثبت رزرو", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 13, 3, "RESERVATION_EDIT", null, null, true, false, "ویرایش رزرو", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 14, 3, "RESERVATION_CANCEL", null, null, true, false, "لغو رزرو", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 15, 3, "RESERVATION_DELETE", null, null, true, false, "حذف رزرو", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 16, 4, "VISIT_VIEW", null, null, true, false, "مشاهده ویزیت‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 17, 4, "VISIT_CREATE", null, null, true, false, "ثبت ویزیت", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 18, 4, "VISIT_EDIT", null, null, true, false, "ویرایش ویزیت", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 19, 4, "VISIT_DELETE", null, null, true, false, "حذف ویزیت", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 20, 4, "VISIT_COMPLETE", null, null, true, false, "ثبت نتیجه ویزیت", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 21, 5, "DASHBOARD_VIEW", null, null, true, false, "مشاهده داشبورد", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 22, 5, "DASHBOARD_STATISTICS", null, null, true, false, "مشاهده آمار و گزارش‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 23, 6, "DATA_IMPORT", null, null, true, false, "ورود اطلاعات", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 24, 6, "DATA_EXPORT", null, null, true, false, "خروج اطلاعات", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 25, 6, "DATA_SYNC", null, null, true, false, "همگام‌سازی اطلاعات", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 26, 7, "LOGS_VIEW", null, null, true, false, "مشاهده تاریخچه فعالیت‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 27, 7, "LOGS_FILTER", null, null, true, false, "جستجو و فیلتر لاگ‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 28, 7, "LOGS_DELETE", null, null, true, false, "حذف تاریخچه فعالیت‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 29, 8, "PERSONEL_VIEW", null, null, true, false, "مشاهده پرسنل", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 30, 8, "PERSONEL_CREATE", null, null, true, false, "ثبت پرسنل", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 31, 8, "PERSONEL_EDIT", null, null, true, false, "ویرایش پرسنل", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 32, 8, "PERSONEL_DELETE", null, null, true, false, "حذف پرسنل", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 33, 8, "PERSONEL_STATUS", null, null, true, false, "فعال/غیرفعال کردن پرسنل", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 34, 9, "ROLE_VIEW", null, null, true, false, "مشاهده نقش‌ها", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 35, 9, "ROLE_CREATE", null, null, true, false, "ایجاد نقش", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 36, 9, "ROLE_EDIT", null, null, true, false, "ویرایش نقش", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 37, 9, "ROLE_DELETE", null, null, true, false, "حذف نقش", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 38, 9, "ROLE_ASSIGN_PERMISSION", null, null, true, false, "تخصیص دسترسی به نقش", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 39, 10, "USER_VIEW", null, null, true, false, "مشاهده کاربران", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 40, 10, "USER_CREATE", null, null, true, false, "ایجاد کاربر", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 41, 10, "USER_EDIT", null, null, true, false, "ویرایش کاربر", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 42, 10, "USER_DELETE", null, null, true, false, "حذف کاربر", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 43, 10, "USER_RESET_PASSWORD", null, null, true, false, "ریست رمز عبور", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 44, 10, "USER_ASSIGN_ROLE", null, null, true, false, "تخصیص نقش به کاربر", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoodImage_FoodItemId",
                schema: "Restaurant",
                table: "FoodImage",
                column: "FoodItemId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodItem_FoodCategoryId",
                schema: "Restaurant",
                table: "FoodItem",
                column: "FoodCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_FoodCategoryId",
                schema: "Authentication",
                table: "MenuItem",
                column: "FoodCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_ParentId",
                schema: "Authentication",
                table: "MenuItem",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermission_MenuId",
                schema: "Authentication",
                table: "MenuPermission",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuPermission_PermissionId",
                schema: "Authentication",
                table: "MenuPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRole_MenuItemId",
                schema: "Authentication",
                table: "MenuRole",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuRole_RoleId_MenuItemId",
                schema: "Authentication",
                table: "MenuRole",
                columns: new[] { "RoleId", "MenuItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_CategoryId",
                schema: "Authentication",
                table: "Permission",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: "Authentication",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                schema: "Authentication",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "Authentication",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                schema: "Authentication",
                table: "UserRole",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "ErrorLog",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "FoodImage",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "MenuPermission",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "MenuRole",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "FoodItem",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "MenuItem",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Authentication");

            migrationBuilder.DropTable(
                name: "FoodCategory",
                schema: "Restaurant");

            migrationBuilder.DropTable(
                name: "PermissionCategory",
                schema: "Authentication");
        }
    }
}
