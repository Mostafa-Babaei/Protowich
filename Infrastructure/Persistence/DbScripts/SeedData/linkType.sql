USE [DrRoshaniDb];
GO

-- اصلاح شده: جایگزینی NULL برای UpdatedAt با مقدار CreatedAt

INSERT INTO [Authentication].[LinkType]
(
    [Code],
    [Title],
    [Description],
    [CreatedAt],
    [CreatedBy],
    [UpdatedAt],
    [UpdatedBy],
    [IsActive],
    [IsDeleted]
)
VALUES
(   'brain_map',                      -- <Code, nvarchar(50),>
    N'نقشه مغزی',                     -- <Title, nvarchar(100),>
    N'لینک مربوط به نقشه مغزی بیمار', -- <Description, nvarchar(255),>
    '2025-12-23 23:22:36',            -- <CreatedAt, datetime2(7),>
    N'System',                        -- <CreatedBy, nvarchar(max),> (مقدار فرضی)
    '2025-12-23 23:22:36',            -- <UpdatedAt, datetime2(7),> (اصلاح شده: تکرار CreatedAt)
    NULL,                             -- <UpdatedBy, nvarchar(max),>
    CAST(1 AS BIT),                   -- <IsActive, bit,> (تبدیل '1' به بیت)
    CAST(0 AS BIT)                    -- <IsDeleted, bit,> (مقدار پیش‌فرض 0 برای حذف نشده)
    );
GO

INSERT INTO [Authentication].[LinkType]
(
    [Code],
    [Title],
    [Description],
    [CreatedAt],
    [CreatedBy],
    [UpdatedAt],
    [UpdatedBy],
    [IsActive],
    [IsDeleted]
)
VALUES
(   'psych_test',                       -- <Code, nvarchar(50),>
    N'تست روانشناسی',                   -- <Title, nvarchar(100),>
    N'لینک مربوط به تست‌های روانشناسی', -- <Description, nvarchar(255),>
    '2025-12-23 23:22:36',              -- <CreatedAt, datetime2(7),>
    N'System',                          -- <CreatedBy, nvarchar(max),> (مقدار فرضی)
    '2025-12-23 23:22:36',              -- <UpdatedAt, datetime2(7),> (اصلاح شده: تکرار CreatedAt)
    NULL,                               -- <UpdatedBy, nvarchar(max),>
    CAST(1 AS BIT),                     -- <IsActive, bit,> (تبدیل '1' به بیت)
    CAST(0 AS BIT)                      -- <IsDeleted, bit,> (مقدار پیش‌فرض 0 برای حذف نشده)
    );
GO
