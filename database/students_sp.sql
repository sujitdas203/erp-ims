-- ============================================================
-- USP_Students_S
-- Lookup SP for Student entity used by MasterConfigRegistry
-- ============================================================
CREATE PROCEDURE [dbo].[USP_Students_S]
    @Action NVARCHAR(20),
    @S_Id UNIQUEIDENTIFIER = NULL,
    @S_TenantId UNIQUEIDENTIFIER = NULL,
    @S_FirstName NVARCHAR(100) = NULL,
    @S_LastName NVARCHAR(100) = NULL,
    @S_StudentCode NVARCHAR(50) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetAll'
        BEGIN
            SELECT S_Id, S_TenantId, S_FirstName, S_LastName, S_StudentCode, S_Email, S_Phone, S_Status
            FROM Students_S
            WHERE S_DeletedAt IS NULL
            ORDER BY S_FirstName, S_LastName;
        END

        IF @Action = 'GetById'
        BEGIN
            SELECT * FROM Students_S WHERE S_Id = @S_Id;
        END

        IF @Action = 'ExistsByField'
        BEGIN
            DECLARE @Sql NVARCHAR(MAX);
            SET @Sql = 'SELECT COUNT(*) FROM Students_S WHERE ' + QUOTENAME(@ColumnName) + ' = @Value'
                      + CASE WHEN @ExcludeId IS NOT NULL THEN ' AND S_Id <> @ExcludeId' ELSE '' END;
            EXEC sp_executesql @Sql, N'@Value NVARCHAR(MAX), @ExcludeId UNIQUEIDENTIFIER', @Value, @ExcludeId;
        END

        IF @Action = 'Insert'
        BEGIN
            SET @NewId = ISNULL(@NewId, NEWID());
            INSERT INTO Students_S (S_Id, S_TenantId, S_FirstName, S_LastName, S_StudentCode, S_AdmissionNumber, S_Status, S_CreatedAt, S_UpdatedAt)
            VALUES (@NewId, @TenantId, @S_FirstName, @S_LastName, @S_StudentCode, @S_StudentCode, 'Active', SYSUTCDATETIME(), SYSUTCDATETIME());
        END

        IF @Action = 'Update'
        BEGIN
            UPDATE Students_S
            SET S_FirstName = @S_FirstName, S_LastName = @S_LastName, S_StudentCode = @S_StudentCode,
                S_UpdatedAt = SYSUTCDATETIME()
            WHERE S_Id = @S_Id;
        END

        IF @Action = 'Delete'
        BEGIN
            DELETE FROM Students_S WHERE S_Id = @S_Id;
        END

        IF @Action = 'Deactivate'
        BEGIN
            UPDATE Students_S SET S_Status = 'inactive', S_UpdatedAt = SYSUTCDATETIME() WHERE S_Id = @S_Id;
        END
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrSev INT = ERROR_SEVERITY();
        DECLARE @ErrState INT = ERROR_STATE();
        RAISERROR(@ErrMsg, @ErrSev, @ErrState);
    END CATCH
END
GO

-- ============================================================
-- BatchStudents_BS Table Schema
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'BatchStudents_BS')
BEGIN
    CREATE TABLE [dbo].[BatchStudents_BS](
        [BS_BatchId] [uniqueidentifier] NOT NULL,
        [BS_StudentId] [uniqueidentifier] NOT NULL,
        [BS_JoinedAt] [date] NOT NULL,
        [BS_LeftAt] [date] NULL,
        CONSTRAINT [PK_BatchStudents_BS] PRIMARY KEY CLUSTERED 
        (
            [BS_BatchId] ASC,
            [BS_StudentId] ASC
        )
    );

    ALTER TABLE [dbo].[BatchStudents_BS] WITH CHECK ADD CONSTRAINT [FK_BatchStudents_BS_BatchId] FOREIGN KEY([BS_BatchId])
    REFERENCES [dbo].[Batches_BT] ([BT_Id])
    ON DELETE CASCADE;

    ALTER TABLE [dbo].[BatchStudents_BS] CHECK CONSTRAINT [FK_BatchStudents_BS_BatchId];

    ALTER TABLE [dbo].[BatchStudents_BS] WITH CHECK ADD CONSTRAINT [FK_BatchStudents_BS_StudentId] FOREIGN KEY([BS_StudentId])
    REFERENCES [dbo].[Students_S] ([S_Id]);

    ALTER TABLE [dbo].[BatchStudents_BS] CHECK CONSTRAINT [FK_BatchStudents_BS_StudentId];

    ALTER TABLE [dbo].[BatchStudents_BS] WITH CHECK ADD CONSTRAINT [CK_BatchStudents_BS_DateRange] CHECK (([BS_LeftAt] IS NULL OR [BS_LeftAt] >= [BS_JoinedAt]));

    ALTER TABLE [dbo].[BatchStudents_BS] CHECK CONSTRAINT [CK_BatchStudents_BS_DateRange];
END
GO

-- ============================================================
-- SP_Students_Create
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Students_Create]
    @S_Id               UNIQUEIDENTIFIER,
    @S_TenantId         UNIQUEIDENTIFIER,
    @S_BranchId         UNIQUEIDENTIFIER,
    @S_UserId           UNIQUEIDENTIFIER = NULL,
    @S_StudentCode      NVARCHAR(50),
    @S_AdmissionNumber  NVARCHAR(50) = NULL,   -- optional: blank => auto-generated
    @S_FirstName        NVARCHAR(100),
    @S_MiddleName       NVARCHAR(100) = NULL,
    @S_LastName         NVARCHAR(100),
    @S_DateOfBirth      DATE = NULL,
    @S_Gender           NVARCHAR(20) = NULL,
    @S_Email            NVARCHAR(255) = NULL,
    @S_Phone            NVARCHAR(30) = NULL,
    @S_AdmissionDate    DATE = NULL,
    @S_Status           NVARCHAR(20),
    @S_ClassId          UNIQUEIDENTIFIER = NULL,
    @S_SectionId        UNIQUEIDENTIFIER = NULL,
    @S_BloodGroup       NVARCHAR(10) = NULL,
    @S_AddressLine1     NVARCHAR(255) = NULL,
    @S_AddressLine2     NVARCHAR(255) = NULL,
    @S_City             NVARCHAR(100) = NULL,
    @S_State            NVARCHAR(100) = NULL,
    @S_PostalCode       NVARCHAR(20) = NULL,
    @S_Country          NVARCHAR(100) = NULL,
    @S_BatchId          UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF (@S_AdmissionNumber IS NULL OR LTRIM(RTRIM(@S_AdmissionNumber)) = '')
    BEGIN
        DECLARE @Year INT = YEAR(SYSUTCDATETIME());
        DECLARE @NextNum INT;
        DECLARE @Counter TABLE (LastNumber INT);

        -- Atomic upsert-and-increment: safe under concurrent inserts
        MERGE dbo.AdmissionNumberCounters_ANC AS target
        USING (SELECT @S_TenantId AS TenantId, @Year AS Year) AS src
            ON target.ANC_TenantId = src.TenantId AND target.ANC_Year = src.Year
        WHEN MATCHED THEN
            UPDATE SET ANC_LastNumber = ANC_LastNumber + 1
        WHEN NOT MATCHED THEN
            INSERT (ANC_TenantId, ANC_Year, ANC_LastNumber) VALUES (src.TenantId, src.Year, 1)
        OUTPUT inserted.ANC_LastNumber INTO @Counter;

        SELECT @NextNum = LastNumber FROM @Counter;

        SET @S_AdmissionNumber = 'ADM-' + CAST(@Year AS NVARCHAR(4)) + '-' + RIGHT('0000' + CAST(@NextNum AS NVARCHAR(10)), 4);
    END

    INSERT INTO dbo.Students_S
        (S_Id, S_TenantId, S_BranchId, S_UserId, S_StudentCode, S_AdmissionNumber,
         S_FirstName, S_MiddleName, S_LastName, S_DateOfBirth, S_Gender, S_Email,
         S_Phone, S_AdmissionDate, S_Status, S_ClassId, S_SectionId, S_BloodGroup,
         S_AddressLine1, S_AddressLine2, S_City, S_State, S_PostalCode, S_Country,
         S_CreatedAt, S_UpdatedAt)
    VALUES
        (@S_Id, @S_TenantId, @S_BranchId, @S_UserId, @S_StudentCode, @S_AdmissionNumber,
         @S_FirstName, @S_MiddleName, @S_LastName, @S_DateOfBirth, @S_Gender, @S_Email,
         @S_Phone, @S_AdmissionDate, @S_Status, @S_ClassId, @S_SectionId, @S_BloodGroup,
         @S_AddressLine1, @S_AddressLine2, @S_City, @S_State, @S_PostalCode, @S_Country,
         SYSUTCDATETIME(), SYSUTCDATETIME());

    -- Add to BatchStudents_BS if Batch is selected
    IF (@S_BatchId IS NOT NULL)
    BEGIN
        DECLARE @JoinedDate DATE = ISNULL(@S_AdmissionDate, CAST(SYSUTCDATETIME() AS DATE));
        
        IF EXISTS (SELECT 1 FROM dbo.BatchStudents_BS WHERE BS_BatchId = @S_BatchId AND BS_StudentId = @S_Id)
        BEGIN
            UPDATE dbo.BatchStudents_BS
            SET BS_JoinedAt = @JoinedDate,
                BS_LeftAt = NULL
            WHERE BS_BatchId = @S_BatchId AND BS_StudentId = @S_Id;
        END
        ELSE
        BEGIN
            INSERT INTO dbo.BatchStudents_BS (BS_BatchId, BS_StudentId, BS_JoinedAt, BS_LeftAt)
            VALUES (@S_BatchId, @S_Id, @JoinedDate, NULL);
        END
    END

    -- Return the (possibly generated) admission number so the app layer can show it
    SELECT @S_AdmissionNumber AS S_AdmissionNumber;
END
GO

-- ============================================================
-- SP_Students_Update
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Students_Update]
    @S_Id               UNIQUEIDENTIFIER,
    @S_TenantId         UNIQUEIDENTIFIER,
    @S_BranchId         UNIQUEIDENTIFIER,
    @S_UserId           UNIQUEIDENTIFIER = NULL,
    @S_StudentCode      NVARCHAR(50),
    @S_AdmissionNumber  NVARCHAR(50),
    @S_FirstName        NVARCHAR(100),
    @S_MiddleName       NVARCHAR(100) = NULL,
    @S_LastName         NVARCHAR(100),
    @S_DateOfBirth      DATE = NULL,
    @S_Gender           NVARCHAR(20) = NULL,
    @S_Email            NVARCHAR(255) = NULL,
    @S_Phone            NVARCHAR(30) = NULL,
    @S_AdmissionDate    DATE = NULL,
    @S_Status           NVARCHAR(20),
    @S_ClassId          UNIQUEIDENTIFIER = NULL,
    @S_SectionId        UNIQUEIDENTIFIER = NULL,
    @S_BloodGroup       NVARCHAR(10) = NULL,
    @S_AddressLine1     NVARCHAR(255) = NULL,
    @S_AddressLine2     NVARCHAR(255) = NULL,
    @S_City             NVARCHAR(100) = NULL,
    @S_State            NVARCHAR(100) = NULL,
    @S_PostalCode       NVARCHAR(20) = NULL,
    @S_Country          NVARCHAR(100) = NULL,
    @S_BatchId          UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Students_S
    SET
        S_BranchId        = @S_BranchId,
        S_UserId          = @S_UserId,
        S_StudentCode     = @S_StudentCode,
        S_AdmissionNumber = @S_AdmissionNumber,
        S_FirstName       = @S_FirstName,
        S_MiddleName      = @S_MiddleName,
        S_LastName        = @S_LastName,
        S_DateOfBirth     = @S_DateOfBirth,
        S_Gender          = @S_Gender,
        S_Email           = @S_Email,
        S_Phone           = @S_Phone,
        S_AdmissionDate   = @S_AdmissionDate,
        S_Status          = @S_Status,
        S_ClassId         = @S_ClassId,
        S_SectionId       = @S_SectionId,
        S_BloodGroup      = @S_BloodGroup,
        S_AddressLine1    = @S_AddressLine1,
        S_AddressLine2    = @S_AddressLine2,
        S_City            = @S_City,
        S_State           = @S_State,
        S_PostalCode      = @S_PostalCode,
        S_Country         = @S_Country,
        S_UpdatedAt       = SYSUTCDATETIME()
    WHERE S_Id = @S_Id
      AND S_TenantId = @S_TenantId
      AND S_DeletedAt IS NULL;

    -- Manage Batch assignment in BatchStudents_BS
    IF (@S_BatchId IS NOT NULL)
    BEGIN
        DECLARE @JoinedDate DATE = ISNULL(@S_AdmissionDate, CAST(SYSUTCDATETIME() AS DATE));
        
        -- Mark left for other batches
        UPDATE dbo.BatchStudents_BS
        SET BS_LeftAt = CAST(SYSUTCDATETIME() AS DATE)
        WHERE BS_StudentId = @S_Id
          AND BS_BatchId <> @S_BatchId
          AND BS_LeftAt IS NULL;

        -- Upsert for current batch
        IF EXISTS (SELECT 1 FROM dbo.BatchStudents_BS WHERE BS_BatchId = @S_BatchId AND BS_StudentId = @S_Id)
        BEGIN
            UPDATE dbo.BatchStudents_BS
            SET BS_LeftAt = NULL
            WHERE BS_BatchId = @S_BatchId AND BS_StudentId = @S_Id;
        END
        ELSE
        BEGIN
            INSERT INTO dbo.BatchStudents_BS (BS_BatchId, BS_StudentId, BS_JoinedAt, BS_LeftAt)
            VALUES (@S_BatchId, @S_Id, @JoinedDate, NULL);
        END
    END

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO

-- ============================================================
-- SP_Students_GetById
-- ============================================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Students_GetById]
    @S_Id       UNIQUEIDENTIFIER,
    @S_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT s.*,
           (SELECT TOP 1 bs.BS_BatchId 
            FROM dbo.BatchStudents_BS bs 
            WHERE bs.BS_StudentId = s.S_Id 
              AND bs.BS_LeftAt IS NULL) AS S_BatchId
    FROM dbo.Students_S s
    WHERE s.S_Id = @S_Id
      AND s.S_TenantId = @S_TenantId
      AND s.S_DeletedAt IS NULL;
END
GO
