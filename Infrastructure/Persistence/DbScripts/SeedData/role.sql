USE [DrRoshaniDb];
GO

-- 1. تعریف متغیر JSON شامل آرایه نقش‌ها (Roles)
DECLARE @json_roles NVARCHAR(MAX)
    = N'
[
{"id":"1","name":"مدیر سیستم","code":"admin","description":"دسترسی کامل به سامانه","is_active":"1","created_at":"2025-12-23 23:22:36","updated_at":"2025-12-23 23:22:36"},
{"id":"2","name":"پذیرش","code":"staff","description":"دسترسی پذیرش مراجعین","is_active":"1","created_at":"2025-12-23 23:22:36","updated_at":"2025-12-23 23:22:36"},
{"id":"3","name":"نقشه مغزی","code":"visitor_operator","description":"مدیریت لینک نقشه مغزی","is_active":"1","created_at":"2025-12-23 23:22:36","updated_at":"2025-12-23 23:22:36"},
{"id":"4","name":"تست روانشناسی","code":"test_operator","description":"مدیریت لینک تست","is_active":"1","created_at":"2025-12-23 23:22:36","updated_at":"2025-12-23 23:22:36"}
]';

-- 2. اجرای INSERT با استفاده از OPENJSON
INSERT INTO [Authentication].[Role]
(
    [Name],
    [DisplayName],
    [Description],
    [CreatedAt],
    [CreatedBy],
    [UpdatedAt],
    [UpdatedBy],
    [IsActive],
    [IsDeleted]
)
SELECT
    -- Name (نام کامل فارسی)
    T.[name] AS [Name],
                                    -- DisplayName (کد نقش - استفاده از کد به عنوان نمایشگر نام)
    T.[code] AS [DisplayName],
                                    -- Description
    T.[description] AS [Description],
                                    -- تاریخ‌ها (تبدیل فرمت تاریخ)
    CAST(T.[created_at] AS DATETIME2(7)) AS [CreatedAt],
    N'MySQL_Import' AS [CreatedBy], -- نام وارد کننده
    CAST(T.[updated_at] AS DATETIME2(7)) AS [UpdatedAt],
    N'MySQL_Import' AS [UpdatedBy], -- نام وارد کننده
                                    -- IsActive: تبدیل "1" به 1 (true)
    CAST(T.[is_active] AS BIT) AS [IsActive],
    0 AS [IsDeleted]                -- فرض می‌کنیم حذف نشده‌اند
FROM
    OPENJSON(@json_roles)
    WITH
    (
        id NVARCHAR(50) '$.id',
        name NVARCHAR(255) '$.name',
        code NVARCHAR(100) '$.code',
        description NVARCHAR(MAX) '$.description',
        is_active NVARCHAR(1) '$.is_active',
        created_at NVARCHAR(25) '$.created_at',
        updated_at NVARCHAR(25) '$.updated_at'
    ) AS T;
