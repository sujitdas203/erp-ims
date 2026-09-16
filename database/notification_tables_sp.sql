-- ============================================================================
-- NOTIFICATION & SMTP CONFIGURATION TABLES AND STORED PROCEDURES
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NotificationSettings')
BEGIN
    CREATE TABLE NotificationSettings (
        NS_TenantId UNIQUEIDENTIFIER PRIMARY KEY,
        NS_InAppEnabled BIT NOT NULL DEFAULT 1,
        NS_EmailEnabled BIT NOT NULL DEFAULT 0,
        NS_SmtpHost NVARCHAR(200) NULL,
        NS_SmtpPort INT NOT NULL DEFAULT 587,
        NS_SmtpUsername NVARCHAR(200) NULL,
        NS_SmtpPassword NVARCHAR(500) NULL,
        NS_FromEmail NVARCHAR(200) NULL,
        NS_FromName NVARCHAR(200) NULL,
        NS_EnableSsl BIT NOT NULL DEFAULT 1,
        NS_EventConfigJson NVARCHAR(MAX) NULL,
        NS_UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserNotifications')
BEGIN
    CREATE TABLE UserNotifications (
        UN_Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        UN_TenantId UNIQUEIDENTIFIER NOT NULL,
        UN_UserId NVARCHAR(100) NULL,
        UN_TargetRole NVARCHAR(100) NULL DEFAULT 'TENANT_ADMIN',
        UN_Title NVARCHAR(250) NOT NULL,
        UN_Message NVARCHAR(1000) NOT NULL,
        UN_EventType NVARCHAR(100) NOT NULL,
        UN_LinkUrl NVARCHAR(500) NULL,
        UN_IsRead BIT NOT NULL DEFAULT 0,
        UN_CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE INDEX IX_UserNotifications_Tenant_User ON UserNotifications(UN_TenantId, UN_UserId, UN_IsRead, UN_CreatedAt DESC);
END
GO
