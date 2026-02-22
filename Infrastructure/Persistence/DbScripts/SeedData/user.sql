USE [DrRoshaniDb];
GO

-- 1. تعریف متغیر JSON شامل آرایه کاربران
DECLARE @json_data NVARCHAR(MAX)
    = N'
[
{"id":"1","username":"admin","password_hash":"$2y$10$Yo8ZSrJT.oaSTN\/Tz3nQKuzHrsNJIZMEuiSSgghhQTChrcLlHetvG","full_name":"Administrator","role":"admin","is_active":"1","created_at":"2025-12-01 11:16:12","updated_at":"2025-12-14 09:51:29"},
{"id":"2","username":"ajami","password_hash":"$2y$10$IaNc5NGp\/JsFQV3nkw8aTO.5k6Te4X801dVWsBAj1VMuYWocrB8ya","full_name":"خانم عجمی","role":"staff","is_active":"1","created_at":"2025-12-01 11:36:19","updated_at":"2026-01-04 09:59:17"},
{"id":"3","username":"soltani","password_hash":"$2y$10$Vv6k.pXCtwtbo.CjCpPN0egUPtRWG.91WtbwUZvnbjKfCDTTBoEj2","full_name":"خانم سلطانی","role":"staff","is_active":"1","created_at":"2025-12-01 11:36:39","updated_at":"2026-01-04 09:58:32"},
{"id":"4","username":"admin2","password_hash":"$2y$10$qIZ\/9SQT3LnqOp6UaJI7K.TNf04n2xjEEfCqAKPUbaP32G9nEVMki","full_name":"مصطفی بابایی","role":"admin","is_active":"1","created_at":"2025-12-02 23:32:17","updated_at":"2025-12-03 14:34:51"},
{"id":"5","username":"Roshani","password_hash":"$2y$10$x4G043PNGfy8PaS1arOMduh7iY36BQxTAbEEPY.v0\/pB6\/eEvzE\/i","full_name":"دکتر نسرین روشنی","role":"admin","is_active":"1","created_at":"2025-12-11 15:59:12","updated_at":"2026-01-04 09:57:59"},
{"id":"6","username":"QEEG","password_hash":"$2y$10$n9Cy0987C\/cFl69aqQdJ2OWratZf7GjEW6xopjlOojtjKulM5ysDW","full_name":"QEEG","role":"visitor_operator","is_active":"1","created_at":"2025-12-19 21:41:00","updated_at":"2025-12-20 09:16:03"},
{"id":"7","username":"mosalmanzadeh","password_hash":"$2y$10$jHmttG5Gz9ZrDJY3S09QgurM81nRbkNVypN85GuZtrofj9eklkFF6","full_name":"خانم مسلمان زاده","role":"admin","is_active":"1","created_at":"2026-01-04 09:57:03","updated_at":"2026-01-04 09:57:42"},
{"id":"8","username":"sameti","password_hash":"$2y$10$Ax49E0GmDXOpQOKDktPgNOWXksk\/ElA0rQa4KWp\/.2dYUh1IhPNk2","full_name":"خانم صامتی","role":"admin","is_active":"1","created_at":"2026-01-04 10:01:15","updated_at":"2026-01-04 10:02:46"}
]';

-- 2. اجرای INSERT با استفاده از OPENJSON
INSERT INTO [Authentication].[User]
(
    [Id],
    [Email],
    [Username],
    [Password],
    [FirstName],
    [LastName],
    [ResetCode],
    [Avatar],
    [Mobile],
    [Phone],
    [LastLogin],
    [IsSystemAdmin],
    [RefreshToken],
    [RefreshTokenExpiry],
    [CreatedAt],
    [CreatedBy],
    [UpdatedAt],
    [UpdatedBy],
    [IsActive],
    [IsDeleted]
)
SELECT
    -- ID: GUID جدید تولید می‌شود (زیرا نگاشت مستقیم از MySQL ID به GUID ممکن نیست)
    NEWID() AS [Id],
                              -- Email: از username + @example.com استفاده می‌شود (زیرا ایمیل در JSON نیست)
    T.[username] + N'@example.com' AS [Email],
    T.[username] AS [Username],
                              -- Password: از password_hash برای رمز عبور استفاده می‌شود (این یک هش است، اگر این فرمت در SQL Server شما سازگار نیست، باید NULL بگذارید)
    'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=' AS [Password],
                              -- FirstName/LastName: از full_name تفکیک شده (تقریباً)
    CASE
        WHEN T.[role] = 'admin' THEN
            T.[full_name]
        ELSE
            NULL
    END AS [FirstName],       -- تفکیک نام کاربری از نام کامل دشوار است، در اینجا برای ادمین‌ها نام کامل را در FirstName می‌گذاریم
    T.[role] AS [LastName],   -- Role را در LastName قرار می‌دهیم برای حفظ ساختار
                              -- سایر فیلدهای اختیاری
    NULL AS [ResetCode],
    NULL AS [Avatar],
    NULL AS [Mobile],
    NULL AS [Phone],
    GETDATE() AS [LastLogin], -- زمان ورود فعلی
    CASE
        WHEN T.[role] = 'admin' THEN
            1
        ELSE
            0
    END AS [IsSystemAdmin],   -- اگر نقش ادمین است، ادمین سیستمی در نظر گرفته شود
    NULL AS [RefreshToken],
    NULL AS [RefreshTokenExpiry],
                              -- Timestamps: تبدیل تاریخ و زمان از فرمت MySQL به SQL Server
    CAST(T.[created_at] AS DATETIME2(7)) AS [CreatedAt],
    N'MySQL_Import' AS [CreatedBy],
    CAST(T.[updated_at] AS DATETIME2(7)) AS [UpdatedAt],
    N'MySQL_Import' AS [UpdatedBy],
                              -- IsActive: تبدیل "1" به 1 (true)
    CAST(T.[is_active] AS BIT) AS [IsActive],
    0 AS [IsDeleted]          -- فرض می‌کنیم حذف نشده‌اند
FROM
    OPENJSON(@json_data)
    WITH
    (
        id NVARCHAR(50) '$.id',
        username NVARCHAR(100) '$.username',
        password_hash NVARCHAR(255) '$.password_hash',
        full_name NVARCHAR(255) '$.full_name',
        role NVARCHAR(50) '$.role',
        is_active NVARCHAR(1) '$.is_active',
        created_at NVARCHAR(25) '$.created_at',
        updated_at NVARCHAR(25) '$.updated_at'
    ) AS T;

-- توجه: برای نام‌های فارسی مانند "خانم عجمی"، این کد ممکن است خطا دهد مگر اینکه Full_Name را به صورت دستی تفکیک کنید.
-- تفکیک "Administrator" به FirstName/LastName در این مثال به صورت ساده انجام شده است.
