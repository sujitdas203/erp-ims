-- ============================================================
-- SQL Script: HomeTasks and MockTests Admin Stored Procedures
-- Tables: dbo.HomeTasks_HT, dbo.HomeTaskSubmissions_HTS,
--         dbo.MockTests_MT, dbo.MockTestResults_MTR
-- ============================================================

-- Ensure Tables Exist
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'HomeTasks_HT')
BEGIN
    CREATE TABLE dbo.HomeTasks_HT (
        HT_Id            UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_HomeTasks_HT PRIMARY KEY CLUSTERED,
        HT_TenantId      UNIQUEIDENTIFIER NOT NULL,
        HT_BatchId       UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_HomeTasks_HT_BatchId REFERENCES dbo.Batches_BT(BT_Id),
        HT_SubjectId     UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_HomeTasks_HT_SubjectId REFERENCES dbo.Subjects_SB(SB_Id),
        HT_TeacherId     UNIQUEIDENTIFIER NULL,
        HT_Title         NVARCHAR(200) NOT NULL,
        HT_Description   NVARCHAR(MAX) NOT NULL,
        HT_AssignedDate  DATE NOT NULL,
        HT_DueDate       DATE NOT NULL,
        HT_AttachmentUrl NVARCHAR(500) NULL,
        HT_MaxMarks      NUMERIC(8,2) NULL,
        HT_Status        NVARCHAR(20) NOT NULL CONSTRAINT DF_HT_Status DEFAULT 'Active', -- Active, Closed
        HT_IsActive      BIT NOT NULL CONSTRAINT DF_HT_IsActive DEFAULT 1,
        HT_CreatedAt     DATETIME2 NOT NULL CONSTRAINT DF_HT_CreatedAt DEFAULT SYSUTCDATETIME(),
        HT_UpdatedAt     DATETIME2 NOT NULL CONSTRAINT DF_HT_UpdatedAt DEFAULT SYSUTCDATETIME()
    );

    CREATE NONCLUSTERED INDEX IX_HomeTasks_HT_BatchSubject ON dbo.HomeTasks_HT(HT_TenantId, HT_BatchId, HT_DueDate);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'HomeTaskSubmissions_HTS')
BEGIN
    CREATE TABLE dbo.HomeTaskSubmissions_HTS (
        HTS_Id             UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_HomeTaskSubmissions_HTS PRIMARY KEY CLUSTERED,
        HTS_HomeTaskId     UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_HTS_HomeTaskId REFERENCES dbo.HomeTasks_HT(HT_Id) ON DELETE CASCADE,
        HTS_StudentId      UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_HTS_StudentId REFERENCES dbo.Students_S(S_Id),
        HTS_SubmissionDate DATETIME2 NOT NULL CONSTRAINT DF_HTS_Date DEFAULT SYSUTCDATETIME(),
        HTS_Content        NVARCHAR(MAX) NULL,
        HTS_AttachmentUrl  NVARCHAR(500) NULL,
        HTS_MarksObtained  NUMERIC(8,2) NULL,
        HTS_TeacherRemarks NVARCHAR(500) NULL,
        HTS_Status         NVARCHAR(20) NOT NULL CONSTRAINT DF_HTS_Status DEFAULT 'Submitted', -- Submitted, Evaluated, Late
        HTS_IsActive       BIT NOT NULL CONSTRAINT DF_HTS_IsActive DEFAULT 1,
        CONSTRAINT UQ_HTS_Task_Student UNIQUE (HTS_HomeTaskId, HTS_StudentId)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'MockTests_MT')
BEGIN
    CREATE TABLE dbo.MockTests_MT (
        MT_Id              UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_MockTests_MT PRIMARY KEY CLUSTERED,
        MT_TenantId        UNIQUEIDENTIFIER NOT NULL,
        MT_BatchId         UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_MT_BatchId REFERENCES dbo.Batches_BT(BT_Id),
        MT_SubjectId       UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_MT_SubjectId REFERENCES dbo.Subjects_SB(SB_Id),
        MT_Title           NVARCHAR(200) NOT NULL,
        MT_Description     NVARCHAR(MAX) NULL,
        MT_TestDate        DATETIME2 NOT NULL,
        MT_DurationMinutes INT NOT NULL,
        MT_TotalMarks      NUMERIC(8,2) NOT NULL,
        MT_PassMarks       NUMERIC(8,2) NOT NULL,
        MT_Status          NVARCHAR(20) NOT NULL CONSTRAINT DF_MT_Status DEFAULT 'Scheduled', -- Scheduled, Ongoing, Completed, Cancelled
        MT_IsActive        BIT NOT NULL CONSTRAINT DF_MT_IsActive DEFAULT 1,
        MT_CreatedAt       DATETIME2 NOT NULL CONSTRAINT DF_MT_CreatedAt DEFAULT SYSUTCDATETIME(),
        MT_UpdatedAt       DATETIME2 NOT NULL CONSTRAINT DF_MT_UpdatedAt DEFAULT SYSUTCDATETIME()
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'MockTestResults_MTR')
BEGIN
    CREATE TABLE dbo.MockTestResults_MTR (
        MTR_Id          UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_MockTestResults_MTR PRIMARY KEY CLUSTERED,
        MTR_MockTestId  UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_MTR_MockTestId REFERENCES dbo.MockTests_MT(MT_Id) ON DELETE CASCADE,
        MTR_StudentId   UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_MTR_StudentId REFERENCES dbo.Students_S(S_Id),
        MTR_Score       NUMERIC(8,2) NOT NULL,
        MTR_Percentage  NUMERIC(5,2) NOT NULL,
        MTR_Grade       NVARCHAR(10) NULL,
        MTR_Status      NVARCHAR(20) NOT NULL, -- Pass, Fail, Absent
        MTR_IsActive    BIT NOT NULL CONSTRAINT DF_MTR_IsActive DEFAULT 1,
        MTR_CompletedAt DATETIME2 NOT NULL CONSTRAINT DF_MTR_CompletedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT UQ_MTR_Test_Student UNIQUE (MTR_MockTestId, MTR_StudentId)
    );
END
GO

-- ============================================================
-- 1. Home Tasks Stored Procedures
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.SP_HomeTasks_GetPaged
    @TenantId   UNIQUEIDENTIFIER,
    @BatchId    UNIQUEIDENTIFIER = NULL,
    @SubjectId  UNIQUEIDENTIFIER = NULL,
    @Status     NVARCHAR(20) = NULL,
    @Search     NVARCHAR(200) = NULL,
    @PageNumber INT = 1,
    @PageSize   INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 OR @PageSize > 100 SET @PageSize = 10;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @SearchPattern NVARCHAR(202) = '%' + ISNULL(@Search, '') + '%';

    -- Result Set 1: Total Count
    SELECT COUNT(1) AS TotalCount
    FROM dbo.HomeTasks_HT ht
    LEFT JOIN dbo.Batches_BT bt ON ht.HT_BatchId = bt.BT_Id
    LEFT JOIN dbo.Subjects_SB sb ON ht.HT_SubjectId = sb.SB_Id
    WHERE ht.HT_TenantId = @TenantId
      AND (@BatchId IS NULL OR ht.HT_BatchId = @BatchId)
      AND (@SubjectId IS NULL OR ht.HT_SubjectId = @SubjectId)
      AND (@Status IS NULL OR @Status = '' OR ht.HT_Status = @Status)
      AND (@Search IS NULL OR @Search = '' OR ht.HT_Title LIKE @SearchPattern OR sb.SB_Name LIKE @SearchPattern OR bt.BT_Name LIKE @SearchPattern);

    -- Result Set 2: Paged Items
    SELECT 
        ht.HT_Id,
        ht.HT_TenantId,
        ht.HT_BatchId,
        bt.BT_Name AS BatchName,
        ht.HT_SubjectId,
        sb.SB_Name AS SubjectName,
        ht.HT_TeacherId,
        ISNULL(t.T_FirstName + ' ' + ISNULL(t.T_LastName, ''), 'Teacher') AS TeacherName,
        ht.HT_Title,
        ht.HT_Description,
        ht.HT_AssignedDate,
        ht.HT_DueDate,
        ht.HT_AttachmentUrl,
        ht.HT_MaxMarks,
        ht.HT_Status,
        ht.HT_IsActive,
        ht.HT_CreatedAt,
        ht.HT_UpdatedAt,
        (SELECT COUNT(1) FROM dbo.HomeTaskSubmissions_HTS hts WHERE hts.HTS_HomeTaskId = ht.HT_Id) AS SubmissionCount
    FROM dbo.HomeTasks_HT ht
    LEFT JOIN dbo.Batches_BT bt ON ht.HT_BatchId = bt.BT_Id
    LEFT JOIN dbo.Subjects_SB sb ON ht.HT_SubjectId = sb.SB_Id
    LEFT JOIN dbo.Teachers_T t ON ht.HT_TeacherId = t.T_Id
    WHERE ht.HT_TenantId = @TenantId
      AND (@BatchId IS NULL OR ht.HT_BatchId = @BatchId)
      AND (@SubjectId IS NULL OR ht.HT_SubjectId = @SubjectId)
      AND (@Status IS NULL OR @Status = '' OR ht.HT_Status = @Status)
      AND (@Search IS NULL OR @Search = '' OR ht.HT_Title LIKE @SearchPattern OR sb.SB_Name LIKE @SearchPattern OR bt.BT_Name LIKE @SearchPattern)
    ORDER BY ht.HT_CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_HomeTasks_GetById
    @HT_Id       UNIQUEIDENTIFIER,
    @HT_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Result Set 1: HomeTask details
    SELECT 
        ht.HT_Id,
        ht.HT_TenantId,
        ht.HT_BatchId,
        bt.BT_Name AS BatchName,
        ht.HT_SubjectId,
        sb.SB_Name AS SubjectName,
        ht.HT_TeacherId,
        ISNULL(t.T_FirstName + ' ' + ISNULL(t.T_LastName, ''), 'Teacher') AS TeacherName,
        ht.HT_Title,
        ht.HT_Description,
        ht.HT_AssignedDate,
        ht.HT_DueDate,
        ht.HT_AttachmentUrl,
        ht.HT_MaxMarks,
        ht.HT_Status,
        ht.HT_IsActive,
        ht.HT_CreatedAt,
        ht.HT_UpdatedAt
    FROM dbo.HomeTasks_HT ht
    LEFT JOIN dbo.Batches_BT bt ON ht.HT_BatchId = bt.BT_Id
    LEFT JOIN dbo.Subjects_SB sb ON ht.HT_SubjectId = sb.SB_Id
    LEFT JOIN dbo.Teachers_T t ON ht.HT_TeacherId = t.T_Id
    WHERE ht.HT_Id = @HT_Id AND ht.HT_TenantId = @HT_TenantId;

    -- Result Set 2: Submissions (if any)
    SELECT 
        hts.HTS_Id,
        hts.HTS_HomeTaskId,
        hts.HTS_StudentId,
        s.S_StudentCode,
        s.S_AdmissionNumber,
        ISNULL(s.S_FirstName + ' ' + ISNULL(s.S_LastName, ''), 'Student') AS StudentName,
        hts.HTS_SubmissionDate,
        hts.HTS_Content,
        hts.HTS_AttachmentUrl,
        hts.HTS_MarksObtained,
        hts.HTS_TeacherRemarks,
        hts.HTS_Status
    FROM dbo.HomeTaskSubmissions_HTS hts
    INNER JOIN dbo.Students_S s ON hts.HTS_StudentId = s.S_Id
    WHERE hts.HTS_HomeTaskId = @HT_Id
    ORDER BY hts.HTS_SubmissionDate DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_HomeTasks_Create
    @HT_Id            UNIQUEIDENTIFIER,
    @HT_TenantId      UNIQUEIDENTIFIER,
    @HT_BatchId       UNIQUEIDENTIFIER,
    @HT_SubjectId     UNIQUEIDENTIFIER,
    @HT_TeacherId     UNIQUEIDENTIFIER = NULL,
    @HT_Title         NVARCHAR(200),
    @HT_Description   NVARCHAR(MAX),
    @HT_AssignedDate  DATE,
    @HT_DueDate       DATE,
    @HT_AttachmentUrl NVARCHAR(500) = NULL,
    @HT_MaxMarks      NUMERIC(8,2) = NULL,
    @HT_Status        NVARCHAR(20) = 'Active'
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.HomeTasks_HT (
        HT_Id, HT_TenantId, HT_BatchId, HT_SubjectId, HT_TeacherId,
        HT_Title, HT_Description, HT_AssignedDate, HT_DueDate,
        HT_AttachmentUrl, HT_MaxMarks, HT_Status, HT_IsActive,
        HT_CreatedAt, HT_UpdatedAt
    )
    VALUES (
        @HT_Id, @HT_TenantId, @HT_BatchId, @HT_SubjectId, @HT_TeacherId,
        @HT_Title, @HT_Description, @HT_AssignedDate, @HT_DueDate,
        @HT_AttachmentUrl, @HT_MaxMarks, @HT_Status, 1,
        SYSUTCDATETIME(), SYSUTCDATETIME()
    );

    SELECT @HT_Id AS HT_Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_HomeTasks_Update
    @HT_Id            UNIQUEIDENTIFIER,
    @HT_TenantId      UNIQUEIDENTIFIER,
    @HT_BatchId       UNIQUEIDENTIFIER,
    @HT_SubjectId     UNIQUEIDENTIFIER,
    @HT_TeacherId     UNIQUEIDENTIFIER = NULL,
    @HT_Title         NVARCHAR(200),
    @HT_Description   NVARCHAR(MAX),
    @HT_AssignedDate  DATE,
    @HT_DueDate       DATE,
    @HT_AttachmentUrl NVARCHAR(500) = NULL,
    @HT_MaxMarks      NUMERIC(8,2) = NULL,
    @HT_Status        NVARCHAR(20) = 'Active'
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.HomeTasks_HT
    SET
        HT_BatchId       = @HT_BatchId,
        HT_SubjectId     = @HT_SubjectId,
        HT_TeacherId     = @HT_TeacherId,
        HT_Title         = @HT_Title,
        HT_Description   = @HT_Description,
        HT_AssignedDate  = @HT_AssignedDate,
        HT_DueDate       = @HT_DueDate,
        HT_AttachmentUrl = @HT_AttachmentUrl,
        HT_MaxMarks      = @HT_MaxMarks,
        HT_Status        = @HT_Status,
        HT_UpdatedAt     = SYSUTCDATETIME()
    WHERE HT_Id = @HT_Id AND HT_TenantId = @HT_TenantId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_HomeTasks_Delete
    @HT_Id       UNIQUEIDENTIFIER,
    @HT_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.HomeTasks_HT
    WHERE HT_Id = @HT_Id AND HT_TenantId = @HT_TenantId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_HomeTasks_ToggleActive
    @HT_Id       UNIQUEIDENTIFIER,
    @HT_TenantId UNIQUEIDENTIFIER,
    @IsActive    BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.HomeTasks_HT
    SET HT_IsActive = @IsActive,
        HT_UpdatedAt = SYSUTCDATETIME()
    WHERE HT_Id = @HT_Id AND HT_TenantId = @HT_TenantId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- ============================================================
-- 2. Mock Tests Stored Procedures
-- ============================================================

CREATE OR ALTER PROCEDURE dbo.SP_MockTests_GetPaged
    @TenantId   UNIQUEIDENTIFIER,
    @BatchId    UNIQUEIDENTIFIER = NULL,
    @SubjectId  UNIQUEIDENTIFIER = NULL,
    @Status     NVARCHAR(20) = NULL,
    @Search     NVARCHAR(200) = NULL,
    @PageNumber INT = 1,
    @PageSize   INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize < 1 OR @PageSize > 100 SET @PageSize = 10;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @SearchPattern NVARCHAR(202) = '%' + ISNULL(@Search, '') + '%';

    -- Result Set 1: Total Count
    SELECT COUNT(1) AS TotalCount
    FROM dbo.MockTests_MT mt
    LEFT JOIN dbo.Batches_BT bt ON mt.MT_BatchId = bt.BT_Id
    LEFT JOIN dbo.Subjects_SB sb ON mt.MT_SubjectId = sb.SB_Id
    WHERE mt.MT_TenantId = @TenantId
      AND (@BatchId IS NULL OR mt.MT_BatchId = @BatchId)
      AND (@SubjectId IS NULL OR mt.MT_SubjectId = @SubjectId)
      AND (@Status IS NULL OR @Status = '' OR mt.MT_Status = @Status)
      AND (@Search IS NULL OR @Search = '' OR mt.MT_Title LIKE @SearchPattern OR sb.SB_Name LIKE @SearchPattern OR bt.BT_Name LIKE @SearchPattern);

    -- Result Set 2: Paged Items
    SELECT 
        mt.MT_Id,
        mt.MT_TenantId,
        mt.MT_BatchId,
        bt.BT_Name AS BatchName,
        mt.MT_SubjectId,
        sb.SB_Name AS SubjectName,
        mt.MT_Title,
        mt.MT_Description,
        mt.MT_TestDate,
        mt.MT_DurationMinutes,
        mt.MT_TotalMarks,
        mt.MT_PassMarks,
        mt.MT_Status,
        mt.MT_IsActive,
        mt.MT_CreatedAt,
        (SELECT COUNT(1) FROM dbo.MockTestResults_MTR mtr WHERE mtr.MTR_MockTestId = mt.MT_Id) AS ResultCount,
        (SELECT AVG(mtr.MTR_Score) FROM dbo.MockTestResults_MTR mtr WHERE mtr.MTR_MockTestId = mt.MT_Id) AS AvgScore
    FROM dbo.MockTests_MT mt
    LEFT JOIN dbo.Batches_BT bt ON mt.MT_BatchId = bt.BT_Id
    LEFT JOIN dbo.Subjects_SB sb ON mt.MT_SubjectId = sb.SB_Id
    WHERE mt.MT_TenantId = @TenantId
      AND (@BatchId IS NULL OR mt.MT_BatchId = @BatchId)
      AND (@SubjectId IS NULL OR mt.MT_SubjectId = @SubjectId)
      AND (@Status IS NULL OR @Status = '' OR mt.MT_Status = @Status)
      AND (@Search IS NULL OR @Search = '' OR mt.MT_Title LIKE @SearchPattern OR sb.SB_Name LIKE @SearchPattern OR bt.BT_Name LIKE @SearchPattern)
    ORDER BY mt.MT_TestDate DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_MockTests_GetById
    @MT_Id       UNIQUEIDENTIFIER,
    @MT_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Result Set 1: MockTest details
    SELECT 
        mt.MT_Id,
        mt.MT_TenantId,
        mt.MT_BatchId,
        bt.BT_Name AS BatchName,
        mt.MT_SubjectId,
        sb.SB_Name AS SubjectName,
        mt.MT_Title,
        mt.MT_Description,
        mt.MT_TestDate,
        mt.MT_DurationMinutes,
        mt.MT_TotalMarks,
        mt.MT_PassMarks,
        mt.MT_Status,
        mt.MT_IsActive,
        mt.MT_CreatedAt
    FROM dbo.MockTests_MT mt
    LEFT JOIN dbo.Batches_BT bt ON mt.MT_BatchId = bt.BT_Id
    LEFT JOIN dbo.Subjects_SB sb ON mt.MT_SubjectId = sb.SB_Id
    WHERE mt.MT_Id = @MT_Id AND mt.MT_TenantId = @MT_TenantId;

    -- Result Set 2: Student Results (if any)
    SELECT 
        mtr.MTR_Id,
        mtr.MTR_MockTestId,
        mtr.MTR_StudentId,
        s.S_StudentCode,
        s.S_AdmissionNumber,
        ISNULL(s.S_FirstName + ' ' + ISNULL(s.S_LastName, ''), 'Student') AS StudentName,
        mtr.MTR_Score,
        mtr.MTR_Percentage,
        mtr.MTR_Grade,
        mtr.MTR_Status,
        mtr.MTR_CompletedAt
    FROM dbo.MockTestResults_MTR mtr
    INNER JOIN dbo.Students_S s ON mtr.MTR_StudentId = s.S_Id
    WHERE mtr.MTR_MockTestId = @MT_Id
    ORDER BY mtr.MTR_Score DESC;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_MockTests_Create
    @MT_Id              UNIQUEIDENTIFIER,
    @MT_TenantId        UNIQUEIDENTIFIER,
    @MT_BatchId         UNIQUEIDENTIFIER,
    @MT_SubjectId       UNIQUEIDENTIFIER,
    @MT_Title           NVARCHAR(200),
    @MT_Description     NVARCHAR(MAX) = NULL,
    @MT_TestDate        DATETIME2,
    @MT_DurationMinutes INT,
    @MT_TotalMarks      NUMERIC(8,2),
    @MT_PassMarks       NUMERIC(8,2),
    @MT_Status          NVARCHAR(20) = 'Scheduled'
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.MockTests_MT (
        MT_Id, MT_TenantId, MT_BatchId, MT_SubjectId,
        MT_Title, MT_Description, MT_TestDate, MT_DurationMinutes,
        MT_TotalMarks, MT_PassMarks, MT_Status, MT_IsActive,
        MT_CreatedAt
    )
    VALUES (
        @MT_Id, @MT_TenantId, @MT_BatchId, @MT_SubjectId,
        @MT_Title, @MT_Description, @MT_TestDate, @MT_DurationMinutes,
        @MT_TotalMarks, @MT_PassMarks, @MT_Status, 1,
        SYSUTCDATETIME()
    );

    SELECT @MT_Id AS MT_Id;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_MockTests_Update
    @MT_Id              UNIQUEIDENTIFIER,
    @MT_TenantId        UNIQUEIDENTIFIER,
    @MT_BatchId         UNIQUEIDENTIFIER,
    @MT_SubjectId       UNIQUEIDENTIFIER,
    @MT_Title           NVARCHAR(200),
    @MT_Description     NVARCHAR(MAX) = NULL,
    @MT_TestDate        DATETIME2,
    @MT_DurationMinutes INT,
    @MT_TotalMarks      NUMERIC(8,2),
    @MT_PassMarks       NUMERIC(8,2),
    @MT_Status          NVARCHAR(20) = 'Scheduled'
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.MockTests_MT
    SET
        MT_BatchId         = @MT_BatchId,
        MT_SubjectId       = @MT_SubjectId,
        MT_Title           = @MT_Title,
        MT_Description     = @MT_Description,
        MT_TestDate        = @MT_TestDate,
        MT_DurationMinutes = @MT_DurationMinutes,
        MT_TotalMarks      = @MT_TotalMarks,
        MT_PassMarks       = @MT_PassMarks,
        MT_Status          = @MT_Status
    WHERE MT_Id = @MT_Id AND MT_TenantId = @MT_TenantId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_MockTests_Delete
    @MT_Id       UNIQUEIDENTIFIER,
    @MT_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM dbo.MockTests_MT
    WHERE MT_Id = @MT_Id AND MT_TenantId = @MT_TenantId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

CREATE OR ALTER PROCEDURE dbo.SP_MockTests_ToggleActive
    @MT_Id       UNIQUEIDENTIFIER,
    @MT_TenantId UNIQUEIDENTIFIER,
    @IsActive    BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.MockTests_MT
    SET MT_IsActive = @IsActive
    WHERE MT_Id = @MT_Id AND MT_TenantId = @MT_TenantId;

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO
