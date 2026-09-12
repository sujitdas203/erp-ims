USE [IMS]
GO
/****** Object:  Table [dbo].[AcademicYears_AY]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AcademicYears_AY](
	[AY_Id] [uniqueidentifier] NOT NULL,
	[AY_TenantId] [uniqueidentifier] NOT NULL,
	[AY_Name] [nvarchar](100) NOT NULL,
	[AY_Code] [nvarchar](50) NOT NULL,
	[AY_StartDate] [date] NOT NULL,
	[AY_EndDate] [date] NOT NULL,
	[AY_IsCurrent] [bit] NOT NULL,
	[AY_CreatedAt] [datetime2](7) NOT NULL,
	[AY_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AcademicYears_AY] PRIMARY KEY CLUSTERED 
(
	[AY_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityLogs_ACL]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityLogs_ACL](
	[ACL_Id] [uniqueidentifier] NOT NULL,
	[ACL_TenantId] [uniqueidentifier] NOT NULL,
	[ACL_UserId] [uniqueidentifier] NULL,
	[ACL_ActivityType] [nvarchar](100) NOT NULL,
	[ACL_Description] [nvarchar](max) NOT NULL,
	[ACL_Metadata] [nvarchar](max) NULL,
	[ACL_CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ActivityLogs_ACL] PRIMARY KEY CLUSTERED 
(
	[ACL_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdmissionApplicationDocuments_AAD]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdmissionApplicationDocuments_AAD](
	[AAD_Id] [uniqueidentifier] NOT NULL,
	[AAD_ApplicationId] [uniqueidentifier] NOT NULL,
	[AAD_DocumentTypeId] [uniqueidentifier] NOT NULL,
	[AAD_DocumentId] [uniqueidentifier] NOT NULL,
	[AAD_IsVerified] [bit] NOT NULL,
	[AAD_VerifiedBy] [uniqueidentifier] NULL,
	[AAD_VerifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_AdmissionApplicationDocuments_AAD] PRIMARY KEY CLUSTERED 
(
	[AAD_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdmissionApplications_AA]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdmissionApplications_AA](
	[AA_Id] [uniqueidentifier] NOT NULL,
	[AA_TenantId] [uniqueidentifier] NOT NULL,
	[AA_BranchId] [uniqueidentifier] NOT NULL,
	[AA_ApplicationNumber] [nvarchar](50) NOT NULL,
	[AA_FirstName] [nvarchar](100) NOT NULL,
	[AA_LastName] [nvarchar](100) NOT NULL,
	[AA_DateOfBirth] [date] NULL,
	[AA_Gender] [nvarchar](20) NULL,
	[AA_Email] [nvarchar](255) NULL,
	[AA_Phone] [nvarchar](30) NOT NULL,
	[AA_CourseId] [uniqueidentifier] NULL,
	[AA_AcademicYearId] [uniqueidentifier] NOT NULL,
	[AA_Status] [nvarchar](20) NOT NULL,
	[AA_SubmittedAt] [datetime2](7) NULL,
	[AA_ReviewedAt] [datetime2](7) NULL,
	[AA_ReviewedBy] [uniqueidentifier] NULL,
	[AA_Notes] [nvarchar](max) NULL,
	[AA_CreatedAt] [datetime2](7) NOT NULL,
	[AA_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AdmissionApplications_AA] PRIMARY KEY CLUSTERED 
(
	[AA_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdmissionNumberCounters_ANC]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdmissionNumberCounters_ANC](
	[ANC_TenantId] [uniqueidentifier] NOT NULL,
	[ANC_Year] [int] NOT NULL,
	[ANC_LastNumber] [int] NOT NULL,
 CONSTRAINT [PK_AdmissionNumberCounters_ANC] PRIMARY KEY CLUSTERED 
(
	[ANC_TenantId] ASC,
	[ANC_Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Announcements_ANN]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Announcements_ANN](
	[ANN_Id] [uniqueidentifier] NOT NULL,
	[ANN_TenantId] [uniqueidentifier] NOT NULL,
	[ANN_BranchId] [uniqueidentifier] NULL,
	[ANN_Title] [nvarchar](200) NOT NULL,
	[ANN_Content] [nvarchar](max) NOT NULL,
	[ANN_PublishedAt] [datetime2](7) NULL,
	[ANN_ExpiresAt] [datetime2](7) NULL,
	[ANN_CreatedBy] [uniqueidentifier] NOT NULL,
	[ANN_CreatedAt] [datetime2](7) NOT NULL,
	[ANN_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Announcements_ANN] PRIMARY KEY CLUSTERED 
(
	[ANN_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttendanceRecords_AR]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttendanceRecords_AR](
	[AR_Id] [uniqueidentifier] NOT NULL,
	[AR_AttendanceSessionId] [uniqueidentifier] NOT NULL,
	[AR_StudentId] [uniqueidentifier] NOT NULL,
	[AR_Status] [nvarchar](20) NOT NULL,
	[AR_Remarks] [nvarchar](max) NULL,
	[AR_CreatedAt] [datetime2](7) NOT NULL,
	[AR_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AttendanceRecords_AR] PRIMARY KEY CLUSTERED 
(
	[AR_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AttendanceSessions_AS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttendanceSessions_AS](
	[AS_Id] [uniqueidentifier] NOT NULL,
	[AS_TenantId] [uniqueidentifier] NOT NULL,
	[AS_BranchId] [uniqueidentifier] NOT NULL,
	[AS_BatchId] [uniqueidentifier] NOT NULL,
	[AS_SubjectId] [uniqueidentifier] NULL,
	[AS_StaffId] [uniqueidentifier] NULL,
	[AS_AttendanceDate] [date] NOT NULL,
	[AS_StartTime] [time](7) NULL,
	[AS_EndTime] [time](7) NULL,
	[AS_Remarks] [nvarchar](max) NULL,
	[AS_CreatedAt] [datetime2](7) NOT NULL,
	[AS_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AttendanceSessions_AS] PRIMARY KEY CLUSTERED 
(
	[AS_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogs_AL]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs_AL](
	[AL_Id] [uniqueidentifier] NOT NULL,
	[AL_TenantId] [uniqueidentifier] NOT NULL,
	[AL_UserId] [uniqueidentifier] NULL,
	[AL_Action] [nvarchar](50) NOT NULL,
	[AL_EntityType] [nvarchar](100) NOT NULL,
	[AL_EntityId] [uniqueidentifier] NOT NULL,
	[AL_OldValues] [nvarchar](max) NULL,
	[AL_NewValues] [nvarchar](max) NULL,
	[AL_IpAddress] [nvarchar](45) NULL,
	[AL_UserAgent] [nvarchar](max) NULL,
	[AL_CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_AuditLogs_AL] PRIMARY KEY CLUSTERED 
(
	[AL_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankMaster_BM]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankMaster_BM](
	[BM_Id] [int] IDENTITY(1,1) NOT NULL,
	[BM_TenantId] [uniqueidentifier] NULL,
	[BM_BankName] [nvarchar](200) NOT NULL,
	[BM_AccountNo] [nvarchar](50) NULL,
	[BM_IFSCCode] [nvarchar](20) NULL,
	[BM_BranchName] [nvarchar](200) NULL,
	[BM_IsActive] [bit] NOT NULL,
	[BM_CreatedAt] [datetime2](7) NOT NULL,
	[BM_CreatedBy] [uniqueidentifier] NULL,
	[BM_UpdatedAt] [datetime2](7) NOT NULL,
	[BM_UpdatedBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_BankMaster_BM] PRIMARY KEY CLUSTERED 
(
	[BM_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Batches_BT]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Batches_BT](
	[BT_Id] [uniqueidentifier] NOT NULL,
	[BT_TenantId] [uniqueidentifier] NOT NULL,
	[BT_BranchId] [uniqueidentifier] NOT NULL,
	[BT_CourseId] [uniqueidentifier] NOT NULL,
	[BT_AcademicYearId] [uniqueidentifier] NOT NULL,
	[BT_Name] [nvarchar](150) NOT NULL,
	[BT_Code] [nvarchar](50) NOT NULL,
	[BT_StartDate] [date] NOT NULL,
	[BT_EndDate] [date] NULL,
	[BT_Capacity] [int] NULL,
	[BT_Status] [nvarchar](20) NOT NULL,
	[BT_CreatedAt] [datetime2](7) NOT NULL,
	[BT_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Batches_BT] PRIMARY KEY CLUSTERED 
(
	[BT_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BatchStudents_BS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BatchStudents_BS](
	[BS_BatchId] [uniqueidentifier] NOT NULL,
	[BS_StudentId] [uniqueidentifier] NOT NULL,
	[BS_JoinedAt] [date] NOT NULL,
	[BS_LeftAt] [date] NULL,
 CONSTRAINT [PK_BatchStudents_BS] PRIMARY KEY CLUSTERED 
(
	[BS_BatchId] ASC,
	[BS_StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Branches_B]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Branches_B](
	[B_Id] [uniqueidentifier] NOT NULL,
	[B_TenantId] [uniqueidentifier] NOT NULL,
	[B_Name] [nvarchar](200) NOT NULL,
	[B_Code] [nvarchar](50) NOT NULL,
	[B_Email] [nvarchar](255) NULL,
	[B_Phone] [nvarchar](30) NULL,
	[B_AddressLine1] [nvarchar](255) NULL,
	[B_AddressLine2] [nvarchar](255) NULL,
	[B_City] [nvarchar](100) NULL,
	[B_State] [nvarchar](100) NULL,
	[B_PostalCode] [nvarchar](20) NULL,
	[B_CountryCode] [char](2) NULL,
	[B_Status] [nvarchar](20) NOT NULL,
	[B_CreatedAt] [datetime2](7) NOT NULL,
	[B_UpdatedAt] [datetime2](7) NOT NULL,
	[B_DeletedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Branches_B] PRIMARY KEY CLUSTERED 
(
	[B_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classrooms_CR]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classrooms_CR](
	[CR_Id] [uniqueidentifier] NOT NULL,
	[CR_TenantId] [uniqueidentifier] NOT NULL,
	[CR_BranchId] [uniqueidentifier] NOT NULL,
	[CR_Name] [nvarchar](100) NOT NULL,
	[CR_Code] [nvarchar](50) NOT NULL,
	[CR_Capacity] [int] NOT NULL,
	[CR_Location] [nvarchar](255) NULL,
	[CR_CreatedAt] [datetime2](7) NOT NULL,
	[CR_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Classrooms_CR] PRIMARY KEY CLUSTERED 
(
	[CR_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Courses_C]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Courses_C](
	[C_Id] [uniqueidentifier] NOT NULL,
	[C_TenantId] [uniqueidentifier] NOT NULL,
	[C_ProgramId] [uniqueidentifier] NULL,
	[C_Name] [nvarchar](200) NOT NULL,
	[C_Code] [nvarchar](50) NOT NULL,
	[C_Description] [nvarchar](max) NULL,
	[C_Status] [nvarchar](20) NOT NULL,
	[C_CreatedAt] [datetime2](7) NOT NULL,
	[C_UpdatedAt] [datetime2](7) NOT NULL,
	[C_DeletedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Courses_C] PRIMARY KEY CLUSTERED 
(
	[C_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CourseSubjects_CS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseSubjects_CS](
	[CS_CourseId] [uniqueidentifier] NOT NULL,
	[CS_SubjectId] [uniqueidentifier] NOT NULL,
	[CS_SequenceNo] [int] NOT NULL,
	[CS_IsMandatory] [bit] NOT NULL,
	[CS_MaxMarks] [numeric](8, 2) NULL,
	[CS_PassMarks] [numeric](8, 2) NULL,
 CONSTRAINT [PK_CourseSubjects_CS] PRIMARY KEY CLUSTERED 
(
	[CS_CourseId] ASC,
	[CS_SubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomFields_CF]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomFields_CF](
	[CF_Id] [uniqueidentifier] NOT NULL,
	[CF_TenantId] [uniqueidentifier] NOT NULL,
	[CF_EntityType] [nvarchar](50) NOT NULL,
	[CF_FieldKey] [nvarchar](100) NOT NULL,
	[CF_Label] [nvarchar](150) NOT NULL,
	[CF_DataType] [nvarchar](30) NOT NULL,
	[CF_IsRequired] [bit] NOT NULL,
	[CF_IsActive] [bit] NOT NULL,
	[CF_DisplayOrder] [int] NOT NULL,
	[CF_Options] [nvarchar](max) NULL,
	[CF_CreatedAt] [datetime2](7) NOT NULL,
	[CF_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_CustomFields_CF] PRIMARY KEY CLUSTERED 
(
	[CF_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomFieldValues_CFV]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomFieldValues_CFV](
	[CFV_Id] [uniqueidentifier] NOT NULL,
	[CFV_CustomFieldId] [uniqueidentifier] NOT NULL,
	[CFV_EntityId] [uniqueidentifier] NOT NULL,
	[CFV_Value] [nvarchar](max) NOT NULL,
	[CFV_CreatedAt] [datetime2](7) NOT NULL,
	[CFV_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_CustomFieldValues_CFV] PRIMARY KEY CLUSTERED 
(
	[CFV_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments_D]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments_D](
	[D_Id] [uniqueidentifier] NOT NULL,
	[D_TenantId] [uniqueidentifier] NOT NULL,
	[D_BranchId] [uniqueidentifier] NOT NULL,
	[D_Name] [nvarchar](150) NOT NULL,
	[D_Code] [nvarchar](50) NOT NULL,
	[D_Description] [nvarchar](max) NULL,
	[D_CreatedAt] [datetime2](7) NOT NULL,
	[D_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Departments_D] PRIMARY KEY CLUSTERED 
(
	[D_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Designations_DS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Designations_DS](
	[DS_Id] [uniqueidentifier] NOT NULL,
	[DS_TenantId] [uniqueidentifier] NOT NULL,
	[DS_Name] [nvarchar](100) NOT NULL,
	[DS_Code] [nvarchar](50) NOT NULL,
	[DS_CreatedAt] [datetime2](7) NOT NULL,
	[DS_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Designations_DS] PRIMARY KEY CLUSTERED 
(
	[DS_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discounts_DIS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discounts_DIS](
	[DIS_Id] [uniqueidentifier] NOT NULL,
	[DIS_TenantId] [uniqueidentifier] NOT NULL,
	[DIS_Name] [nvarchar](100) NOT NULL,
	[DIS_Code] [nvarchar](50) NOT NULL,
	[DIS_DiscountType] [nvarchar](20) NOT NULL,
	[DIS_Value] [numeric](12, 2) NOT NULL,
	[DIS_Description] [nvarchar](max) NULL,
	[DIS_IsActive] [bit] NOT NULL,
	[DIS_CreatedAt] [datetime2](7) NOT NULL,
	[DIS_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Discounts_DIS] PRIMARY KEY CLUSTERED 
(
	[DIS_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Documents_DOC]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Documents_DOC](
	[DOC_Id] [uniqueidentifier] NOT NULL,
	[DOC_TenantId] [uniqueidentifier] NOT NULL,
	[DOC_FileName] [nvarchar](255) NOT NULL,
	[DOC_StorageKey] [nvarchar](max) NOT NULL,
	[DOC_MimeType] [nvarchar](100) NOT NULL,
	[DOC_FileSize] [bigint] NOT NULL,
	[DOC_Checksum] [nvarchar](128) NULL,
	[DOC_UploadedBy] [uniqueidentifier] NOT NULL,
	[DOC_CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Documents_DOC] PRIMARY KEY CLUSTERED 
(
	[DOC_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentTypes_DT]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentTypes_DT](
	[DT_Id] [uniqueidentifier] NOT NULL,
	[DT_TenantId] [uniqueidentifier] NOT NULL,
	[DT_Name] [nvarchar](100) NOT NULL,
	[DT_Code] [nvarchar](50) NOT NULL,
	[DT_EntityType] [nvarchar](30) NOT NULL,
	[DT_IsRequired] [bit] NOT NULL,
	[DT_CreatedAt] [datetime2](7) NOT NULL,
	[DT_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DocumentTypes_DT] PRIMARY KEY CLUSTERED 
(
	[DT_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollments_E]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollments_E](
	[E_Id] [uniqueidentifier] NOT NULL,
	[E_TenantId] [uniqueidentifier] NOT NULL,
	[E_StudentId] [uniqueidentifier] NOT NULL,
	[E_AcademicYearId] [uniqueidentifier] NOT NULL,
	[E_CourseId] [uniqueidentifier] NOT NULL,
	[E_BatchId] [uniqueidentifier] NOT NULL,
	[E_EnrollmentNumber] [nvarchar](50) NOT NULL,
	[E_EnrollmentDate] [date] NOT NULL,
	[E_Status] [nvarchar](20) NOT NULL,
	[E_CompletionDate] [date] NULL,
	[E_CreatedAt] [datetime2](7) NOT NULL,
	[E_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Enrollments_E] PRIMARY KEY CLUSTERED 
(
	[E_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityDocuments_ED]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityDocuments_ED](
	[ED_Id] [uniqueidentifier] NOT NULL,
	[ED_TenantId] [uniqueidentifier] NOT NULL,
	[ED_DocumentId] [uniqueidentifier] NOT NULL,
	[ED_DocumentTypeId] [uniqueidentifier] NOT NULL,
	[ED_EntityType] [nvarchar](30) NOT NULL,
	[ED_EntityId] [uniqueidentifier] NOT NULL,
	[ED_CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_EntityDocuments_ED] PRIMARY KEY CLUSTERED 
(
	[ED_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exams_EX]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exams_EX](
	[EX_Id] [uniqueidentifier] NOT NULL,
	[EX_TenantId] [uniqueidentifier] NOT NULL,
	[EX_AcademicYearId] [uniqueidentifier] NOT NULL,
	[EX_CourseId] [uniqueidentifier] NOT NULL,
	[EX_BatchId] [uniqueidentifier] NOT NULL,
	[EX_ExamTypeId] [uniqueidentifier] NOT NULL,
	[EX_Name] [nvarchar](150) NOT NULL,
	[EX_Code] [nvarchar](50) NOT NULL,
	[EX_StartDate] [date] NOT NULL,
	[EX_EndDate] [date] NOT NULL,
	[EX_Status] [nvarchar](20) NOT NULL,
	[EX_CreatedAt] [datetime2](7) NOT NULL,
	[EX_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Exams_EX] PRIMARY KEY CLUSTERED 
(
	[EX_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamSchedules_ESC]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamSchedules_ESC](
	[ESC_Id] [uniqueidentifier] NOT NULL,
	[ESC_ExamSubjectId] [uniqueidentifier] NOT NULL,
	[ESC_ExamDate] [date] NOT NULL,
	[ESC_StartTime] [time](7) NOT NULL,
	[ESC_EndTime] [time](7) NOT NULL,
	[ESC_ClassroomId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ExamSchedules_ESC] PRIMARY KEY CLUSTERED 
(
	[ESC_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamSubjects_ES]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamSubjects_ES](
	[ES_Id] [uniqueidentifier] NOT NULL,
	[ES_ExamId] [uniqueidentifier] NOT NULL,
	[ES_SubjectId] [uniqueidentifier] NOT NULL,
	[ES_MaxMarks] [numeric](8, 2) NOT NULL,
	[ES_PassMarks] [numeric](8, 2) NOT NULL,
	[ES_Weightage] [numeric](5, 2) NULL,
 CONSTRAINT [PK_ExamSubjects_ES] PRIMARY KEY CLUSTERED 
(
	[ES_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExamTypes_ET]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExamTypes_ET](
	[ET_Id] [uniqueidentifier] NOT NULL,
	[ET_TenantId] [uniqueidentifier] NOT NULL,
	[ET_Name] [nvarchar](100) NOT NULL,
	[ET_Code] [nvarchar](50) NOT NULL,
	[ET_Description] [nvarchar](max) NULL,
	[ET_WeightagePercentage] [decimal](5, 2) NULL,
 CONSTRAINT [PK_ExamTypes_ET] PRIMARY KEY CLUSTERED 
(
	[ET_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExpenseCategories_EC]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExpenseCategories_EC](
	[EC_Id] [uniqueidentifier] NOT NULL,
	[EC_TenantId] [uniqueidentifier] NOT NULL,
	[EC_Name] [nvarchar](100) NOT NULL,
	[EC_Code] [nvarchar](50) NOT NULL,
	[EC_Description] [nvarchar](max) NULL,
	[EC_CreatedAt] [datetime2](7) NOT NULL,
	[EC_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ExpenseCategories_EC] PRIMARY KEY CLUSTERED 
(
	[EC_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenses_EXP]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses_EXP](
	[EXP_Id] [uniqueidentifier] NOT NULL,
	[EXP_TenantId] [uniqueidentifier] NOT NULL,
	[EXP_BranchId] [uniqueidentifier] NOT NULL,
	[EXP_ExpenseCategoryId] [uniqueidentifier] NOT NULL,
	[EXP_VendorId] [uniqueidentifier] NULL,
	[EXP_ExpenseNumber] [nvarchar](50) NOT NULL,
	[EXP_ExpenseDate] [date] NOT NULL,
	[EXP_Amount] [numeric](12, 2) NOT NULL,
	[EXP_Description] [nvarchar](max) NULL,
	[EXP_PaymentMethodId] [uniqueidentifier] NULL,
	[EXP_CreatedBy] [uniqueidentifier] NOT NULL,
	[EXP_CreatedAt] [datetime2](7) NOT NULL,
	[EXP_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Expenses_EXP] PRIMARY KEY CLUSTERED 
(
	[EXP_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeeCategories_FC]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeCategories_FC](
	[FC_Id] [uniqueidentifier] NOT NULL,
	[FC_TenantId] [uniqueidentifier] NOT NULL,
	[FC_Name] [nvarchar](100) NOT NULL,
	[FC_Code] [nvarchar](50) NOT NULL,
	[FC_Description] [nvarchar](max) NULL,
	[FC_IsRefundable] [bit] NOT NULL,
	[FC_CreatedAt] [datetime2](7) NOT NULL,
	[FC_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_FeeCategories_FC] PRIMARY KEY CLUSTERED 
(
	[FC_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeeInvoiceItems_FII]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeInvoiceItems_FII](
	[FII_Id] [uniqueidentifier] NOT NULL,
	[FII_InvoiceId] [uniqueidentifier] NOT NULL,
	[FII_FeeCategoryId] [uniqueidentifier] NOT NULL,
	[FII_Description] [nvarchar](255) NOT NULL,
	[FII_Quantity] [numeric](10, 2) NOT NULL,
	[FII_UnitAmount] [numeric](12, 2) NOT NULL,
	[FII_DiscountAmount] [numeric](12, 2) NOT NULL,
	[FII_TaxAmount] [numeric](12, 2) NOT NULL,
	[FII_TotalAmount] [numeric](12, 2) NOT NULL,
 CONSTRAINT [PK_FeeInvoiceItems_FII] PRIMARY KEY CLUSTERED 
(
	[FII_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeeInvoices_FI]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeInvoices_FI](
	[FI_Id] [uniqueidentifier] NOT NULL,
	[FI_TenantId] [uniqueidentifier] NOT NULL,
	[FI_StudentId] [uniqueidentifier] NOT NULL,
	[FI_InvoiceNumber] [nvarchar](50) NOT NULL,
	[FI_InvoiceDate] [date] NOT NULL,
	[FI_DueDate] [date] NOT NULL,
	[FI_Subtotal] [numeric](12, 2) NOT NULL,
	[FI_DiscountAmount] [numeric](12, 2) NOT NULL,
	[FI_TaxAmount] [numeric](12, 2) NOT NULL,
	[FI_TotalAmount] [numeric](12, 2) NOT NULL,
	[FI_PaidAmount] [numeric](12, 2) NOT NULL,
	[FI_BalanceAmount] [numeric](12, 2) NOT NULL,
	[FI_Status] [nvarchar](20) NOT NULL,
	[FI_Notes] [nvarchar](max) NULL,
	[FI_CreatedAt] [datetime2](7) NOT NULL,
	[FI_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_FeeInvoices_FI] PRIMARY KEY CLUSTERED 
(
	[FI_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeeStructureItems_FSI]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeStructureItems_FSI](
	[FSI_Id] [uniqueidentifier] NOT NULL,
	[FSI_FeeStructureId] [uniqueidentifier] NOT NULL,
	[FSI_FeeCategoryId] [uniqueidentifier] NOT NULL,
	[FSI_Amount] [numeric](12, 2) NOT NULL,
	[FSI_DueDays] [int] NULL,
	[FSI_IsMandatory] [bit] NOT NULL,
 CONSTRAINT [PK_FeeStructureItems_FSI] PRIMARY KEY CLUSTERED 
(
	[FSI_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeeStructures_FS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeStructures_FS](
	[FS_Id] [uniqueidentifier] NOT NULL,
	[FS_TenantId] [uniqueidentifier] NOT NULL,
	[FS_Name] [nvarchar](150) NOT NULL,
	[FS_Code] [nvarchar](50) NOT NULL,
	[FS_CourseId] [uniqueidentifier] NULL,
	[FS_BatchId] [uniqueidentifier] NULL,
	[FS_AcademicYearId] [uniqueidentifier] NOT NULL,
	[FS_Description] [nvarchar](max) NULL,
	[FS_IsActive] [bit] NOT NULL,
	[FS_CreatedAt] [datetime2](7) NOT NULL,
	[FS_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_FeeStructures_FS] PRIMARY KEY CLUSTERED 
(
	[FS_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GradeScaleItems_GSI]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GradeScaleItems_GSI](
	[GSI_Id] [uniqueidentifier] NOT NULL,
	[GSI_GradeScaleId] [uniqueidentifier] NOT NULL,
	[GSI_Grade] [nvarchar](20) NOT NULL,
	[GSI_MinPercentage] [numeric](5, 2) NOT NULL,
	[GSI_MaxPercentage] [numeric](5, 2) NOT NULL,
	[GSI_GradePoint] [numeric](5, 2) NULL,
	[GSI_Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_GradeScaleItems_GSI] PRIMARY KEY CLUSTERED 
(
	[GSI_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GradeScales_GS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GradeScales_GS](
	[GS_Id] [uniqueidentifier] NOT NULL,
	[GS_TenantId] [uniqueidentifier] NOT NULL,
	[GS_Name] [nvarchar](100) NOT NULL,
	[GS_Code] [nvarchar](50) NOT NULL,
	[GS_Description] [nvarchar](max) NULL,
	[GS_IsDefault] [bit] NOT NULL,
 CONSTRAINT [PK_GradeScales_GS] PRIMARY KEY CLUSTERED 
(
	[GS_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Guardians_G]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Guardians_G](
	[G_Id] [uniqueidentifier] NOT NULL,
	[G_TenantId] [uniqueidentifier] NOT NULL,
	[G_FirstName] [nvarchar](100) NOT NULL,
	[G_LastName] [nvarchar](100) NOT NULL,
	[G_Email] [nvarchar](255) NULL,
	[G_Phone] [nvarchar](30) NOT NULL,
	[G_Occupation] [nvarchar](150) NULL,
	[G_Address] [nvarchar](max) NULL,
	[G_CreatedAt] [datetime2](7) NOT NULL,
	[G_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Guardians_G] PRIMARY KEY CLUSTERED 
(
	[G_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Marks_M]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marks_M](
	[M_Id] [uniqueidentifier] NOT NULL,
	[M_ExamSubjectId] [uniqueidentifier] NOT NULL,
	[M_StudentId] [uniqueidentifier] NOT NULL,
	[M_MarksObtained] [numeric](8, 2) NOT NULL,
	[M_Percentage] [numeric](6, 2) NULL,
	[M_GradeScaleItemId] [uniqueidentifier] NULL,
	[M_Remarks] [nvarchar](max) NULL,
	[M_CreatedAt] [datetime2](7) NOT NULL,
	[M_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Marks_M] PRIMARY KEY CLUSTERED 
(
	[M_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications_N]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications_N](
	[N_Id] [uniqueidentifier] NOT NULL,
	[N_TenantId] [uniqueidentifier] NOT NULL,
	[N_UserId] [uniqueidentifier] NOT NULL,
	[N_Channel] [nvarchar](20) NOT NULL,
	[N_Title] [nvarchar](255) NOT NULL,
	[N_Message] [nvarchar](max) NOT NULL,
	[N_IsRead] [bit] NOT NULL,
	[N_ReadAt] [datetime2](7) NULL,
	[N_SentAt] [datetime2](7) NULL,
	[N_CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Notifications_N] PRIMARY KEY CLUSTERED 
(
	[N_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationTemplates_NT]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationTemplates_NT](
	[NT_Id] [uniqueidentifier] NOT NULL,
	[NT_TenantId] [uniqueidentifier] NOT NULL,
	[NT_Name] [nvarchar](150) NOT NULL,
	[NT_EventKey] [nvarchar](100) NOT NULL,
	[NT_Channel] [nvarchar](20) NOT NULL,
	[NT_Subject] [nvarchar](255) NULL,
	[NT_BodyTemplate] [nvarchar](max) NOT NULL,
	[NT_IsActive] [bit] NOT NULL,
	[NT_CreatedAt] [datetime2](7) NOT NULL,
	[NT_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_NotificationTemplates_NT] PRIMARY KEY CLUSTERED 
(
	[NT_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organizations_O]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizations_O](
	[O_Id] [uniqueidentifier] NOT NULL,
	[O_Name] [nvarchar](200) NOT NULL,
	[O_Code] [nvarchar](50) NOT NULL,
	[O_LegalName] [nvarchar](250) NULL,
	[O_Email] [nvarchar](255) NULL,
	[O_Phone] [nvarchar](30) NULL,
	[O_Website] [nvarchar](255) NULL,
	[O_LogoUrl] [nvarchar](max) NULL,
	[O_Timezone] [nvarchar](50) NOT NULL,
	[O_CurrencyCode] [char](3) NOT NULL,
	[O_CountryCode] [char](2) NULL,
	[O_Status] [nvarchar](20) NOT NULL,
	[O_CreatedAt] [datetime2](7) NOT NULL,
	[O_UpdatedAt] [datetime2](7) NOT NULL,
	[O_DeletedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Organizations_O] PRIMARY KEY CLUSTERED 
(
	[O_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationSettings_OS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationSettings_OS](
	[OS_Id] [uniqueidentifier] NOT NULL,
	[OS_TenantId] [uniqueidentifier] NOT NULL,
	[OS_SettingKey] [nvarchar](100) NOT NULL,
	[OS_SettingValue] [nvarchar](max) NULL,
	[OS_CreatedAt] [datetime2](7) NOT NULL,
	[OS_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_OrganizationSettings_OS] PRIMARY KEY CLUSTERED 
(
	[OS_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentAllocations_PA]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentAllocations_PA](
	[PA_Id] [uniqueidentifier] NOT NULL,
	[PA_PaymentId] [uniqueidentifier] NOT NULL,
	[PA_InvoiceId] [uniqueidentifier] NOT NULL,
	[PA_AllocatedAmount] [numeric](12, 2) NOT NULL,
	[PA_CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_PaymentAllocations_PA] PRIMARY KEY CLUSTERED 
(
	[PA_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethods_PM]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethods_PM](
	[PM_Id] [uniqueidentifier] NOT NULL,
	[PM_TenantId] [uniqueidentifier] NOT NULL,
	[PM_Name] [nvarchar](100) NOT NULL,
	[PM_Type] [nvarchar](30) NOT NULL,
	[PM_IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_PaymentMethods_PM] PRIMARY KEY CLUSTERED 
(
	[PM_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments_PAY]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments_PAY](
	[PAY_Id] [uniqueidentifier] NOT NULL,
	[PAY_TenantId] [uniqueidentifier] NOT NULL,
	[PAY_StudentId] [uniqueidentifier] NOT NULL,
	[PAY_PaymentNumber] [nvarchar](50) NOT NULL,
	[PAY_PaymentDate] [datetime2](7) NOT NULL,
	[PAY_Amount] [numeric](12, 2) NOT NULL,
	[PAY_PaymentMethodId] [uniqueidentifier] NOT NULL,
	[PAY_Status] [nvarchar](20) NOT NULL,
	[PAY_TransactionReference] [nvarchar](150) NULL,
	[PAY_GatewayReference] [nvarchar](150) NULL,
	[PAY_Notes] [nvarchar](max) NULL,
	[PAY_CreatedBy] [uniqueidentifier] NOT NULL,
	[PAY_CreatedAt] [datetime2](7) NOT NULL,
	[PAY_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Payments_PAY] PRIMARY KEY CLUSTERED 
(
	[PAY_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Programs_P]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Programs_P](
	[P_Id] [uniqueidentifier] NOT NULL,
	[P_TenantId] [uniqueidentifier] NOT NULL,
	[P_Name] [nvarchar](200) NOT NULL,
	[P_Code] [nvarchar](50) NOT NULL,
	[P_Description] [nvarchar](max) NULL,
	[P_DurationValue] [int] NULL,
	[P_DurationUnit] [nvarchar](20) NULL,
	[P_Status] [nvarchar](20) NOT NULL,
	[P_CreatedAt] [datetime2](7) NOT NULL,
	[P_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Programs_P] PRIMARY KEY CLUSTERED 
(
	[P_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Refunds_RF]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Refunds_RF](
	[RF_Id] [uniqueidentifier] NOT NULL,
	[RF_TenantId] [uniqueidentifier] NOT NULL,
	[RF_PaymentId] [uniqueidentifier] NOT NULL,
	[RF_StudentId] [uniqueidentifier] NOT NULL,
	[RF_RefundNumber] [nvarchar](50) NOT NULL,
	[RF_Amount] [numeric](12, 2) NOT NULL,
	[RF_RefundDate] [datetime2](7) NOT NULL,
	[RF_Reason] [nvarchar](max) NOT NULL,
	[RF_Status] [nvarchar](20) NOT NULL,
	[RF_CreatedBy] [uniqueidentifier] NOT NULL,
	[RF_CreatedAt] [datetime2](7) NOT NULL,
	[RF_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Refunds_RF] PRIMARY KEY CLUSTERED 
(
	[RF_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Results_R]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Results_R](
	[R_Id] [uniqueidentifier] NOT NULL,
	[R_ExamId] [uniqueidentifier] NOT NULL,
	[R_StudentId] [uniqueidentifier] NOT NULL,
	[R_TotalMarks] [numeric](10, 2) NOT NULL,
	[R_MarksObtained] [numeric](10, 2) NOT NULL,
	[R_Percentage] [numeric](6, 2) NOT NULL,
	[R_Grade] [nvarchar](20) NULL,
	[R_ResultStatus] [nvarchar](30) NOT NULL,
	[R_Remarks] [nvarchar](max) NULL,
	[R_PublishedAt] [datetime2](7) NULL,
	[R_CreatedAt] [datetime2](7) NOT NULL,
	[R_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Results_R] PRIMARY KEY CLUSTERED 
(
	[R_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff_ST]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff_ST](
	[ST_Id] [uniqueidentifier] NOT NULL,
	[ST_TenantId] [uniqueidentifier] NOT NULL,
	[ST_BranchId] [uniqueidentifier] NOT NULL,
	[ST_UserId] [uniqueidentifier] NULL,
	[ST_DepartmentId] [uniqueidentifier] NULL,
	[ST_DesignationId] [uniqueidentifier] NULL,
	[ST_EmployeeCode] [nvarchar](50) NOT NULL,
	[ST_FirstName] [nvarchar](100) NOT NULL,
	[ST_LastName] [nvarchar](100) NOT NULL,
	[ST_Email] [nvarchar](255) NULL,
	[ST_Phone] [nvarchar](30) NULL,
	[ST_JoiningDate] [date] NULL,
	[ST_Status] [nvarchar](20) NOT NULL,
	[ST_CreatedAt] [datetime2](7) NOT NULL,
	[ST_UpdatedAt] [datetime2](7) NOT NULL,
	[ST_DeletedAt] [datetime2](7) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentFeeAssignments_SFA]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentFeeAssignments_SFA](
	[SFA_Id] [uniqueidentifier] NOT NULL,
	[SFA_StudentId] [uniqueidentifier] NOT NULL,
	[SFA_FeeStructureId] [uniqueidentifier] NOT NULL,
	[SFA_AssignedAmount] [numeric](12, 2) NOT NULL,
	[SFA_DiscountAmount] [numeric](12, 2) NOT NULL,
	[SFA_FinalAmount] [numeric](12, 2) NOT NULL,
	[SFA_AssignedDate] [date] NOT NULL,
	[SFA_Status] [nvarchar](30) NOT NULL,
	[SFA_CreatedAt] [datetime2](7) NOT NULL,
	[SFA_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_StudentFeeAssignments_SFA] PRIMARY KEY CLUSTERED 
(
	[SFA_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentGuardians_SG]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentGuardians_SG](
	[SG_StudentId] [uniqueidentifier] NOT NULL,
	[SG_GuardianId] [uniqueidentifier] NOT NULL,
	[SG_Relationship] [nvarchar](50) NOT NULL,
	[SG_IsPrimary] [bit] NOT NULL,
	[SG_IsEmergency] [bit] NOT NULL,
 CONSTRAINT [PK_StudentGuardians_SG] PRIMARY KEY CLUSTERED 
(
	[SG_StudentId] ASC,
	[SG_GuardianId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students_Guardians]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students_Guardians](
	[SG_Id] [uniqueidentifier] NOT NULL,
	[SG_StudentId] [uniqueidentifier] NOT NULL,
	[SG_GuardianId] [uniqueidentifier] NOT NULL,
	[SG_Relation] [nvarchar](50) NOT NULL,
	[SG_IsPrimary] [bit] NOT NULL,
	[SG_CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Students_Guardians] PRIMARY KEY CLUSTERED 
(
	[SG_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students_S]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students_S](
	[S_Id] [uniqueidentifier] NOT NULL,
	[S_TenantId] [uniqueidentifier] NOT NULL,
	[S_BranchId] [uniqueidentifier] NOT NULL,
	[S_UserId] [uniqueidentifier] NULL,
	[S_StudentCode] [nvarchar](50) NOT NULL,
	[S_AdmissionNumber] [nvarchar](50) NOT NULL,
	[S_FirstName] [nvarchar](100) NOT NULL,
	[S_MiddleName] [nvarchar](100) NULL,
	[S_LastName] [nvarchar](100) NOT NULL,
	[S_DateOfBirth] [date] NULL,
	[S_Gender] [nvarchar](20) NULL,
	[S_Email] [nvarchar](255) NULL,
	[S_Phone] [nvarchar](30) NULL,
	[S_AdmissionDate] [date] NULL,
	[S_Status] [nvarchar](20) NOT NULL,
	[S_CreatedAt] [datetime2](7) NOT NULL,
	[S_UpdatedAt] [datetime2](7) NOT NULL,
	[S_DeletedAt] [datetime2](7) NULL,
	[S_ClassId] [uniqueidentifier] NULL,
	[S_SectionId] [uniqueidentifier] NULL,
	[S_BloodGroup] [nvarchar](10) NULL,
	[S_AddressLine1] [nvarchar](255) NULL,
	[S_AddressLine2] [nvarchar](255) NULL,
	[S_City] [nvarchar](100) NULL,
	[S_State] [nvarchar](100) NULL,
	[S_PostalCode] [nvarchar](20) NULL,
	[S_Country] [nvarchar](100) NULL,
 CONSTRAINT [PK_Students_S] PRIMARY KEY CLUSTERED 
(
	[S_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects_SB]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects_SB](
	[SB_Id] [uniqueidentifier] NOT NULL,
	[SB_TenantId] [uniqueidentifier] NOT NULL,
	[SB_Name] [nvarchar](200) NOT NULL,
	[SB_Code] [nvarchar](50) NOT NULL,
	[SB_Description] [nvarchar](max) NULL,
	[SB_Credits] [numeric](5, 2) NULL,
	[SB_MaxMarks] [numeric](8, 2) NULL,
	[SB_PassMarks] [numeric](8, 2) NULL,
	[SB_CreatedAt] [datetime2](7) NOT NULL,
	[SB_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Subjects_SB] PRIMARY KEY CLUSTERED 
(
	[SB_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherAttendance_TA]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherAttendance_TA](
	[TA_Id] [uniqueidentifier] NOT NULL,
	[TA_TenantId] [uniqueidentifier] NOT NULL,
	[TA_TeacherId] [uniqueidentifier] NOT NULL,
	[TA_Date] [date] NOT NULL,
	[TA_Status] [nvarchar](20) NOT NULL,
	[TA_Remarks] [nvarchar](500) NULL,
	[TA_MarkedBy] [uniqueidentifier] NOT NULL,
	[TA_CreatedAt] [datetime2](7) NOT NULL,
	[TA_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_TeacherAttendance_TA] PRIMARY KEY CLUSTERED 
(
	[TA_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherLeaves_TL]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherLeaves_TL](
	[TL_Id] [uniqueidentifier] NOT NULL,
	[TL_TenantId] [uniqueidentifier] NOT NULL,
	[TL_TeacherId] [uniqueidentifier] NOT NULL,
	[TL_TeacherName] [nvarchar](200) NOT NULL,
	[TL_LeaveType] [nvarchar](30) NOT NULL,
	[TL_FromDate] [date] NOT NULL,
	[TL_ToDate] [date] NOT NULL,
	[TL_TotalDays] [int] NOT NULL,
	[TL_Reason] [nvarchar](500) NULL,
	[TL_Status] [nvarchar](20) NOT NULL,
	[TL_AppliedAt] [datetime2](7) NOT NULL,
	[TL_AppliedBy] [uniqueidentifier] NOT NULL,
	[TL_ApprovedBy] [uniqueidentifier] NULL,
	[TL_ApprovedAt] [datetime2](7) NULL,
	[TL_RejectionReason] [nvarchar](500) NULL,
	[TL_CreatedAt] [datetime2](7) NOT NULL,
	[TL_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_TeacherLeaves_TL] PRIMARY KEY CLUSTERED 
(
	[TL_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teachers_T]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teachers_T](
	[T_Id] [uniqueidentifier] NOT NULL,
	[T_TenantId] [uniqueidentifier] NOT NULL,
	[T_BranchId] [uniqueidentifier] NOT NULL,
	[T_EmployeeCode] [nvarchar](50) NOT NULL,
	[T_Designation] [nvarchar](100) NULL,
	[T_Department] [nvarchar](100) NULL,
	[T_JoiningDate] [date] NULL,
	[T_Qualification] [nvarchar](255) NULL,
	[T_ExperienceYears] [int] NULL,
	[T_BloodGroup] [nvarchar](10) NULL,
	[T_Status] [nvarchar](20) NOT NULL,
	[T_IsActive] [bit] NOT NULL,
	[T_CreatedAt] [datetime2](7) NOT NULL,
	[T_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Teachers_T] PRIMARY KEY CLUSTERED 
(
	[T_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Timetables_TT]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Timetables_TT](
	[TT_Id] [uniqueidentifier] NOT NULL,
	[TT_TenantId] [uniqueidentifier] NOT NULL,
	[TT_BranchId] [uniqueidentifier] NOT NULL,
	[TT_BatchId] [uniqueidentifier] NOT NULL,
	[TT_SubjectId] [uniqueidentifier] NOT NULL,
	[TT_StaffId] [uniqueidentifier] NOT NULL,
	[TT_ClassroomId] [uniqueidentifier] NULL,
	[TT_DayOfWeek] [smallint] NOT NULL,
	[TT_StartTime] [time](7) NOT NULL,
	[TT_EndTime] [time](7) NOT NULL,
	[TT_EffectiveFrom] [date] NULL,
	[TT_EffectiveTo] [date] NULL,
	[TT_CreatedAt] [datetime2](7) NOT NULL,
	[TT_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Timetables_TT] PRIMARY KEY CLUSTERED 
(
	[TT_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendors_V]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendors_V](
	[V_Id] [uniqueidentifier] NOT NULL,
	[V_TenantId] [uniqueidentifier] NOT NULL,
	[V_Name] [nvarchar](200) NOT NULL,
	[V_Code] [nvarchar](50) NOT NULL,
	[V_Email] [nvarchar](255) NULL,
	[V_Phone] [nvarchar](30) NULL,
	[V_Address] [nvarchar](max) NULL,
	[V_TaxNumber] [nvarchar](100) NULL,
	[V_CreatedAt] [datetime2](7) NOT NULL,
	[V_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Vendors_V] PRIMARY KEY CLUSTERED 
(
	[V_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMode_PM]    Script Date: 07-09-2026 12:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMode_PM](
	[PM_Id] [uniqueidentifier] NOT NULL,
	[PM_TenantId] [uniqueidentifier] NOT NULL,
	[PM_ModeName] [nvarchar](100) NOT NULL,
	[PM_ModeCode] [nvarchar](50) NOT NULL,
	[PM_IsActive] [bit] NOT NULL,
	[PM_CreatedAt] [datetime2](7) NOT NULL,
	[PM_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_PaymentMode_PM] PRIMARY KEY CLUSTERED 
(
	[PM_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory_PC]    Script Date: 07-09-2026 12:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory_PC](
	[PC_Id] [uniqueidentifier] NOT NULL,
	[PC_TenantId] [uniqueidentifier] NOT NULL,
	[PC_CategoryName] [nvarchar](150) NOT NULL,
	[PC_CategoryCode] [nvarchar](50) NOT NULL,
	[PC_IsActive] [bit] NOT NULL,
	[PC_CreatedAt] [datetime2](7) NOT NULL,
	[PC_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ProductCategory_PC] PRIMARY KEY CLUSTERED 
(
	[PC_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductBrand_PB]    Script Date: 07-09-2026 12:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductBrand_PB](
	[PB_Id] [uniqueidentifier] NOT NULL,
	[PB_TenantId] [uniqueidentifier] NOT NULL,
	[PB_BrandName] [nvarchar](150) NOT NULL,
	[PB_BrandCode] [nvarchar](50) NOT NULL,
	[PB_IsActive] [bit] NOT NULL,
	[PB_CreatedAt] [datetime2](7) NOT NULL,
	[PB_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ProductBrand_PB] PRIMARY KEY CLUSTERED 
(
	[PB_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductUnit_PU]    Script Date: 07-09-2026 12:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductUnit_PU](
	[PU_Id] [uniqueidentifier] NOT NULL,
	[PU_TenantId] [uniqueidentifier] NOT NULL,
	[PU_UnitName] [nvarchar](100) NOT NULL,
	[PU_UnitCode] [nvarchar](50) NOT NULL,
	[PU_IsActive] [bit] NOT NULL,
	[PU_CreatedAt] [datetime2](7) NOT NULL,
	[PU_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_ProductUnit_PU] PRIMARY KEY CLUSTERED 
(
	[PU_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VendorCategories_VC]    Script Date: 07-09-2026 12:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendorCategories_VC](
	[VC_Id] [uniqueidentifier] NOT NULL,
	[VC_TenantId] [uniqueidentifier] NOT NULL,
	[VC_CategoryName] [nvarchar](150) NOT NULL,
	[VC_CategoryCode] [nvarchar](50) NOT NULL,
	[VC_IsActive] [bit] NOT NULL,
	[VC_CreatedAt] [datetime2](7) NOT NULL,
	[VC_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_VendorCategories_VC] PRIMARY KEY CLUSTERED 
(
	[VC_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classes_CL]    Script Date: 07-09-2026 12:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classes_CL](
	[CL_Id] [uniqueidentifier] NOT NULL,
	[CL_TenantId] [uniqueidentifier] NOT NULL,
	[CL_BranchId] [uniqueidentifier] NOT NULL,
	[CL_Name] [nvarchar](100) NOT NULL,
	[CL_Code] [nvarchar](50) NOT NULL,
	[CL_Description] [nvarchar](255) NULL,
	[CL_IsActive] [bit] NOT NULL,
	[CL_CreatedAt] [datetime2](7) NOT NULL,
	[CL_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Classes_CL] PRIMARY KEY CLUSTERED 
(
	[CL_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sections_S]    Script Date: 07-09-2026 12:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sections_S](
	[S_Sect_Id] [uniqueidentifier] NOT NULL,
	[S_Sect_TenantId] [uniqueidentifier] NOT NULL,
	[S_Sect_BranchId] [uniqueidentifier] NOT NULL,
	[S_Sect_ClassId] [uniqueidentifier] NOT NULL,
	[S_Sect_Name] [nvarchar](100) NOT NULL,
	[S_Sect_Code] [nvarchar](50) NOT NULL,
	[S_Sect_IsActive] [bit] NOT NULL,
	[S_Sect_CreatedAt] [datetime2](7) NOT NULL,
	[S_Sect_UpdatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Sections_S] PRIMARY KEY CLUSTERED 
(
	[S_Sect_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AcademicYears_AY] ([AY_Id], [AY_TenantId], [AY_Name], [AY_Code], [AY_StartDate], [AY_EndDate], [AY_IsCurrent], [AY_CreatedAt], [AY_UpdatedAt]) VALUES (N'33333333-3333-3333-3333-333333333301', N'11111111-1111-1111-1111-111111111111', N'2023-2024', N'AY-2023', CAST(N'2023-04-01' AS Date), CAST(N'2024-03-31' AS Date), 0, CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2), CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2))
INSERT [dbo].[AcademicYears_AY] ([AY_Id], [AY_TenantId], [AY_Name], [AY_Code], [AY_StartDate], [AY_EndDate], [AY_IsCurrent], [AY_CreatedAt], [AY_UpdatedAt]) VALUES (N'33333333-3333-3333-3333-333333333302', N'11111111-1111-1111-1111-111111111111', N'2024-2025', N'AY-2024', CAST(N'2024-04-01' AS Date), CAST(N'2025-03-31' AS Date), 0, CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2), CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2))
INSERT [dbo].[AcademicYears_AY] ([AY_Id], [AY_TenantId], [AY_Name], [AY_Code], [AY_StartDate], [AY_EndDate], [AY_IsCurrent], [AY_CreatedAt], [AY_UpdatedAt]) VALUES (N'33333333-3333-3333-3333-333333333303', N'11111111-1111-1111-1111-111111111111', N'2025-2026', N'AY-2025', CAST(N'2025-04-01' AS Date), CAST(N'2026-03-31' AS Date), 1, CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2), CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2))
INSERT [dbo].[AcademicYears_AY] ([AY_Id], [AY_TenantId], [AY_Name], [AY_Code], [AY_StartDate], [AY_EndDate], [AY_IsCurrent], [AY_CreatedAt], [AY_UpdatedAt]) VALUES (N'33333333-3333-3333-3333-333333333304', N'11111111-1111-1111-1111-111111111111', N'2026-2027', N'AY-2026', CAST(N'2026-04-01' AS Date), CAST(N'2027-03-31' AS Date), 0, CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2), CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2))
INSERT [dbo].[AcademicYears_AY] ([AY_Id], [AY_TenantId], [AY_Name], [AY_Code], [AY_StartDate], [AY_EndDate], [AY_IsCurrent], [AY_CreatedAt], [AY_UpdatedAt]) VALUES (N'33333333-3333-3333-3333-333333333305', N'11111111-1111-1111-1111-111111111111', N'2027-2028', N'AY-2027', CAST(N'2027-04-01' AS Date), CAST(N'2028-03-31' AS Date), 0, CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2), CAST(N'2026-08-27T01:59:03.5046544' AS DateTime2))
INSERT [dbo].[AcademicYears_AY] ([AY_Id], [AY_TenantId], [AY_Name], [AY_Code], [AY_StartDate], [AY_EndDate], [AY_IsCurrent], [AY_CreatedAt], [AY_UpdatedAt]) VALUES (N'b4afef85-23f9-46ff-a90b-39c61e0c101f', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'New2000', N'Y-2000', CAST(N'2027-01-01' AS Date), CAST(N'2027-12-01' AS Date), 1, CAST(N'2026-09-06T14:29:23.6851859' AS DateTime2), CAST(N'2026-09-06T14:29:23.6851859' AS DateTime2))
INSERT [dbo].[AcademicYears_AY] ([AY_Id], [AY_TenantId], [AY_Name], [AY_Code], [AY_StartDate], [AY_EndDate], [AY_IsCurrent], [AY_CreatedAt], [AY_UpdatedAt]) VALUES (N'b3d1cc8f-728d-4865-a332-a403a675e1d0', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'AY-2028', N'AY-2028', CAST(N'2028-01-02' AS Date), CAST(N'2028-12-24' AS Date), 1, CAST(N'2026-08-29T06:14:33.9886659' AS DateTime2), CAST(N'2026-08-29T06:19:11.2393396' AS DateTime2))
GO
INSERT [dbo].[AdmissionApplications_AA] ([AA_Id], [AA_TenantId], [AA_BranchId], [AA_ApplicationNumber], [AA_FirstName], [AA_LastName], [AA_DateOfBirth], [AA_Gender], [AA_Email], [AA_Phone], [AA_CourseId], [AA_AcademicYearId], [AA_Status], [AA_SubmittedAt], [AA_ReviewedAt], [AA_ReviewedBy], [AA_Notes], [AA_CreatedAt], [AA_UpdatedAt]) VALUES (N'55a9bdc1-c2f0-4422-8931-8a4b5802c48f', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', N'APP-26-0001', N'Sujit', N'Das', CAST(N'2003-04-02' AS Date), N'Male', N'sujit@gmail.com', N'9857458745', N'77777777-7777-7777-7777-777777777703', N'33333333-3333-3333-3333-333333333304', N'Approved', CAST(N'2026-08-30T11:19:33.2713656' AS DateTime2), CAST(N'2026-08-30T11:21:09.9166667' AS DateTime2), N'ce0ff4e4-6f5b-472b-84b2-59204f71ef46', N'ami to admission niye nichhi... akhane re', CAST(N'2026-08-30T11:19:34.1666667' AS DateTime2), CAST(N'2026-08-30T11:21:09.9166667' AS DateTime2))
INSERT [dbo].[AdmissionApplications_AA] ([AA_Id], [AA_TenantId], [AA_BranchId], [AA_ApplicationNumber], [AA_FirstName], [AA_LastName], [AA_DateOfBirth], [AA_Gender], [AA_Email], [AA_Phone], [AA_CourseId], [AA_AcademicYearId], [AA_Status], [AA_SubmittedAt], [AA_ReviewedAt], [AA_ReviewedBy], [AA_Notes], [AA_CreatedAt], [AA_UpdatedAt]) VALUES (N'c1980c8e-1920-4719-bf59-e8e850a3430d', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', N'APP-26-0002', N'sdfhjk', N'zxfchgvjh', CAST(N'2018-08-02' AS Date), N'Male', N'msd2wrety@gmail.com', N'earsetdrf', N'96141a47-d36d-4bbe-b5e5-352b694f9b3d', N'33333333-3333-3333-3333-333333333305', N'Approved', CAST(N'2026-08-30T11:23:24.3850393' AS DateTime2), NULL, NULL, NULL, CAST(N'2026-08-30T11:23:24.4766667' AS DateTime2), CAST(N'2026-08-30T11:23:24.4766667' AS DateTime2))
GO
INSERT [dbo].[AdmissionNumberCounters_ANC] ([ANC_TenantId], [ANC_Year], [ANC_LastNumber]) VALUES (N'4150d676-e932-49af-a9d3-dc673fc1891d', 2026, 1)
GO
INSERT [dbo].[AttendanceRecords_AR] ([AR_Id], [AR_AttendanceSessionId], [AR_StudentId], [AR_Status], [AR_Remarks], [AR_CreatedAt], [AR_UpdatedAt]) VALUES (N'2f69952b-e2ff-4db1-8781-8a98ce537581', N'c91b81d9-0f4f-4760-9cea-ef6e6d449a35', N'33449bb9-af6f-411d-bde7-615e2e2fb9e1', N'present', NULL, CAST(N'2026-09-05T16:09:56.2945294' AS DateTime2), CAST(N'2026-09-05T16:09:56.2945294' AS DateTime2))
GO
INSERT [dbo].[AttendanceSessions_AS] ([AS_Id], [AS_TenantId], [AS_BranchId], [AS_BatchId], [AS_SubjectId], [AS_StaffId], [AS_AttendanceDate], [AS_StartTime], [AS_EndTime], [AS_Remarks], [AS_CreatedAt], [AS_UpdatedAt]) VALUES (N'c91b81d9-0f4f-4760-9cea-ef6e6d449a35', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', N'e31894f1-78dd-432e-a694-bdadee54a634', N'1aff9516-a518-44a7-8fbf-000a4e145607', N'1b3b5cfd-af27-4869-9a97-d2f9aac167ef', CAST(N'2026-08-30' AS Date), CAST(N'20:54:00' AS Time), CAST(N'21:54:00' AS Time), N'ijohgyuftdre', CAST(N'2026-08-30T15:24:49.5295635' AS DateTime2), CAST(N'2026-08-30T15:24:49.5295635' AS DateTime2))
GO
INSERT [dbo].[Batches_BT] ([BT_Id], [BT_TenantId], [BT_BranchId], [BT_CourseId], [BT_AcademicYearId], [BT_Name], [BT_Code], [BT_StartDate], [BT_EndDate], [BT_Capacity], [BT_Status], [BT_CreatedAt], [BT_UpdatedAt]) VALUES (N'02c51608-4afd-4085-a580-527cbe323927', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', N'96141a47-d36d-4bbe-b5e5-352b694f9b3d', N'33333333-3333-3333-3333-333333333305', N'Section-20260', N'BATCH-0010', CAST(N'2026-08-30' AS Date), CAST(N'2027-02-07' AS Date), NULL, N'Active', CAST(N'2026-08-30T12:56:21.3166667' AS DateTime2), CAST(N'2026-08-30T12:56:21.3166667' AS DateTime2))
INSERT [dbo].[Batches_BT] ([BT_Id], [BT_TenantId], [BT_BranchId], [BT_CourseId], [BT_AcademicYearId], [BT_Name], [BT_Code], [BT_StartDate], [BT_EndDate], [BT_Capacity], [BT_Status], [BT_CreatedAt], [BT_UpdatedAt]) VALUES (N'e31894f1-78dd-432e-a694-bdadee54a634', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', N'96141a47-d36d-4bbe-b5e5-352b694f9b3d', N'b3d1cc8f-728d-4865-a332-a403a675e1d0', N'Section-2026', N'BATCH-001', CAST(N'2026-08-30' AS Date), CAST(N'2027-01-29' AS Date), 100, N'Active', CAST(N'2026-08-30T11:00:01.8566667' AS DateTime2), CAST(N'2026-08-30T11:00:01.8566667' AS DateTime2))
GO
INSERT [dbo].[Branches_B] ([B_Id], [B_TenantId], [B_Name], [B_Code], [B_Email], [B_Phone], [B_AddressLine1], [B_AddressLine2], [B_City], [B_State], [B_PostalCode], [B_CountryCode], [B_Status], [B_CreatedAt], [B_UpdatedAt], [B_DeletedAt]) VALUES (N'22222222-2222-2222-2222-222222222201', N'11111111-1111-1111-1111-111111111111', N'Main Campus', N'BR-001', N'main@ims.com', N'9000000001', N'123 College Road', N'Near Central Park', N'Mumbai', N'Maharashtra', N'400001', N'IN', N'active', CAST(N'2026-08-26T18:59:28.7990922' AS DateTime2), CAST(N'2026-08-26T18:59:28.7990922' AS DateTime2), NULL)
INSERT [dbo].[Branches_B] ([B_Id], [B_TenantId], [B_Name], [B_Code], [B_Email], [B_Phone], [B_AddressLine1], [B_AddressLine2], [B_City], [B_State], [B_PostalCode], [B_CountryCode], [B_Status], [B_CreatedAt], [B_UpdatedAt], [B_DeletedAt]) VALUES (N'22222222-2222-2222-2222-222222222202', N'11111111-1111-1111-1111-111111111111', N'North Branch', N'BR-002', N'north@ims.com', N'9000000002', N'456 School Street', N'Opp. City Mall', N'Delhi', N'Delhi', N'110001', N'IN', N'active', CAST(N'2026-08-26T18:59:28.7990922' AS DateTime2), CAST(N'2026-08-26T18:59:28.7990922' AS DateTime2), NULL)
INSERT [dbo].[Branches_B] ([B_Id], [B_TenantId], [B_Name], [B_Code], [B_Email], [B_Phone], [B_AddressLine1], [B_AddressLine2], [B_City], [B_State], [B_PostalCode], [B_CountryCode], [B_Status], [B_CreatedAt], [B_UpdatedAt], [B_DeletedAt]) VALUES (N'22222222-2222-2222-2222-222222222203', N'11111111-1111-1111-1111-111111111111', N'South Branch', N'BR-003', N'south@ims.com', N'9000000003', N'789 Academy Lane', N'Near Bus Stand', N'Chennai', N'Tamil Nadu', N'600001', N'IN', N'active', CAST(N'2026-08-26T18:59:28.7990922' AS DateTime2), CAST(N'2026-08-26T18:59:28.7990922' AS DateTime2), NULL)
INSERT [dbo].[Branches_B] ([B_Id], [B_TenantId], [B_Name], [B_Code], [B_Email], [B_Phone], [B_AddressLine1], [B_AddressLine2], [B_City], [B_State], [B_PostalCode], [B_CountryCode], [B_Status], [B_CreatedAt], [B_UpdatedAt], [B_DeletedAt]) VALUES (N'22222222-2222-2222-2222-222222222204', N'11111111-1111-1111-1111-111111111111', N'East Wing', N'BR-004', N'east@ims.com', N'9000000004', N'321 Learning Ave', N'Behind Library', N'Kolkata', N'West Bengal', N'700001', N'IN', N'active', CAST(N'2026-08-26T18:59:28.7990922' AS DateTime2), CAST(N'2026-08-26T18:59:28.7990922' AS DateTime2), NULL)
INSERT [dbo].[Branches_B] ([B_Id], [B_TenantId], [B_Name], [B_Code], [B_Email], [B_Phone], [B_AddressLine1], [B_AddressLine2], [B_City], [B_State], [B_PostalCode], [B_CountryCode], [B_Status], [B_CreatedAt], [B_UpdatedAt], [B_DeletedAt]) VALUES (N'22222222-2222-2222-2222-222222222205', N'11111111-1111-1111-1111-111111111111', N'West Campus', N'BR-005', N'west@ims.com', N'9000000005', N'654 Education Blvd', N'Next to Stadium', N'Pune', N'Maharashtra', N'411001', N'IN', N'inactive', CAST(N'2026-08-26T18:59:28.7990922' AS DateTime2), CAST(N'2026-08-29T08:06:01.5656193' AS DateTime2), CAST(N'2026-08-29T08:06:01.5656193' AS DateTime2))
INSERT [dbo].[Branches_B] ([B_Id], [B_TenantId], [B_Name], [B_Code], [B_Email], [B_Phone], [B_AddressLine1], [B_AddressLine2], [B_City], [B_State], [B_PostalCode], [B_CountryCode], [B_Status], [B_CreatedAt], [B_UpdatedAt], [B_DeletedAt]) VALUES (N'a912f9f7-ed29-4520-971c-751279170020', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'njgejkjk', N'ojogne', N'fnvobn', N'nvoenb', N'fnonb', N'onvoe', N'noveb', N'novb ', N'fnov bi', N'96', N'active', CAST(N'2026-08-29T08:16:42.6254915' AS DateTime2), CAST(N'2026-08-29T08:17:08.1313668' AS DateTime2), NULL)
INSERT [dbo].[Branches_B] ([B_Id], [B_TenantId], [B_Name], [B_Code], [B_Email], [B_Phone], [B_AddressLine1], [B_AddressLine2], [B_City], [B_State], [B_PostalCode], [B_CountryCode], [B_Status], [B_CreatedAt], [B_UpdatedAt], [B_DeletedAt]) VALUES (N'07697467-42c6-4d9c-b6cf-7ecde512e717', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'Coochbehar', N'798645213456', N'msd20958@gmail.com', N'3245678697', N'sterdyf', N'ygukh', N'uygihjl', N'ufygiuh', N'897465', N'91', N'inactive', CAST(N'2026-08-29T05:18:57.1402204' AS DateTime2), CAST(N'2026-08-29T05:19:52.8511331' AS DateTime2), CAST(N'2026-08-29T05:19:52.8511331' AS DateTime2))
INSERT [dbo].[Branches_B] ([B_Id], [B_TenantId], [B_Name], [B_Code], [B_Email], [B_Phone], [B_AddressLine1], [B_AddressLine2], [B_City], [B_State], [B_PostalCode], [B_CountryCode], [B_Status], [B_CreatedAt], [B_UpdatedAt], [B_DeletedAt]) VALUES (N'f4761997-6081-4385-b197-fc602f4cf595', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'Kolkata', N'798645', N'fhgjjhbgh@gmail.com', N'78964531264', N'sterdyf', N'ygukh', N'uygihjl', N'ufygiuh', N'897465', N'91', N'active', CAST(N'2026-08-26T18:37:05.4780894' AS DateTime2), CAST(N'2026-08-26T18:37:24.4024258' AS DateTime2), NULL)
GO
INSERT [dbo].[Classrooms_CR] ([CR_Id], [CR_TenantId], [CR_BranchId], [CR_Name], [CR_Code], [CR_Capacity], [CR_Location], [CR_CreatedAt], [CR_UpdatedAt]) VALUES (N'e412161e-9361-4818-ae2e-22fa9210e7b1', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'f4761997-6081-4385-b197-fc602f4cf595', N'6ytguh', N'987', 10000, N'hiygutfrdt', CAST(N'2026-08-29T06:58:22.6389466' AS DateTime2), CAST(N'2026-08-29T06:58:50.1755096' AS DateTime2))
GO
INSERT [dbo].[Courses_C] ([C_Id], [C_TenantId], [C_ProgramId], [C_Name], [C_Code], [C_Description], [C_Status], [C_CreatedAt], [C_UpdatedAt], [C_DeletedAt]) VALUES (N'96141a47-d36d-4bbe-b5e5-352b694f9b3d', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'44444444-4444-4444-4444-444444444403', N'Abc courcse aefwgrb', N'ABC678', N'dfegrthjukyil', N'active', CAST(N'2026-08-29T05:21:07.2085964' AS DateTime2), CAST(N'2026-08-29T05:21:48.3371074' AS DateTime2), NULL)
INSERT [dbo].[Courses_C] ([C_Id], [C_TenantId], [C_ProgramId], [C_Name], [C_Code], [C_Description], [C_Status], [C_CreatedAt], [C_UpdatedAt], [C_DeletedAt]) VALUES (N'eb0d1638-5328-45c3-9ea3-676bc033ca1e', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'44444444-4444-4444-4444-444444444402', N'Abc New1', N'Course579864', N'This is new course', N'active', CAST(N'2026-09-06T14:25:08.4388580' AS DateTime2), CAST(N'2026-09-06T14:25:27.0705486' AS DateTime2), NULL)
INSERT [dbo].[Courses_C] ([C_Id], [C_TenantId], [C_ProgramId], [C_Name], [C_Code], [C_Description], [C_Status], [C_CreatedAt], [C_UpdatedAt], [C_DeletedAt]) VALUES (N'77777777-7777-7777-7777-777777777701', N'11111111-1111-1111-1111-111111111111', N'44444444-4444-4444-4444-444444444401', N'B.Sc. Computer Science', N'C-BSC-CS', N'Bachelor of Science in Computer Science', N'active', CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), NULL)
INSERT [dbo].[Courses_C] ([C_Id], [C_TenantId], [C_ProgramId], [C_Name], [C_Code], [C_Description], [C_Status], [C_CreatedAt], [C_UpdatedAt], [C_DeletedAt]) VALUES (N'77777777-7777-7777-7777-777777777702', N'11111111-1111-1111-1111-111111111111', N'44444444-4444-4444-4444-444444444401', N'B.Sc. Mathematics', N'C-BSC-MATH', N'Bachelor of Science in Mathematics', N'active', CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), NULL)
INSERT [dbo].[Courses_C] ([C_Id], [C_TenantId], [C_ProgramId], [C_Name], [C_Code], [C_Description], [C_Status], [C_CreatedAt], [C_UpdatedAt], [C_DeletedAt]) VALUES (N'77777777-7777-7777-7777-777777777703', N'11111111-1111-1111-1111-111111111111', N'44444444-4444-4444-4444-444444444402', N'B.A. English', N'C-BA-ENG', N'Bachelor of Arts in English', N'active', CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), NULL)
INSERT [dbo].[Courses_C] ([C_Id], [C_TenantId], [C_ProgramId], [C_Name], [C_Code], [C_Description], [C_Status], [C_CreatedAt], [C_UpdatedAt], [C_DeletedAt]) VALUES (N'77777777-7777-7777-7777-777777777704', N'11111111-1111-1111-1111-111111111111', N'44444444-4444-4444-4444-444444444403', N'M.Sc. Computer Science', N'C-MSC-CS', N'Master of Science in Computer Science', N'active', CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), NULL)
INSERT [dbo].[Courses_C] ([C_Id], [C_TenantId], [C_ProgramId], [C_Name], [C_Code], [C_Description], [C_Status], [C_CreatedAt], [C_UpdatedAt], [C_DeletedAt]) VALUES (N'77777777-7777-7777-7777-777777777705', N'11111111-1111-1111-1111-111111111111', N'44444444-4444-4444-4444-444444444404', N'Diploma in Web Dev', N'C-DCS-WEB', N'Diploma in Web Development', N'active', CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), CAST(N'2026-08-27T02:01:15.7455390' AS DateTime2), NULL)
GO
INSERT [dbo].[CourseSubjects_CS] ([CS_CourseId], [CS_SubjectId], [CS_SequenceNo], [CS_IsMandatory], [CS_MaxMarks], [CS_PassMarks]) VALUES (N'96141a47-d36d-4bbe-b5e5-352b694f9b3d', N'1aff9516-a518-44a7-8fbf-000a4e145607', 1, 1, CAST(100.00 AS Numeric(8, 2)), CAST(50.00 AS Numeric(8, 2)))
INSERT [dbo].[CourseSubjects_CS] ([CS_CourseId], [CS_SubjectId], [CS_SequenceNo], [CS_IsMandatory], [CS_MaxMarks], [CS_PassMarks]) VALUES (N'77777777-7777-7777-7777-777777777701', N'88888888-8888-8888-8888-888888888803', 1, 1, CAST(100.00 AS Numeric(8, 2)), CAST(50.00 AS Numeric(8, 2)))
INSERT [dbo].[CourseSubjects_CS] ([CS_CourseId], [CS_SubjectId], [CS_SequenceNo], [CS_IsMandatory], [CS_MaxMarks], [CS_PassMarks]) VALUES (N'77777777-7777-7777-7777-777777777703', N'88888888-8888-8888-8888-888888888805', 1, 1, CAST(100.00 AS Numeric(8, 2)), CAST(40.00 AS Numeric(8, 2)))
INSERT [dbo].[CourseSubjects_CS] ([CS_CourseId], [CS_SubjectId], [CS_SequenceNo], [CS_IsMandatory], [CS_MaxMarks], [CS_PassMarks]) VALUES (N'77777777-7777-7777-7777-777777777705', N'88888888-8888-8888-8888-888888888804', 2, 1, CAST(100.00 AS Numeric(8, 2)), CAST(50.00 AS Numeric(8, 2)))
GO
INSERT [dbo].[Departments_D] ([D_Id], [D_TenantId], [D_BranchId], [D_Name], [D_Code], [D_Description], [D_CreatedAt], [D_UpdatedAt]) VALUES (N'9ab421bf-7acd-4abd-8351-3b02cad3ddd6', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'f4761997-6081-4385-b197-fc602f4cf595', N'ABC Dept rdytfugiho', N'DEpt23456', N'sdbgfnm ', CAST(N'2026-08-29T06:19:52.0655005' AS DateTime2), CAST(N'2026-08-29T06:20:02.8475211' AS DateTime2))
INSERT [dbo].[Departments_D] ([D_Id], [D_TenantId], [D_BranchId], [D_Name], [D_Code], [D_Description], [D_CreatedAt], [D_UpdatedAt]) VALUES (N'b57551c8-3c1f-4648-a5ef-48f9efcc587c', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222204', N'Bengali', N'Bengali', N'erwegrh', CAST(N'2026-08-27T02:14:11.6725927' AS DateTime2), CAST(N'2026-08-27T02:15:09.5387793' AS DateTime2))
INSERT [dbo].[Departments_D] ([D_Id], [D_TenantId], [D_BranchId], [D_Name], [D_Code], [D_Description], [D_CreatedAt], [D_UpdatedAt]) VALUES (N'55555555-5555-5555-5555-555555555501', N'11111111-1111-1111-1111-111111111111', N'22222222-2222-2222-2222-222222222201', N'Computer Science', N'DEPT-CS', N'CS and IT department', CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2), CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2))
INSERT [dbo].[Departments_D] ([D_Id], [D_TenantId], [D_BranchId], [D_Name], [D_Code], [D_Description], [D_CreatedAt], [D_UpdatedAt]) VALUES (N'55555555-5555-5555-5555-555555555502', N'11111111-1111-1111-1111-111111111111', N'22222222-2222-2222-2222-222222222201', N'Mathematics', N'DEPT-MATH', N'Mathematics department', CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2), CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2))
INSERT [dbo].[Departments_D] ([D_Id], [D_TenantId], [D_BranchId], [D_Name], [D_Code], [D_Description], [D_CreatedAt], [D_UpdatedAt]) VALUES (N'55555555-5555-5555-5555-555555555503', N'11111111-1111-1111-1111-111111111111', N'22222222-2222-2222-2222-222222222202', N'Physics', N'DEPT-PHYS', N'Physics department', CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2), CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2))
INSERT [dbo].[Departments_D] ([D_Id], [D_TenantId], [D_BranchId], [D_Name], [D_Code], [D_Description], [D_CreatedAt], [D_UpdatedAt]) VALUES (N'55555555-5555-5555-5555-555555555504', N'11111111-1111-1111-1111-111111111111', N'22222222-2222-2222-2222-222222222203', N'Chemistry', N'DEPT-CHEM', N'Chemistry department', CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2), CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2))
INSERT [dbo].[Departments_D] ([D_Id], [D_TenantId], [D_BranchId], [D_Name], [D_Code], [D_Description], [D_CreatedAt], [D_UpdatedAt]) VALUES (N'55555555-5555-5555-5555-555555555505', N'11111111-1111-1111-1111-111111111111', N'22222222-2222-2222-2222-222222222201', N'Administration', N'DEPT-ADM', N'Administrative department', CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2), CAST(N'2026-08-27T02:00:35.6156390' AS DateTime2))
INSERT [dbo].[Departments_D] ([D_Id], [D_TenantId], [D_BranchId], [D_Name], [D_Code], [D_Description], [D_CreatedAt], [D_UpdatedAt]) VALUES (N'49132450-84c1-4cf6-8dd1-a14c3b16a0e1', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222204', N'NewDepertment2000', N'NewDep2000', N'This is new department ', CAST(N'2026-09-06T14:31:16.2200833' AS DateTime2), CAST(N'2026-09-06T14:31:42.8167259' AS DateTime2))
GO
INSERT [dbo].[Designations_DS] ([DS_Id], [DS_TenantId], [DS_Name], [DS_Code], [DS_CreatedAt], [DS_UpdatedAt]) VALUES (N'811bf012-78fd-4854-995a-3edd5c82e3a2', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'HR', N'5ex6ryctuv2026', CAST(N'2026-08-29T06:20:57.0183284' AS DateTime2), CAST(N'2026-08-29T06:21:04.9746614' AS DateTime2))
INSERT [dbo].[Designations_DS] ([DS_Id], [DS_TenantId], [DS_Name], [DS_Code], [DS_CreatedAt], [DS_UpdatedAt]) VALUES (N'76ea0f00-db2a-40a0-82df-45d7c6cd6398', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'NewDesignation2000', N'Designation2000', CAST(N'2026-09-06T14:32:36.7511388' AS DateTime2), CAST(N'2026-09-06T14:33:05.3246257' AS DateTime2))
INSERT [dbo].[Designations_DS] ([DS_Id], [DS_TenantId], [DS_Name], [DS_Code], [DS_CreatedAt], [DS_UpdatedAt]) VALUES (N'66666666-6666-6666-6666-666666666601', N'11111111-1111-1111-1111-111111111111', N'Professor', N'DS-PROF', CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2), CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2))
INSERT [dbo].[Designations_DS] ([DS_Id], [DS_TenantId], [DS_Name], [DS_Code], [DS_CreatedAt], [DS_UpdatedAt]) VALUES (N'66666666-6666-6666-6666-666666666602', N'11111111-1111-1111-1111-111111111111', N'Associate Professor', N'DS-APROF', CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2), CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2))
INSERT [dbo].[Designations_DS] ([DS_Id], [DS_TenantId], [DS_Name], [DS_Code], [DS_CreatedAt], [DS_UpdatedAt]) VALUES (N'66666666-6666-6666-6666-666666666603', N'11111111-1111-1111-1111-111111111111', N'Assistant Professor', N'DS-ASST', CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2), CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2))
INSERT [dbo].[Designations_DS] ([DS_Id], [DS_TenantId], [DS_Name], [DS_Code], [DS_CreatedAt], [DS_UpdatedAt]) VALUES (N'66666666-6666-6666-6666-666666666604', N'11111111-1111-1111-1111-111111111111', N'Lecturer', N'DS-LECT', CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2), CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2))
INSERT [dbo].[Designations_DS] ([DS_Id], [DS_TenantId], [DS_Name], [DS_Code], [DS_CreatedAt], [DS_UpdatedAt]) VALUES (N'66666666-6666-6666-6666-666666666605', N'11111111-1111-1111-1111-111111111111', N'Lab Assistant', N'DS-LAB', CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2), CAST(N'2026-08-27T02:00:56.6429201' AS DateTime2))
GO
INSERT [dbo].[Discounts_DIS] ([DIS_Id], [DIS_TenantId], [DIS_Name], [DIS_Code], [DIS_DiscountType], [DIS_Value], [DIS_Description], [DIS_IsActive], [DIS_CreatedAt], [DIS_UpdatedAt]) VALUES (N'ee46d89b-aba7-4f61-9a1a-20f927aeac2f', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'defwrtr', N'wfgeb', N'Percentage', CAST(213.00 AS Numeric(12, 2)), NULL, 0, CAST(N'2026-08-29T07:18:52.7557060' AS DateTime2), CAST(N'2026-08-29T07:19:46.8764414' AS DateTime2))
INSERT [dbo].[Discounts_DIS] ([DIS_Id], [DIS_TenantId], [DIS_Name], [DIS_Code], [DIS_DiscountType], [DIS_Value], [DIS_Description], [DIS_IsActive], [DIS_CreatedAt], [DIS_UpdatedAt]) VALUES (N'aaaa7777-7777-7777-7777-777777777701', N'11111111-1111-1111-1111-111111111111', N'Sibling Discount', N'DIS-SIB', N'Percentage', CAST(10.00 AS Numeric(12, 2)), N'Discount for second child from same family', 1, CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2), CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2))
INSERT [dbo].[Discounts_DIS] ([DIS_Id], [DIS_TenantId], [DIS_Name], [DIS_Code], [DIS_DiscountType], [DIS_Value], [DIS_Description], [DIS_IsActive], [DIS_CreatedAt], [DIS_UpdatedAt]) VALUES (N'aaaa7777-7777-7777-7777-777777777702', N'11111111-1111-1111-1111-111111111111', N'Early Bird Discount', N'DIS-EBD', N'Percentage', CAST(5.00 AS Numeric(12, 2)), N'Fee paid before due date', 1, CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2), CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2))
INSERT [dbo].[Discounts_DIS] ([DIS_Id], [DIS_TenantId], [DIS_Name], [DIS_Code], [DIS_DiscountType], [DIS_Value], [DIS_Description], [DIS_IsActive], [DIS_CreatedAt], [DIS_UpdatedAt]) VALUES (N'aaaa7777-7777-7777-7777-777777777703', N'11111111-1111-1111-1111-111111111111', N'Staff Ward Discount', N'DIS-SWD', N'Percentage', CAST(25.00 AS Numeric(12, 2)), N'Discount for children of staff members', 1, CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2), CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2))
INSERT [dbo].[Discounts_DIS] ([DIS_Id], [DIS_TenantId], [DIS_Name], [DIS_Code], [DIS_DiscountType], [DIS_Value], [DIS_Description], [DIS_IsActive], [DIS_CreatedAt], [DIS_UpdatedAt]) VALUES (N'aaaa7777-7777-7777-7777-777777777704', N'11111111-1111-1111-1111-111111111111', N'Merit Scholarship', N'DIS-MER', N'Fixed', CAST(5000.00 AS Numeric(12, 2)), N'Scholarship for top scorers', 1, CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2), CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2))
INSERT [dbo].[Discounts_DIS] ([DIS_Id], [DIS_TenantId], [DIS_Name], [DIS_Code], [DIS_DiscountType], [DIS_Value], [DIS_Description], [DIS_IsActive], [DIS_CreatedAt], [DIS_UpdatedAt]) VALUES (N'aaaa7777-7777-7777-7777-777777777705', N'11111111-1111-1111-1111-111111111111', N'Government Subsidy', N'DIS-GOV', N'Percentage', CAST(15.00 AS Numeric(12, 2)), N'Government scheme discount', 1, CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2), CAST(N'2026-08-27T02:10:53.9067922' AS DateTime2))
INSERT [dbo].[Discounts_DIS] ([DIS_Id], [DIS_TenantId], [DIS_Name], [DIS_Code], [DIS_DiscountType], [DIS_Value], [DIS_Description], [DIS_IsActive], [DIS_CreatedAt], [DIS_UpdatedAt]) VALUES (N'd545f9d4-1ce4-410d-9464-eda0f61b398a', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'defwrtrfxgch', N'wfgebrxdtcfv', N'percentage', CAST(21.00 AS Numeric(12, 2)), N'dfsdgrjhehvjbnmk,', 0, CAST(N'2026-08-29T07:19:34.8969292' AS DateTime2), CAST(N'2026-08-29T07:23:07.9337843' AS DateTime2))
GO
INSERT [dbo].[DocumentTypes_DT] ([DT_Id], [DT_TenantId], [DT_Name], [DT_Code], [DT_EntityType], [DT_IsRequired], [DT_CreatedAt], [DT_UpdatedAt]) VALUES (N'722397a2-cdd9-4f8d-a157-2cc8e7e121f4', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'NewDocument2000', N'NewDoc2000', N'ABC2000', 1, CAST(N'2026-09-06T14:33:50.9102151' AS DateTime2), CAST(N'2026-09-06T14:34:11.6607293' AS DateTime2))
INSERT [dbo].[DocumentTypes_DT] ([DT_Id], [DT_TenantId], [DT_Name], [DT_Code], [DT_EntityType], [DT_IsRequired], [DT_CreatedAt], [DT_UpdatedAt]) VALUES (N'99999999-9999-9999-9999-999999999901', N'11111111-1111-1111-1111-111111111111', N'Admission Form', N'DT-ADM', N'student', 1, CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2), CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2))
INSERT [dbo].[DocumentTypes_DT] ([DT_Id], [DT_TenantId], [DT_Name], [DT_Code], [DT_EntityType], [DT_IsRequired], [DT_CreatedAt], [DT_UpdatedAt]) VALUES (N'99999999-9999-9999-9999-999999999902', N'11111111-1111-1111-1111-111111111111', N'Marksheet', N'DT-MARK', N'student', 0, CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2), CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2))
INSERT [dbo].[DocumentTypes_DT] ([DT_Id], [DT_TenantId], [DT_Name], [DT_Code], [DT_EntityType], [DT_IsRequired], [DT_CreatedAt], [DT_UpdatedAt]) VALUES (N'99999999-9999-9999-9999-999999999903', N'11111111-1111-1111-1111-111111111111', N'ID Proof', N'DT-ID', N'student', 1, CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2), CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2))
INSERT [dbo].[DocumentTypes_DT] ([DT_Id], [DT_TenantId], [DT_Name], [DT_Code], [DT_EntityType], [DT_IsRequired], [DT_CreatedAt], [DT_UpdatedAt]) VALUES (N'99999999-9999-9999-9999-999999999904', N'11111111-1111-1111-1111-111111111111', N'Transfer Certificate', N'DT-TC', N'student', 0, CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2), CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2))
INSERT [dbo].[DocumentTypes_DT] ([DT_Id], [DT_TenantId], [DT_Name], [DT_Code], [DT_EntityType], [DT_IsRequired], [DT_CreatedAt], [DT_UpdatedAt]) VALUES (N'99999999-9999-9999-9999-999999999905', N'11111111-1111-1111-1111-111111111111', N'Staff Appointment', N'DT-STAFF', N'staff', 1, CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2), CAST(N'2026-08-27T02:01:48.2049082' AS DateTime2))
INSERT [dbo].[DocumentTypes_DT] ([DT_Id], [DT_TenantId], [DT_Name], [DT_Code], [DT_EntityType], [DT_IsRequired], [DT_CreatedAt], [DT_UpdatedAt]) VALUES (N'e5004fee-d61a-4686-8363-c6257200c814', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'xtycuvjbhk', N'xryctuv', N'Abcyutfyd iuguy', 0, CAST(N'2026-08-29T06:31:24.2120462' AS DateTime2), CAST(N'2026-08-29T06:31:51.9282611' AS DateTime2))
GO
INSERT [dbo].[Enrollments_E] ([E_Id], [E_TenantId], [E_StudentId], [E_AcademicYearId], [E_CourseId], [E_BatchId], [E_EnrollmentNumber], [E_EnrollmentDate], [E_Status], [E_CompletionDate], [E_CreatedAt], [E_UpdatedAt]) VALUES (N'4b9ce5a2-62a0-4977-b786-be8d856114cc', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'33449bb9-af6f-411d-bde7-615e2e2fb9e1', N'b3d1cc8f-728d-4865-a332-a403a675e1d0', N'96141a47-d36d-4bbe-b5e5-352b694f9b3d', N'e31894f1-78dd-432e-a694-bdadee54a634', N'ENR-26-0001', CAST(N'2026-08-30' AS Date), N'Active', CAST(N'2026-12-03' AS Date), CAST(N'2026-08-30T15:01:43.9100000' AS DateTime2), CAST(N'2026-08-30T15:01:43.9100000' AS DateTime2))
GO
INSERT [dbo].[Exams_EX] ([EX_Id], [EX_TenantId], [EX_AcademicYearId], [EX_CourseId], [EX_BatchId], [EX_ExamTypeId], [EX_Name], [EX_Code], [EX_StartDate], [EX_EndDate], [EX_Status], [EX_CreatedAt], [EX_UpdatedAt]) VALUES (N'1ef441c3-e786-467a-970b-aa3f5cf77bee', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'33333333-3333-3333-3333-333333333305', N'96141a47-d36d-4bbe-b5e5-352b694f9b3d', N'e31894f1-78dd-432e-a694-bdadee54a634', N'0b465b45-c1e0-4d75-9cfe-e9c19fcc1778', N'Test-1', N'Test-1', CAST(N'2026-08-30' AS Date), CAST(N'2026-08-30' AS Date), N'Scheduled', CAST(N'2026-08-30T15:52:54.3167093' AS DateTime2), CAST(N'2026-08-30T15:53:23.7432439' AS DateTime2))
GO
INSERT [dbo].[ExamTypes_ET] ([ET_Id], [ET_TenantId], [ET_Name], [ET_Code], [ET_Description], [ET_WeightagePercentage]) VALUES (N'aaaa1111-1111-1111-1111-111111111101', N'11111111-1111-1111-1111-111111111111', N'Unit Test 1', N'ET-UT1', NULL, NULL)
INSERT [dbo].[ExamTypes_ET] ([ET_Id], [ET_TenantId], [ET_Name], [ET_Code], [ET_Description], [ET_WeightagePercentage]) VALUES (N'aaaa1111-1111-1111-1111-111111111102', N'11111111-1111-1111-1111-111111111111', N'Unit Test 2', N'ET-UT2', NULL, NULL)
INSERT [dbo].[ExamTypes_ET] ([ET_Id], [ET_TenantId], [ET_Name], [ET_Code], [ET_Description], [ET_WeightagePercentage]) VALUES (N'aaaa1111-1111-1111-1111-111111111103', N'11111111-1111-1111-1111-111111111111', N'Mid-Term Exam', N'ET-MID', NULL, NULL)
INSERT [dbo].[ExamTypes_ET] ([ET_Id], [ET_TenantId], [ET_Name], [ET_Code], [ET_Description], [ET_WeightagePercentage]) VALUES (N'aaaa1111-1111-1111-1111-111111111104', N'11111111-1111-1111-1111-111111111111', N'Pre-Final Exam', N'ET-PREL', NULL, NULL)
INSERT [dbo].[ExamTypes_ET] ([ET_Id], [ET_TenantId], [ET_Name], [ET_Code], [ET_Description], [ET_WeightagePercentage]) VALUES (N'aaaa1111-1111-1111-1111-111111111105', N'11111111-1111-1111-1111-111111111111', N'Final Exam', N'ET-FINAL', NULL, NULL)
INSERT [dbo].[ExamTypes_ET] ([ET_Id], [ET_TenantId], [ET_Name], [ET_Code], [ET_Description], [ET_WeightagePercentage]) VALUES (N'5e432f35-90c7-43aa-b7a2-c861c17a3640', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'NewPrili', N'New2000', NULL, CAST(100.00 AS Decimal(5, 2)))
INSERT [dbo].[ExamTypes_ET] ([ET_Id], [ET_TenantId], [ET_Name], [ET_Code], [ET_Description], [ET_WeightagePercentage]) VALUES (N'0b465b45-c1e0-4d75-9cfe-e9c19fcc1778', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'Abc test', N'jgfhdgs tyy', NULL, CAST(504.00 AS Decimal(5, 2)))
GO
INSERT [dbo].[ExpenseCategories_EC] ([EC_Id], [EC_TenantId], [EC_Name], [EC_Code], [EC_Description], [EC_CreatedAt], [EC_UpdatedAt]) VALUES (N'aaaa2222-2222-2222-2222-222222222201', N'11111111-1111-1111-1111-111111111111', N'Staff Salary', N'EC-SAL', N'Monthly staff salary payments', CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2), CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2))
INSERT [dbo].[ExpenseCategories_EC] ([EC_Id], [EC_TenantId], [EC_Name], [EC_Code], [EC_Description], [EC_CreatedAt], [EC_UpdatedAt]) VALUES (N'aaaa2222-2222-2222-2222-222222222202', N'11111111-1111-1111-1111-111111111111', N'Electricity Bill', N'EC-ELEC', N'Monthly electricity charges', CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2), CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2))
INSERT [dbo].[ExpenseCategories_EC] ([EC_Id], [EC_TenantId], [EC_Name], [EC_Code], [EC_Description], [EC_CreatedAt], [EC_UpdatedAt]) VALUES (N'aaaa2222-2222-2222-2222-222222222203', N'11111111-1111-1111-1111-111111111111', N'Lab Equipment', N'EC-LAB', N'Science lab equipment purchase', CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2), CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2))
INSERT [dbo].[ExpenseCategories_EC] ([EC_Id], [EC_TenantId], [EC_Name], [EC_Code], [EC_Description], [EC_CreatedAt], [EC_UpdatedAt]) VALUES (N'aaaa2222-2222-2222-2222-222222222204', N'11111111-1111-1111-1111-111111111111', N'Stationery', N'EC-STAT', N'Pens, papers, and office supplies', CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2), CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2))
INSERT [dbo].[ExpenseCategories_EC] ([EC_Id], [EC_TenantId], [EC_Name], [EC_Code], [EC_Description], [EC_CreatedAt], [EC_UpdatedAt]) VALUES (N'aaaa2222-2222-2222-2222-222222222205', N'11111111-1111-1111-1111-111111111111', N'Maintenance', N'EC-MAINT', N'Building and furniture maintenance', CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2), CAST(N'2026-08-27T02:04:42.1348487' AS DateTime2))
INSERT [dbo].[ExpenseCategories_EC] ([EC_Id], [EC_TenantId], [EC_Name], [EC_Code], [EC_Description], [EC_CreatedAt], [EC_UpdatedAt]) VALUES (N'8b77b6c0-f2b7-403f-9353-2c749031fd8a', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'dfgvbhnjmk', N'dfhvgjbhk', N'164vchfcgh', CAST(N'2026-08-29T06:44:40.6969470' AS DateTime2), CAST(N'2026-08-29T06:44:50.2309074' AS DateTime2))
GO
INSERT [dbo].[Expenses_EXP] ([EXP_Id], [EXP_TenantId], [EXP_BranchId], [EXP_ExpenseCategoryId], [EXP_VendorId], [EXP_ExpenseNumber], [EXP_ExpenseDate], [EXP_Amount], [EXP_Description], [EXP_PaymentMethodId], [EXP_CreatedBy], [EXP_CreatedAt], [EXP_UpdatedAt]) VALUES (N'ba597610-019d-4b8c-bb84-3292b880df32', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', N'aaaa2222-2222-2222-2222-222222222202', N'aaaa8888-8888-8888-8888-888888888804', N'EXP-0001', CAST(N'2026-08-30' AS Date), CAST(1000.00 AS Numeric(12, 2)), NULL, N'664ad59a-d086-4e3e-a420-98a8450ffac4', N'5e5604f8-6553-4524-bd84-6cde0d027578', CAST(N'2026-08-30T15:57:22.2959047' AS DateTime2), CAST(N'2026-08-30T15:57:37.4177684' AS DateTime2))
GO
INSERT [dbo].[FeeCategories_FC] ([FC_Id], [FC_TenantId], [FC_Name], [FC_Code], [FC_Description], [FC_IsRefundable], [FC_CreatedAt], [FC_UpdatedAt]) VALUES (N'de26da59-cd4f-4ae1-b19a-12ad5b9388b1', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'srtdryftugykjvj j', N'rzxtcyvubi ub', NULL, 1, CAST(N'2026-08-29T06:48:09.9848323' AS DateTime2), CAST(N'2026-08-29T06:48:52.4832765' AS DateTime2))
GO
INSERT [dbo].[FeeStructures_FS] ([FS_Id], [FS_TenantId], [FS_Name], [FS_Code], [FS_CourseId], [FS_BatchId], [FS_AcademicYearId], [FS_Description], [FS_IsActive], [FS_CreatedAt], [FS_UpdatedAt]) VALUES (N'4311ea18-2800-40e8-96cf-3262a1049981', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'Fee-2022', N'fee2026', N'96141a47-d36d-4bbe-b5e5-352b694f9b3d', N'e31894f1-78dd-432e-a694-bdadee54a634', N'b3d1cc8f-728d-4865-a332-a403a675e1d0', NULL, 1, CAST(N'2026-08-30T15:47:02.5037041' AS DateTime2), CAST(N'2026-08-30T15:47:02.5037041' AS DateTime2))
GO
INSERT [dbo].[GradeScales_GS] ([GS_Id], [GS_TenantId], [GS_Name], [GS_Code], [GS_Description], [GS_IsDefault]) VALUES (N'0a36f5fe-aab0-448d-a732-daff5d2711bd', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'segchvjbkn', N'zsxgchvjbk', N'xjk', 0)
GO
INSERT [dbo].[Guardians_G] ([G_Id], [G_TenantId], [G_FirstName], [G_LastName], [G_Email], [G_Phone], [G_Occupation], [G_Address], [G_CreatedAt], [G_UpdatedAt]) VALUES (N'024d63c4-c200-46aa-b2a8-75788a0ee01d', N'4150d676-e932-49af-a9d3-dc673fc1891d', N' wed wje', N'ejfbe ', N'msd2568@gmail.com', N'78496464', N'3fngib ', NULL, CAST(N'2026-08-29T12:03:46.5084238' AS DateTime2), CAST(N'2026-08-29T12:03:46.5084238' AS DateTime2))
GO
INSERT [dbo].[NotificationTemplates_NT] ([NT_Id], [NT_TenantId], [NT_Name], [NT_EventKey], [NT_Channel], [NT_Subject], [NT_BodyTemplate], [NT_IsActive], [NT_CreatedAt], [NT_UpdatedAt]) VALUES (N'aaaa9999-9999-9999-9999-999999999901', N'11111111-1111-1111-1111-111111111111', N'Fee Receipt', N'NT-FEE', N'Email', N'Fee Payment Receipt', N'Dear {StudentName}, your fee payment of {Amount} has been received. Receipt #{ReceiptNo}.', 1, CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2), CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2))
INSERT [dbo].[NotificationTemplates_NT] ([NT_Id], [NT_TenantId], [NT_Name], [NT_EventKey], [NT_Channel], [NT_Subject], [NT_BodyTemplate], [NT_IsActive], [NT_CreatedAt], [NT_UpdatedAt]) VALUES (N'aaaa9999-9999-9999-9999-999999999902', N'11111111-1111-1111-1111-111111111111', N'Exam Schedule', N'NT-EXAM', N'Email', N'Upcoming Exam Schedule', N'Dear {StudentName}, your {ExamName} is scheduled on {ExamDate}. Please prepare well.', 1, CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2), CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2))
INSERT [dbo].[NotificationTemplates_NT] ([NT_Id], [NT_TenantId], [NT_Name], [NT_EventKey], [NT_Channel], [NT_Subject], [NT_BodyTemplate], [NT_IsActive], [NT_CreatedAt], [NT_UpdatedAt]) VALUES (N'aaaa9999-9999-9999-9999-999999999903', N'11111111-1111-1111-1111-111111111111', N'Attendance Alert', N'NT-ATT', N'SMS', N'Attendance Alert', N'Dear Parent, {StudentName} was marked absent on {Date}.', 1, CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2), CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2))
INSERT [dbo].[NotificationTemplates_NT] ([NT_Id], [NT_TenantId], [NT_Name], [NT_EventKey], [NT_Channel], [NT_Subject], [NT_BodyTemplate], [NT_IsActive], [NT_CreatedAt], [NT_UpdatedAt]) VALUES (N'aaaa9999-9999-9999-9999-999999999904', N'11111111-1111-1111-1111-111111111111', N'Admission Confirmation', N'NT-ADM', N'Email', N'Admission Confirmed', N'Dear {StudentName}, your admission to {CourseName} has been confirmed. Welcome aboard!', 1, CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2), CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2))
INSERT [dbo].[NotificationTemplates_NT] ([NT_Id], [NT_TenantId], [NT_Name], [NT_EventKey], [NT_Channel], [NT_Subject], [NT_BodyTemplate], [NT_IsActive], [NT_CreatedAt], [NT_UpdatedAt]) VALUES (N'aaaa9999-9999-9999-9999-999999999905', N'11111111-1111-1111-1111-111111111111', N'Result Published', N'NT-RESULT', N'Email', N'Exam Results Published', N'Dear {StudentName}, your {ExamName} results are now available. Percentage: {Percentage}%.', 1, CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2), CAST(N'2026-08-27T02:11:20.0736214' AS DateTime2))
INSERT [dbo].[NotificationTemplates_NT] ([NT_Id], [NT_TenantId], [NT_Name], [NT_EventKey], [NT_Channel], [NT_Subject], [NT_BodyTemplate], [NT_IsActive], [NT_CreatedAt], [NT_UpdatedAt]) VALUES (N'6b338505-40ac-436e-b4a6-b0aaca9bb277', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'dsdgfhgj', N'FEE_RECEIPT', N'sms', N'andbdh', N'dxgcfhvjb', 1, CAST(N'2026-08-29T07:29:17.1884229' AS DateTime2), CAST(N'2026-08-29T07:35:09.2098038' AS DateTime2))
GO
INSERT [dbo].[PaymentMethods_PM] ([PM_Id], [PM_TenantId], [PM_Name], [PM_Type], [PM_IsActive]) VALUES (N'90556563-6f87-4b21-a52d-2a744ef843c7', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'xzs5133', N'cash', 1)
INSERT [dbo].[PaymentMethods_PM] ([PM_Id], [PM_TenantId], [PM_Name], [PM_Type], [PM_IsActive]) VALUES (N'1fc9bfc8-4d18-4027-a137-935ff92a373e', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'xzs5121', N'card', 1)
INSERT [dbo].[PaymentMethods_PM] ([PM_Id], [PM_TenantId], [PM_Name], [PM_Type], [PM_IsActive]) VALUES (N'664ad59a-d086-4e3e-a420-98a8450ffac4', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'xzs51', N'card', 1)
INSERT [dbo].[PaymentMethods_PM] ([PM_Id], [PM_TenantId], [PM_Name], [PM_Type], [PM_IsActive]) VALUES (N'f996edbd-bb8f-43d6-ae73-cb69807cde7e', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'xzsrdtfyguhij', N'cash', 0)
GO
INSERT [dbo].[Programs_P] ([P_Id], [P_TenantId], [P_Name], [P_Code], [P_Description], [P_DurationValue], [P_DurationUnit], [P_Status], [P_CreatedAt], [P_UpdatedAt]) VALUES (N'44444444-4444-4444-4444-444444444401', N'11111111-1111-1111-1111-111111111111', N'Bachelor of Science', N'P-BSC', N'Undergraduate science program', 3, N'Years', N'active', CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2), CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2))
INSERT [dbo].[Programs_P] ([P_Id], [P_TenantId], [P_Name], [P_Code], [P_Description], [P_DurationValue], [P_DurationUnit], [P_Status], [P_CreatedAt], [P_UpdatedAt]) VALUES (N'44444444-4444-4444-4444-444444444402', N'11111111-1111-1111-1111-111111111111', N'Bachelor of Arts', N'P-BA', N'Undergraduate arts program', 3, N'Years', N'active', CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2), CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2))
INSERT [dbo].[Programs_P] ([P_Id], [P_TenantId], [P_Name], [P_Code], [P_Description], [P_DurationValue], [P_DurationUnit], [P_Status], [P_CreatedAt], [P_UpdatedAt]) VALUES (N'44444444-4444-4444-4444-444444444403', N'11111111-1111-1111-1111-111111111111', N'Master of Science', N'P-MSC', N'Postgraduate science program', 2, N'Years', N'active', CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2), CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2))
INSERT [dbo].[Programs_P] ([P_Id], [P_TenantId], [P_Name], [P_Code], [P_Description], [P_DurationValue], [P_DurationUnit], [P_Status], [P_CreatedAt], [P_UpdatedAt]) VALUES (N'44444444-4444-4444-4444-444444444404', N'11111111-1111-1111-1111-111111111111', N'Diploma in Computer Science', N'P-DCS', N'One-year diploma program', 1, N'Years', N'active', CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2), CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2))
INSERT [dbo].[Programs_P] ([P_Id], [P_TenantId], [P_Name], [P_Code], [P_Description], [P_DurationValue], [P_DurationUnit], [P_Status], [P_CreatedAt], [P_UpdatedAt]) VALUES (N'44444444-4444-4444-4444-444444444405', N'11111111-1111-1111-1111-111111111111', N'Higher Secondary', N'P-HSC', N'11th and 12th grade program', 2, N'Years', N'active', CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2), CAST(N'2026-08-27T02:00:15.0912798' AS DateTime2))
INSERT [dbo].[Programs_P] ([P_Id], [P_TenantId], [P_Name], [P_Code], [P_Description], [P_DurationValue], [P_DurationUnit], [P_Status], [P_CreatedAt], [P_UpdatedAt]) VALUES (N'b020e0ed-e4cc-4bf8-bd2d-a558f075cac1', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'AAAAAAAAAAAAA', N'hgfd', N'rgdhfgk', 2, N'Years', N'inactive', CAST(N'2026-08-29T07:24:39.3431131' AS DateTime2), CAST(N'2026-08-29T07:25:01.0486170' AS DateTime2))
GO
INSERT [dbo].[Staff_ST] ([ST_Id], [ST_TenantId], [ST_BranchId], [ST_UserId], [ST_DepartmentId], [ST_DesignationId], [ST_EmployeeCode], [ST_FirstName], [ST_LastName], [ST_Email], [ST_Phone], [ST_JoiningDate], [ST_Status], [ST_CreatedAt], [ST_UpdatedAt], [ST_DeletedAt]) VALUES (N'1b3b5cfd-af27-4869-9a97-d2f9aac167ef', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', NULL, N'9ab421bf-7acd-4abd-8351-3b02cad3ddd6', N'66666666-6666-6666-6666-666666666603', N'651320', N'xzcv', N'Sujit', N'msd20958@gmail.com', N'4896514654656', CAST(N'2026-08-01' AS Date), N'Active', CAST(N'2026-08-30T12:57:47.0861197' AS DateTime2), CAST(N'2026-08-30T12:57:47.0861197' AS DateTime2), NULL)
GO
INSERT [dbo].[Students_Guardians] ([SG_Id], [SG_StudentId], [SG_GuardianId], [SG_Relation], [SG_IsPrimary], [SG_CreatedAt]) VALUES (N'cbd14a5c-395f-4d1c-a459-1416b047a17a', N'33449bb9-af6f-411d-bde7-615e2e2fb9e1', N'024d63c4-c200-46aa-b2a8-75788a0ee01d', N'Father', 1, CAST(N'2026-08-29T12:03:46.5351270' AS DateTime2))
GO
INSERT [dbo].[Students_S] ([S_Id], [S_TenantId], [S_BranchId], [S_UserId], [S_StudentCode], [S_AdmissionNumber], [S_FirstName], [S_MiddleName], [S_LastName], [S_DateOfBirth], [S_Gender], [S_Email], [S_Phone], [S_AdmissionDate], [S_Status], [S_CreatedAt], [S_UpdatedAt], [S_DeletedAt], [S_ClassId], [S_SectionId], [S_BloodGroup], [S_AddressLine1], [S_AddressLine2], [S_City], [S_State], [S_PostalCode], [S_Country]) VALUES (N'33449bb9-af6f-411d-bde7-615e2e2fb9e1', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', NULL, N'STU-260829-09FE', N'ADM-2026-0001', N'Sujit', NULL, N'Das', CAST(N'2021-02-09' AS Date), N'Male', N'msd20958@gmail.com', N'9748586828', CAST(N'2026-08-29' AS Date), N'Active', CAST(N'2026-08-29T12:03:46.4875693' AS DateTime2), CAST(N'2026-08-29T12:03:46.4875693' AS DateTime2), NULL, N'33333333-3333-3333-3333-333333333301', N'44444444-4444-4444-4444-444444444401', N'A+', N'Kolkata', N'Kolkata', N'Kolkata', N'WEST', N'736120', N'jhdf')
GO
INSERT [dbo].[Subjects_SB] ([SB_Id], [SB_TenantId], [SB_Name], [SB_Code], [SB_Description], [SB_Credits], [SB_MaxMarks], [SB_PassMarks], [SB_CreatedAt], [SB_UpdatedAt]) VALUES (N'1aff9516-a518-44a7-8fbf-000a4e145607', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'abc 464', N'abs', N'10rytguh', CAST(10.00 AS Numeric(5, 2)), CAST(10.00 AS Numeric(8, 2)), CAST(5.00 AS Numeric(8, 2)), CAST(N'2026-08-29T07:23:40.6576757' AS DateTime2), CAST(N'2026-08-29T07:23:53.5314107' AS DateTime2))
INSERT [dbo].[Subjects_SB] ([SB_Id], [SB_TenantId], [SB_Name], [SB_Code], [SB_Description], [SB_Credits], [SB_MaxMarks], [SB_PassMarks], [SB_CreatedAt], [SB_UpdatedAt]) VALUES (N'88888888-8888-8888-8888-888888888801', N'11111111-1111-1111-1111-111111111111', N'Mathematics', N'SB-MATH', N'Core mathematics subject', CAST(4.00 AS Numeric(5, 2)), CAST(100.00 AS Numeric(8, 2)), CAST(40.00 AS Numeric(8, 2)), CAST(N'2026-08-27T02:01:33.9098378' AS DateTime2), CAST(N'2026-08-27T02:01:33.9098378' AS DateTime2))
INSERT [dbo].[Subjects_SB] ([SB_Id], [SB_TenantId], [SB_Name], [SB_Code], [SB_Description], [SB_Credits], [SB_MaxMarks], [SB_PassMarks], [SB_CreatedAt], [SB_UpdatedAt]) VALUES (N'88888888-8888-8888-8888-888888888802', N'11111111-1111-1111-1111-111111111111', N'Physics', N'SB-PHYS', N'Classical and modern physics', CAST(4.00 AS Numeric(5, 2)), CAST(100.00 AS Numeric(8, 2)), CAST(40.00 AS Numeric(8, 2)), CAST(N'2026-08-27T02:01:33.9098378' AS DateTime2), CAST(N'2026-08-27T02:16:03.3495166' AS DateTime2))
INSERT [dbo].[Subjects_SB] ([SB_Id], [SB_TenantId], [SB_Name], [SB_Code], [SB_Description], [SB_Credits], [SB_MaxMarks], [SB_PassMarks], [SB_CreatedAt], [SB_UpdatedAt]) VALUES (N'88888888-8888-8888-8888-888888888803', N'11111111-1111-1111-1111-111111111111', N'Computer Science', N'SB-CS', N'Programming and algorithms', CAST(4.00 AS Numeric(5, 2)), CAST(100.00 AS Numeric(8, 2)), CAST(40.00 AS Numeric(8, 2)), CAST(N'2026-08-27T02:01:33.9098378' AS DateTime2), CAST(N'2026-08-27T02:01:33.9098378' AS DateTime2))
INSERT [dbo].[Subjects_SB] ([SB_Id], [SB_TenantId], [SB_Name], [SB_Code], [SB_Description], [SB_Credits], [SB_MaxMarks], [SB_PassMarks], [SB_CreatedAt], [SB_UpdatedAt]) VALUES (N'88888888-8888-8888-8888-888888888804', N'11111111-1111-1111-1111-111111111111', N'English', N'SB-ENG', N'Language and literature', CAST(3.00 AS Numeric(5, 2)), CAST(100.00 AS Numeric(8, 2)), CAST(40.00 AS Numeric(8, 2)), CAST(N'2026-08-27T02:01:33.9098378' AS DateTime2), CAST(N'2026-08-27T02:01:33.9098378' AS DateTime2))
INSERT [dbo].[Subjects_SB] ([SB_Id], [SB_TenantId], [SB_Name], [SB_Code], [SB_Description], [SB_Credits], [SB_MaxMarks], [SB_PassMarks], [SB_CreatedAt], [SB_UpdatedAt]) VALUES (N'88888888-8888-8888-8888-888888888805', N'11111111-1111-1111-1111-111111111111', N'Chemistry', N'SB-CHEM', N'General chemistry', CAST(4.00 AS Numeric(5, 2)), CAST(100.00 AS Numeric(8, 2)), CAST(40.00 AS Numeric(8, 2)), CAST(N'2026-08-27T02:01:33.9098378' AS DateTime2), CAST(N'2026-08-27T02:01:33.9098378' AS DateTime2))
GO
INSERT [dbo].[TeacherAttendance_TA] ([TA_Id], [TA_TenantId], [TA_TeacherId], [TA_Date], [TA_Status], [TA_Remarks], [TA_MarkedBy], [TA_CreatedAt], [TA_UpdatedAt]) VALUES (N'a8d11343-422c-48bc-9707-a0615e16e3fa', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'c8b0a84f-e745-469b-93c7-fcd16d113cae', CAST(N'2026-09-05' AS Date), N'OnLeave', N'fyguyi', N'ce0ff4e4-6f5b-472b-84b2-59204f71ef46', CAST(N'2026-09-05T16:39:53.2551792' AS DateTime2), CAST(N'2026-09-05T16:39:53.2551792' AS DateTime2))
INSERT [dbo].[TeacherAttendance_TA] ([TA_Id], [TA_TenantId], [TA_TeacherId], [TA_Date], [TA_Status], [TA_Remarks], [TA_MarkedBy], [TA_CreatedAt], [TA_UpdatedAt]) VALUES (N'aafac5cb-0ad7-49f9-a94a-c4c37dcb5c37', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'dab2bf20-4c53-4ce6-a046-3440c7e9aa98', CAST(N'2026-09-06' AS Date), N'Present', NULL, N'ce0ff4e4-6f5b-472b-84b2-59204f71ef46', CAST(N'2026-09-06T14:20:23.9676587' AS DateTime2), CAST(N'2026-09-06T14:20:23.9676587' AS DateTime2))
INSERT [dbo].[TeacherAttendance_TA] ([TA_Id], [TA_TenantId], [TA_TeacherId], [TA_Date], [TA_Status], [TA_Remarks], [TA_MarkedBy], [TA_CreatedAt], [TA_UpdatedAt]) VALUES (N'c93ecc24-75c3-45c6-8d1a-e0b568aebb24', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'dab2bf20-4c53-4ce6-a046-3440c7e9aa98', CAST(N'2026-09-05' AS Date), N'Present', NULL, N'ce0ff4e4-6f5b-472b-84b2-59204f71ef46', CAST(N'2026-09-05T16:40:08.9683103' AS DateTime2), CAST(N'2026-09-05T16:40:08.9683103' AS DateTime2))
INSERT [dbo].[TeacherAttendance_TA] ([TA_Id], [TA_TenantId], [TA_TeacherId], [TA_Date], [TA_Status], [TA_Remarks], [TA_MarkedBy], [TA_CreatedAt], [TA_UpdatedAt]) VALUES (N'97b2f751-1d5c-48ae-8ac7-e2947460d304', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'c8b0a84f-e745-469b-93c7-fcd16d113cae', CAST(N'2026-09-06' AS Date), N'Present', NULL, N'ce0ff4e4-6f5b-472b-84b2-59204f71ef46', CAST(N'2026-09-06T14:20:14.2505573' AS DateTime2), CAST(N'2026-09-06T14:20:14.2505573' AS DateTime2))
GO
INSERT [dbo].[TeacherLeaves_TL] ([TL_Id], [TL_TenantId], [TL_TeacherId], [TL_TeacherName], [TL_LeaveType], [TL_FromDate], [TL_ToDate], [TL_TotalDays], [TL_Reason], [TL_Status], [TL_AppliedAt], [TL_AppliedBy], [TL_ApprovedBy], [TL_ApprovedAt], [TL_RejectionReason], [TL_CreatedAt], [TL_UpdatedAt]) VALUES (N'af4e80f2-4bf3-4468-afdd-9ac1a78cc1d2', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'c8b0a84f-e745-469b-93c7-fcd16d113cae', N'Anika Suarez', N'Sick', CAST(N'2026-09-04' AS Date), CAST(N'2026-09-05' AS Date), 2, N'rydtcuvibuln', N'Approved', CAST(N'2026-09-05T16:33:56.3205261' AS DateTime2), N'c8b0a84f-e745-469b-93c7-fcd16d113cae', N'ce0ff4e4-6f5b-472b-84b2-59204f71ef46', CAST(N'2026-09-05T16:39:04.3101526' AS DateTime2), NULL, CAST(N'2026-09-05T16:33:56.3205261' AS DateTime2), CAST(N'2026-09-05T16:39:04.3101526' AS DateTime2))
GO
INSERT [dbo].[Teachers_T] ([T_Id], [T_TenantId], [T_BranchId], [T_EmployeeCode], [T_Designation], [T_Department], [T_JoiningDate], [T_Qualification], [T_ExperienceYears], [T_BloodGroup], [T_Status], [T_IsActive], [T_CreatedAt], [T_UpdatedAt]) VALUES (N'dab2bf20-4c53-4ce6-a046-3440c7e9aa98', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222202', N'EMP-260905-C52F', N'Teacher', N'Bengali', CAST(N'2026-08-31' AS Date), N'M Sc', 6, N'B+', N'Active', 1, CAST(N'2026-09-05T16:31:49.1787326' AS DateTime2), CAST(N'2026-09-05T16:32:13.7813778' AS DateTime2))
INSERT [dbo].[Teachers_T] ([T_Id], [T_TenantId], [T_BranchId], [T_EmployeeCode], [T_Designation], [T_Department], [T_JoiningDate], [T_Qualification], [T_ExperienceYears], [T_BloodGroup], [T_Status], [T_IsActive], [T_CreatedAt], [T_UpdatedAt]) VALUES (N'c8b0a84f-e745-469b-93c7-fcd16d113cae', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', N'EMP-260905-2528', N'Teacher', NULL, CAST(N'2026-09-05' AS Date), N'BA ', 55, N'A+', N'Active', 1, CAST(N'2026-09-05T16:30:04.3603004' AS DateTime2), CAST(N'2026-09-05T16:32:30.3416059' AS DateTime2))
GO
INSERT [dbo].[Timetables_TT] ([TT_Id], [TT_TenantId], [TT_BranchId], [TT_BatchId], [TT_SubjectId], [TT_StaffId], [TT_ClassroomId], [TT_DayOfWeek], [TT_StartTime], [TT_EndTime], [TT_EffectiveFrom], [TT_EffectiveTo], [TT_CreatedAt], [TT_UpdatedAt]) VALUES (N'd9f03170-2bb4-4914-942a-18e255407e10', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', N'e31894f1-78dd-432e-a694-bdadee54a634', N'88888888-8888-8888-8888-888888888803', N'1b3b5cfd-af27-4869-9a97-d2f9aac167ef', N'e412161e-9361-4818-ae2e-22fa9210e7b1', 1, CAST(N'20:52:00' AS Time), CAST(N'21:52:00' AS Time), CAST(N'2026-08-01' AS Date), CAST(N'2026-09-30' AS Date), CAST(N'2026-08-30T15:23:51.9000000' AS DateTime2), CAST(N'2026-08-30T15:23:51.9000000' AS DateTime2))
INSERT [dbo].[Timetables_TT] ([TT_Id], [TT_TenantId], [TT_BranchId], [TT_BatchId], [TT_SubjectId], [TT_StaffId], [TT_ClassroomId], [TT_DayOfWeek], [TT_StartTime], [TT_EndTime], [TT_EffectiveFrom], [TT_EffectiveTo], [TT_CreatedAt], [TT_UpdatedAt]) VALUES (N'35bcb454-36a2-4fb4-a03d-32be83c3bb1c', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'22222222-2222-2222-2222-222222222201', N'e31894f1-78dd-432e-a694-bdadee54a634', N'1aff9516-a518-44a7-8fbf-000a4e145607', N'1b3b5cfd-af27-4869-9a97-d2f9aac167ef', N'e412161e-9361-4818-ae2e-22fa9210e7b1', 1, CAST(N'20:32:00' AS Time), CAST(N'20:34:00' AS Time), CAST(N'2026-08-01' AS Date), CAST(N'2026-08-15' AS Date), CAST(N'2026-08-30T15:02:52.8000000' AS DateTime2), CAST(N'2026-08-30T15:14:17.4366667' AS DateTime2))
GO
INSERT [dbo].[Vendors_V] ([V_Id], [V_TenantId], [V_Name], [V_Code], [V_Email], [V_Phone], [V_Address], [V_TaxNumber], [V_CreatedAt], [V_UpdatedAt]) VALUES (N'9c1d3df2-08cd-4e00-a133-4aa63de756d0', N'501cbcdf-b2ee-498b-9c2d-dec9402d44cc', N'AADtyu', N'ABD45', N'team.fastapps@gmail.com', N'567', NULL, N'-4567890', CAST(N'2026-08-29T07:25:31.3147515' AS DateTime2), CAST(N'2026-08-29T07:26:03.3953012' AS DateTime2))
INSERT [dbo].[Vendors_V] ([V_Id], [V_TenantId], [V_Name], [V_Code], [V_Email], [V_Phone], [V_Address], [V_TaxNumber], [V_CreatedAt], [V_UpdatedAt]) VALUES (N'aaaa8888-8888-8888-8888-888888888801', N'11111111-1111-1111-1111-111111111111', N'PrintWorld', N'VEN-PRT', N'info@printworld.com', N'9110000001', N'12 Printing Press Lane, Mumbai', N'GSTIN27AABCU9603R1ZM', CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2), CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2))
INSERT [dbo].[Vendors_V] ([V_Id], [V_TenantId], [V_Name], [V_Code], [V_Email], [V_Phone], [V_Address], [V_TaxNumber], [V_CreatedAt], [V_UpdatedAt]) VALUES (N'aaaa8888-8888-8888-8888-888888888802', N'11111111-1111-1111-1111-111111111111', N'TechSupply India', N'VEN-TECH', N'sales@techsupply.in', N'9110000002', N'45 IT Park Road, Delhi', N'GSTIN07AACCT1234F1ZP', CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2), CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2))
INSERT [dbo].[Vendors_V] ([V_Id], [V_TenantId], [V_Name], [V_Code], [V_Email], [V_Phone], [V_Address], [V_TaxNumber], [V_CreatedAt], [V_UpdatedAt]) VALUES (N'aaaa8888-8888-8888-8888-888888888803', N'11111111-1111-1111-1111-111111111111', N'FurniturePlus', N'VEN-FUR', N'contact@furnplus.com', N'9110000003', N'78 Industrial Area, Chennai', N'GSTIN33BBBFU5678G1ZQ', CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2), CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2))
INSERT [dbo].[Vendors_V] ([V_Id], [V_TenantId], [V_Name], [V_Code], [V_Email], [V_Phone], [V_Address], [V_TaxNumber], [V_CreatedAt], [V_UpdatedAt]) VALUES (N'aaaa8888-8888-8888-8888-888888888804', N'11111111-1111-1111-1111-111111111111', N'CleanFresh Services', N'VEN-CLN', N'hello@cleanfresh.in', N'9110000004', N'90 Service Block, Kolkata', N'GSTIN19CCCFG9012H1ZR', CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2), CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2))
INSERT [dbo].[Vendors_V] ([V_Id], [V_TenantId], [V_Name], [V_Code], [V_Email], [V_Phone], [V_Address], [V_TaxNumber], [V_CreatedAt], [V_UpdatedAt]) VALUES (N'aaaa8888-8888-8888-8888-888888888805', N'11111111-1111-1111-1111-111111111111', N'EduBooks Publishers', N'VEN-EDU', N'orders@edubooks.in', N'9110000005', N'23 Book Market, Pune', N'GSTIN24DDD EB3456I1ZS', CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2), CAST(N'2026-08-27T02:11:08.5757819' AS DateTime2))
INSERT [dbo].[Vendors_V] ([V_Id], [V_TenantId], [V_Name], [V_Code], [V_Email], [V_Phone], [V_Address], [V_TaxNumber], [V_CreatedAt], [V_UpdatedAt]) VALUES (N'600a5ca2-bb04-4017-b409-a64203e5ae1f', N'4150d676-e932-49af-a9d3-dc673fc1891d', N'hvjk', N'vjbk', N'', N'', N'', N'', CAST(N'2026-08-27T02:16:24.1974163' AS DateTime2), CAST(N'2026-08-27T02:16:24.1974163' AS DateTime2))
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_AcademicYears_AY_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[AcademicYears_AY] ADD  CONSTRAINT [UQ_AcademicYears_AY_Org_Code] UNIQUE NONCLUSTERED 
(
	[AY_TenantId] ASC,
	[AY_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_AdmissionApplicationDocuments_AAD_App_DocType]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[AdmissionApplicationDocuments_AAD] ADD  CONSTRAINT [UQ_AdmissionApplicationDocuments_AAD_App_DocType] UNIQUE NONCLUSTERED 
(
	[AAD_ApplicationId] ASC,
	[AAD_DocumentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_AdmissionApplications_AA_Org_ApplicationNumber]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[AdmissionApplications_AA] ADD  CONSTRAINT [UQ_AdmissionApplications_AA_Org_ApplicationNumber] UNIQUE NONCLUSTERED 
(
	[AA_TenantId] ASC,
	[AA_ApplicationNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_AttendanceRecords_AR_Session_Student]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[AttendanceRecords_AR] ADD  CONSTRAINT [UQ_AttendanceRecords_AR_Session_Student] UNIQUE NONCLUSTERED 
(
	[AR_AttendanceSessionId] ASC,
	[AR_StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Batches_BT_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Batches_BT] ADD  CONSTRAINT [UQ_Batches_BT_Org_Code] UNIQUE NONCLUSTERED 
(
	[BT_TenantId] ASC,
	[BT_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Branches_B_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Branches_B] ADD  CONSTRAINT [UQ_Branches_B_Org_Code] UNIQUE NONCLUSTERED 
(
	[B_TenantId] ASC,
	[B_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Classrooms_CR_Branch_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Classrooms_CR] ADD  CONSTRAINT [UQ_Classrooms_CR_Branch_Code] UNIQUE NONCLUSTERED 
(
	[CR_BranchId] ASC,
	[CR_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Courses_C_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Courses_C] ADD  CONSTRAINT [UQ_Courses_C_Org_Code] UNIQUE NONCLUSTERED 
(
	[C_TenantId] ASC,
	[C_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_CourseSubjects_CS_Course_SequenceNo]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[CourseSubjects_CS] ADD  CONSTRAINT [UQ_CourseSubjects_CS_Course_SequenceNo] UNIQUE NONCLUSTERED 
(
	[CS_CourseId] ASC,
	[CS_SequenceNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_CustomFields_CF_Org_EntityType_FieldKey]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[CustomFields_CF] ADD  CONSTRAINT [UQ_CustomFields_CF_Org_EntityType_FieldKey] UNIQUE NONCLUSTERED 
(
	[CF_TenantId] ASC,
	[CF_EntityType] ASC,
	[CF_FieldKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_CustomFieldValues_CFV_Field_Entity]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[CustomFieldValues_CFV] ADD  CONSTRAINT [UQ_CustomFieldValues_CFV_Field_Entity] UNIQUE NONCLUSTERED 
(
	[CFV_CustomFieldId] ASC,
	[CFV_EntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Departments_D_Branch_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Departments_D] ADD  CONSTRAINT [UQ_Departments_D_Branch_Code] UNIQUE NONCLUSTERED 
(
	[D_BranchId] ASC,
	[D_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Designations_DS_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Designations_DS] ADD  CONSTRAINT [UQ_Designations_DS_Org_Code] UNIQUE NONCLUSTERED 
(
	[DS_TenantId] ASC,
	[DS_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Discounts_DIS_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Discounts_DIS] ADD  CONSTRAINT [UQ_Discounts_DIS_Org_Code] UNIQUE NONCLUSTERED 
(
	[DIS_TenantId] ASC,
	[DIS_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_DocumentTypes_DT_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[DocumentTypes_DT] ADD  CONSTRAINT [UQ_DocumentTypes_DT_Org_Code] UNIQUE NONCLUSTERED 
(
	[DT_TenantId] ASC,
	[DT_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Enrollments_E_Org_EnrollmentNumber]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Enrollments_E] ADD  CONSTRAINT [UQ_Enrollments_E_Org_EnrollmentNumber] UNIQUE NONCLUSTERED 
(
	[E_TenantId] ASC,
	[E_EnrollmentNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_Enrollments_E_Student_AcademicYear_Course]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Enrollments_E] ADD  CONSTRAINT [UQ_Enrollments_E_Student_AcademicYear_Course] UNIQUE NONCLUSTERED 
(
	[E_StudentId] ASC,
	[E_AcademicYearId] ASC,
	[E_CourseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Exams_EX_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Exams_EX] ADD  CONSTRAINT [UQ_Exams_EX_Org_Code] UNIQUE NONCLUSTERED 
(
	[EX_TenantId] ASC,
	[EX_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_ExamSchedules_ESC_ExamSubjectId]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[ExamSchedules_ESC] ADD  CONSTRAINT [UQ_ExamSchedules_ESC_ExamSubjectId] UNIQUE NONCLUSTERED 
(
	[ESC_ExamSubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_ExamSubjects_ES_Exam_Subject]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[ExamSubjects_ES] ADD  CONSTRAINT [UQ_ExamSubjects_ES_Exam_Subject] UNIQUE NONCLUSTERED 
(
	[ES_ExamId] ASC,
	[ES_SubjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_ExamTypes_ET_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[ExamTypes_ET] ADD  CONSTRAINT [UQ_ExamTypes_ET_Org_Code] UNIQUE NONCLUSTERED 
(
	[ET_TenantId] ASC,
	[ET_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_ExpenseCategories_EC_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[ExpenseCategories_EC] ADD  CONSTRAINT [UQ_ExpenseCategories_EC_Org_Code] UNIQUE NONCLUSTERED 
(
	[EC_TenantId] ASC,
	[EC_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Expenses_EXP_Org_ExpenseNumber]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Expenses_EXP] ADD  CONSTRAINT [UQ_Expenses_EXP_Org_ExpenseNumber] UNIQUE NONCLUSTERED 
(
	[EXP_TenantId] ASC,
	[EXP_ExpenseNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_FeeCategories_FC_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[FeeCategories_FC] ADD  CONSTRAINT [UQ_FeeCategories_FC_Org_Code] UNIQUE NONCLUSTERED 
(
	[FC_TenantId] ASC,
	[FC_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_FeeInvoices_FI_Org_InvoiceNumber]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[FeeInvoices_FI] ADD  CONSTRAINT [UQ_FeeInvoices_FI_Org_InvoiceNumber] UNIQUE NONCLUSTERED 
(
	[FI_TenantId] ASC,
	[FI_InvoiceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_FeeStructureItems_FSI_Structure_Category]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[FeeStructureItems_FSI] ADD  CONSTRAINT [UQ_FeeStructureItems_FSI_Structure_Category] UNIQUE NONCLUSTERED 
(
	[FSI_FeeStructureId] ASC,
	[FSI_FeeCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_FeeStructures_FS_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[FeeStructures_FS] ADD  CONSTRAINT [UQ_FeeStructures_FS_Org_Code] UNIQUE NONCLUSTERED 
(
	[FS_TenantId] ASC,
	[FS_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_GradeScaleItems_GSI_Scale_Grade]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[GradeScaleItems_GSI] ADD  CONSTRAINT [UQ_GradeScaleItems_GSI_Scale_Grade] UNIQUE NONCLUSTERED 
(
	[GSI_GradeScaleId] ASC,
	[GSI_Grade] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_GradeScales_GS_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[GradeScales_GS] ADD  CONSTRAINT [UQ_GradeScales_GS_Org_Code] UNIQUE NONCLUSTERED 
(
	[GS_TenantId] ASC,
	[GS_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_Marks_M_ExamSubject_Student]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Marks_M] ADD  CONSTRAINT [UQ_Marks_M_ExamSubject_Student] UNIQUE NONCLUSTERED 
(
	[M_ExamSubjectId] ASC,
	[M_StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_NotificationTemplates_NT_Org_EventKey_Channel]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[NotificationTemplates_NT] ADD  CONSTRAINT [UQ_NotificationTemplates_NT_Org_EventKey_Channel] UNIQUE NONCLUSTERED 
(
	[NT_TenantId] ASC,
	[NT_EventKey] ASC,
	[NT_Channel] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Organizations_O_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Organizations_O] ADD  CONSTRAINT [UQ_Organizations_O_Code] UNIQUE NONCLUSTERED 
(
	[O_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Organizations_O_Email]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Organizations_O] ADD  CONSTRAINT [UQ_Organizations_O_Email] UNIQUE NONCLUSTERED 
(
	[O_Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_OrganizationSettings_OS_Org_Key]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[OrganizationSettings_OS] ADD  CONSTRAINT [UQ_OrganizationSettings_OS_Org_Key] UNIQUE NONCLUSTERED 
(
	[OS_TenantId] ASC,
	[OS_SettingKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_PaymentAllocations_PA_Payment_Invoice]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[PaymentAllocations_PA] ADD  CONSTRAINT [UQ_PaymentAllocations_PA_Payment_Invoice] UNIQUE NONCLUSTERED 
(
	[PA_PaymentId] ASC,
	[PA_InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_PaymentMethods_PM_Org_Name]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[PaymentMethods_PM] ADD  CONSTRAINT [UQ_PaymentMethods_PM_Org_Name] UNIQUE NONCLUSTERED 
(
	[PM_TenantId] ASC,
	[PM_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Payments_PAY_Org_PaymentNumber]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Payments_PAY] ADD  CONSTRAINT [UQ_Payments_PAY_Org_PaymentNumber] UNIQUE NONCLUSTERED 
(
	[PAY_TenantId] ASC,
	[PAY_PaymentNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Programs_P_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Programs_P] ADD  CONSTRAINT [UQ_Programs_P_Org_Code] UNIQUE NONCLUSTERED 
(
	[P_TenantId] ASC,
	[P_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Refunds_RF_Org_RefundNumber]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Refunds_RF] ADD  CONSTRAINT [UQ_Refunds_RF_Org_RefundNumber] UNIQUE NONCLUSTERED 
(
	[RF_TenantId] ASC,
	[RF_RefundNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_Results_R_Exam_Student]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Results_R] ADD  CONSTRAINT [UQ_Results_R_Exam_Student] UNIQUE NONCLUSTERED 
(
	[R_ExamId] ASC,
	[R_StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_StudentFeeAssignments_SFA_Student_Structure]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] ADD  CONSTRAINT [UQ_StudentFeeAssignments_SFA_Student_Structure] UNIQUE NONCLUSTERED 
(
	[SFA_StudentId] ASC,
	[SFA_FeeStructureId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_Students_Guardians]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Students_Guardians] ADD  CONSTRAINT [UQ_Students_Guardians] UNIQUE NONCLUSTERED 
(
	[SG_StudentId] ASC,
	[SG_GuardianId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Students_S_Org_AdmissionNumber]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Students_S] ADD  CONSTRAINT [UQ_Students_S_Org_AdmissionNumber] UNIQUE NONCLUSTERED 
(
	[S_TenantId] ASC,
	[S_AdmissionNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Students_S_Org_StudentCode]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Students_S] ADD  CONSTRAINT [UQ_Students_S_Org_StudentCode] UNIQUE NONCLUSTERED 
(
	[S_TenantId] ASC,
	[S_StudentCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Subjects_SB_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Subjects_SB] ADD  CONSTRAINT [UQ_Subjects_SB_Org_Code] UNIQUE NONCLUSTERED 
(
	[SB_TenantId] ASC,
	[SB_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_TeacherAttendance_TA]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[TeacherAttendance_TA] ADD  CONSTRAINT [UQ_TeacherAttendance_TA] UNIQUE NONCLUSTERED 
(
	[TA_TenantId] ASC,
	[TA_TeacherId] ASC,
	[TA_Date] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Teachers_T_Org_EmployeeCode]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Teachers_T] ADD  CONSTRAINT [UQ_Teachers_T_Org_EmployeeCode] UNIQUE NONCLUSTERED 
(
	[T_TenantId] ASC,
	[T_EmployeeCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Vendors_V_Org_Code]    Script Date: 06-09-2026 20:21:06 ******/
ALTER TABLE [dbo].[Vendors_V] ADD  CONSTRAINT [UQ_Vendors_V_Org_Code] UNIQUE NONCLUSTERED 
(
	[V_TenantId] ASC,
	[V_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AcademicYears_AY] ADD  CONSTRAINT [DF_AcademicYears_AY_Id]  DEFAULT (newid()) FOR [AY_Id]
GO
ALTER TABLE [dbo].[AcademicYears_AY] ADD  CONSTRAINT [DF_AcademicYears_AY_IsCurrent]  DEFAULT ((0)) FOR [AY_IsCurrent]
GO
ALTER TABLE [dbo].[AcademicYears_AY] ADD  CONSTRAINT [DF_AcademicYears_AY_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [AY_CreatedAt]
GO
ALTER TABLE [dbo].[AcademicYears_AY] ADD  CONSTRAINT [DF_AcademicYears_AY_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [AY_UpdatedAt]
GO
ALTER TABLE [dbo].[ActivityLogs_ACL] ADD  CONSTRAINT [DF_ActivityLogs_ACL_Id]  DEFAULT (newid()) FOR [ACL_Id]
GO
ALTER TABLE [dbo].[ActivityLogs_ACL] ADD  CONSTRAINT [DF_ActivityLogs_ACL_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [ACL_CreatedAt]
GO
ALTER TABLE [dbo].[AdmissionApplicationDocuments_AAD] ADD  CONSTRAINT [DF_AdmissionApplicationDocuments_AAD_Id]  DEFAULT (newid()) FOR [AAD_Id]
GO
ALTER TABLE [dbo].[AdmissionApplicationDocuments_AAD] ADD  CONSTRAINT [DF_AdmissionApplicationDocuments_AAD_IsVerified]  DEFAULT ((0)) FOR [AAD_IsVerified]
GO
ALTER TABLE [dbo].[AdmissionApplications_AA] ADD  CONSTRAINT [DF_AdmissionApplications_AA_Id]  DEFAULT (newid()) FOR [AA_Id]
GO
ALTER TABLE [dbo].[AdmissionApplications_AA] ADD  CONSTRAINT [DF_AdmissionApplications_AA_Status]  DEFAULT ('submitted') FOR [AA_Status]
GO
ALTER TABLE [dbo].[AdmissionApplications_AA] ADD  CONSTRAINT [DF_AdmissionApplications_AA_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [AA_CreatedAt]
GO
ALTER TABLE [dbo].[AdmissionApplications_AA] ADD  CONSTRAINT [DF_AdmissionApplications_AA_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [AA_UpdatedAt]
GO
ALTER TABLE [dbo].[AdmissionNumberCounters_ANC] ADD  CONSTRAINT [DF_ANC_LastNumber]  DEFAULT ((0)) FOR [ANC_LastNumber]
GO
ALTER TABLE [dbo].[Announcements_ANN] ADD  CONSTRAINT [DF_Announcements_ANN_Id]  DEFAULT (newid()) FOR [ANN_Id]
GO
ALTER TABLE [dbo].[Announcements_ANN] ADD  CONSTRAINT [DF_Announcements_ANN_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [ANN_CreatedAt]
GO
ALTER TABLE [dbo].[Announcements_ANN] ADD  CONSTRAINT [DF_Announcements_ANN_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [ANN_UpdatedAt]
GO
ALTER TABLE [dbo].[AttendanceRecords_AR] ADD  CONSTRAINT [DF_AttendanceRecords_AR_Id]  DEFAULT (newid()) FOR [AR_Id]
GO
ALTER TABLE [dbo].[AttendanceRecords_AR] ADD  CONSTRAINT [DF_AttendanceRecords_AR_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [AR_CreatedAt]
GO
ALTER TABLE [dbo].[AttendanceRecords_AR] ADD  CONSTRAINT [DF_AttendanceRecords_AR_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [AR_UpdatedAt]
GO
ALTER TABLE [dbo].[AttendanceSessions_AS] ADD  CONSTRAINT [DF_AttendanceSessions_AS_Id]  DEFAULT (newid()) FOR [AS_Id]
GO
ALTER TABLE [dbo].[AttendanceSessions_AS] ADD  CONSTRAINT [DF_AttendanceSessions_AS_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [AS_CreatedAt]
GO
ALTER TABLE [dbo].[AttendanceSessions_AS] ADD  CONSTRAINT [DF_AttendanceSessions_AS_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [AS_UpdatedAt]
GO
ALTER TABLE [dbo].[AuditLogs_AL] ADD  CONSTRAINT [DF_AuditLogs_AL_Id]  DEFAULT (newid()) FOR [AL_Id]
GO
ALTER TABLE [dbo].[AuditLogs_AL] ADD  CONSTRAINT [DF_AuditLogs_AL_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [AL_CreatedAt]
GO
ALTER TABLE [dbo].[BankMaster_BM] ADD  CONSTRAINT [DF_BankMaster_BM_IsActive]  DEFAULT ((1)) FOR [BM_IsActive]
GO
ALTER TABLE [dbo].[BankMaster_BM] ADD  CONSTRAINT [DF_BankMaster_BM_CreatedAt]  DEFAULT (getutcdate()) FOR [BM_CreatedAt]
GO
ALTER TABLE [dbo].[BankMaster_BM] ADD  CONSTRAINT [DF_BankMaster_BM_UpdatedAt]  DEFAULT (getutcdate()) FOR [BM_UpdatedAt]
GO
ALTER TABLE [dbo].[Batches_BT] ADD  CONSTRAINT [DF_Batches_BT_Id]  DEFAULT (newid()) FOR [BT_Id]
GO
ALTER TABLE [dbo].[Batches_BT] ADD  CONSTRAINT [DF_Batches_BT_Status]  DEFAULT ('active') FOR [BT_Status]
GO
ALTER TABLE [dbo].[Batches_BT] ADD  CONSTRAINT [DF_Batches_BT_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [BT_CreatedAt]
GO
ALTER TABLE [dbo].[Batches_BT] ADD  CONSTRAINT [DF_Batches_BT_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [BT_UpdatedAt]
GO
ALTER TABLE [dbo].[Branches_B] ADD  CONSTRAINT [DF_Branches_B_Id]  DEFAULT (newid()) FOR [B_Id]
GO
ALTER TABLE [dbo].[Branches_B] ADD  CONSTRAINT [DF_Branches_B_Status]  DEFAULT ('active') FOR [B_Status]
GO
ALTER TABLE [dbo].[Branches_B] ADD  CONSTRAINT [DF_Branches_B_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [B_CreatedAt]
GO
ALTER TABLE [dbo].[Branches_B] ADD  CONSTRAINT [DF_Branches_B_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [B_UpdatedAt]
GO
ALTER TABLE [dbo].[Classrooms_CR] ADD  CONSTRAINT [DF_Classrooms_CR_Id]  DEFAULT (newid()) FOR [CR_Id]
GO
ALTER TABLE [dbo].[Classrooms_CR] ADD  CONSTRAINT [DF_Classrooms_CR_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CR_CreatedAt]
GO
ALTER TABLE [dbo].[Classrooms_CR] ADD  CONSTRAINT [DF_Classrooms_CR_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [CR_UpdatedAt]
GO
ALTER TABLE [dbo].[Courses_C] ADD  CONSTRAINT [DF_Courses_C_Id]  DEFAULT (newid()) FOR [C_Id]
GO
ALTER TABLE [dbo].[Courses_C] ADD  CONSTRAINT [DF_Courses_C_Status]  DEFAULT ('active') FOR [C_Status]
GO
ALTER TABLE [dbo].[Courses_C] ADD  CONSTRAINT [DF_Courses_C_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [C_CreatedAt]
GO
ALTER TABLE [dbo].[Courses_C] ADD  CONSTRAINT [DF_Courses_C_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [C_UpdatedAt]
GO
ALTER TABLE [dbo].[CourseSubjects_CS] ADD  CONSTRAINT [DF_CourseSubjects_CS_IsMandatory]  DEFAULT ((1)) FOR [CS_IsMandatory]
GO
ALTER TABLE [dbo].[CustomFields_CF] ADD  CONSTRAINT [DF_CustomFields_CF_Id]  DEFAULT (newid()) FOR [CF_Id]
GO
ALTER TABLE [dbo].[CustomFields_CF] ADD  CONSTRAINT [DF_CustomFields_CF_IsRequired]  DEFAULT ((0)) FOR [CF_IsRequired]
GO
ALTER TABLE [dbo].[CustomFields_CF] ADD  CONSTRAINT [DF_CustomFields_CF_IsActive]  DEFAULT ((1)) FOR [CF_IsActive]
GO
ALTER TABLE [dbo].[CustomFields_CF] ADD  CONSTRAINT [DF_CustomFields_CF_DisplayOrder]  DEFAULT ((0)) FOR [CF_DisplayOrder]
GO
ALTER TABLE [dbo].[CustomFields_CF] ADD  CONSTRAINT [DF_CustomFields_CF_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CF_CreatedAt]
GO
ALTER TABLE [dbo].[CustomFields_CF] ADD  CONSTRAINT [DF_CustomFields_CF_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [CF_UpdatedAt]
GO
ALTER TABLE [dbo].[CustomFieldValues_CFV] ADD  CONSTRAINT [DF_CustomFieldValues_CFV_Id]  DEFAULT (newid()) FOR [CFV_Id]
GO
ALTER TABLE [dbo].[CustomFieldValues_CFV] ADD  CONSTRAINT [DF_CustomFieldValues_CFV_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CFV_CreatedAt]
GO
ALTER TABLE [dbo].[CustomFieldValues_CFV] ADD  CONSTRAINT [DF_CustomFieldValues_CFV_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [CFV_UpdatedAt]
GO
ALTER TABLE [dbo].[Departments_D] ADD  CONSTRAINT [DF_Departments_D_Id]  DEFAULT (newid()) FOR [D_Id]
GO
ALTER TABLE [dbo].[Departments_D] ADD  CONSTRAINT [DF_Departments_D_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [D_CreatedAt]
GO
ALTER TABLE [dbo].[Departments_D] ADD  CONSTRAINT [DF_Departments_D_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [D_UpdatedAt]
GO
ALTER TABLE [dbo].[Designations_DS] ADD  CONSTRAINT [DF_Designations_DS_Id]  DEFAULT (newid()) FOR [DS_Id]
GO
ALTER TABLE [dbo].[Designations_DS] ADD  CONSTRAINT [DF_Designations_DS_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [DS_CreatedAt]
GO
ALTER TABLE [dbo].[Designations_DS] ADD  CONSTRAINT [DF_Designations_DS_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [DS_UpdatedAt]
GO
ALTER TABLE [dbo].[Discounts_DIS] ADD  CONSTRAINT [DF_Discounts_DIS_Id]  DEFAULT (newid()) FOR [DIS_Id]
GO
ALTER TABLE [dbo].[Discounts_DIS] ADD  CONSTRAINT [DF_Discounts_DIS_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [DIS_CreatedAt]
GO
ALTER TABLE [dbo].[Discounts_DIS] ADD  CONSTRAINT [DF_Discounts_DIS_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [DIS_UpdatedAt]
GO
ALTER TABLE [dbo].[Documents_DOC] ADD  CONSTRAINT [DF_Documents_DOC_Id]  DEFAULT (newid()) FOR [DOC_Id]
GO
ALTER TABLE [dbo].[Documents_DOC] ADD  CONSTRAINT [DF_Documents_DOC_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [DOC_CreatedAt]
GO
ALTER TABLE [dbo].[DocumentTypes_DT] ADD  CONSTRAINT [DF_DocumentTypes_DT_Id]  DEFAULT (newid()) FOR [DT_Id]
GO
ALTER TABLE [dbo].[DocumentTypes_DT] ADD  CONSTRAINT [DF_DocumentTypes_DT_IsRequired]  DEFAULT ((0)) FOR [DT_IsRequired]
GO
ALTER TABLE [dbo].[DocumentTypes_DT] ADD  CONSTRAINT [DF_DocumentTypes_DT_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [DT_CreatedAt]
GO
ALTER TABLE [dbo].[DocumentTypes_DT] ADD  CONSTRAINT [DF_DocumentTypes_DT_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [DT_UpdatedAt]
GO
ALTER TABLE [dbo].[Enrollments_E] ADD  CONSTRAINT [DF_Enrollments_E_Id]  DEFAULT (newid()) FOR [E_Id]
GO
ALTER TABLE [dbo].[Enrollments_E] ADD  CONSTRAINT [DF_Enrollments_E_Status]  DEFAULT ('active') FOR [E_Status]
GO
ALTER TABLE [dbo].[Enrollments_E] ADD  CONSTRAINT [DF_Enrollments_E_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [E_CreatedAt]
GO
ALTER TABLE [dbo].[Enrollments_E] ADD  CONSTRAINT [DF_Enrollments_E_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [E_UpdatedAt]
GO
ALTER TABLE [dbo].[EntityDocuments_ED] ADD  CONSTRAINT [DF_EntityDocuments_ED_Id]  DEFAULT (newid()) FOR [ED_Id]
GO
ALTER TABLE [dbo].[EntityDocuments_ED] ADD  CONSTRAINT [DF_EntityDocuments_ED_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [ED_CreatedAt]
GO
ALTER TABLE [dbo].[Exams_EX] ADD  CONSTRAINT [DF_Exams_EX_Id]  DEFAULT (newid()) FOR [EX_Id]
GO
ALTER TABLE [dbo].[Exams_EX] ADD  CONSTRAINT [DF_Exams_EX_Status]  DEFAULT ('scheduled') FOR [EX_Status]
GO
ALTER TABLE [dbo].[Exams_EX] ADD  CONSTRAINT [DF_Exams_EX_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [EX_CreatedAt]
GO
ALTER TABLE [dbo].[Exams_EX] ADD  CONSTRAINT [DF_Exams_EX_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [EX_UpdatedAt]
GO
ALTER TABLE [dbo].[ExamSchedules_ESC] ADD  CONSTRAINT [DF_ExamSchedules_ESC_Id]  DEFAULT (newid()) FOR [ESC_Id]
GO
ALTER TABLE [dbo].[ExamSubjects_ES] ADD  CONSTRAINT [DF_ExamSubjects_ES_Id]  DEFAULT (newid()) FOR [ES_Id]
GO
ALTER TABLE [dbo].[ExamTypes_ET] ADD  CONSTRAINT [DF_ExamTypes_ET_Id]  DEFAULT (newid()) FOR [ET_Id]
GO
ALTER TABLE [dbo].[ExpenseCategories_EC] ADD  CONSTRAINT [DF_ExpenseCategories_EC_Id]  DEFAULT (newid()) FOR [EC_Id]
GO
ALTER TABLE [dbo].[ExpenseCategories_EC] ADD  CONSTRAINT [DF_ExpenseCategories_EC_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [EC_CreatedAt]
GO
ALTER TABLE [dbo].[ExpenseCategories_EC] ADD  CONSTRAINT [DF_ExpenseCategories_EC_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [EC_UpdatedAt]
GO
ALTER TABLE [dbo].[Expenses_EXP] ADD  CONSTRAINT [DF_Expenses_EXP_Id]  DEFAULT (newid()) FOR [EXP_Id]
GO
ALTER TABLE [dbo].[Expenses_EXP] ADD  CONSTRAINT [DF_Expenses_EXP_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [EXP_CreatedAt]
GO
ALTER TABLE [dbo].[Expenses_EXP] ADD  CONSTRAINT [DF_Expenses_EXP_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [EXP_UpdatedAt]
GO
ALTER TABLE [dbo].[FeeCategories_FC] ADD  CONSTRAINT [DF_FeeCategories_FC_Id]  DEFAULT (newid()) FOR [FC_Id]
GO
ALTER TABLE [dbo].[FeeCategories_FC] ADD  CONSTRAINT [DF_FeeCategories_FC_IsRefundable]  DEFAULT ((0)) FOR [FC_IsRefundable]
GO
ALTER TABLE [dbo].[FeeCategories_FC] ADD  CONSTRAINT [DF_FeeCategories_FC_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [FC_CreatedAt]
GO
ALTER TABLE [dbo].[FeeCategories_FC] ADD  CONSTRAINT [DF_FeeCategories_FC_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [FC_UpdatedAt]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] ADD  CONSTRAINT [DF_FeeInvoiceItems_FII_Id]  DEFAULT (newid()) FOR [FII_Id]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] ADD  CONSTRAINT [DF_FeeInvoiceItems_FII_Quantity]  DEFAULT ((1)) FOR [FII_Quantity]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] ADD  CONSTRAINT [DF_FeeInvoiceItems_FII_DiscountAmount]  DEFAULT ((0)) FOR [FII_DiscountAmount]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] ADD  CONSTRAINT [DF_FeeInvoiceItems_FII_TaxAmount]  DEFAULT ((0)) FOR [FII_TaxAmount]
GO
ALTER TABLE [dbo].[FeeInvoices_FI] ADD  CONSTRAINT [DF_FeeInvoices_FI_Id]  DEFAULT (newid()) FOR [FI_Id]
GO
ALTER TABLE [dbo].[FeeInvoices_FI] ADD  CONSTRAINT [DF_FeeInvoices_FI_PaidAmount]  DEFAULT ((0)) FOR [FI_PaidAmount]
GO
ALTER TABLE [dbo].[FeeInvoices_FI] ADD  CONSTRAINT [DF_FeeInvoices_FI_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [FI_CreatedAt]
GO
ALTER TABLE [dbo].[FeeInvoices_FI] ADD  CONSTRAINT [DF_FeeInvoices_FI_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [FI_UpdatedAt]
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI] ADD  CONSTRAINT [DF_FeeStructureItems_FSI_Id]  DEFAULT (newid()) FOR [FSI_Id]
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI] ADD  CONSTRAINT [DF_FeeStructureItems_FSI_IsMandatory]  DEFAULT ((1)) FOR [FSI_IsMandatory]
GO
ALTER TABLE [dbo].[FeeStructures_FS] ADD  CONSTRAINT [DF_FeeStructures_FS_Id]  DEFAULT (newid()) FOR [FS_Id]
GO
ALTER TABLE [dbo].[FeeStructures_FS] ADD  CONSTRAINT [DF_FeeStructures_FS_IsActive]  DEFAULT ((1)) FOR [FS_IsActive]
GO
ALTER TABLE [dbo].[FeeStructures_FS] ADD  CONSTRAINT [DF_FeeStructures_FS_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [FS_CreatedAt]
GO
ALTER TABLE [dbo].[FeeStructures_FS] ADD  CONSTRAINT [DF_FeeStructures_FS_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [FS_UpdatedAt]
GO
ALTER TABLE [dbo].[GradeScaleItems_GSI] ADD  CONSTRAINT [DF_GradeScaleItems_GSI_Id]  DEFAULT (newid()) FOR [GSI_Id]
GO
ALTER TABLE [dbo].[GradeScales_GS] ADD  CONSTRAINT [DF_GradeScales_GS_Id]  DEFAULT (newid()) FOR [GS_Id]
GO
ALTER TABLE [dbo].[GradeScales_GS] ADD  CONSTRAINT [DF_GradeScales_GS_IsDefault]  DEFAULT ((0)) FOR [GS_IsDefault]
GO
ALTER TABLE [dbo].[Guardians_G] ADD  CONSTRAINT [DF_Guardians_G_Id]  DEFAULT (newid()) FOR [G_Id]
GO
ALTER TABLE [dbo].[Guardians_G] ADD  CONSTRAINT [DF_Guardians_G_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [G_CreatedAt]
GO
ALTER TABLE [dbo].[Guardians_G] ADD  CONSTRAINT [DF_Guardians_G_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [G_UpdatedAt]
GO
ALTER TABLE [dbo].[Marks_M] ADD  CONSTRAINT [DF_Marks_M_Id]  DEFAULT (newid()) FOR [M_Id]
GO
ALTER TABLE [dbo].[Marks_M] ADD  CONSTRAINT [DF_Marks_M_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [M_CreatedAt]
GO
ALTER TABLE [dbo].[Marks_M] ADD  CONSTRAINT [DF_Marks_M_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [M_UpdatedAt]
GO
ALTER TABLE [dbo].[Notifications_N] ADD  CONSTRAINT [DF_Notifications_N_Id]  DEFAULT (newid()) FOR [N_Id]
GO
ALTER TABLE [dbo].[Notifications_N] ADD  CONSTRAINT [DF_Notifications_N_IsRead]  DEFAULT ((0)) FOR [N_IsRead]
GO
ALTER TABLE [dbo].[Notifications_N] ADD  CONSTRAINT [DF_Notifications_N_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [N_CreatedAt]
GO
ALTER TABLE [dbo].[NotificationTemplates_NT] ADD  CONSTRAINT [DF_NotificationTemplates_NT_Id]  DEFAULT (newid()) FOR [NT_Id]
GO
ALTER TABLE [dbo].[NotificationTemplates_NT] ADD  CONSTRAINT [DF_NotificationTemplates_NT_IsActive]  DEFAULT ((1)) FOR [NT_IsActive]
GO
ALTER TABLE [dbo].[NotificationTemplates_NT] ADD  CONSTRAINT [DF_NotificationTemplates_NT_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [NT_CreatedAt]
GO
ALTER TABLE [dbo].[NotificationTemplates_NT] ADD  CONSTRAINT [DF_NotificationTemplates_NT_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [NT_UpdatedAt]
GO
ALTER TABLE [dbo].[Organizations_O] ADD  CONSTRAINT [DF_Organizations_O_Id]  DEFAULT (newid()) FOR [O_Id]
GO
ALTER TABLE [dbo].[Organizations_O] ADD  CONSTRAINT [DF_Organizations_O_Timezone]  DEFAULT ('UTC') FOR [O_Timezone]
GO
ALTER TABLE [dbo].[Organizations_O] ADD  CONSTRAINT [DF_Organizations_O_CurrencyCode]  DEFAULT ('USD') FOR [O_CurrencyCode]
GO
ALTER TABLE [dbo].[Organizations_O] ADD  CONSTRAINT [DF_Organizations_O_Status]  DEFAULT ('active') FOR [O_Status]
GO
ALTER TABLE [dbo].[Organizations_O] ADD  CONSTRAINT [DF_Organizations_O_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [O_CreatedAt]
GO
ALTER TABLE [dbo].[Organizations_O] ADD  CONSTRAINT [DF_Organizations_O_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [O_UpdatedAt]
GO
ALTER TABLE [dbo].[OrganizationSettings_OS] ADD  CONSTRAINT [DF_OrganizationSettings_OS_Id]  DEFAULT (newid()) FOR [OS_Id]
GO
ALTER TABLE [dbo].[OrganizationSettings_OS] ADD  CONSTRAINT [DF_OrganizationSettings_OS_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [OS_CreatedAt]
GO
ALTER TABLE [dbo].[OrganizationSettings_OS] ADD  CONSTRAINT [DF_OrganizationSettings_OS_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [OS_UpdatedAt]
GO
ALTER TABLE [dbo].[PaymentAllocations_PA] ADD  CONSTRAINT [DF_PaymentAllocations_PA_Id]  DEFAULT (newid()) FOR [PA_Id]
GO
ALTER TABLE [dbo].[PaymentAllocations_PA] ADD  CONSTRAINT [DF_PaymentAllocations_PA_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [PA_CreatedAt]
GO
ALTER TABLE [dbo].[PaymentMethods_PM] ADD  CONSTRAINT [DF_PaymentMethods_PM_Id]  DEFAULT (newid()) FOR [PM_Id]
GO
ALTER TABLE [dbo].[PaymentMethods_PM] ADD  CONSTRAINT [DF_PaymentMethods_PM_IsActive]  DEFAULT ((1)) FOR [PM_IsActive]
GO
ALTER TABLE [dbo].[Payments_PAY] ADD  CONSTRAINT [DF_Payments_PAY_Id]  DEFAULT (newid()) FOR [PAY_Id]
GO
ALTER TABLE [dbo].[Payments_PAY] ADD  CONSTRAINT [DF_Payments_PAY_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [PAY_CreatedAt]
GO
ALTER TABLE [dbo].[Payments_PAY] ADD  CONSTRAINT [DF_Payments_PAY_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [PAY_UpdatedAt]
GO
ALTER TABLE [dbo].[Programs_P] ADD  CONSTRAINT [DF_Programs_P_Id]  DEFAULT (newid()) FOR [P_Id]
GO
ALTER TABLE [dbo].[Programs_P] ADD  CONSTRAINT [DF_Programs_P_Status]  DEFAULT ('active') FOR [P_Status]
GO
ALTER TABLE [dbo].[Programs_P] ADD  CONSTRAINT [DF_Programs_P_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [P_CreatedAt]
GO
ALTER TABLE [dbo].[Programs_P] ADD  CONSTRAINT [DF_Programs_P_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [P_UpdatedAt]
GO
ALTER TABLE [dbo].[Refunds_RF] ADD  CONSTRAINT [DF_Refunds_RF_Id]  DEFAULT (newid()) FOR [RF_Id]
GO
ALTER TABLE [dbo].[Refunds_RF] ADD  CONSTRAINT [DF_Refunds_RF_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [RF_CreatedAt]
GO
ALTER TABLE [dbo].[Refunds_RF] ADD  CONSTRAINT [DF_Refunds_RF_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [RF_UpdatedAt]
GO
ALTER TABLE [dbo].[Results_R] ADD  CONSTRAINT [DF_Results_R_Id]  DEFAULT (newid()) FOR [R_Id]
GO
ALTER TABLE [dbo].[Results_R] ADD  CONSTRAINT [DF_Results_R_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [R_CreatedAt]
GO
ALTER TABLE [dbo].[Results_R] ADD  CONSTRAINT [DF_Results_R_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [R_UpdatedAt]
GO
ALTER TABLE [dbo].[Staff_ST] ADD  CONSTRAINT [DF_Staff_ST_Id]  DEFAULT (newid()) FOR [ST_Id]
GO
ALTER TABLE [dbo].[Staff_ST] ADD  CONSTRAINT [DF_Staff_ST_Status]  DEFAULT ('active') FOR [ST_Status]
GO
ALTER TABLE [dbo].[Staff_ST] ADD  CONSTRAINT [DF_Staff_ST_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [ST_CreatedAt]
GO
ALTER TABLE [dbo].[Staff_ST] ADD  CONSTRAINT [DF_Staff_ST_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [ST_UpdatedAt]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] ADD  CONSTRAINT [DF_StudentFeeAssignments_SFA_Id]  DEFAULT (newid()) FOR [SFA_Id]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] ADD  CONSTRAINT [DF_StudentFeeAssignments_SFA_DiscountAmount]  DEFAULT ((0)) FOR [SFA_DiscountAmount]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] ADD  CONSTRAINT [DF_StudentFeeAssignments_SFA_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [SFA_CreatedAt]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] ADD  CONSTRAINT [DF_StudentFeeAssignments_SFA_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [SFA_UpdatedAt]
GO
ALTER TABLE [dbo].[StudentGuardians_SG] ADD  CONSTRAINT [DF_StudentGuardians_SG_IsPrimary]  DEFAULT ((0)) FOR [SG_IsPrimary]
GO
ALTER TABLE [dbo].[StudentGuardians_SG] ADD  CONSTRAINT [DF_StudentGuardians_SG_IsEmergency]  DEFAULT ((0)) FOR [SG_IsEmergency]
GO
ALTER TABLE [dbo].[Students_Guardians] ADD  CONSTRAINT [DF_SG_IsPrimary]  DEFAULT ((0)) FOR [SG_IsPrimary]
GO
ALTER TABLE [dbo].[Students_Guardians] ADD  CONSTRAINT [DF_SG_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [SG_CreatedAt]
GO
ALTER TABLE [dbo].[Subjects_SB] ADD  CONSTRAINT [DF_Subjects_SB_Id]  DEFAULT (newid()) FOR [SB_Id]
GO
ALTER TABLE [dbo].[Subjects_SB] ADD  CONSTRAINT [DF_Subjects_SB_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [SB_CreatedAt]
GO
ALTER TABLE [dbo].[Subjects_SB] ADD  CONSTRAINT [DF_Subjects_SB_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [SB_UpdatedAt]
GO
ALTER TABLE [dbo].[TeacherAttendance_TA] ADD  CONSTRAINT [DF_TA_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [TA_CreatedAt]
GO
ALTER TABLE [dbo].[TeacherAttendance_TA] ADD  CONSTRAINT [DF_TA_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [TA_UpdatedAt]
GO
ALTER TABLE [dbo].[TeacherLeaves_TL] ADD  CONSTRAINT [DF_TL_Status]  DEFAULT ('Pending') FOR [TL_Status]
GO
ALTER TABLE [dbo].[TeacherLeaves_TL] ADD  CONSTRAINT [DF_TL_AppliedAt]  DEFAULT (sysutcdatetime()) FOR [TL_AppliedAt]
GO
ALTER TABLE [dbo].[TeacherLeaves_TL] ADD  CONSTRAINT [DF_TL_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [TL_CreatedAt]
GO
ALTER TABLE [dbo].[TeacherLeaves_TL] ADD  CONSTRAINT [DF_TL_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [TL_UpdatedAt]
GO
ALTER TABLE [dbo].[Teachers_T] ADD  CONSTRAINT [DF_Teachers_T_IsActive]  DEFAULT ((1)) FOR [T_IsActive]
GO
ALTER TABLE [dbo].[Timetables_TT] ADD  CONSTRAINT [DF_Timetables_TT_Id]  DEFAULT (newid()) FOR [TT_Id]
GO
ALTER TABLE [dbo].[Timetables_TT] ADD  CONSTRAINT [DF_Timetables_TT_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [TT_CreatedAt]
GO
ALTER TABLE [dbo].[Timetables_TT] ADD  CONSTRAINT [DF_Timetables_TT_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [TT_UpdatedAt]
GO
ALTER TABLE [dbo].[Vendors_V] ADD  CONSTRAINT [DF_Vendors_V_Id]  DEFAULT (newid()) FOR [V_Id]
GO
ALTER TABLE [dbo].[Vendors_V] ADD  CONSTRAINT [DF_Vendors_V_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [V_CreatedAt]
GO
ALTER TABLE [dbo].[Vendors_V] ADD  CONSTRAINT [DF_Vendors_V_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [V_UpdatedAt]
GO
ALTER TABLE [dbo].[AdmissionApplicationDocuments_AAD]  WITH CHECK ADD  CONSTRAINT [FK_AdmissionApplicationDocuments_AAD_ApplicationId] FOREIGN KEY([AAD_ApplicationId])
REFERENCES [dbo].[AdmissionApplications_AA] ([AA_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AdmissionApplicationDocuments_AAD] CHECK CONSTRAINT [FK_AdmissionApplicationDocuments_AAD_ApplicationId]
GO
ALTER TABLE [dbo].[AdmissionApplications_AA]  WITH CHECK ADD  CONSTRAINT [FK_AdmissionApplications_AA_AcademicYearId] FOREIGN KEY([AA_AcademicYearId])
REFERENCES [dbo].[AcademicYears_AY] ([AY_Id])
GO
ALTER TABLE [dbo].[AdmissionApplications_AA] CHECK CONSTRAINT [FK_AdmissionApplications_AA_AcademicYearId]
GO
ALTER TABLE [dbo].[AdmissionApplications_AA]  WITH CHECK ADD  CONSTRAINT [FK_AdmissionApplications_AA_BranchId] FOREIGN KEY([AA_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[AdmissionApplications_AA] CHECK CONSTRAINT [FK_AdmissionApplications_AA_BranchId]
GO
ALTER TABLE [dbo].[AdmissionApplications_AA]  WITH CHECK ADD  CONSTRAINT [FK_AdmissionApplications_AA_CourseId] FOREIGN KEY([AA_CourseId])
REFERENCES [dbo].[Courses_C] ([C_Id])
GO
ALTER TABLE [dbo].[AdmissionApplications_AA] CHECK CONSTRAINT [FK_AdmissionApplications_AA_CourseId]
GO
ALTER TABLE [dbo].[Announcements_ANN]  WITH CHECK ADD  CONSTRAINT [FK_Announcements_ANN_BranchId] FOREIGN KEY([ANN_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[Announcements_ANN] CHECK CONSTRAINT [FK_Announcements_ANN_BranchId]
GO
ALTER TABLE [dbo].[AttendanceRecords_AR]  WITH CHECK ADD  CONSTRAINT [FK_AttendanceRecords_AR_AttendanceSessionId] FOREIGN KEY([AR_AttendanceSessionId])
REFERENCES [dbo].[AttendanceSessions_AS] ([AS_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AttendanceRecords_AR] CHECK CONSTRAINT [FK_AttendanceRecords_AR_AttendanceSessionId]
GO
ALTER TABLE [dbo].[AttendanceRecords_AR]  WITH CHECK ADD  CONSTRAINT [FK_AttendanceRecords_AR_StudentId] FOREIGN KEY([AR_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[AttendanceRecords_AR] CHECK CONSTRAINT [FK_AttendanceRecords_AR_StudentId]
GO
ALTER TABLE [dbo].[AttendanceSessions_AS]  WITH CHECK ADD  CONSTRAINT [FK_AttendanceSessions_AS_BatchId] FOREIGN KEY([AS_BatchId])
REFERENCES [dbo].[Batches_BT] ([BT_Id])
GO
ALTER TABLE [dbo].[AttendanceSessions_AS] CHECK CONSTRAINT [FK_AttendanceSessions_AS_BatchId]
GO
ALTER TABLE [dbo].[AttendanceSessions_AS]  WITH CHECK ADD  CONSTRAINT [FK_AttendanceSessions_AS_BranchId] FOREIGN KEY([AS_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[AttendanceSessions_AS] CHECK CONSTRAINT [FK_AttendanceSessions_AS_BranchId]
GO
ALTER TABLE [dbo].[AttendanceSessions_AS]  WITH CHECK ADD  CONSTRAINT [FK_AttendanceSessions_AS_SubjectId] FOREIGN KEY([AS_SubjectId])
REFERENCES [dbo].[Subjects_SB] ([SB_Id])
GO
ALTER TABLE [dbo].[AttendanceSessions_AS] CHECK CONSTRAINT [FK_AttendanceSessions_AS_SubjectId]
GO
ALTER TABLE [dbo].[Batches_BT]  WITH CHECK ADD  CONSTRAINT [FK_Batches_BT_AcademicYearId] FOREIGN KEY([BT_AcademicYearId])
REFERENCES [dbo].[AcademicYears_AY] ([AY_Id])
GO
ALTER TABLE [dbo].[Batches_BT] CHECK CONSTRAINT [FK_Batches_BT_AcademicYearId]
GO
ALTER TABLE [dbo].[Batches_BT]  WITH CHECK ADD  CONSTRAINT [FK_Batches_BT_BranchId] FOREIGN KEY([BT_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[Batches_BT] CHECK CONSTRAINT [FK_Batches_BT_BranchId]
GO
ALTER TABLE [dbo].[Batches_BT]  WITH CHECK ADD  CONSTRAINT [FK_Batches_BT_CourseId] FOREIGN KEY([BT_CourseId])
REFERENCES [dbo].[Courses_C] ([C_Id])
GO
ALTER TABLE [dbo].[Batches_BT] CHECK CONSTRAINT [FK_Batches_BT_CourseId]
GO
ALTER TABLE [dbo].[BatchStudents_BS]  WITH CHECK ADD  CONSTRAINT [FK_BatchStudents_BS_BatchId] FOREIGN KEY([BS_BatchId])
REFERENCES [dbo].[Batches_BT] ([BT_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BatchStudents_BS] CHECK CONSTRAINT [FK_BatchStudents_BS_BatchId]
GO
ALTER TABLE [dbo].[BatchStudents_BS]  WITH CHECK ADD  CONSTRAINT [FK_BatchStudents_BS_StudentId] FOREIGN KEY([BS_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[BatchStudents_BS] CHECK CONSTRAINT [FK_BatchStudents_BS_StudentId]
GO
ALTER TABLE [dbo].[Classrooms_CR]  WITH CHECK ADD  CONSTRAINT [FK_Classrooms_CR_BranchId] FOREIGN KEY([CR_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[Classrooms_CR] CHECK CONSTRAINT [FK_Classrooms_CR_BranchId]
GO
ALTER TABLE [dbo].[Courses_C]  WITH CHECK ADD  CONSTRAINT [FK_Courses_C_ProgramId] FOREIGN KEY([C_ProgramId])
REFERENCES [dbo].[Programs_P] ([P_Id])
GO
ALTER TABLE [dbo].[Courses_C] CHECK CONSTRAINT [FK_Courses_C_ProgramId]
GO
ALTER TABLE [dbo].[CourseSubjects_CS]  WITH CHECK ADD  CONSTRAINT [FK_CourseSubjects_CS_CourseId] FOREIGN KEY([CS_CourseId])
REFERENCES [dbo].[Courses_C] ([C_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CourseSubjects_CS] CHECK CONSTRAINT [FK_CourseSubjects_CS_CourseId]
GO
ALTER TABLE [dbo].[CourseSubjects_CS]  WITH CHECK ADD  CONSTRAINT [FK_CourseSubjects_CS_SubjectId] FOREIGN KEY([CS_SubjectId])
REFERENCES [dbo].[Subjects_SB] ([SB_Id])
GO
ALTER TABLE [dbo].[CourseSubjects_CS] CHECK CONSTRAINT [FK_CourseSubjects_CS_SubjectId]
GO
ALTER TABLE [dbo].[CustomFieldValues_CFV]  WITH CHECK ADD  CONSTRAINT [FK_CustomFieldValues_CFV_CustomFieldId] FOREIGN KEY([CFV_CustomFieldId])
REFERENCES [dbo].[CustomFields_CF] ([CF_Id])
GO
ALTER TABLE [dbo].[CustomFieldValues_CFV] CHECK CONSTRAINT [FK_CustomFieldValues_CFV_CustomFieldId]
GO
ALTER TABLE [dbo].[Departments_D]  WITH CHECK ADD  CONSTRAINT [FK_Departments_D_BranchId] FOREIGN KEY([D_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[Departments_D] CHECK CONSTRAINT [FK_Departments_D_BranchId]
GO
ALTER TABLE [dbo].[Enrollments_E]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_E_AcademicYearId] FOREIGN KEY([E_AcademicYearId])
REFERENCES [dbo].[AcademicYears_AY] ([AY_Id])
GO
ALTER TABLE [dbo].[Enrollments_E] CHECK CONSTRAINT [FK_Enrollments_E_AcademicYearId]
GO
ALTER TABLE [dbo].[Enrollments_E]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_E_BatchId] FOREIGN KEY([E_BatchId])
REFERENCES [dbo].[Batches_BT] ([BT_Id])
GO
ALTER TABLE [dbo].[Enrollments_E] CHECK CONSTRAINT [FK_Enrollments_E_BatchId]
GO
ALTER TABLE [dbo].[Enrollments_E]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_E_CourseId] FOREIGN KEY([E_CourseId])
REFERENCES [dbo].[Courses_C] ([C_Id])
GO
ALTER TABLE [dbo].[Enrollments_E] CHECK CONSTRAINT [FK_Enrollments_E_CourseId]
GO
ALTER TABLE [dbo].[Enrollments_E]  WITH CHECK ADD  CONSTRAINT [FK_Enrollments_E_StudentId] FOREIGN KEY([E_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[Enrollments_E] CHECK CONSTRAINT [FK_Enrollments_E_StudentId]
GO
ALTER TABLE [dbo].[EntityDocuments_ED]  WITH CHECK ADD  CONSTRAINT [FK_EntityDocuments_ED_DocumentId] FOREIGN KEY([ED_DocumentId])
REFERENCES [dbo].[Documents_DOC] ([DOC_Id])
GO
ALTER TABLE [dbo].[EntityDocuments_ED] CHECK CONSTRAINT [FK_EntityDocuments_ED_DocumentId]
GO
ALTER TABLE [dbo].[EntityDocuments_ED]  WITH CHECK ADD  CONSTRAINT [FK_EntityDocuments_ED_DocumentTypeId] FOREIGN KEY([ED_DocumentTypeId])
REFERENCES [dbo].[DocumentTypes_DT] ([DT_Id])
GO
ALTER TABLE [dbo].[EntityDocuments_ED] CHECK CONSTRAINT [FK_EntityDocuments_ED_DocumentTypeId]
GO
ALTER TABLE [dbo].[Exams_EX]  WITH CHECK ADD  CONSTRAINT [FK_Exams_EX_AcademicYearId] FOREIGN KEY([EX_AcademicYearId])
REFERENCES [dbo].[AcademicYears_AY] ([AY_Id])
GO
ALTER TABLE [dbo].[Exams_EX] CHECK CONSTRAINT [FK_Exams_EX_AcademicYearId]
GO
ALTER TABLE [dbo].[Exams_EX]  WITH CHECK ADD  CONSTRAINT [FK_Exams_EX_BatchId] FOREIGN KEY([EX_BatchId])
REFERENCES [dbo].[Batches_BT] ([BT_Id])
GO
ALTER TABLE [dbo].[Exams_EX] CHECK CONSTRAINT [FK_Exams_EX_BatchId]
GO
ALTER TABLE [dbo].[Exams_EX]  WITH CHECK ADD  CONSTRAINT [FK_Exams_EX_CourseId] FOREIGN KEY([EX_CourseId])
REFERENCES [dbo].[Courses_C] ([C_Id])
GO
ALTER TABLE [dbo].[Exams_EX] CHECK CONSTRAINT [FK_Exams_EX_CourseId]
GO
ALTER TABLE [dbo].[Exams_EX]  WITH CHECK ADD  CONSTRAINT [FK_Exams_EX_ExamTypeId] FOREIGN KEY([EX_ExamTypeId])
REFERENCES [dbo].[ExamTypes_ET] ([ET_Id])
GO
ALTER TABLE [dbo].[Exams_EX] CHECK CONSTRAINT [FK_Exams_EX_ExamTypeId]
GO
ALTER TABLE [dbo].[ExamSchedules_ESC]  WITH CHECK ADD  CONSTRAINT [FK_ExamSchedules_ESC_ClassroomId] FOREIGN KEY([ESC_ClassroomId])
REFERENCES [dbo].[Classrooms_CR] ([CR_Id])
GO
ALTER TABLE [dbo].[ExamSchedules_ESC] CHECK CONSTRAINT [FK_ExamSchedules_ESC_ClassroomId]
GO
ALTER TABLE [dbo].[ExamSchedules_ESC]  WITH CHECK ADD  CONSTRAINT [FK_ExamSchedules_ESC_ExamSubjectId] FOREIGN KEY([ESC_ExamSubjectId])
REFERENCES [dbo].[ExamSubjects_ES] ([ES_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExamSchedules_ESC] CHECK CONSTRAINT [FK_ExamSchedules_ESC_ExamSubjectId]
GO
ALTER TABLE [dbo].[ExamSubjects_ES]  WITH CHECK ADD  CONSTRAINT [FK_ExamSubjects_ES_ExamId] FOREIGN KEY([ES_ExamId])
REFERENCES [dbo].[Exams_EX] ([EX_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExamSubjects_ES] CHECK CONSTRAINT [FK_ExamSubjects_ES_ExamId]
GO
ALTER TABLE [dbo].[ExamSubjects_ES]  WITH CHECK ADD  CONSTRAINT [FK_ExamSubjects_ES_SubjectId] FOREIGN KEY([ES_SubjectId])
REFERENCES [dbo].[Subjects_SB] ([SB_Id])
GO
ALTER TABLE [dbo].[ExamSubjects_ES] CHECK CONSTRAINT [FK_ExamSubjects_ES_SubjectId]
GO
ALTER TABLE [dbo].[Expenses_EXP]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_EXP_BranchId] FOREIGN KEY([EXP_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[Expenses_EXP] CHECK CONSTRAINT [FK_Expenses_EXP_BranchId]
GO
ALTER TABLE [dbo].[Expenses_EXP]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_EXP_ExpenseCategoryId] FOREIGN KEY([EXP_ExpenseCategoryId])
REFERENCES [dbo].[ExpenseCategories_EC] ([EC_Id])
GO
ALTER TABLE [dbo].[Expenses_EXP] CHECK CONSTRAINT [FK_Expenses_EXP_ExpenseCategoryId]
GO
ALTER TABLE [dbo].[Expenses_EXP]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_EXP_PaymentMethodId] FOREIGN KEY([EXP_PaymentMethodId])
REFERENCES [dbo].[PaymentMethods_PM] ([PM_Id])
GO
ALTER TABLE [dbo].[Expenses_EXP] CHECK CONSTRAINT [FK_Expenses_EXP_PaymentMethodId]
GO
ALTER TABLE [dbo].[Expenses_EXP]  WITH CHECK ADD  CONSTRAINT [FK_Expenses_EXP_VendorId] FOREIGN KEY([EXP_VendorId])
REFERENCES [dbo].[Vendors_V] ([V_Id])
GO
ALTER TABLE [dbo].[Expenses_EXP] CHECK CONSTRAINT [FK_Expenses_EXP_VendorId]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII]  WITH CHECK ADD  CONSTRAINT [FK_FeeInvoiceItems_FII_FeeCategoryId] FOREIGN KEY([FII_FeeCategoryId])
REFERENCES [dbo].[FeeCategories_FC] ([FC_Id])
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] CHECK CONSTRAINT [FK_FeeInvoiceItems_FII_FeeCategoryId]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII]  WITH CHECK ADD  CONSTRAINT [FK_FeeInvoiceItems_FII_InvoiceId] FOREIGN KEY([FII_InvoiceId])
REFERENCES [dbo].[FeeInvoices_FI] ([FI_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] CHECK CONSTRAINT [FK_FeeInvoiceItems_FII_InvoiceId]
GO
ALTER TABLE [dbo].[FeeInvoices_FI]  WITH CHECK ADD  CONSTRAINT [FK_FeeInvoices_FI_StudentId] FOREIGN KEY([FI_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[FeeInvoices_FI] CHECK CONSTRAINT [FK_FeeInvoices_FI_StudentId]
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI]  WITH CHECK ADD  CONSTRAINT [FK_FeeStructureItems_FSI_FeeCategoryId] FOREIGN KEY([FSI_FeeCategoryId])
REFERENCES [dbo].[FeeCategories_FC] ([FC_Id])
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI] CHECK CONSTRAINT [FK_FeeStructureItems_FSI_FeeCategoryId]
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI]  WITH CHECK ADD  CONSTRAINT [FK_FeeStructureItems_FSI_FeeStructureId] FOREIGN KEY([FSI_FeeStructureId])
REFERENCES [dbo].[FeeStructures_FS] ([FS_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI] CHECK CONSTRAINT [FK_FeeStructureItems_FSI_FeeStructureId]
GO
ALTER TABLE [dbo].[FeeStructures_FS]  WITH CHECK ADD  CONSTRAINT [FK_FeeStructures_FS_AcademicYearId] FOREIGN KEY([FS_AcademicYearId])
REFERENCES [dbo].[AcademicYears_AY] ([AY_Id])
GO
ALTER TABLE [dbo].[FeeStructures_FS] CHECK CONSTRAINT [FK_FeeStructures_FS_AcademicYearId]
GO
ALTER TABLE [dbo].[FeeStructures_FS]  WITH CHECK ADD  CONSTRAINT [FK_FeeStructures_FS_BatchId] FOREIGN KEY([FS_BatchId])
REFERENCES [dbo].[Batches_BT] ([BT_Id])
GO
ALTER TABLE [dbo].[FeeStructures_FS] CHECK CONSTRAINT [FK_FeeStructures_FS_BatchId]
GO
ALTER TABLE [dbo].[FeeStructures_FS]  WITH CHECK ADD  CONSTRAINT [FK_FeeStructures_FS_CourseId] FOREIGN KEY([FS_CourseId])
REFERENCES [dbo].[Courses_C] ([C_Id])
GO
ALTER TABLE [dbo].[FeeStructures_FS] CHECK CONSTRAINT [FK_FeeStructures_FS_CourseId]
GO
ALTER TABLE [dbo].[GradeScaleItems_GSI]  WITH CHECK ADD  CONSTRAINT [FK_GradeScaleItems_GSI_GradeScaleId] FOREIGN KEY([GSI_GradeScaleId])
REFERENCES [dbo].[GradeScales_GS] ([GS_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GradeScaleItems_GSI] CHECK CONSTRAINT [FK_GradeScaleItems_GSI_GradeScaleId]
GO
ALTER TABLE [dbo].[Marks_M]  WITH CHECK ADD  CONSTRAINT [FK_Marks_M_ExamSubjectId] FOREIGN KEY([M_ExamSubjectId])
REFERENCES [dbo].[ExamSubjects_ES] ([ES_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Marks_M] CHECK CONSTRAINT [FK_Marks_M_ExamSubjectId]
GO
ALTER TABLE [dbo].[Marks_M]  WITH CHECK ADD  CONSTRAINT [FK_Marks_M_GradeScaleItemId] FOREIGN KEY([M_GradeScaleItemId])
REFERENCES [dbo].[GradeScaleItems_GSI] ([GSI_Id])
GO
ALTER TABLE [dbo].[Marks_M] CHECK CONSTRAINT [FK_Marks_M_GradeScaleItemId]
GO
ALTER TABLE [dbo].[Marks_M]  WITH CHECK ADD  CONSTRAINT [FK_Marks_M_StudentId] FOREIGN KEY([M_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[Marks_M] CHECK CONSTRAINT [FK_Marks_M_StudentId]
GO
ALTER TABLE [dbo].[PaymentAllocations_PA]  WITH CHECK ADD  CONSTRAINT [FK_PaymentAllocations_PA_InvoiceId] FOREIGN KEY([PA_InvoiceId])
REFERENCES [dbo].[FeeInvoices_FI] ([FI_Id])
GO
ALTER TABLE [dbo].[PaymentAllocations_PA] CHECK CONSTRAINT [FK_PaymentAllocations_PA_InvoiceId]
GO
ALTER TABLE [dbo].[PaymentAllocations_PA]  WITH CHECK ADD  CONSTRAINT [FK_PaymentAllocations_PA_PaymentId] FOREIGN KEY([PA_PaymentId])
REFERENCES [dbo].[Payments_PAY] ([PAY_Id])
GO
ALTER TABLE [dbo].[PaymentAllocations_PA] CHECK CONSTRAINT [FK_PaymentAllocations_PA_PaymentId]
GO
ALTER TABLE [dbo].[Payments_PAY]  WITH CHECK ADD  CONSTRAINT [FK_Payments_PAY_PaymentMethodId] FOREIGN KEY([PAY_PaymentMethodId])
REFERENCES [dbo].[PaymentMethods_PM] ([PM_Id])
GO
ALTER TABLE [dbo].[Payments_PAY] CHECK CONSTRAINT [FK_Payments_PAY_PaymentMethodId]
GO
ALTER TABLE [dbo].[Payments_PAY]  WITH CHECK ADD  CONSTRAINT [FK_Payments_PAY_StudentId] FOREIGN KEY([PAY_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[Payments_PAY] CHECK CONSTRAINT [FK_Payments_PAY_StudentId]
GO
ALTER TABLE [dbo].[Refunds_RF]  WITH CHECK ADD  CONSTRAINT [FK_Refunds_RF_PaymentId] FOREIGN KEY([RF_PaymentId])
REFERENCES [dbo].[Payments_PAY] ([PAY_Id])
GO
ALTER TABLE [dbo].[Refunds_RF] CHECK CONSTRAINT [FK_Refunds_RF_PaymentId]
GO
ALTER TABLE [dbo].[Refunds_RF]  WITH CHECK ADD  CONSTRAINT [FK_Refunds_RF_StudentId] FOREIGN KEY([RF_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[Refunds_RF] CHECK CONSTRAINT [FK_Refunds_RF_StudentId]
GO
ALTER TABLE [dbo].[Results_R]  WITH CHECK ADD  CONSTRAINT [FK_Results_R_ExamId] FOREIGN KEY([R_ExamId])
REFERENCES [dbo].[Exams_EX] ([EX_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Results_R] CHECK CONSTRAINT [FK_Results_R_ExamId]
GO
ALTER TABLE [dbo].[Results_R]  WITH CHECK ADD  CONSTRAINT [FK_Results_R_StudentId] FOREIGN KEY([R_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[Results_R] CHECK CONSTRAINT [FK_Results_R_StudentId]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA]  WITH CHECK ADD  CONSTRAINT [FK_StudentFeeAssignments_SFA_FeeStructureId] FOREIGN KEY([SFA_FeeStructureId])
REFERENCES [dbo].[FeeStructures_FS] ([FS_Id])
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] CHECK CONSTRAINT [FK_StudentFeeAssignments_SFA_FeeStructureId]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA]  WITH CHECK ADD  CONSTRAINT [FK_StudentFeeAssignments_SFA_StudentId] FOREIGN KEY([SFA_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] CHECK CONSTRAINT [FK_StudentFeeAssignments_SFA_StudentId]
GO
ALTER TABLE [dbo].[StudentGuardians_SG]  WITH CHECK ADD  CONSTRAINT [FK_StudentGuardians_SG_StudentId] FOREIGN KEY([SG_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[StudentGuardians_SG] CHECK CONSTRAINT [FK_StudentGuardians_SG_StudentId]
GO
ALTER TABLE [dbo].[Students_Guardians]  WITH CHECK ADD  CONSTRAINT [FK_SG_Guardian] FOREIGN KEY([SG_GuardianId])
REFERENCES [dbo].[Guardians_G] ([G_Id])
GO
ALTER TABLE [dbo].[Students_Guardians] CHECK CONSTRAINT [FK_SG_Guardian]
GO
ALTER TABLE [dbo].[Students_Guardians]  WITH CHECK ADD  CONSTRAINT [FK_SG_Student] FOREIGN KEY([SG_StudentId])
REFERENCES [dbo].[Students_S] ([S_Id])
GO
ALTER TABLE [dbo].[Students_Guardians] CHECK CONSTRAINT [FK_SG_Student]
GO
ALTER TABLE [dbo].[TeacherAttendance_TA]  WITH CHECK ADD  CONSTRAINT [FK_TA_Teacher] FOREIGN KEY([TA_TeacherId])
REFERENCES [dbo].[Teachers_T] ([T_Id])
GO
ALTER TABLE [dbo].[TeacherAttendance_TA] CHECK CONSTRAINT [FK_TA_Teacher]
GO
ALTER TABLE [dbo].[TeacherLeaves_TL]  WITH CHECK ADD  CONSTRAINT [FK_TL_Teacher] FOREIGN KEY([TL_TeacherId])
REFERENCES [dbo].[Teachers_T] ([T_Id])
GO
ALTER TABLE [dbo].[TeacherLeaves_TL] CHECK CONSTRAINT [FK_TL_Teacher]
GO
ALTER TABLE [dbo].[Timetables_TT]  WITH CHECK ADD  CONSTRAINT [FK_Timetables_TT_BatchId] FOREIGN KEY([TT_BatchId])
REFERENCES [dbo].[Batches_BT] ([BT_Id])
GO
ALTER TABLE [dbo].[Timetables_TT] CHECK CONSTRAINT [FK_Timetables_TT_BatchId]
GO
ALTER TABLE [dbo].[Timetables_TT]  WITH CHECK ADD  CONSTRAINT [FK_Timetables_TT_BranchId] FOREIGN KEY([TT_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[Timetables_TT] CHECK CONSTRAINT [FK_Timetables_TT_BranchId]
GO
ALTER TABLE [dbo].[Timetables_TT]  WITH CHECK ADD  CONSTRAINT [FK_Timetables_TT_ClassroomId] FOREIGN KEY([TT_ClassroomId])
REFERENCES [dbo].[Classrooms_CR] ([CR_Id])
GO
ALTER TABLE [dbo].[Timetables_TT] CHECK CONSTRAINT [FK_Timetables_TT_ClassroomId]
GO
ALTER TABLE [dbo].[Timetables_TT]  WITH CHECK ADD  CONSTRAINT [FK_Timetables_TT_SubjectId] FOREIGN KEY([TT_SubjectId])
REFERENCES [dbo].[Subjects_SB] ([SB_Id])
GO
ALTER TABLE [dbo].[Timetables_TT] CHECK CONSTRAINT [FK_Timetables_TT_SubjectId]
GO
ALTER TABLE [dbo].[AcademicYears_AY]  WITH CHECK ADD  CONSTRAINT [CK_AcademicYears_AY_DateRange] CHECK  (([AY_EndDate]>[AY_StartDate]))
GO
ALTER TABLE [dbo].[AcademicYears_AY] CHECK CONSTRAINT [CK_AcademicYears_AY_DateRange]
GO
ALTER TABLE [dbo].[ActivityLogs_ACL]  WITH CHECK ADD  CONSTRAINT [CK_ActivityLogs_ACL_Metadata_IsJson] CHECK  (([ACL_Metadata] IS NULL OR isjson([ACL_Metadata])=(1)))
GO
ALTER TABLE [dbo].[ActivityLogs_ACL] CHECK CONSTRAINT [CK_ActivityLogs_ACL_Metadata_IsJson]
GO
ALTER TABLE [dbo].[AdmissionApplications_AA]  WITH CHECK ADD  CONSTRAINT [CK_AdmissionApplications_AA_Gender] CHECK  (([AA_Gender] IS NULL OR ([AA_Gender]='prefer_not_to_say' OR [AA_Gender]='other' OR [AA_Gender]='female' OR [AA_Gender]='male')))
GO
ALTER TABLE [dbo].[AdmissionApplications_AA] CHECK CONSTRAINT [CK_AdmissionApplications_AA_Gender]
GO
ALTER TABLE [dbo].[AdmissionApplications_AA]  WITH CHECK ADD  CONSTRAINT [CK_AdmissionApplications_AA_Status] CHECK  (([AA_Status]='cancelled' OR [AA_Status]='enrolled' OR [AA_Status]='waitlisted' OR [AA_Status]='rejected' OR [AA_Status]='approved' OR [AA_Status]='under_review' OR [AA_Status]='submitted'))
GO
ALTER TABLE [dbo].[AdmissionApplications_AA] CHECK CONSTRAINT [CK_AdmissionApplications_AA_Status]
GO
ALTER TABLE [dbo].[Announcements_ANN]  WITH CHECK ADD  CONSTRAINT [CK_Announcements_ANN_ExpiresAfterPublish] CHECK  (([ANN_ExpiresAt] IS NULL OR [ANN_PublishedAt] IS NULL OR [ANN_ExpiresAt]>[ANN_PublishedAt]))
GO
ALTER TABLE [dbo].[Announcements_ANN] CHECK CONSTRAINT [CK_Announcements_ANN_ExpiresAfterPublish]
GO
ALTER TABLE [dbo].[AttendanceRecords_AR]  WITH CHECK ADD  CONSTRAINT [CK_AttendanceRecords_AR_Status] CHECK  (([AR_Status]='half_day' OR [AR_Status]='excused' OR [AR_Status]='late' OR [AR_Status]='absent' OR [AR_Status]='present'))
GO
ALTER TABLE [dbo].[AttendanceRecords_AR] CHECK CONSTRAINT [CK_AttendanceRecords_AR_Status]
GO
ALTER TABLE [dbo].[AttendanceSessions_AS]  WITH CHECK ADD  CONSTRAINT [CK_AttendanceSessions_AS_TimeRange] CHECK  (([AS_StartTime] IS NULL OR [AS_EndTime] IS NULL OR [AS_EndTime]>[AS_StartTime]))
GO
ALTER TABLE [dbo].[AttendanceSessions_AS] CHECK CONSTRAINT [CK_AttendanceSessions_AS_TimeRange]
GO
ALTER TABLE [dbo].[AuditLogs_AL]  WITH CHECK ADD  CONSTRAINT [CK_AuditLogs_AL_NewValues_IsJson] CHECK  (([AL_NewValues] IS NULL OR isjson([AL_NewValues])=(1)))
GO
ALTER TABLE [dbo].[AuditLogs_AL] CHECK CONSTRAINT [CK_AuditLogs_AL_NewValues_IsJson]
GO
ALTER TABLE [dbo].[AuditLogs_AL]  WITH CHECK ADD  CONSTRAINT [CK_AuditLogs_AL_OldValues_IsJson] CHECK  (([AL_OldValues] IS NULL OR isjson([AL_OldValues])=(1)))
GO
ALTER TABLE [dbo].[AuditLogs_AL] CHECK CONSTRAINT [CK_AuditLogs_AL_OldValues_IsJson]
GO
ALTER TABLE [dbo].[Batches_BT]  WITH CHECK ADD  CONSTRAINT [CK_Batches_BT_Capacity_Positive] CHECK  (([BT_Capacity] IS NULL OR [BT_Capacity]>(0)))
GO
ALTER TABLE [dbo].[Batches_BT] CHECK CONSTRAINT [CK_Batches_BT_Capacity_Positive]
GO
ALTER TABLE [dbo].[Batches_BT]  WITH CHECK ADD  CONSTRAINT [CK_Batches_BT_DateRange] CHECK  (([BT_EndDate] IS NULL OR [BT_EndDate]>[BT_StartDate]))
GO
ALTER TABLE [dbo].[Batches_BT] CHECK CONSTRAINT [CK_Batches_BT_DateRange]
GO
ALTER TABLE [dbo].[Batches_BT]  WITH CHECK ADD  CONSTRAINT [CK_Batches_BT_Status] CHECK  (([BT_Status]='pending' OR [BT_Status]='suspended' OR [BT_Status]='inactive' OR [BT_Status]='active'))
GO
ALTER TABLE [dbo].[Batches_BT] CHECK CONSTRAINT [CK_Batches_BT_Status]
GO
ALTER TABLE [dbo].[BatchStudents_BS]  WITH CHECK ADD  CONSTRAINT [CK_BatchStudents_BS_DateRange] CHECK  (([BS_LeftAt] IS NULL OR [BS_LeftAt]>=[BS_JoinedAt]))
GO
ALTER TABLE [dbo].[BatchStudents_BS] CHECK CONSTRAINT [CK_BatchStudents_BS_DateRange]
GO
ALTER TABLE [dbo].[Branches_B]  WITH CHECK ADD  CONSTRAINT [CK_Branches_B_Status] CHECK  (([B_Status]='pending' OR [B_Status]='suspended' OR [B_Status]='inactive' OR [B_Status]='active'))
GO
ALTER TABLE [dbo].[Branches_B] CHECK CONSTRAINT [CK_Branches_B_Status]
GO
ALTER TABLE [dbo].[Classrooms_CR]  WITH CHECK ADD  CONSTRAINT [CK_Classrooms_CR_Capacity_Positive] CHECK  (([CR_Capacity]>(0)))
GO
ALTER TABLE [dbo].[Classrooms_CR] CHECK CONSTRAINT [CK_Classrooms_CR_Capacity_Positive]
GO
ALTER TABLE [dbo].[Courses_C]  WITH CHECK ADD  CONSTRAINT [CK_Courses_C_Status] CHECK  (([C_Status]='pending' OR [C_Status]='suspended' OR [C_Status]='inactive' OR [C_Status]='active'))
GO
ALTER TABLE [dbo].[Courses_C] CHECK CONSTRAINT [CK_Courses_C_Status]
GO
ALTER TABLE [dbo].[CourseSubjects_CS]  WITH CHECK ADD  CONSTRAINT [CK_CourseSubjects_CS_MaxMarks_NonNegative] CHECK  (([CS_MaxMarks] IS NULL OR [CS_MaxMarks]>=(0)))
GO
ALTER TABLE [dbo].[CourseSubjects_CS] CHECK CONSTRAINT [CK_CourseSubjects_CS_MaxMarks_NonNegative]
GO
ALTER TABLE [dbo].[CourseSubjects_CS]  WITH CHECK ADD  CONSTRAINT [CK_CourseSubjects_CS_PassMarks_LE_MaxMarks] CHECK  (([CS_PassMarks] IS NULL OR [CS_MaxMarks] IS NULL OR [CS_PassMarks]<=[CS_MaxMarks]))
GO
ALTER TABLE [dbo].[CourseSubjects_CS] CHECK CONSTRAINT [CK_CourseSubjects_CS_PassMarks_LE_MaxMarks]
GO
ALTER TABLE [dbo].[CourseSubjects_CS]  WITH CHECK ADD  CONSTRAINT [CK_CourseSubjects_CS_PassMarks_NonNegative] CHECK  (([CS_PassMarks] IS NULL OR [CS_PassMarks]>=(0)))
GO
ALTER TABLE [dbo].[CourseSubjects_CS] CHECK CONSTRAINT [CK_CourseSubjects_CS_PassMarks_NonNegative]
GO
ALTER TABLE [dbo].[CustomFields_CF]  WITH CHECK ADD  CONSTRAINT [CK_CustomFields_CF_Options_IsJson] CHECK  (([CF_Options] IS NULL OR isjson([CF_Options])=(1)))
GO
ALTER TABLE [dbo].[CustomFields_CF] CHECK CONSTRAINT [CK_CustomFields_CF_Options_IsJson]
GO
ALTER TABLE [dbo].[CustomFieldValues_CFV]  WITH CHECK ADD  CONSTRAINT [CK_CustomFieldValues_CFV_Value_IsJson] CHECK  ((isjson([CFV_Value])=(1)))
GO
ALTER TABLE [dbo].[CustomFieldValues_CFV] CHECK CONSTRAINT [CK_CustomFieldValues_CFV_Value_IsJson]
GO
ALTER TABLE [dbo].[Discounts_DIS]  WITH CHECK ADD  CONSTRAINT [CK_Discounts_DIS_DiscountType] CHECK  (([DIS_DiscountType]='fixed' OR [DIS_DiscountType]='percentage'))
GO
ALTER TABLE [dbo].[Discounts_DIS] CHECK CONSTRAINT [CK_Discounts_DIS_DiscountType]
GO
ALTER TABLE [dbo].[Discounts_DIS]  WITH CHECK ADD  CONSTRAINT [CK_Discounts_DIS_Value_NonNegative] CHECK  (([DIS_Value]>=(0)))
GO
ALTER TABLE [dbo].[Discounts_DIS] CHECK CONSTRAINT [CK_Discounts_DIS_Value_NonNegative]
GO
ALTER TABLE [dbo].[Documents_DOC]  WITH CHECK ADD  CONSTRAINT [CK_Documents_DOC_FileSize_NonNegative] CHECK  (([DOC_FileSize]>=(0)))
GO
ALTER TABLE [dbo].[Documents_DOC] CHECK CONSTRAINT [CK_Documents_DOC_FileSize_NonNegative]
GO
ALTER TABLE [dbo].[Enrollments_E]  WITH CHECK ADD  CONSTRAINT [CK_Enrollments_E_CompletionDate] CHECK  (([E_CompletionDate] IS NULL OR [E_CompletionDate]>=[E_EnrollmentDate]))
GO
ALTER TABLE [dbo].[Enrollments_E] CHECK CONSTRAINT [CK_Enrollments_E_CompletionDate]
GO
ALTER TABLE [dbo].[Enrollments_E]  WITH CHECK ADD  CONSTRAINT [CK_Enrollments_E_Status] CHECK  (([E_Status]='on_hold' OR [E_Status]='transferred' OR [E_Status]='discontinued' OR [E_Status]='completed' OR [E_Status]='active'))
GO
ALTER TABLE [dbo].[Enrollments_E] CHECK CONSTRAINT [CK_Enrollments_E_Status]
GO
ALTER TABLE [dbo].[EntityDocuments_ED]  WITH CHECK ADD  CONSTRAINT [CK_EntityDocuments_ED_EntityType] CHECK  (([ED_EntityType]='other' OR [ED_EntityType]='guardian' OR [ED_EntityType]='application' OR [ED_EntityType]='staff' OR [ED_EntityType]='student'))
GO
ALTER TABLE [dbo].[EntityDocuments_ED] CHECK CONSTRAINT [CK_EntityDocuments_ED_EntityType]
GO
ALTER TABLE [dbo].[Exams_EX]  WITH CHECK ADD  CONSTRAINT [CK_Exams_EX_DateRange] CHECK  (([EX_EndDate]>=[EX_StartDate]))
GO
ALTER TABLE [dbo].[Exams_EX] CHECK CONSTRAINT [CK_Exams_EX_DateRange]
GO
ALTER TABLE [dbo].[Exams_EX]  WITH CHECK ADD  CONSTRAINT [CK_Exams_EX_Status] CHECK  (([EX_Status]='results_published' OR [EX_Status]='cancelled' OR [EX_Status]='completed' OR [EX_Status]='ongoing' OR [EX_Status]='scheduled'))
GO
ALTER TABLE [dbo].[Exams_EX] CHECK CONSTRAINT [CK_Exams_EX_Status]
GO
ALTER TABLE [dbo].[ExamSchedules_ESC]  WITH CHECK ADD  CONSTRAINT [CK_ExamSchedules_ESC_TimeRange] CHECK  (([ESC_EndTime]>[ESC_StartTime]))
GO
ALTER TABLE [dbo].[ExamSchedules_ESC] CHECK CONSTRAINT [CK_ExamSchedules_ESC_TimeRange]
GO
ALTER TABLE [dbo].[ExamSubjects_ES]  WITH CHECK ADD  CONSTRAINT [CK_ExamSubjects_ES_MaxMarks_Positive] CHECK  (([ES_MaxMarks]>(0)))
GO
ALTER TABLE [dbo].[ExamSubjects_ES] CHECK CONSTRAINT [CK_ExamSubjects_ES_MaxMarks_Positive]
GO
ALTER TABLE [dbo].[ExamSubjects_ES]  WITH CHECK ADD  CONSTRAINT [CK_ExamSubjects_ES_PassMarks_LE_MaxMarks] CHECK  (([ES_PassMarks]<=[ES_MaxMarks]))
GO
ALTER TABLE [dbo].[ExamSubjects_ES] CHECK CONSTRAINT [CK_ExamSubjects_ES_PassMarks_LE_MaxMarks]
GO
ALTER TABLE [dbo].[ExamSubjects_ES]  WITH CHECK ADD  CONSTRAINT [CK_ExamSubjects_ES_PassMarks_NonNegative] CHECK  (([ES_PassMarks]>=(0)))
GO
ALTER TABLE [dbo].[ExamSubjects_ES] CHECK CONSTRAINT [CK_ExamSubjects_ES_PassMarks_NonNegative]
GO
ALTER TABLE [dbo].[ExamSubjects_ES]  WITH CHECK ADD  CONSTRAINT [CK_ExamSubjects_ES_Weightage_Range] CHECK  (([ES_Weightage] IS NULL OR [ES_Weightage]>=(0) AND [ES_Weightage]<=(100)))
GO
ALTER TABLE [dbo].[ExamSubjects_ES] CHECK CONSTRAINT [CK_ExamSubjects_ES_Weightage_Range]
GO
ALTER TABLE [dbo].[Expenses_EXP]  WITH CHECK ADD  CONSTRAINT [CK_Expenses_EXP_Amount_Positive] CHECK  (([EXP_Amount]>(0)))
GO
ALTER TABLE [dbo].[Expenses_EXP] CHECK CONSTRAINT [CK_Expenses_EXP_Amount_Positive]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoiceItems_FII_DiscountAmount_NonNegative] CHECK  (([FII_DiscountAmount]>=(0)))
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] CHECK CONSTRAINT [CK_FeeInvoiceItems_FII_DiscountAmount_NonNegative]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoiceItems_FII_Quantity_Positive] CHECK  (([FII_Quantity]>(0)))
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] CHECK CONSTRAINT [CK_FeeInvoiceItems_FII_Quantity_Positive]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoiceItems_FII_TaxAmount_NonNegative] CHECK  (([FII_TaxAmount]>=(0)))
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] CHECK CONSTRAINT [CK_FeeInvoiceItems_FII_TaxAmount_NonNegative]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoiceItems_FII_TotalAmount_NonNegative] CHECK  (([FII_TotalAmount]>=(0)))
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] CHECK CONSTRAINT [CK_FeeInvoiceItems_FII_TotalAmount_NonNegative]
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoiceItems_FII_UnitAmount_NonNegative] CHECK  (([FII_UnitAmount]>=(0)))
GO
ALTER TABLE [dbo].[FeeInvoiceItems_FII] CHECK CONSTRAINT [CK_FeeInvoiceItems_FII_UnitAmount_NonNegative]
GO
ALTER TABLE [dbo].[FeeInvoices_FI]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoices_FI_DiscountAmount_NonNegative] CHECK  (([FI_DiscountAmount]>=(0)))
GO
ALTER TABLE [dbo].[FeeInvoices_FI] CHECK CONSTRAINT [CK_FeeInvoices_FI_DiscountAmount_NonNegative]
GO
ALTER TABLE [dbo].[FeeInvoices_FI]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoices_FI_DueDate] CHECK  (([FI_DueDate]>=[FI_InvoiceDate]))
GO
ALTER TABLE [dbo].[FeeInvoices_FI] CHECK CONSTRAINT [CK_FeeInvoices_FI_DueDate]
GO
ALTER TABLE [dbo].[FeeInvoices_FI]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoices_FI_PaidAmount_NonNegative] CHECK  (([FI_PaidAmount]>=(0)))
GO
ALTER TABLE [dbo].[FeeInvoices_FI] CHECK CONSTRAINT [CK_FeeInvoices_FI_PaidAmount_NonNegative]
GO
ALTER TABLE [dbo].[FeeInvoices_FI]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoices_FI_Status] CHECK  (([FI_Status]='cancelled' OR [FI_Status]='overdue' OR [FI_Status]='paid' OR [FI_Status]='partially_paid' OR [FI_Status]='issued' OR [FI_Status]='draft'))
GO
ALTER TABLE [dbo].[FeeInvoices_FI] CHECK CONSTRAINT [CK_FeeInvoices_FI_Status]
GO
ALTER TABLE [dbo].[FeeInvoices_FI]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoices_FI_Subtotal_NonNegative] CHECK  (([FI_Subtotal]>=(0)))
GO
ALTER TABLE [dbo].[FeeInvoices_FI] CHECK CONSTRAINT [CK_FeeInvoices_FI_Subtotal_NonNegative]
GO
ALTER TABLE [dbo].[FeeInvoices_FI]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoices_FI_TaxAmount_NonNegative] CHECK  (([FI_TaxAmount]>=(0)))
GO
ALTER TABLE [dbo].[FeeInvoices_FI] CHECK CONSTRAINT [CK_FeeInvoices_FI_TaxAmount_NonNegative]
GO
ALTER TABLE [dbo].[FeeInvoices_FI]  WITH CHECK ADD  CONSTRAINT [CK_FeeInvoices_FI_TotalAmount_NonNegative] CHECK  (([FI_TotalAmount]>=(0)))
GO
ALTER TABLE [dbo].[FeeInvoices_FI] CHECK CONSTRAINT [CK_FeeInvoices_FI_TotalAmount_NonNegative]
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI]  WITH CHECK ADD  CONSTRAINT [CK_FeeStructureItems_FSI_Amount_NonNegative] CHECK  (([FSI_Amount]>=(0)))
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI] CHECK CONSTRAINT [CK_FeeStructureItems_FSI_Amount_NonNegative]
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI]  WITH CHECK ADD  CONSTRAINT [CK_FeeStructureItems_FSI_DueDays_NonNegative] CHECK  (([FSI_DueDays] IS NULL OR [FSI_DueDays]>=(0)))
GO
ALTER TABLE [dbo].[FeeStructureItems_FSI] CHECK CONSTRAINT [CK_FeeStructureItems_FSI_DueDays_NonNegative]
GO
ALTER TABLE [dbo].[GradeScaleItems_GSI]  WITH CHECK ADD  CONSTRAINT [CK_GradeScaleItems_GSI_GradePoint_NonNegative] CHECK  (([GSI_GradePoint] IS NULL OR [GSI_GradePoint]>=(0)))
GO
ALTER TABLE [dbo].[GradeScaleItems_GSI] CHECK CONSTRAINT [CK_GradeScaleItems_GSI_GradePoint_NonNegative]
GO
ALTER TABLE [dbo].[GradeScaleItems_GSI]  WITH CHECK ADD  CONSTRAINT [CK_GradeScaleItems_GSI_PercentageRange] CHECK  (([GSI_MinPercentage]>=(0) AND [GSI_MaxPercentage]<=(100) AND [GSI_MaxPercentage]>[GSI_MinPercentage]))
GO
ALTER TABLE [dbo].[GradeScaleItems_GSI] CHECK CONSTRAINT [CK_GradeScaleItems_GSI_PercentageRange]
GO
ALTER TABLE [dbo].[Marks_M]  WITH CHECK ADD  CONSTRAINT [CK_Marks_M_MarksObtained_NonNegative] CHECK  (([M_MarksObtained]>=(0)))
GO
ALTER TABLE [dbo].[Marks_M] CHECK CONSTRAINT [CK_Marks_M_MarksObtained_NonNegative]
GO
ALTER TABLE [dbo].[Marks_M]  WITH CHECK ADD  CONSTRAINT [CK_Marks_M_Percentage_Range] CHECK  (([M_Percentage] IS NULL OR [M_Percentage]>=(0) AND [M_Percentage]<=(100)))
GO
ALTER TABLE [dbo].[Marks_M] CHECK CONSTRAINT [CK_Marks_M_Percentage_Range]
GO
ALTER TABLE [dbo].[Notifications_N]  WITH CHECK ADD  CONSTRAINT [CK_Notifications_N_Channel] CHECK  (([N_Channel]='in_app' OR [N_Channel]='push' OR [N_Channel]='sms' OR [N_Channel]='email'))
GO
ALTER TABLE [dbo].[Notifications_N] CHECK CONSTRAINT [CK_Notifications_N_Channel]
GO
ALTER TABLE [dbo].[Organizations_O]  WITH CHECK ADD  CONSTRAINT [CK_Organizations_O_Status] CHECK  (([O_Status]='pending' OR [O_Status]='suspended' OR [O_Status]='inactive' OR [O_Status]='active'))
GO
ALTER TABLE [dbo].[Organizations_O] CHECK CONSTRAINT [CK_Organizations_O_Status]
GO
ALTER TABLE [dbo].[OrganizationSettings_OS]  WITH CHECK ADD  CONSTRAINT [CK_OrganizationSettings_OS_SettingValue_IsJson] CHECK  (([OS_SettingValue] IS NULL OR isjson([OS_SettingValue])=(1)))
GO
ALTER TABLE [dbo].[OrganizationSettings_OS] CHECK CONSTRAINT [CK_OrganizationSettings_OS_SettingValue_IsJson]
GO
ALTER TABLE [dbo].[PaymentAllocations_PA]  WITH CHECK ADD  CONSTRAINT [CK_PaymentAllocations_PA_AllocatedAmount_Positive] CHECK  (([PA_AllocatedAmount]>(0)))
GO
ALTER TABLE [dbo].[PaymentAllocations_PA] CHECK CONSTRAINT [CK_PaymentAllocations_PA_AllocatedAmount_Positive]
GO
ALTER TABLE [dbo].[Payments_PAY]  WITH CHECK ADD  CONSTRAINT [CK_Payments_PAY_Amount_Positive] CHECK  (([PAY_Amount]>(0)))
GO
ALTER TABLE [dbo].[Payments_PAY] CHECK CONSTRAINT [CK_Payments_PAY_Amount_Positive]
GO
ALTER TABLE [dbo].[Payments_PAY]  WITH CHECK ADD  CONSTRAINT [CK_Payments_PAY_Status] CHECK  (([PAY_Status]='cancelled' OR [PAY_Status]='refunded' OR [PAY_Status]='failed' OR [PAY_Status]='completed' OR [PAY_Status]='pending'))
GO
ALTER TABLE [dbo].[Payments_PAY] CHECK CONSTRAINT [CK_Payments_PAY_Status]
GO
ALTER TABLE [dbo].[Programs_P]  WITH CHECK ADD  CONSTRAINT [CK_Programs_P_DurationUnit] CHECK  (([P_DurationUnit] IS NULL OR ([P_DurationUnit]='terms' OR [P_DurationUnit]='semesters' OR [P_DurationUnit]='years' OR [P_DurationUnit]='months' OR [P_DurationUnit]='weeks' OR [P_DurationUnit]='days')))
GO
ALTER TABLE [dbo].[Programs_P] CHECK CONSTRAINT [CK_Programs_P_DurationUnit]
GO
ALTER TABLE [dbo].[Programs_P]  WITH CHECK ADD  CONSTRAINT [CK_Programs_P_Status] CHECK  (([P_Status]='pending' OR [P_Status]='suspended' OR [P_Status]='inactive' OR [P_Status]='active'))
GO
ALTER TABLE [dbo].[Programs_P] CHECK CONSTRAINT [CK_Programs_P_Status]
GO
ALTER TABLE [dbo].[Refunds_RF]  WITH CHECK ADD  CONSTRAINT [CK_Refunds_RF_Amount_Positive] CHECK  (([RF_Amount]>(0)))
GO
ALTER TABLE [dbo].[Refunds_RF] CHECK CONSTRAINT [CK_Refunds_RF_Amount_Positive]
GO
ALTER TABLE [dbo].[Refunds_RF]  WITH CHECK ADD  CONSTRAINT [CK_Refunds_RF_Status] CHECK  (([RF_Status]='rejected' OR [RF_Status]='processed' OR [RF_Status]='approved' OR [RF_Status]='pending'))
GO
ALTER TABLE [dbo].[Refunds_RF] CHECK CONSTRAINT [CK_Refunds_RF_Status]
GO
ALTER TABLE [dbo].[Results_R]  WITH CHECK ADD  CONSTRAINT [CK_Results_R_MarksObtained_LE_TotalMarks] CHECK  (([R_MarksObtained]<=[R_TotalMarks]))
GO
ALTER TABLE [dbo].[Results_R] CHECK CONSTRAINT [CK_Results_R_MarksObtained_LE_TotalMarks]
GO
ALTER TABLE [dbo].[Results_R]  WITH CHECK ADD  CONSTRAINT [CK_Results_R_MarksObtained_NonNegative] CHECK  (([R_MarksObtained]>=(0)))
GO
ALTER TABLE [dbo].[Results_R] CHECK CONSTRAINT [CK_Results_R_MarksObtained_NonNegative]
GO
ALTER TABLE [dbo].[Results_R]  WITH CHECK ADD  CONSTRAINT [CK_Results_R_Percentage_Range] CHECK  (([R_Percentage]>=(0) AND [R_Percentage]<=(100)))
GO
ALTER TABLE [dbo].[Results_R] CHECK CONSTRAINT [CK_Results_R_Percentage_Range]
GO
ALTER TABLE [dbo].[Results_R]  WITH CHECK ADD  CONSTRAINT [CK_Results_R_ResultStatus] CHECK  (([R_ResultStatus]='pending' OR [R_ResultStatus]='withheld' OR [R_ResultStatus]='absent' OR [R_ResultStatus]='fail' OR [R_ResultStatus]='pass'))
GO
ALTER TABLE [dbo].[Results_R] CHECK CONSTRAINT [CK_Results_R_ResultStatus]
GO
ALTER TABLE [dbo].[Results_R]  WITH CHECK ADD  CONSTRAINT [CK_Results_R_TotalMarks_Positive] CHECK  (([R_TotalMarks]>(0)))
GO
ALTER TABLE [dbo].[Results_R] CHECK CONSTRAINT [CK_Results_R_TotalMarks_Positive]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA]  WITH CHECK ADD  CONSTRAINT [CK_StudentFeeAssignments_SFA_AssignedAmount_NonNegative] CHECK  (([SFA_AssignedAmount]>=(0)))
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] CHECK CONSTRAINT [CK_StudentFeeAssignments_SFA_AssignedAmount_NonNegative]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA]  WITH CHECK ADD  CONSTRAINT [CK_StudentFeeAssignments_SFA_DiscountAmount_NonNegative] CHECK  (([SFA_DiscountAmount]>=(0)))
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] CHECK CONSTRAINT [CK_StudentFeeAssignments_SFA_DiscountAmount_NonNegative]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA]  WITH CHECK ADD  CONSTRAINT [CK_StudentFeeAssignments_SFA_FinalAmount_NonNegative] CHECK  (([SFA_FinalAmount]>=(0)))
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] CHECK CONSTRAINT [CK_StudentFeeAssignments_SFA_FinalAmount_NonNegative]
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA]  WITH CHECK ADD  CONSTRAINT [CK_StudentFeeAssignments_SFA_Status] CHECK  (([SFA_Status]='waived' OR [SFA_Status]='cancelled' OR [SFA_Status]='completed' OR [SFA_Status]='active'))
GO
ALTER TABLE [dbo].[StudentFeeAssignments_SFA] CHECK CONSTRAINT [CK_StudentFeeAssignments_SFA_Status]
GO
ALTER TABLE [dbo].[Subjects_SB]  WITH CHECK ADD  CONSTRAINT [CK_Subjects_SB_Credits_NonNegative] CHECK  (([SB_Credits] IS NULL OR [SB_Credits]>=(0)))
GO
ALTER TABLE [dbo].[Subjects_SB] CHECK CONSTRAINT [CK_Subjects_SB_Credits_NonNegative]
GO
ALTER TABLE [dbo].[Subjects_SB]  WITH CHECK ADD  CONSTRAINT [CK_Subjects_SB_MaxMarks_NonNegative] CHECK  (([SB_MaxMarks] IS NULL OR [SB_MaxMarks]>=(0)))
GO
ALTER TABLE [dbo].[Subjects_SB] CHECK CONSTRAINT [CK_Subjects_SB_MaxMarks_NonNegative]
GO
ALTER TABLE [dbo].[Subjects_SB]  WITH CHECK ADD  CONSTRAINT [CK_Subjects_SB_PassMarks_LE_MaxMarks] CHECK  (([SB_PassMarks] IS NULL OR [SB_MaxMarks] IS NULL OR [SB_PassMarks]<=[SB_MaxMarks]))
GO
ALTER TABLE [dbo].[Subjects_SB] CHECK CONSTRAINT [CK_Subjects_SB_PassMarks_LE_MaxMarks]
GO
ALTER TABLE [dbo].[Subjects_SB]  WITH CHECK ADD  CONSTRAINT [CK_Subjects_SB_PassMarks_NonNegative] CHECK  (([SB_PassMarks] IS NULL OR [SB_PassMarks]>=(0)))
GO
ALTER TABLE [dbo].[Subjects_SB] CHECK CONSTRAINT [CK_Subjects_SB_PassMarks_NonNegative]
GO
ALTER TABLE [dbo].[TeacherAttendance_TA]  WITH CHECK ADD  CONSTRAINT [CK_TA_Status] CHECK  (([TA_Status]='OnLeave' OR [TA_Status]='HalfDay' OR [TA_Status]='Late' OR [TA_Status]='Absent' OR [TA_Status]='Present'))
GO
ALTER TABLE [dbo].[TeacherAttendance_TA] CHECK CONSTRAINT [CK_TA_Status]
GO
ALTER TABLE [dbo].[TeacherLeaves_TL]  WITH CHECK ADD  CONSTRAINT [CK_TL_DateRange] CHECK  (([TL_ToDate]>=[TL_FromDate]))
GO
ALTER TABLE [dbo].[TeacherLeaves_TL] CHECK CONSTRAINT [CK_TL_DateRange]
GO
ALTER TABLE [dbo].[TeacherLeaves_TL]  WITH CHECK ADD  CONSTRAINT [CK_TL_Status] CHECK  (([TL_Status]='Cancelled' OR [TL_Status]='Rejected' OR [TL_Status]='Approved' OR [TL_Status]='Pending'))
GO
ALTER TABLE [dbo].[TeacherLeaves_TL] CHECK CONSTRAINT [CK_TL_Status]
GO
ALTER TABLE [dbo].[Timetables_TT]  WITH CHECK ADD  CONSTRAINT [CK_Timetables_TT_DayOfWeek] CHECK  (([TT_DayOfWeek]>=(1) AND [TT_DayOfWeek]<=(7)))
GO
ALTER TABLE [dbo].[Timetables_TT] CHECK CONSTRAINT [CK_Timetables_TT_DayOfWeek]
GO
ALTER TABLE [dbo].[Timetables_TT]  WITH CHECK ADD  CONSTRAINT [CK_Timetables_TT_EffectiveRange] CHECK  (([TT_EffectiveTo] IS NULL OR [TT_EffectiveFrom] IS NULL OR [TT_EffectiveTo]>=[TT_EffectiveFrom]))
GO
ALTER TABLE [dbo].[Timetables_TT] CHECK CONSTRAINT [CK_Timetables_TT_EffectiveRange]
GO
ALTER TABLE [dbo].[Timetables_TT]  WITH CHECK ADD  CONSTRAINT [CK_Timetables_TT_TimeRange] CHECK  (([TT_EndTime]>[TT_StartTime]))
GO
ALTER TABLE [dbo].[Timetables_TT] CHECK CONSTRAINT [CK_Timetables_TT_TimeRange]
GO
ALTER TABLE [dbo].[Classes_CL] ADD  CONSTRAINT [DF_Classes_CL_Id]  DEFAULT (newid()) FOR [CL_Id]
GO
ALTER TABLE [dbo].[Classes_CL] ADD  CONSTRAINT [DF_Classes_CL_IsActive]  DEFAULT ((1)) FOR [CL_IsActive]
GO
ALTER TABLE [dbo].[Classes_CL] ADD  CONSTRAINT [DF_Classes_CL_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CL_CreatedAt]
GO
ALTER TABLE [dbo].[Classes_CL] ADD  CONSTRAINT [DF_Classes_CL_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [CL_UpdatedAt]
GO
ALTER TABLE [dbo].[Classes_CL]  WITH CHECK ADD  CONSTRAINT [FK_Classes_CL_BranchId] FOREIGN KEY([CL_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[Classes_CL] CHECK CONSTRAINT [FK_Classes_CL_BranchId]
GO
ALTER TABLE [dbo].[Sections_S] ADD  CONSTRAINT [DF_Sections_S_Id]  DEFAULT (newid()) FOR [S_Sect_Id]
GO
ALTER TABLE [dbo].[Sections_S] ADD  CONSTRAINT [DF_Sections_S_IsActive]  DEFAULT ((1)) FOR [S_Sect_IsActive]
GO
ALTER TABLE [dbo].[Sections_S] ADD  CONSTRAINT [DF_Sections_S_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [S_Sect_CreatedAt]
GO
ALTER TABLE [dbo].[Sections_S] ADD  CONSTRAINT [DF_Sections_S_UpdatedAt]  DEFAULT (sysutcdatetime()) FOR [S_Sect_UpdatedAt]
GO
ALTER TABLE [dbo].[Sections_S]  WITH CHECK ADD  CONSTRAINT [FK_Sections_S_BranchId] FOREIGN KEY([S_Sect_BranchId])
REFERENCES [dbo].[Branches_B] ([B_Id])
GO
ALTER TABLE [dbo].[Sections_S] CHECK CONSTRAINT [FK_Sections_S_BranchId]
GO
ALTER TABLE [dbo].[Sections_S]  WITH CHECK ADD  CONSTRAINT [FK_Sections_S_ClassId] FOREIGN KEY([S_Sect_ClassId])
REFERENCES [dbo].[Classes_CL] ([CL_Id])
GO
ALTER TABLE [dbo].[Sections_S] CHECK CONSTRAINT [FK_Sections_S_ClassId]
GO
ALTER TABLE [dbo].[Students_S]  WITH CHECK ADD  CONSTRAINT [FK_Students_S_ClassId] FOREIGN KEY([S_ClassId])
REFERENCES [dbo].[Classes_CL] ([CL_Id])
GO
ALTER TABLE [dbo].[Students_S] CHECK CONSTRAINT [FK_Students_S_ClassId]
GO
ALTER TABLE [dbo].[Students_S]  WITH CHECK ADD  CONSTRAINT [FK_Students_S_SectionId] FOREIGN KEY([S_SectionId])
REFERENCES [dbo].[Sections_S] ([S_Sect_Id])
GO
ALTER TABLE [dbo].[Students_S] CHECK CONSTRAINT [FK_Students_S_SectionId]
GO
ALTER TABLE [dbo].[Staff_ST] ADD  CONSTRAINT [PK_Staff_ST] PRIMARY KEY CLUSTERED 
(
	[ST_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_Guardians_Create]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Guardians_Create]
    @G_Id           UNIQUEIDENTIFIER,
    @G_TenantId     UNIQUEIDENTIFIER,
    @G_FirstName    NVARCHAR(100),
    @G_LastName     NVARCHAR(100),
    @G_Phone        NVARCHAR(30),
    @G_Email        NVARCHAR(255) = NULL,
    @G_Occupation   NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Guardians_G
        (G_Id, G_TenantId, G_FirstName, G_LastName, G_Email, G_Phone, G_Occupation, G_CreatedAt, G_UpdatedAt)
    VALUES
        (@G_Id, @G_TenantId, @G_FirstName, @G_LastName, @G_Email, @G_Phone, @G_Occupation, SYSUTCDATETIME(), SYSUTCDATETIME());
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Guardians_GetById]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_Guardians_GetById]
    @G_Id UNIQUEIDENTIFIER,
    @G_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.Guardians_G
    WHERE G_Id = @G_Id AND G_TenantId = @G_TenantId;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Guardians_Search]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Guardians_Search]
    @G_TenantId  UNIQUEIDENTIFIER,
    @SearchTerm  NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Pattern NVARCHAR(257) = '%' + @SearchTerm + '%';

    SELECT TOP 10 G_Id, G_FirstName, G_LastName, G_Phone, G_Email, G_Occupation
    FROM dbo.Guardians_G
    WHERE G_TenantId = @G_TenantId
      AND (G_FirstName LIKE @Pattern OR G_LastName LIKE @Pattern OR G_Phone LIKE @Pattern OR G_Email LIKE @Pattern)
    ORDER BY G_FirstName;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Guardians_Update]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Guardians_Update]
    @G_Id           UNIQUEIDENTIFIER,
    @G_TenantId     UNIQUEIDENTIFIER,
    @G_FirstName    NVARCHAR(100),
    @G_LastName     NVARCHAR(100),
    @G_Phone        NVARCHAR(30),
    @G_Email        NVARCHAR(255) = NULL,
    @G_Occupation   NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Guardians_G
    SET G_FirstName = @G_FirstName,
        G_LastName  = @G_LastName,
        G_Phone     = @G_Phone,
        G_Email     = @G_Email,
        G_Occupation = @G_Occupation,
        G_UpdatedAt = SYSUTCDATETIME()
    WHERE G_Id = @G_Id AND G_TenantId = @G_TenantId;

    SELECT @@ROWCOUNT AS RowsAffected;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_StudentGuardians_Add]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_StudentGuardians_Add]
    @SG_Id          UNIQUEIDENTIFIER,
    @SG_StudentId   UNIQUEIDENTIFIER,
    @SG_GuardianId  UNIQUEIDENTIFIER,
    @SG_Relation    NVARCHAR(50),
    @SG_IsPrimary   BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Students_Guardians WHERE SG_StudentId = @SG_StudentId AND SG_GuardianId = @SG_GuardianId)
    BEGIN
        INSERT INTO dbo.Students_Guardians (SG_Id, SG_StudentId, SG_GuardianId, SG_Relation, SG_IsPrimary, SG_CreatedAt)
        VALUES (@SG_Id, @SG_StudentId, @SG_GuardianId, @SG_Relation, @SG_IsPrimary, SYSUTCDATETIME());
    END
END

GO
/****** Object:  StoredProcedure [dbo].[SP_StudentGuardians_GetByStudentId]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_StudentGuardians_GetByStudentId]
    @SG_StudentId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        sg.SG_Id, sg.SG_StudentId, sg.SG_GuardianId, sg.SG_Relation, sg.SG_IsPrimary,
        g.G_FirstName, g.G_LastName, g.G_Phone, g.G_Email, g.G_Occupation
    FROM dbo.Students_Guardians sg
    INNER JOIN dbo.Guardians_G g ON g.G_Id = sg.SG_GuardianId
    WHERE sg.SG_StudentId = @SG_StudentId
    ORDER BY sg.SG_IsPrimary DESC, g.G_FirstName;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_StudentGuardians_RemoveByStudent]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_StudentGuardians_RemoveByStudent]
    @SG_StudentId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Students_Guardians WHERE SG_StudentId = @SG_StudentId;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Students_CheckAdmissionNumberExists]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Students_CheckAdmissionNumberExists]
    @S_TenantId        UNIQUEIDENTIFIER,
    @AdmissionNumber   NVARCHAR(50),
    @ExcludeId         UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM dbo.Students_S
        WHERE S_TenantId = @S_TenantId
          AND S_AdmissionNumber = @AdmissionNumber
          AND S_DeletedAt IS NULL
          AND (@ExcludeId IS NULL OR S_Id <> @ExcludeId)
    ) THEN 1 ELSE 0 END AS IsTaken;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Students_CheckStudentCodeExists]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Students_CheckStudentCodeExists]
    @S_TenantId    UNIQUEIDENTIFIER,
    @StudentCode   NVARCHAR(50),
    @ExcludeId     UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM dbo.Students_S
        WHERE S_TenantId = @S_TenantId
          AND S_StudentCode = @StudentCode
          AND S_DeletedAt IS NULL
          AND (@ExcludeId IS NULL OR S_Id <> @ExcludeId)
    ) THEN 1 ELSE 0 END AS IsTaken;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Students_Create]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Students_Create]
    @S_Id               UNIQUEIDENTIFIER,
    @S_TenantId         UNIQUEIDENTIFIER,
    @S_BranchId         UNIQUEIDENTIFIER,
    @S_UserId           UNIQUEIDENTIFIER = NULL,
    @S_StudentCode      NVARCHAR(50),
    @S_AdmissionNumber  NVARCHAR(50) = NULL,   -- optional now: blank => auto-generated
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
    @S_Country          NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF (@S_AdmissionNumber IS NULL OR LTRIM(RTRIM(@S_AdmissionNumber)) = '')
    BEGIN
        DECLARE @Year INT = YEAR(SYSUTCDATETIME());
        DECLARE @NextNum INT;
        DECLARE @Counter TABLE (LastNumber INT);

        -- Atomic upsert-and-increment: safe under concurrent inserts
        -- (two admins saving at the same moment cannot get the same number).
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

    -- Return the (possibly generated) admission number so the app layer can show it
    SELECT @S_AdmissionNumber AS S_AdmissionNumber;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Students_GetById]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Students_GetById]
    @S_Id       UNIQUEIDENTIFIER,
    @S_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM dbo.Students_S
    WHERE S_Id = @S_Id
      AND S_TenantId = @S_TenantId
      AND S_DeletedAt IS NULL;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Students_GetPaged]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Students_GetPaged]
    @S_TenantId    UNIQUEIDENTIFIER,
    @SearchTerm    NVARCHAR(255) = NULL,
    @Status        NVARCHAR(20)  = NULL,
    @BranchId      UNIQUEIDENTIFIER = NULL,
    @PageNumber    INT = 1,
    @PageSize      INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize NOT BETWEEN 1 AND 100 SET @PageSize = 10;

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    DECLARE @SearchPattern NVARCHAR(257) = '%' + @SearchTerm + '%';

    -- Result set 1: total count
    SELECT COUNT(*) AS TotalCount
    FROM dbo.Students_S
    WHERE S_TenantId = @S_TenantId
      AND S_DeletedAt IS NULL
      AND (@SearchTerm IS NULL OR
           S_FirstName LIKE @SearchPattern OR
           S_LastName LIKE @SearchPattern OR
           S_AdmissionNumber LIKE @SearchPattern OR
           S_StudentCode LIKE @SearchPattern)
      AND (@Status IS NULL OR S_Status = @Status)
      AND (@BranchId IS NULL OR S_BranchId = @BranchId);

    -- Result set 2: page of rows
    SELECT *
    FROM dbo.Students_S
    WHERE S_TenantId = @S_TenantId
      AND S_DeletedAt IS NULL
      AND (@SearchTerm IS NULL OR
           S_FirstName LIKE @SearchPattern OR
           S_LastName LIKE @SearchPattern OR
           S_AdmissionNumber LIKE @SearchPattern OR
           S_StudentCode LIKE @SearchPattern)
      AND (@Status IS NULL OR S_Status = @Status)
      AND (@BranchId IS NULL OR S_BranchId = @BranchId)
    ORDER BY S_CreatedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Students_SoftDelete]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Students_SoftDelete]
    @S_Id       UNIQUEIDENTIFIER,
    @S_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Students_S
    SET S_DeletedAt = SYSUTCDATETIME(),
        S_UpdatedAt  = SYSUTCDATETIME()
    WHERE S_Id = @S_Id
      AND S_TenantId = @S_TenantId
      AND S_DeletedAt IS NULL;

    SELECT @@ROWCOUNT AS RowsAffected;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_Students_Update]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[SP_Students_Update]
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
    @S_Country          NVARCHAR(100) = NULL
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

    SELECT @@ROWCOUNT AS RowsAffected;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherAttendance_AddEdit]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =========================================================
-- 5. Attendance — Upsert (auto On-Leave override lives here)
-- =========================================================
CREATE   PROCEDURE [dbo].[SP_TeacherAttendance_AddEdit]
    @TA_Id          UNIQUEIDENTIFIER,
    @TA_TenantId    UNIQUEIDENTIFIER,
    @TA_TeacherId   UNIQUEIDENTIFIER,
    @TA_Date        DATE,
    @TA_Status      NVARCHAR(20),
    @TA_Remarks     NVARCHAR(500) = NULL,
    @TA_MarkedBy    UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @WasOverridden BIT = 0;

    -- If this teacher has an Approved leave covering this date, the day is
    -- always "OnLeave" regardless of what status was requested - don't let
    -- an admin accidentally mark someone Absent on an approved leave day.
    IF EXISTS (
        SELECT 1 FROM dbo.TeacherLeaves_TL
        WHERE TL_TenantId = @TA_TenantId
          AND TL_TeacherId = @TA_TeacherId
          AND TL_Status = 'Approved'
          AND @TA_Date BETWEEN TL_FromDate AND TL_ToDate
    )
    BEGIN
        IF (@TA_Status <> 'OnLeave') SET @WasOverridden = 1;
        SET @TA_Status = 'OnLeave';
    END

    MERGE dbo.TeacherAttendance_TA AS target
    USING (SELECT @TA_TenantId AS TenantId, @TA_TeacherId AS TeacherId, @TA_Date AS [Date]) AS src
        ON target.TA_TenantId = src.TenantId
       AND target.TA_TeacherId = src.TeacherId
       AND target.TA_Date = src.[Date]
    WHEN MATCHED THEN
        UPDATE SET TA_Status = @TA_Status,
                   TA_Remarks = @TA_Remarks,
                   TA_MarkedBy = @TA_MarkedBy,
                   TA_UpdatedAt = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (TA_Id, TA_TenantId, TA_TeacherId, TA_Date, TA_Status, TA_Remarks, TA_MarkedBy, TA_CreatedAt, TA_UpdatedAt)
        VALUES (@TA_Id, @TA_TenantId, @TA_TeacherId, @TA_Date, @TA_Status, @TA_Remarks, @TA_MarkedBy, SYSUTCDATETIME(), SYSUTCDATETIME());

    SELECT @TA_Status AS FinalStatus, @WasOverridden AS WasOverriddenToOnLeave;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherAttendance_GetByDate]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =========================================================
-- 6. Attendance — Reads
-- =========================================================
CREATE   PROCEDURE [dbo].[SP_TeacherAttendance_GetByDate]
    @TA_TenantId    UNIQUEIDENTIFIER,
    @TA_Date        DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.TeacherAttendance_TA
    WHERE TA_TenantId = @TA_TenantId AND TA_Date = @TA_Date;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherAttendance_GetByTeacherAndRange]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_TeacherAttendance_GetByTeacherAndRange]
    @TA_TenantId    UNIQUEIDENTIFIER,
    @TA_TeacherId   UNIQUEIDENTIFIER,
    @FromDate       DATE,
    @ToDate         DATE
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.TeacherAttendance_TA
    WHERE TA_TenantId = @TA_TenantId
      AND TA_TeacherId = @TA_TeacherId
      AND TA_Date BETWEEN @FromDate AND @ToDate
    ORDER BY TA_Date DESC;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherLeaves_Apply]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =========================================================
-- 2. Leave — Apply (business rules: date validity, overlap check, day-count calc)
-- =========================================================
CREATE   PROCEDURE [dbo].[SP_TeacherLeaves_Apply]
    @TL_Id          UNIQUEIDENTIFIER,
    @TL_TenantId    UNIQUEIDENTIFIER,
    @TL_TeacherId   UNIQUEIDENTIFIER,
    @TL_TeacherName NVARCHAR(200),
    @TL_LeaveType   NVARCHAR(30),
    @TL_FromDate    DATE,
    @TL_ToDate      DATE,
    @TL_Reason      NVARCHAR(500) = NULL,
    @TL_AppliedBy   UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF (@TL_ToDate < @TL_FromDate)
            THROW 51001, 'End date cannot be before the start date.', 1;

        -- Overlap guard: block a second Pending/Approved request that
        -- shares any day with an existing Pending/Approved request for
        -- the same teacher.
        IF EXISTS (
            SELECT 1 FROM dbo.TeacherLeaves_TL
            WHERE TL_TenantId = @TL_TenantId
              AND TL_TeacherId = @TL_TeacherId
              AND TL_Status IN ('Pending','Approved')
              AND @TL_FromDate <= TL_ToDate
              AND @TL_ToDate >= TL_FromDate
        )
            THROW 51002, 'This teacher already has a pending or approved leave request that overlaps these dates.', 1;

        DECLARE @TotalDays INT = DATEDIFF(DAY, @TL_FromDate, @TL_ToDate) + 1;

        INSERT INTO dbo.TeacherLeaves_TL
            (TL_Id, TL_TenantId, TL_TeacherId, TL_TeacherName, TL_LeaveType,
             TL_FromDate, TL_ToDate, TL_TotalDays, TL_Reason, TL_Status,
             TL_AppliedAt, TL_AppliedBy, TL_CreatedAt, TL_UpdatedAt)
        VALUES
            (@TL_Id, @TL_TenantId, @TL_TeacherId, @TL_TeacherName, @TL_LeaveType,
             @TL_FromDate, @TL_ToDate, @TotalDays, @TL_Reason, 'Pending',
             SYSUTCDATETIME(), @TL_AppliedBy, SYSUTCDATETIME(), SYSUTCDATETIME());

        SELECT @TL_Id AS TL_Id, @TotalDays AS TotalDays;
    END TRY
    BEGIN CATCH
        THROW; -- re-raise as-is; the app layer catches SqlException and surfaces Message via toastr
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherLeaves_Approve]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =========================================================
-- 3. Leave — Approve / Reject / Cancel (status-transition guards)
-- =========================================================
CREATE   PROCEDURE [dbo].[SP_TeacherLeaves_Approve]
    @TL_Id          UNIQUEIDENTIFIER,
    @TL_TenantId    UNIQUEIDENTIFIER,
    @ApprovedBy     UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.TeacherLeaves_TL
    SET TL_Status = 'Approved',
        TL_ApprovedBy = @ApprovedBy,
        TL_ApprovedAt = SYSUTCDATETIME(),
        TL_UpdatedAt = SYSUTCDATETIME()
    WHERE TL_Id = @TL_Id AND TL_TenantId = @TL_TenantId AND TL_Status = 'Pending';

    SELECT @@ROWCOUNT AS RowsAffected; -- 0 means it wasn't Pending anymore (already actioned) or didn't exist
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherLeaves_Cancel]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_TeacherLeaves_Cancel]
    @TL_Id                  UNIQUEIDENTIFIER,
    @TL_TenantId            UNIQUEIDENTIFIER,
    @RequestingTeacherId    UNIQUEIDENTIFIER  -- enforces a teacher can only cancel THEIR OWN request
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.TeacherLeaves_TL
    SET TL_Status = 'Cancelled',
        TL_UpdatedAt = SYSUTCDATETIME()
    WHERE TL_Id = @TL_Id
      AND TL_TenantId = @TL_TenantId
      AND TL_TeacherId = @RequestingTeacherId
      AND TL_Status = 'Pending';

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherLeaves_GetAllPaged]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =========================================================
-- 4. Leave — Reads
-- =========================================================
CREATE   PROCEDURE [dbo].[SP_TeacherLeaves_GetAllPaged]
    @TL_TenantId    UNIQUEIDENTIFIER,
    @Status         NVARCHAR(20) = NULL,
    @PageNumber     INT = 1,
    @PageSize       INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize NOT BETWEEN 1 AND 100 SET @PageSize = 10;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM dbo.TeacherLeaves_TL
    WHERE TL_TenantId = @TL_TenantId AND (@Status IS NULL OR TL_Status = @Status);

    SELECT *
    FROM dbo.TeacherLeaves_TL
    WHERE TL_TenantId = @TL_TenantId AND (@Status IS NULL OR TL_Status = @Status)
    ORDER BY CASE WHEN TL_Status = 'Pending' THEN 0 ELSE 1 END, TL_AppliedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherLeaves_GetByTeacherId]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_TeacherLeaves_GetByTeacherId]
    @TL_TenantId    UNIQUEIDENTIFIER,
    @TL_TeacherId   UNIQUEIDENTIFIER,
    @PageNumber     INT = 1,
    @PageSize       INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    IF @PageNumber < 1 SET @PageNumber = 1;
    IF @PageSize NOT BETWEEN 1 AND 100 SET @PageSize = 10;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount FROM dbo.TeacherLeaves_TL
    WHERE TL_TenantId = @TL_TenantId AND TL_TeacherId = @TL_TeacherId;

    SELECT * FROM dbo.TeacherLeaves_TL
    WHERE TL_TenantId = @TL_TenantId AND TL_TeacherId = @TL_TeacherId
    ORDER BY TL_AppliedAt DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherLeaves_Reject]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_TeacherLeaves_Reject]
    @TL_Id              UNIQUEIDENTIFIER,
    @TL_TenantId        UNIQUEIDENTIFIER,
    @ApprovedBy         UNIQUEIDENTIFIER,
    @RejectionReason    NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.TeacherLeaves_TL
    SET TL_Status = 'Rejected',
        TL_ApprovedBy = @ApprovedBy,
        TL_ApprovedAt = SYSUTCDATETIME(),
        TL_RejectionReason = @RejectionReason,
        TL_UpdatedAt = SYSUTCDATETIME()
    WHERE TL_Id = @TL_Id AND TL_TenantId = @TL_TenantId AND TL_Status = 'Pending';

    SELECT @@ROWCOUNT AS RowsAffected;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_TeacherLeaves_Update]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[SP_TeacherLeaves_Update]
    @TL_Id                  UNIQUEIDENTIFIER,
    @TL_TenantId            UNIQUEIDENTIFIER,
    @RequestingTeacherId    UNIQUEIDENTIFIER,  -- ownership check: must match TL_TeacherId
    @TL_LeaveType           NVARCHAR(30),
    @TL_FromDate            DATE,
    @TL_ToDate              DATE,
    @TL_Reason              NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        IF (@TL_ToDate < @TL_FromDate)
            THROW 51001, 'End date cannot be before the start date.', 1;

        -- Ownership + status guard up front - if this doesn't match, nothing
        -- to update, and we don't want to leak whether the ID exists at all.
        IF NOT EXISTS (
            SELECT 1 FROM dbo.TeacherLeaves_TL
            WHERE TL_Id = @TL_Id AND TL_TenantId = @TL_TenantId
              AND TL_TeacherId = @RequestingTeacherId AND TL_Status = 'Pending'
        )
        BEGIN
            SELECT 0 AS RowsAffected, 0 AS TotalDays;
            RETURN;
        END

        -- Same overlap guard as Apply, EXCLUDING this row itself
        IF EXISTS (
            SELECT 1 FROM dbo.TeacherLeaves_TL
            WHERE TL_TenantId = @TL_TenantId
              AND TL_TeacherId = @RequestingTeacherId
              AND TL_Id <> @TL_Id
              AND TL_Status IN ('Pending','Approved')
              AND @TL_FromDate <= TL_ToDate
              AND @TL_ToDate >= TL_FromDate
        )
            THROW 51002, 'This overlaps another pending or approved leave request.', 1;

        DECLARE @TotalDays INT = DATEDIFF(DAY, @TL_FromDate, @TL_ToDate) + 1;

        UPDATE dbo.TeacherLeaves_TL
        SET TL_LeaveType = @TL_LeaveType,
            TL_FromDate = @TL_FromDate,
            TL_ToDate = @TL_ToDate,
            TL_TotalDays = @TotalDays,
            TL_Reason = @TL_Reason,
            TL_UpdatedAt = SYSUTCDATETIME()
        WHERE TL_Id = @TL_Id AND TL_TenantId = @TL_TenantId
          AND TL_TeacherId = @RequestingTeacherId AND TL_Status = 'Pending';

        SELECT @@ROWCOUNT AS RowsAffected, @TotalDays AS TotalDays;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Teachers_AddEdit]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Teachers_AddEdit]
    @T_Id UNIQUEIDENTIFIER,
    @T_TenantId UNIQUEIDENTIFIER,
    @T_BranchId UNIQUEIDENTIFIER,
    @T_EmployeeCode NVARCHAR(50),
    @T_Designation NVARCHAR(100) = NULL,
    @T_Department NVARCHAR(100) = NULL,
    @T_JoiningDate DATE = NULL,
    @T_Qualification NVARCHAR(255) = NULL,
    @T_ExperienceYears INT = NULL,
    @T_BloodGroup NVARCHAR(10) = NULL,
    @T_Status NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    MERGE dbo.Teachers_T AS target
    USING (SELECT @T_Id AS T_Id, @T_TenantId AS T_TenantId) AS src
        ON target.T_Id = src.T_Id AND target.T_TenantId = src.T_TenantId
    WHEN MATCHED THEN
        UPDATE SET
            T_BranchId = @T_BranchId,
            T_EmployeeCode = @T_EmployeeCode,
            T_Designation = @T_Designation,
            T_Department = @T_Department,
            T_JoiningDate = @T_JoiningDate,
            T_Qualification = @T_Qualification,
            T_ExperienceYears = @T_ExperienceYears,
            T_BloodGroup = @T_BloodGroup,
            T_Status = @T_Status,
            T_IsActive = 1,
            T_UpdatedAt = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (T_Id, T_TenantId, T_BranchId, T_EmployeeCode, T_Designation, T_Department,
                T_JoiningDate, T_Qualification, T_ExperienceYears, T_BloodGroup, T_Status, T_IsActive, T_CreatedAt, T_UpdatedAt)
        VALUES (@T_Id, @T_TenantId, @T_BranchId, @T_EmployeeCode, @T_Designation, @T_Department,
                @T_JoiningDate, @T_Qualification, @T_ExperienceYears, @T_BloodGroup, @T_Status, 1, SYSUTCDATETIME(), SYSUTCDATETIME());

    SELECT @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Teachers_CheckEmployeeCodeExists]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Teachers_CheckEmployeeCodeExists]
    @T_TenantId UNIQUEIDENTIFIER,
    @EmployeeCode NVARCHAR(50),
    @ExcludeId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CASE WHEN EXISTS (
        SELECT 1 FROM dbo.Teachers_T
        WHERE T_TenantId = @T_TenantId AND T_EmployeeCode = @EmployeeCode AND T_IsActive = 1
          AND (@ExcludeId IS NULL OR T_Id <> @ExcludeId)
    ) THEN 1 ELSE 0 END;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Teachers_GetById]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Teachers_GetById]
    @T_Id UNIQUEIDENTIFIER,
    @T_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM dbo.Teachers_T
    WHERE T_Id = @T_Id AND T_TenantId = @T_TenantId;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Teachers_GetByIds]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_Teachers_GetByIds]
    @IdsCsv NVARCHAR(MAX),
    @T_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT t.* FROM dbo.Teachers_T t
    INNER JOIN (SELECT CAST(value AS UNIQUEIDENTIFIER) AS Id FROM STRING_SPLIT(@IdsCsv, ',')) ids
        ON t.T_Id = ids.Id
    WHERE t.T_TenantId = @T_TenantId AND t.T_IsActive = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Teachers_SoftDelete]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_Teachers_SoftDelete]
    @T_Id UNIQUEIDENTIFIER,
    @T_TenantId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Teachers_T SET T_IsActive = 0, T_UpdatedAt = SYSUTCDATETIME()
    WHERE T_Id = @T_Id AND T_TenantId = @T_TenantId AND T_IsActive = 1;

    SELECT @@ROWCOUNT;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_AcademicYears_AY]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_AcademicYears_AY]
    @Action NVARCHAR(20),
    @AY_Id UNIQUEIDENTIFIER = NULL,
    @AY_Name NVARCHAR(100) = NULL,
    @AY_Code NVARCHAR(50) = NULL,
    @AY_StartDate DATE = NULL,
    @AY_EndDate DATE = NULL,
    @AY_IsCurrent BIT = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT
            AY_Id,
            AY_TenantId,
            AY_Name,
            AY_Code,
            AY_StartDate,
            AY_EndDate,
            AY_IsCurrent,
            AY_CreatedAt,
            AY_UpdatedAt
        FROM dbo.AcademicYears_AY
        WHERE @TenantId IS NULL OR AY_TenantId = @TenantId
        ORDER BY AY_StartDate DESC;

        RETURN;
    END

    IF @Action = 'GetById'
    BEGIN
        SELECT
            AY_Id,
            AY_TenantId,
            AY_Name,
            AY_Code,
            AY_StartDate,
            AY_EndDate,
            AY_IsCurrent,
            AY_CreatedAt,
            AY_UpdatedAt
        FROM dbo.AcademicYears_AY
        WHERE AY_Id = @AY_Id
          AND (@TenantId IS NULL OR AY_TenantId = @TenantId);

        RETURN;
    END

    IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();

        INSERT INTO dbo.AcademicYears_AY
        (
            AY_Id,
            AY_TenantId,
            AY_Name,
            AY_Code,
            AY_StartDate,
            AY_EndDate,
            AY_IsCurrent
        )
        VALUES
        (
            @NewId,
            @TenantId,
            @AY_Name,
            @AY_Code,
            @AY_StartDate,
            @AY_EndDate,
            ISNULL(@AY_IsCurrent, 0)
        );

        RETURN;
    END

    IF @Action = 'Update'
    BEGIN
        UPDATE dbo.AcademicYears_AY
        SET
            AY_Name = @AY_Name,
            AY_Code = @AY_Code,
            AY_StartDate = @AY_StartDate,
            AY_EndDate = @AY_EndDate,
            AY_IsCurrent = ISNULL(@AY_IsCurrent, 0),
            AY_UpdatedAt = SYSUTCDATETIME()
        WHERE AY_Id = @AY_Id
          AND (@TenantId IS NULL OR AY_TenantId = @TenantId);

        RETURN;
    END

    IF @Action = 'Deactivate'
    BEGIN
        RAISERROR('AcademicYears_AY does not support soft delete.', 16, 1);
        RETURN;
    END

    IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);

        IF @ColumnName NOT IN
        (
            'AY_Name',
            'AY_Code',
            'AY_StartDate',
            'AY_EndDate'
        )
        BEGIN
            RAISERROR('Invalid column name.', 16, 1);
            RETURN;
        END

        IF @ColumnName IN ('AY_StartDate', 'AY_EndDate')
        BEGIN
            DECLARE @DateValue DATE;

            SET @DateValue = TRY_CONVERT(DATE, @Value, 23);

            IF @DateValue IS NULL
            BEGIN
                SET @DateValue = TRY_CONVERT(DATE, @Value);
            END

            IF @DateValue IS NULL
            BEGIN
                RAISERROR('Invalid date value.', 16, 1);
                RETURN;
            END

            SET @Sql = N'
                SELECT COUNT(1)
                FROM dbo.AcademicYears_AY
                WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val
                  AND (@P_TenantId IS NULL OR AY_TenantId = @P_TenantId)';

            IF @ExcludeId IS NOT NULL
                SET @Sql += N' AND AY_Id <> @P_ExId';

            EXEC sp_executesql
                @Sql,
                N'@P_Val DATE,
                  @P_ExId UNIQUEIDENTIFIER,
                  @P_TenantId UNIQUEIDENTIFIER',
                @P_Val = @DateValue,
                @P_ExId = @ExcludeId,
                @P_TenantId = @TenantId;

            RETURN;
        END

        SET @Sql = N'
            SELECT COUNT(1)
            FROM dbo.AcademicYears_AY
            WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val
              AND (@P_TenantId IS NULL OR AY_TenantId = @P_TenantId)';

        IF @ExcludeId IS NOT NULL
            SET @Sql += N' AND AY_Id <> @P_ExId';

        EXEC sp_executesql
            @Sql,
            N'@P_Val NVARCHAR(MAX),
              @P_ExId UNIQUEIDENTIFIER,
              @P_TenantId UNIQUEIDENTIFIER',
            @P_Val = @Value,
            @P_ExId = @ExcludeId,
            @P_TenantId = @TenantId;

        RETURN;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_AdmissionApplications_AA]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================================
-- 5. USP_AdmissionApplications_AA
-- ============================================================================
CREATE PROCEDURE [dbo].[USP_AdmissionApplications_AA]
    @Action NVARCHAR(20),
    @AA_Id UNIQUEIDENTIFIER = NULL,
    @AA_TenantId UNIQUEIDENTIFIER = NULL,
    @AA_BranchId UNIQUEIDENTIFIER = NULL,
    @AA_ApplicationNumber NVARCHAR(50) = NULL,
    @AA_FirstName NVARCHAR(100) = NULL,
    @AA_LastName NVARCHAR(100) = NULL,
    @AA_DateOfBirth DATE = NULL,
    @AA_Gender NVARCHAR(20) = NULL,
    @AA_Email NVARCHAR(255) = NULL,
    @AA_Phone NVARCHAR(30) = NULL,
    @AA_CourseId UNIQUEIDENTIFIER = NULL,
    @AA_AcademicYearId UNIQUEIDENTIFIER = NULL,
    @AA_Status NVARCHAR(20) = NULL,
    @AA_SubmittedAt DATETIME2 = NULL,
    @AA_Notes NVARCHAR(MAX) = NULL,
    @AA_ReviewedBy UNIQUEIDENTIFIER = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @BranchId UNIQUEIDENTIFIER = NULL,
    @CourseId UNIQUEIDENTIFIER = NULL,
    @AcademicYearId UNIQUEIDENTIFIER = NULL,
    @Status NVARCHAR(20) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetById'
        BEGIN
            SELECT a.*, c.C_Name AS CourseName, ay.AY_Name AS AcademicYearName
            FROM AdmissionApplications_AA a
            LEFT JOIN Courses_C c ON a.AA_CourseId = c.C_Id
            LEFT JOIN AcademicYears_AY ay ON a.AA_AcademicYearId = ay.AY_Id
            WHERE a.AA_Id = @AA_Id AND a.AA_TenantId = @AA_TenantId;
        END

        ELSE IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM AdmissionApplications_AA a
            WHERE a.AA_TenantId = @AA_TenantId
              AND (@SearchTerm IS NULL OR a.AA_ApplicationNumber LIKE '%' + @SearchTerm + '%'
                   OR a.AA_FirstName LIKE '%' + @SearchTerm + '%' OR a.AA_LastName LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR a.AA_BranchId = @BranchId)
              AND (@CourseId IS NULL OR a.AA_CourseId = @CourseId)
              AND (@AcademicYearId IS NULL OR a.AA_AcademicYearId = @AcademicYearId)
              AND (@Status IS NULL OR a.AA_Status = @Status);

            SELECT a.*, c.C_Name AS CourseName, ay.AY_Name AS AcademicYearName
            FROM AdmissionApplications_AA a
            LEFT JOIN Courses_C c ON a.AA_CourseId = c.C_Id
            LEFT JOIN AcademicYears_AY ay ON a.AA_AcademicYearId = ay.AY_Id
            WHERE a.AA_TenantId = @AA_TenantId
              AND (@SearchTerm IS NULL OR a.AA_ApplicationNumber LIKE '%' + @SearchTerm + '%'
                   OR a.AA_FirstName LIKE '%' + @SearchTerm + '%' OR a.AA_LastName LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR a.AA_BranchId = @BranchId)
              AND (@CourseId IS NULL OR a.AA_CourseId = @CourseId)
              AND (@AcademicYearId IS NULL OR a.AA_AcademicYearId = @AcademicYearId)
              AND (@Status IS NULL OR a.AA_Status = @Status)
            ORDER BY a.AA_CreatedAt DESC
            OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
        END

        ELSE IF @Action = 'ExistsByNumber'
        BEGIN
            SELECT CASE WHEN EXISTS (
                SELECT 1 FROM AdmissionApplications_AA WHERE AA_TenantId = @AA_TenantId AND AA_ApplicationNumber = @AA_ApplicationNumber
                AND (@ExcludeId IS NULL OR AA_Id <> @ExcludeId)
            ) THEN 1 ELSE 0 END;
        END

        ELSE IF @Action = 'Insert'
        BEGIN
            IF @AA_ApplicationNumber IS NULL OR @AA_ApplicationNumber = ''
            BEGIN
                DECLARE @YearPart VARCHAR(4) = FORMAT(GETUTCDATE(), 'yy');
                DECLARE @Seq INT = ISNULL((SELECT MAX(CAST(RIGHT(AA_ApplicationNumber, 4) AS INT)) FROM AdmissionApplications_AA WHERE AA_TenantId = @AA_TenantId AND AA_ApplicationNumber LIKE 'APP-' + @YearPart + '-%'), 0) + 1;
                SET @AA_ApplicationNumber = 'APP-' + @YearPart + '-' + FORMAT(@Seq, '0000');
            END

            INSERT INTO AdmissionApplications_AA (AA_Id, AA_TenantId, AA_BranchId, AA_ApplicationNumber, AA_FirstName, AA_LastName, AA_DateOfBirth, AA_Gender, AA_Email, AA_Phone, AA_CourseId, AA_AcademicYearId, AA_Status, AA_SubmittedAt, AA_Notes, AA_CreatedAt, AA_UpdatedAt)
            VALUES (@AA_Id, @AA_TenantId, @AA_BranchId, @AA_ApplicationNumber, @AA_FirstName, @AA_LastName, @AA_DateOfBirth, @AA_Gender, @AA_Email, @AA_Phone, @AA_CourseId, @AA_AcademicYearId, @AA_Status, @AA_SubmittedAt, @AA_Notes, GETUTCDATE(), GETUTCDATE());
            SELECT @AA_Id;
        END

        ELSE IF @Action = 'Update'
        BEGIN
            UPDATE AdmissionApplications_AA SET
                AA_BranchId = @AA_BranchId, AA_ApplicationNumber = ISNULL(@AA_ApplicationNumber, AA_ApplicationNumber),
                AA_FirstName = @AA_FirstName, AA_LastName = @AA_LastName, AA_DateOfBirth = @AA_DateOfBirth,
                AA_Gender = @AA_Gender, AA_Email = @AA_Email, AA_Phone = @AA_Phone,
                AA_CourseId = @AA_CourseId, AA_AcademicYearId = @AA_AcademicYearId,
                AA_Status = @AA_Status, AA_Notes = @AA_Notes, AA_UpdatedAt = GETUTCDATE()
            WHERE AA_Id = @AA_Id AND AA_TenantId = @AA_TenantId;
            SELECT @@ROWCOUNT;
        END

        ELSE IF @Action = 'Delete'
        BEGIN
            DELETE FROM AdmissionApplications_AA WHERE AA_Id = @AA_Id AND AA_TenantId = @AA_TenantId;
            SELECT @@ROWCOUNT;
        END

        ELSE IF @Action = 'Review'
        BEGIN
            UPDATE AdmissionApplications_AA SET
                AA_Status = @AA_Status,
                AA_Notes = ISNULL(@AA_Notes, AA_Notes),
                AA_ReviewedAt = GETUTCDATE(),
                AA_ReviewedBy = @AA_ReviewedBy,
                AA_UpdatedAt = GETUTCDATE()
            WHERE AA_Id = @AA_Id AND AA_TenantId = @AA_TenantId;
            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_AttendanceRecords_AR]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- USP_AttendanceRecords_AR
-- ============================================================
CREATE PROCEDURE [dbo].[USP_AttendanceRecords_AR]
    @Action NVARCHAR(20),
    @AR_Id UNIQUEIDENTIFIER = NULL,
    @AR_AttendanceSessionId UNIQUEIDENTIFIER = NULL,
    @AR_StudentId UNIQUEIDENTIFIER = NULL,
    @AR_Status NVARCHAR(20) = NULL,
    @AR_Remarks NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetBySession'
        BEGIN
            SELECT ar.*,
                   s.S_FirstName + ' ' + s.S_LastName AS StudentName,
                   s.S_StudentCode AS StudentCode
            FROM AttendanceRecords_AR ar
            LEFT JOIN Students_S s ON ar.AR_StudentId = s.S_Id
            WHERE ar.AR_AttendanceSessionId = @AR_AttendanceSessionId
            ORDER BY s.S_FirstName, s.S_LastName;
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO AttendanceRecords_AR (AR_Id, AR_AttendanceSessionId, AR_StudentId, AR_Status, AR_Remarks, AR_CreatedAt, AR_UpdatedAt)
            VALUES (@AR_Id, @AR_AttendanceSessionId, @AR_StudentId, @AR_Status, @AR_Remarks, SYSUTCDATETIME(), SYSUTCDATETIME());
        END

        IF @Action = 'DeleteBySession'
        BEGIN
            DELETE FROM AttendanceRecords_AR WHERE AR_AttendanceSessionId = @AR_AttendanceSessionId;
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
/****** Object:  StoredProcedure [dbo].[USP_AttendanceSessions_AS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- USP_AttendanceSessions_AS
-- ============================================================
CREATE PROCEDURE [dbo].[USP_AttendanceSessions_AS]
    @Action NVARCHAR(20),
    @AS_Id UNIQUEIDENTIFIER = NULL,
    @AS_TenantId UNIQUEIDENTIFIER = NULL,
    @AS_BranchId UNIQUEIDENTIFIER = NULL,
    @AS_BatchId UNIQUEIDENTIFIER = NULL,
    @AS_SubjectId UNIQUEIDENTIFIER = NULL,
    @AS_StaffId UNIQUEIDENTIFIER = NULL,
    @AS_AttendanceDate DATE = NULL,
    @AS_StartTime TIME(7) = NULL,
    @AS_EndTime TIME(7) = NULL,
    @AS_Remarks NVARCHAR(MAX) = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @BranchId UNIQUEIDENTIFIER = NULL,
    @BatchId UNIQUEIDENTIFIER = NULL,
    @Date DATE = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetById'
        BEGIN
            SELECT s.*,
                   b.B_Name AS BranchName,
                   bt.BT_Name AS BatchName,
                   sb.SB_Name AS SubjectName,
                   st.ST_FirstName + ' ' + st.ST_LastName AS StaffName
            FROM AttendanceSessions_AS s
            LEFT JOIN Branches_B b ON s.AS_BranchId = b.B_Id
            LEFT JOIN Batches_BT bt ON s.AS_BatchId = bt.BT_Id
            LEFT JOIN Subjects_SB sb ON s.AS_SubjectId = sb.SB_Id
            LEFT JOIN Staff_ST st ON s.AS_StaffId = st.ST_Id
            WHERE s.AS_Id = @AS_Id AND s.AS_TenantId = @AS_TenantId;
        END

        IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM AttendanceSessions_AS s
            LEFT JOIN Batches_BT bt ON s.AS_BatchId = bt.BT_Id
            LEFT JOIN Subjects_SB sb ON s.AS_SubjectId = sb.SB_Id
            WHERE s.AS_TenantId = @AS_TenantId
              AND (@SearchTerm IS NULL OR bt.BT_Name LIKE '%' + @SearchTerm + '%' OR sb.SB_Name LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR s.AS_BranchId = @BranchId)
              AND (@BatchId IS NULL OR s.AS_BatchId = @BatchId)
              AND (@Date IS NULL OR s.AS_AttendanceDate = @Date);

            SELECT s.*,
                   b.B_Name AS BranchName,
                   bt.BT_Name AS BatchName,
                   sb.SB_Name AS SubjectName,
                   st.ST_FirstName + ' ' + st.ST_LastName AS StaffName
            FROM AttendanceSessions_AS s
            LEFT JOIN Branches_B b ON s.AS_BranchId = b.B_Id
            LEFT JOIN Batches_BT bt ON s.AS_BatchId = bt.BT_Id
            LEFT JOIN Subjects_SB sb ON s.AS_SubjectId = sb.SB_Id
            LEFT JOIN Staff_ST st ON s.AS_StaffId = st.ST_Id
            WHERE s.AS_TenantId = @AS_TenantId
              AND (@SearchTerm IS NULL OR bt.BT_Name LIKE '%' + @SearchTerm + '%' OR sb.SB_Name LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR s.AS_BranchId = @BranchId)
              AND (@BatchId IS NULL OR s.AS_BatchId = @BatchId)
              AND (@Date IS NULL OR s.AS_AttendanceDate = @Date)
            ORDER BY s.AS_AttendanceDate DESC, s.AS_CreatedAt DESC
            OFFSET (@PageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO AttendanceSessions_AS (AS_Id, AS_TenantId, AS_BranchId, AS_BatchId, AS_SubjectId, AS_StaffId, AS_AttendanceDate, AS_StartTime, AS_EndTime, AS_Remarks, AS_CreatedAt, AS_UpdatedAt)
            VALUES (@AS_Id, @AS_TenantId, @AS_BranchId, @AS_BatchId, @AS_SubjectId, @AS_StaffId, @AS_AttendanceDate, @AS_StartTime, @AS_EndTime, @AS_Remarks, SYSUTCDATETIME(), SYSUTCDATETIME());

            SELECT @AS_Id;
        END

        IF @Action = 'Update'
        BEGIN
            UPDATE AttendanceSessions_AS
            SET AS_BranchId = @AS_BranchId,
                AS_BatchId = @AS_BatchId,
                AS_SubjectId = @AS_SubjectId,
                AS_StaffId = @AS_StaffId,
                AS_AttendanceDate = @AS_AttendanceDate,
                AS_StartTime = @AS_StartTime,
                AS_EndTime = @AS_EndTime,
                AS_Remarks = @AS_Remarks,
                AS_UpdatedAt = SYSUTCDATETIME()
            WHERE AS_Id = @AS_Id AND AS_TenantId = @AS_TenantId;

            SELECT @@ROWCOUNT;
        END

        IF @Action = 'Delete'
        BEGIN
            DELETE FROM AttendanceSessions_AS WHERE AS_Id = @AS_Id AND AS_TenantId = @AS_TenantId;
            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_Bank]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_Bank]
    @Action NVARCHAR(20),
    @BM_Id INT = NULL,
    @BM_BankName NVARCHAR(200) = NULL,
    @BM_AccountNo NVARCHAR(50) = NULL,
    @BM_IFSCCode NVARCHAR(20) = NULL,
    @BM_BranchName NVARCHAR(200) = NULL,
    @BM_IsActive BIT = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId INT = NULL,
    @NewId INT = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT BM_Id, BM_TenantId, BM_BankName, BM_AccountNo, BM_IFSCCode, BM_BranchName,
               BM_IsActive, BM_CreatedAt, BM_CreatedBy, BM_UpdatedAt, BM_UpdatedBy
        FROM dbo.BankMaster_BM
        WHERE BM_IsActive = 1
        ORDER BY BM_BankName;
    END

    ELSE IF @Action = 'GetById'
    BEGIN
        SELECT BM_Id, BM_TenantId, BM_BankName, BM_AccountNo, BM_IFSCCode, BM_BranchName,
               BM_IsActive, BM_CreatedAt, BM_CreatedBy, BM_UpdatedAt, BM_UpdatedBy
        FROM dbo.BankMaster_BM
        WHERE BM_Id = @BM_Id;
    END

    ELSE IF @Action = 'Insert'
    BEGIN
        INSERT INTO dbo.BankMaster_BM (BM_TenantId, BM_BankName, BM_AccountNo, BM_IFSCCode, BM_BranchName, BM_IsActive, BM_CreatedBy, BM_UpdatedBy)
        VALUES (@TenantId, @BM_BankName, @BM_AccountNo, @BM_IFSCCode, @BM_BranchName, ISNULL(@BM_IsActive, 1), @CreatedBy, @UpdatedBy);

        SET @NewId = SCOPE_IDENTITY();
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE dbo.BankMaster_BM
        SET BM_BankName = @BM_BankName,
            BM_AccountNo = @BM_AccountNo,
            BM_IFSCCode = @BM_IFSCCode,
            BM_BranchName = @BM_BranchName,
            BM_IsActive = @BM_IsActive,
            BM_UpdatedAt = GETUTCDATE(),
            BM_UpdatedBy = @UpdatedBy
        WHERE BM_Id = @BM_Id;
    END

    ELSE IF @Action = 'Deactivate'
    BEGIN
        UPDATE dbo.BankMaster_BM
        SET BM_IsActive = 0, BM_UpdatedAt = GETUTCDATE()
        WHERE BM_Id = @BM_Id;
    END

    ELSE IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'SELECT COUNT(1) FROM dbo.BankMaster_BM WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val AND BM_IsActive = 1';
        IF @ExcludeId IS NOT NULL
            SET @Sql = @Sql + N' AND BM_Id <> @P_ExId';
        EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX), @P_ExId INT', @Value, @ExcludeId;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Batches_BT]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Academic Module Stored Procedures
-- Batches, CourseSubjects, Enrollments, Timetables, AdmissionApplications

-- ============================================================================
-- 1. USP_Batches_BT
-- ============================================================================
CREATE PROCEDURE [dbo].[USP_Batches_BT]
    @Action NVARCHAR(20),
    @BT_Id UNIQUEIDENTIFIER = NULL,
    @BT_TenantId UNIQUEIDENTIFIER = NULL,
    @BT_BranchId UNIQUEIDENTIFIER = NULL,
    @BT_CourseId UNIQUEIDENTIFIER = NULL,
    @BT_AcademicYearId UNIQUEIDENTIFIER = NULL,
    @BT_Name NVARCHAR(150) = NULL,
    @BT_Code NVARCHAR(50) = NULL,
    @BT_StartDate DATE = NULL,
    @BT_EndDate DATE = NULL,
    @BT_Capacity INT = NULL,
    @BT_Status NVARCHAR(20) = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @BranchId UNIQUEIDENTIFIER = NULL,
    @CourseId UNIQUEIDENTIFIER = NULL,
    @AcademicYearId UNIQUEIDENTIFIER = NULL,
    @Status NVARCHAR(20) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetAll'
        BEGIN
            SELECT bt.*, c.C_Name AS CourseName, ay.AY_Name AS AcademicYearName,
                   (SELECT COUNT(*) FROM BatchStudents_BS bs WHERE bs.BS_BatchId = bt.BT_Id AND bs.BS_LeftAt IS NULL) AS EnrolledCount
            FROM Batches_BT bt
            LEFT JOIN Courses_C c ON bt.BT_CourseId = c.C_Id
            LEFT JOIN AcademicYears_AY ay ON bt.BT_AcademicYearId = ay.AY_Id
            ORDER BY bt.BT_Name;
        END

        ELSE IF @Action = 'GetById'
        BEGIN
            SELECT bt.*, c.C_Name AS CourseName, ay.AY_Name AS AcademicYearName,
                   (SELECT COUNT(*) FROM BatchStudents_BS bs WHERE bs.BS_BatchId = bt.BT_Id AND bs.BS_LeftAt IS NULL) AS EnrolledCount
            FROM Batches_BT bt
            LEFT JOIN Courses_C c ON bt.BT_CourseId = c.C_Id
            LEFT JOIN AcademicYears_AY ay ON bt.BT_AcademicYearId = ay.AY_Id
            WHERE bt.BT_Id = @BT_Id AND bt.BT_TenantId = @BT_TenantId;
        END

        ELSE IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM Batches_BT bt
            WHERE bt.BT_TenantId = @BT_TenantId
              AND (@SearchTerm IS NULL OR bt.BT_Name LIKE '%' + @SearchTerm + '%' OR bt.BT_Code LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR bt.BT_BranchId = @BranchId)
              AND (@CourseId IS NULL OR bt.BT_CourseId = @CourseId)
              AND (@AcademicYearId IS NULL OR bt.BT_AcademicYearId = @AcademicYearId)
              AND (@Status IS NULL OR bt.BT_Status = @Status);

            SELECT bt.*, c.C_Name AS CourseName, ay.AY_Name AS AcademicYearName,
                   (SELECT COUNT(*) FROM BatchStudents_BS bs WHERE bs.BS_BatchId = bt.BT_Id AND bs.BS_LeftAt IS NULL) AS EnrolledCount
            FROM Batches_BT bt
            LEFT JOIN Courses_C c ON bt.BT_CourseId = c.C_Id
            LEFT JOIN AcademicYears_AY ay ON bt.BT_AcademicYearId = ay.AY_Id
            WHERE bt.BT_TenantId = @BT_TenantId
              AND (@SearchTerm IS NULL OR bt.BT_Name LIKE '%' + @SearchTerm + '%' OR bt.BT_Code LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR bt.BT_BranchId = @BranchId)
              AND (@CourseId IS NULL OR bt.BT_CourseId = @CourseId)
              AND (@AcademicYearId IS NULL OR bt.BT_AcademicYearId = @AcademicYearId)
              AND (@Status IS NULL OR bt.BT_Status = @Status)
            ORDER BY bt.BT_Name
            OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
        END

        ELSE IF @Action = 'Insert'
        BEGIN
            INSERT INTO Batches_BT (BT_Id, BT_TenantId, BT_BranchId, BT_CourseId, BT_AcademicYearId, BT_Name, BT_Code, BT_StartDate, BT_EndDate, BT_Capacity, BT_Status, BT_CreatedAt, BT_UpdatedAt)
            VALUES (@BT_Id, @BT_TenantId, @BT_BranchId, @BT_CourseId, @BT_AcademicYearId, @BT_Name, @BT_Code, @BT_StartDate, @BT_EndDate, @BT_Capacity, @BT_Status, GETUTCDATE(), GETUTCDATE());
            SELECT @BT_Id;
        END

        ELSE IF @Action = 'Update'
        BEGIN
            UPDATE Batches_BT SET
                BT_BranchId = @BT_BranchId, BT_CourseId = @BT_CourseId, BT_AcademicYearId = @BT_AcademicYearId,
                BT_Name = @BT_Name, BT_Code = @BT_Code, BT_StartDate = @BT_StartDate, BT_EndDate = @BT_EndDate,
                BT_Capacity = @BT_Capacity, BT_Status = @BT_Status, BT_UpdatedAt = GETUTCDATE()
            WHERE BT_Id = @BT_Id AND BT_TenantId = @BT_TenantId;
            SELECT @@ROWCOUNT;
        END

        ELSE IF @Action = 'Delete'
        BEGIN
            DELETE FROM Batches_BT WHERE BT_Id = @BT_Id AND BT_TenantId = @BT_TenantId;
            SELECT @@ROWCOUNT;
        END

        ELSE IF @Action = 'ExistsByCode'
        BEGIN
            SELECT CASE WHEN EXISTS (
                SELECT 1 FROM Batches_BT WHERE BT_TenantId = @BT_TenantId AND BT_Code = @BT_Code
                AND (@ExcludeId IS NULL OR BT_Id <> @ExcludeId)
            ) THEN 1 ELSE 0 END;
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
/****** Object:  StoredProcedure [dbo].[USP_Branches_B]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_Branches_B]
    @Action NVARCHAR(20),
    @B_Id UNIQUEIDENTIFIER = NULL,
    @B_Name NVARCHAR(200) = NULL,
    @B_Code NVARCHAR(50) = NULL,
    @B_Email NVARCHAR(255) = NULL,
    @B_Phone NVARCHAR(30) = NULL,
    @B_AddressLine1 NVARCHAR(255) = NULL,
    @B_AddressLine2 NVARCHAR(255) = NULL,
    @B_City NVARCHAR(100) = NULL,
    @B_State NVARCHAR(100) = NULL,
    @B_PostalCode NVARCHAR(20) = NULL,
    @B_CountryCode CHAR(2) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT B_Id, B_TenantId, B_Name, B_Code, B_Email, B_Phone,
               B_AddressLine1, B_AddressLine2, B_City, B_State, B_PostalCode, B_CountryCode,
               B_Status, B_CreatedAt, B_UpdatedAt
        FROM dbo.Branches_B
        WHERE B_Status = 'active' AND B_DeletedAt IS NULL
        ORDER BY B_Name;
    END

    ELSE IF @Action = 'GetById'
    BEGIN
        SELECT B_Id, B_TenantId, B_Name, B_Code, B_Email, B_Phone,
               B_AddressLine1, B_AddressLine2, B_City, B_State, B_PostalCode, B_CountryCode,
               B_Status, B_CreatedAt, B_UpdatedAt
        FROM dbo.Branches_B
        WHERE B_Id = @B_Id;
    END

    ELSE IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();
        INSERT INTO dbo.Branches_B (B_Id, B_TenantId, B_Name, B_Code, B_Email, B_Phone,
            B_AddressLine1, B_AddressLine2, B_City, B_State, B_PostalCode, B_CountryCode, B_Status)
        VALUES (@NewId, @TenantId, @B_Name, @B_Code, @B_Email, @B_Phone,
            @B_AddressLine1, @B_AddressLine2, @B_City, @B_State, @B_PostalCode, @B_CountryCode, 'active');
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE dbo.Branches_B
        SET B_Name = @B_Name,
            B_Code = @B_Code,
            B_Email = @B_Email,
            B_Phone = @B_Phone,
            B_AddressLine1 = @B_AddressLine1,
            B_AddressLine2 = @B_AddressLine2,
            B_City = @B_City,
            B_State = @B_State,
            B_PostalCode = @B_PostalCode,
            B_CountryCode = @B_CountryCode,
            B_UpdatedAt = SYSUTCDATETIME()
        WHERE B_Id = @B_Id;
    END

    ELSE IF @Action = 'Deactivate'
    BEGIN
        UPDATE dbo.Branches_B
        SET B_Status = 'inactive', B_DeletedAt = SYSUTCDATETIME(), B_UpdatedAt = SYSUTCDATETIME()
        WHERE B_Id = @B_Id;
    END

    ELSE IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'SELECT COUNT(1) FROM dbo.Branches_B WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val AND B_Status = ''active'' AND B_DeletedAt IS NULL';
        IF @ExcludeId IS NOT NULL
            SET @Sql = @Sql + N' AND B_Id <> @P_ExId';
        EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX), @P_ExId UNIQUEIDENTIFIER', @Value, @ExcludeId;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Classrooms_CR]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_Classrooms_CR]
    @Action NVARCHAR(20),
    @CR_Id UNIQUEIDENTIFIER = NULL,
    @CR_Name NVARCHAR(100) = NULL,
    @CR_Code NVARCHAR(50) = NULL,
    @CR_BranchId UNIQUEIDENTIFIER = NULL,
    @CR_Capacity INT = NULL,
    @CR_Location NVARCHAR(255) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT
            CR_Id,
            CR_TenantId,
            CR_BranchId,
            CR_Name,
            CR_Code,
            CR_Capacity,
            CR_Location,
            CR_CreatedAt,
            CR_UpdatedAt
        FROM dbo.Classrooms_CR
        WHERE @TenantId IS NULL
           OR CR_TenantId = @TenantId
        ORDER BY CR_Name;

        RETURN;
    END;

    IF @Action = 'GetById'
    BEGIN
        SELECT
            CR_Id,
            CR_TenantId,
            CR_BranchId,
            CR_Name,
            CR_Code,
            CR_Capacity,
            CR_Location,
            CR_CreatedAt,
            CR_UpdatedAt
        FROM dbo.Classrooms_CR
        WHERE CR_Id = @CR_Id
          AND (
                @TenantId IS NULL
                OR CR_TenantId = @TenantId
              );

        RETURN;
    END;

    IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();

        INSERT INTO dbo.Classrooms_CR
        (
            CR_Id,
            CR_TenantId,
            CR_BranchId,
            CR_Name,
            CR_Code,
            CR_Capacity,
            CR_Location
        )
        VALUES
        (
            @NewId,
            @TenantId,
            @CR_BranchId,
            LTRIM(RTRIM(@CR_Name)),
            LTRIM(RTRIM(@CR_Code)),
            @CR_Capacity,
            LTRIM(RTRIM(@CR_Location))
        );

        RETURN;
    END;

    IF @Action = 'Update'
    BEGIN
        UPDATE dbo.Classrooms_CR
        SET
            CR_BranchId = @CR_BranchId,
            CR_Name = LTRIM(RTRIM(@CR_Name)),
            CR_Code = LTRIM(RTRIM(@CR_Code)),
            CR_Capacity = @CR_Capacity,
            CR_Location = LTRIM(RTRIM(@CR_Location)),
            CR_UpdatedAt = SYSUTCDATETIME()
        WHERE CR_Id = @CR_Id
          AND (
                @TenantId IS NULL
                OR CR_TenantId = @TenantId
              );

        RETURN;
    END;

    IF @Action = 'Deactivate'
    BEGIN
        RAISERROR(
            'Classrooms_CR does not support soft delete.',
            16,
            1
        );

        RETURN;
    END;

    IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);

        IF @ColumnName NOT IN
        (
            'CR_Name',
            'CR_Code',
            'CR_BranchId',
            'CR_Capacity',
            'CR_Location'
        )
        BEGIN
            RAISERROR('Invalid column name.', 16, 1);
            RETURN;
        END;

        -- Integer field
        IF @ColumnName = 'CR_Capacity'
        BEGIN
            DECLARE @CapacityValue INT;

            SET @CapacityValue = TRY_CONVERT(INT, @Value);

            IF @CapacityValue IS NULL
            BEGIN
                RAISERROR('Invalid seating capacity.', 16, 1);
                RETURN;
            END;

            SET @Sql = N'
                SELECT COUNT(1)
                FROM dbo.Classrooms_CR
                WHERE CR_Capacity = @P_Val
                  AND (
                        @P_TenantId IS NULL
                        OR CR_TenantId = @P_TenantId
                      )';

            IF @ExcludeId IS NOT NULL
                SET @Sql += N'
                    AND CR_Id <> @P_ExId';

            EXEC sp_executesql
                @Sql,
                N'@P_Val INT,
                  @P_ExId UNIQUEIDENTIFIER,
                  @P_TenantId UNIQUEIDENTIFIER',
                @P_Val = @CapacityValue,
                @P_ExId = @ExcludeId,
                @P_TenantId = @TenantId;

            RETURN;
        END;

        -- GUID field
        IF @ColumnName = 'CR_BranchId'
        BEGIN
            DECLARE @BranchIdValue UNIQUEIDENTIFIER;

            SET @BranchIdValue = TRY_CONVERT(UNIQUEIDENTIFIER, @Value);

            IF @BranchIdValue IS NULL
            BEGIN
                RAISERROR('Invalid Branch.', 16, 1);
                RETURN;
            END;

            SET @Sql = N'
                SELECT COUNT(1)
                FROM dbo.Classrooms_CR
                WHERE CR_BranchId = @P_Val
                  AND (
                        @P_TenantId IS NULL
                        OR CR_TenantId = @P_TenantId
                      )';

            IF @ExcludeId IS NOT NULL
                SET @Sql += N'
                    AND CR_Id <> @P_ExId';

            EXEC sp_executesql
                @Sql,
                N'@P_Val UNIQUEIDENTIFIER,
                  @P_ExId UNIQUEIDENTIFIER,
                  @P_TenantId UNIQUEIDENTIFIER',
                @P_Val = @BranchIdValue,
                @P_ExId = @ExcludeId,
                @P_TenantId = @TenantId;

            RETURN;
        END;

        -- Text fields
        SET @Sql = N'
            SELECT COUNT(1)
            FROM dbo.Classrooms_CR
            WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val
              AND (
                    @P_TenantId IS NULL
                    OR CR_TenantId = @P_TenantId
                  )';

        IF @ExcludeId IS NOT NULL
            SET @Sql += N'
                AND CR_Id <> @P_ExId';

        EXEC sp_executesql
            @Sql,
            N'@P_Val NVARCHAR(MAX),
              @P_ExId UNIQUEIDENTIFIER,
              @P_TenantId UNIQUEIDENTIFIER',
            @P_Val = @Value,
            @P_ExId = @ExcludeId,
            @P_TenantId = @TenantId;

        RETURN;
    END;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Courses_C]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_Courses_C]
    @Action NVARCHAR(20),
    @C_Id UNIQUEIDENTIFIER = NULL,
    @C_Name NVARCHAR(200) = NULL,
    @C_Code NVARCHAR(50) = NULL,
    @C_ProgramId UNIQUEIDENTIFIER = NULL,
    @C_Description NVARCHAR(MAX) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT C_Id, C_TenantId, C_ProgramId, C_Name, C_Code,
               C_Description, C_Status, C_CreatedAt, C_UpdatedAt
        FROM dbo.Courses_C
        WHERE C_Status = 'active' AND C_DeletedAt IS NULL
        ORDER BY C_Name;
    END

    ELSE IF @Action = 'GetById'
    BEGIN
        SELECT C_Id, C_TenantId, C_ProgramId, C_Name, C_Code,
               C_Description, C_Status, C_CreatedAt, C_UpdatedAt
        FROM dbo.Courses_C
        WHERE C_Id = @C_Id;
    END

    ELSE IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();
        INSERT INTO dbo.Courses_C (C_Id, C_TenantId, C_ProgramId, C_Name, C_Code, C_Description, C_Status)
        VALUES (@NewId, @TenantId, @C_ProgramId, @C_Name, @C_Code, @C_Description, 'active');
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE dbo.Courses_C
        SET C_ProgramId = @C_ProgramId,
            C_Name = @C_Name,
            C_Code = @C_Code,
            C_Description = @C_Description,
            C_UpdatedAt = SYSUTCDATETIME()
        WHERE C_Id = @C_Id;
    END

    ELSE IF @Action = 'Deactivate'
    BEGIN
        UPDATE dbo.Courses_C
        SET C_Status = 'inactive', C_DeletedAt = SYSUTCDATETIME(), C_UpdatedAt = SYSUTCDATETIME()
        WHERE C_Id = @C_Id;
    END

    ELSE IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'SELECT COUNT(1) FROM dbo.Courses_C WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val AND C_Status = ''active'' AND C_DeletedAt IS NULL';
        IF @ExcludeId IS NOT NULL
            SET @Sql = @Sql + N' AND C_Id <> @P_ExId';
        EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX), @P_ExId UNIQUEIDENTIFIER', @Value, @ExcludeId;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_CourseSubjects_CS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================================
-- 2. USP_CourseSubjects_CS
-- ============================================================================
CREATE PROCEDURE [dbo].[USP_CourseSubjects_CS]
    @Action NVARCHAR(20),
    @CS_CourseId UNIQUEIDENTIFIER = NULL,
    @CS_SubjectId UNIQUEIDENTIFIER = NULL,
    @CS_SequenceNo INT = NULL,
    @CS_IsMandatory BIT = NULL,
    @CS_MaxMarks DECIMAL(8,2) = NULL,
    @CS_PassMarks DECIMAL(8,2) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetAll'
        BEGIN
            SELECT cs.*, c.C_Name AS CourseName, s.SB_Name AS SubjectName
            FROM CourseSubjects_CS cs
            LEFT JOIN Courses_C c ON cs.CS_CourseId = c.C_Id
            LEFT JOIN Subjects_SB s ON cs.CS_SubjectId = s.SB_Id
            WHERE c.C_TenantId = @TenantId
            ORDER BY cs.CS_SequenceNo;
        END

        ELSE IF @Action = 'GetByCourseId'
        BEGIN
            SELECT cs.*, c.C_Name AS CourseName, s.SB_Name AS SubjectName
            FROM CourseSubjects_CS cs
            LEFT JOIN Courses_C c ON cs.CS_CourseId = c.C_Id
            LEFT JOIN Subjects_SB s ON cs.CS_SubjectId = s.SB_Id
            WHERE cs.CS_CourseId = @CS_CourseId AND c.C_TenantId = @TenantId
            ORDER BY cs.CS_SequenceNo;
        END

        ELSE IF @Action = 'GetById'
        BEGIN
            SELECT cs.*, c.C_Name AS CourseName, s.SB_Name AS SubjectName
            FROM CourseSubjects_CS cs
            LEFT JOIN Courses_C c ON cs.CS_CourseId = c.C_Id
            LEFT JOIN Subjects_SB s ON cs.CS_SubjectId = s.SB_Id
            WHERE cs.CS_CourseId = @CS_CourseId AND cs.CS_SubjectId = @CS_SubjectId AND c.C_TenantId = @TenantId;
        END

        ELSE IF @Action = 'Exists'
        BEGIN
            SELECT CASE WHEN EXISTS (
                SELECT 1 FROM CourseSubjects_CS WHERE CS_CourseId = @CS_CourseId AND CS_SubjectId = @CS_SubjectId
            ) THEN 1 ELSE 0 END;
        END

        ELSE IF @Action = 'Insert'
        BEGIN
            INSERT INTO CourseSubjects_CS (CS_CourseId, CS_SubjectId, CS_SequenceNo, CS_IsMandatory, CS_MaxMarks, CS_PassMarks)
            VALUES (@CS_CourseId, @CS_SubjectId, ISNULL(@CS_SequenceNo, 0), ISNULL(@CS_IsMandatory, 1), @CS_MaxMarks, @CS_PassMarks);
        END

        ELSE IF @Action = 'Update'
        BEGIN
            UPDATE CourseSubjects_CS SET
                CS_SequenceNo = ISNULL(@CS_SequenceNo, CS_SequenceNo),
                CS_IsMandatory = ISNULL(@CS_IsMandatory, CS_IsMandatory),
                CS_MaxMarks = @CS_MaxMarks,
                CS_PassMarks = @CS_PassMarks
            WHERE CS_CourseId = @CS_CourseId AND CS_SubjectId = @CS_SubjectId;
        END

        ELSE IF @Action = 'Delete'
        BEGIN
            DELETE FROM CourseSubjects_CS WHERE CS_CourseId = @CS_CourseId AND CS_SubjectId = @CS_SubjectId;
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
/****** Object:  StoredProcedure [dbo].[USP_Departments_D]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_Departments_D]
    @Action NVARCHAR(20),
    @D_Id UNIQUEIDENTIFIER = NULL,
    @D_Name NVARCHAR(150) = NULL,
    @D_Code NVARCHAR(50) = NULL,
    @D_BranchId UNIQUEIDENTIFIER = NULL,
    @D_Description NVARCHAR(MAX) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT D_Id, D_TenantId, D_BranchId, D_Name, D_Code, D_Description,
               D_CreatedAt, D_UpdatedAt
        FROM dbo.Departments_D
        ORDER BY D_Name;
    END

    ELSE IF @Action = 'GetById'
    BEGIN
        SELECT D_Id, D_TenantId, D_BranchId, D_Name, D_Code, D_Description,
               D_CreatedAt, D_UpdatedAt
        FROM dbo.Departments_D
        WHERE D_Id = @D_Id;
    END

    ELSE IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();
        INSERT INTO dbo.Departments_D (D_Id, D_TenantId, D_BranchId, D_Name, D_Code, D_Description)
        VALUES (@NewId, @TenantId, @D_BranchId, @D_Name, @D_Code, @D_Description);
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE dbo.Departments_D
        SET D_BranchId = @D_BranchId,
            D_Name = @D_Name,
            D_Code = @D_Code,
            D_Description = @D_Description,
            D_UpdatedAt = SYSUTCDATETIME()
        WHERE D_Id = @D_Id;
    END

    ELSE IF @Action = 'Deactivate'
    BEGIN
        RAISERROR('Departments_D does not support soft delete.', 16, 1);
    END

    ELSE IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'SELECT COUNT(1) FROM dbo.Departments_D WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val';
        IF @ExcludeId IS NOT NULL
            SET @Sql = @Sql + N' AND D_Id <> @P_ExId';
        EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX), @P_ExId UNIQUEIDENTIFIER', @Value, @ExcludeId;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Designations_DS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_Designations_DS]
    @Action NVARCHAR(20),
    @DS_Id UNIQUEIDENTIFIER = NULL,
    @DS_Name NVARCHAR(100) = NULL,
    @DS_Code NVARCHAR(50) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT DS_Id, DS_TenantId, DS_Name, DS_Code, DS_CreatedAt, DS_UpdatedAt
        FROM dbo.Designations_DS
        ORDER BY DS_Name;
    END

    ELSE IF @Action = 'GetById'
    BEGIN
        SELECT DS_Id, DS_TenantId, DS_Name, DS_Code, DS_CreatedAt, DS_UpdatedAt
        FROM dbo.Designations_DS
        WHERE DS_Id = @DS_Id;
    END

    ELSE IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();
        INSERT INTO dbo.Designations_DS (DS_Id, DS_TenantId, DS_Name, DS_Code)
        VALUES (@NewId, @TenantId, @DS_Name, @DS_Code);
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE dbo.Designations_DS
        SET DS_Name = @DS_Name,
            DS_Code = @DS_Code,
            DS_UpdatedAt = SYSUTCDATETIME()
        WHERE DS_Id = @DS_Id;
    END

    ELSE IF @Action = 'Deactivate'
    BEGIN
        RAISERROR('Designations_DS does not support soft delete.', 16, 1);
    END

    ELSE IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'SELECT COUNT(1) FROM dbo.Designations_DS WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val';
        IF @ExcludeId IS NOT NULL
            SET @Sql = @Sql + N' AND DS_Id <> @P_ExId';
        EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX), @P_ExId UNIQUEIDENTIFIER', @Value, @ExcludeId;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Discounts_DIS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_Discounts_DIS]
    @Action NVARCHAR(20),
    @DIS_Id UNIQUEIDENTIFIER = NULL,
    @DIS_Name NVARCHAR(100) = NULL,
    @DIS_Code NVARCHAR(50) = NULL,
    @DIS_DiscountType NVARCHAR(20) = NULL,
    @DIS_Value DECIMAL(18,2) = NULL,
    @DIS_Description NVARCHAR(MAX) = NULL,
    @DIS_IsActive BIT = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @DIS_Name = NULLIF(LTRIM(RTRIM(@DIS_Name)), '');
    SET @DIS_Code = NULLIF(LTRIM(RTRIM(@DIS_Code)), '');
    SET @DIS_DiscountType = LOWER(NULLIF(LTRIM(RTRIM(@DIS_DiscountType)), ''));

    -- GET ALL
    IF @Action = 'GetAll'
    BEGIN
        SELECT
            DIS_Id,
            DIS_TenantId,
            DIS_Name,
            DIS_Code,
            DIS_DiscountType,
            DIS_Value,
            DIS_Description,
            DIS_IsActive,
            DIS_CreatedAt,
            DIS_UpdatedAt
        FROM dbo.Discounts_DIS
        WHERE DIS_IsActive = 1
          AND (
                @TenantId IS NULL
                OR DIS_TenantId = @TenantId
              )
        ORDER BY DIS_Name;

        RETURN;
    END;

    -- GET BY ID
    IF @Action = 'GetById'
    BEGIN
        SELECT
            DIS_Id,
            DIS_TenantId,
            DIS_Name,
            DIS_Code,
            DIS_DiscountType,
            DIS_Value,
            DIS_Description,
            DIS_IsActive,
            DIS_CreatedAt,
            DIS_UpdatedAt
        FROM dbo.Discounts_DIS
        WHERE DIS_Id = @DIS_Id
          AND (
                @TenantId IS NULL
                OR DIS_TenantId = @TenantId
              );

        RETURN;
    END;

    -- INSERT
    IF @Action = 'Insert'
    BEGIN
        IF @DIS_Name IS NULL
        BEGIN
            RAISERROR('Discount Policy Name is required.', 16, 1);
            RETURN;
        END;

        IF @DIS_Code IS NULL
        BEGIN
            RAISERROR('Discount Code is required.', 16, 1);
            RETURN;
        END;

        IF @DIS_DiscountType IS NULL
        BEGIN
            RAISERROR('Discount Type is required.', 16, 1);
            RETURN;
        END;

        IF @DIS_DiscountType NOT IN ('percentage', 'fixed')
        BEGIN
            RAISERROR(
                'Invalid Discount Type. Allowed values are Percentage or Fixed.',
                16,
                1
            );
            RETURN;
        END;

        IF @DIS_Value IS NULL
        BEGIN
            RAISERROR('Discount Value is required.', 16, 1);
            RETURN;
        END;

        IF @DIS_Value < 0
        BEGIN
            RAISERROR('Discount Value cannot be negative.', 16, 1);
            RETURN;
        END;

        IF @DIS_DiscountType = 'percentage' AND @DIS_Value > 100
        BEGIN
            RAISERROR(
                'Percentage discount cannot be greater than 100.',
                16,
                1
            );
            RETURN;
        END;

        SET @NewId = NEWID();

        INSERT INTO dbo.Discounts_DIS
        (
            DIS_Id,
            DIS_TenantId,
            DIS_Name,
            DIS_Code,
            DIS_DiscountType,
            DIS_Value,
            DIS_Description,
            DIS_IsActive
        )
        VALUES
        (
            @NewId,
            @TenantId,
            @DIS_Name,
            @DIS_Code,
            @DIS_DiscountType,
            @DIS_Value,
            @DIS_Description,
            ISNULL(@DIS_IsActive, 1)
        );

        RETURN;
    END;

    -- UPDATE
    IF @Action = 'Update'
    BEGIN
        IF @DIS_Name IS NULL
        BEGIN
            RAISERROR('Discount Policy Name is required.', 16, 1);
            RETURN;
        END;

        IF @DIS_Code IS NULL
        BEGIN
            RAISERROR('Discount Code is required.', 16, 1);
            RETURN;
        END;

        IF @DIS_DiscountType IS NULL
        BEGIN
            RAISERROR('Discount Type is required.', 16, 1);
            RETURN;
        END;

        IF @DIS_DiscountType NOT IN ('percentage', 'fixed')
        BEGIN
            RAISERROR(
                'Invalid Discount Type. Allowed values are Percentage or Fixed.',
                16,
                1
            );
            RETURN;
        END;

        IF @DIS_Value IS NULL OR @DIS_Value < 0
        BEGIN
            RAISERROR('Discount Value must be zero or greater.', 16, 1);
            RETURN;
        END;

        IF @DIS_DiscountType = 'percentage' AND @DIS_Value > 100
        BEGIN
            RAISERROR(
                'Percentage discount cannot be greater than 100.',
                16,
                1
            );
            RETURN;
        END;

        UPDATE dbo.Discounts_DIS
        SET
            DIS_Name = @DIS_Name,
            DIS_Code = @DIS_Code,
            DIS_DiscountType = @DIS_DiscountType,
            DIS_Value = @DIS_Value,
            DIS_Description = @DIS_Description,
            DIS_IsActive = ISNULL(@DIS_IsActive, 1),
            DIS_UpdatedAt = SYSUTCDATETIME()
        WHERE DIS_Id = @DIS_Id
          AND (
                @TenantId IS NULL
                OR DIS_TenantId = @TenantId
              );

        RETURN;
    END;

    -- DEACTIVATE
    IF @Action = 'Deactivate'
    BEGIN
        UPDATE dbo.Discounts_DIS
        SET
            DIS_IsActive = 0,
            DIS_UpdatedAt = SYSUTCDATETIME()
        WHERE DIS_Id = @DIS_Id
          AND (
                @TenantId IS NULL
                OR DIS_TenantId = @TenantId
              );

        RETURN;
    END;

    -- EXISTS BY FIELD
    IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);

        IF @ColumnName NOT IN
        (
            'DIS_Name',
            'DIS_Code',
            'DIS_DiscountType',
            'DIS_Value'
        )
        BEGIN
            RAISERROR('Invalid column name.', 16, 1);
            RETURN;
        END;

        -- Numeric comparison
        IF @ColumnName = 'DIS_Value'
        BEGIN
            DECLARE @NumberValue DECIMAL(18,2);

            SET @NumberValue = TRY_CONVERT(DECIMAL(18,2), @Value);

            IF @NumberValue IS NULL
            BEGIN
                RAISERROR('Invalid discount value.', 16, 1);
                RETURN;
            END;

            SET @Sql = N'
                SELECT COUNT(1)
                FROM dbo.Discounts_DIS
                WHERE DIS_Value = @P_Val
                  AND DIS_IsActive = 1
                  AND (
                        @P_TenantId IS NULL
                        OR DIS_TenantId = @P_TenantId
                      )';

            IF @ExcludeId IS NOT NULL
                SET @Sql += N' AND DIS_Id <> @P_ExId';

            EXEC sp_executesql
                @Sql,
                N'@P_Val DECIMAL(18,2),
                  @P_ExId UNIQUEIDENTIFIER,
                  @P_TenantId UNIQUEIDENTIFIER',
                @P_Val = @NumberValue,
                @P_ExId = @ExcludeId,
                @P_TenantId = @TenantId;

            RETURN;
        END;

        -- Text comparison
        SET @Sql = N'
            SELECT COUNT(1)
            FROM dbo.Discounts_DIS
            WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val
              AND DIS_IsActive = 1
              AND (
                    @P_TenantId IS NULL
                    OR DIS_TenantId = @P_TenantId
                  )';

        IF @ExcludeId IS NOT NULL
            SET @Sql += N' AND DIS_Id <> @P_ExId';

        EXEC sp_executesql
            @Sql,
            N'@P_Val NVARCHAR(MAX),
              @P_ExId UNIQUEIDENTIFIER,
              @P_TenantId UNIQUEIDENTIFIER',
            @P_Val = @Value,
            @P_ExId = @ExcludeId,
            @P_TenantId = @TenantId;

        RETURN;
    END;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DocumentTypes_DT]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_DocumentTypes_DT]
    @Action NVARCHAR(20),
    @DT_Id UNIQUEIDENTIFIER = NULL,
    @DT_Name NVARCHAR(100) = NULL,
    @DT_Code NVARCHAR(50) = NULL,
    @DT_EntityType NVARCHAR(30) = NULL,
    @DT_IsRequired BIT = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT
            DT_Id,
            DT_TenantId,
            DT_Name,
            DT_Code,
            DT_EntityType,
            DT_IsRequired,
            DT_CreatedAt,
            DT_UpdatedAt
        FROM dbo.DocumentTypes_DT
        WHERE @TenantId IS NULL
           OR DT_TenantId = @TenantId
        ORDER BY DT_Name;

        RETURN;
    END;

    IF @Action = 'GetById'
    BEGIN
        SELECT
            DT_Id,
            DT_TenantId,
            DT_Name,
            DT_Code,
            DT_EntityType,
            DT_IsRequired,
            DT_CreatedAt,
            DT_UpdatedAt
        FROM dbo.DocumentTypes_DT
        WHERE DT_Id = @DT_Id
          AND (
                @TenantId IS NULL
                OR DT_TenantId = @TenantId
              );

        RETURN;
    END;

    IF @Action = 'Insert'
    BEGIN
        IF NULLIF(LTRIM(RTRIM(@DT_EntityType)), '') IS NULL
        BEGIN
            RAISERROR('Entity Type is required.', 16, 1);
            RETURN;
        END;

        SET @DT_EntityType = LTRIM(RTRIM(@DT_EntityType));

        SET @NewId = NEWID();

        INSERT INTO dbo.DocumentTypes_DT
        (
            DT_Id,
            DT_TenantId,
            DT_Name,
            DT_Code,
            DT_EntityType,
            DT_IsRequired
        )
        VALUES
        (
            @NewId,
            @TenantId,
            LTRIM(RTRIM(@DT_Name)),
            LTRIM(RTRIM(@DT_Code)),
            @DT_EntityType,
            ISNULL(@DT_IsRequired, 0)
        );

        RETURN;
    END;

    IF @Action = 'Update'
    BEGIN
        IF NULLIF(LTRIM(RTRIM(@DT_EntityType)), '') IS NULL
        BEGIN
            RAISERROR('Entity Type is required.', 16, 1);
            RETURN;
        END;

        SET @DT_EntityType = LTRIM(RTRIM(@DT_EntityType));

        UPDATE dbo.DocumentTypes_DT
        SET
            DT_Name = LTRIM(RTRIM(@DT_Name)),
            DT_Code = LTRIM(RTRIM(@DT_Code)),
            DT_EntityType = @DT_EntityType,
            DT_IsRequired = ISNULL(@DT_IsRequired, 0),
            DT_UpdatedAt = SYSUTCDATETIME()
        WHERE DT_Id = @DT_Id
          AND (
                @TenantId IS NULL
                OR DT_TenantId = @TenantId
              );

        RETURN;
    END;

    IF @Action = 'Deactivate'
    BEGIN
        RAISERROR(
            'DocumentTypes_DT does not support soft delete.',
            16,
            1
        );
        RETURN;
    END;

    IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);

        IF @ColumnName NOT IN
        (
            'DT_Name',
            'DT_Code',
            'DT_EntityType'
        )
        BEGIN
            RAISERROR('Invalid column name.', 16, 1);
            RETURN;
        END;

        SET @Sql = N'
            SELECT COUNT(1)
            FROM dbo.DocumentTypes_DT
            WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val
              AND (
                    @P_TenantId IS NULL
                    OR DT_TenantId = @P_TenantId
                  )';

        IF @ExcludeId IS NOT NULL
        BEGIN
            SET @Sql += N'
                AND DT_Id <> @P_ExId';
        END;

        EXEC sp_executesql
            @Sql,
            N'
                @P_Val NVARCHAR(MAX),
                @P_ExId UNIQUEIDENTIFIER,
                @P_TenantId UNIQUEIDENTIFIER
            ',
            @P_Val = @Value,
            @P_ExId = @ExcludeId,
            @P_TenantId = @TenantId;

        RETURN;
    END;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Enrollments_E]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================================
-- 3. USP_Enrollments_E
-- ============================================================================
CREATE PROCEDURE [dbo].[USP_Enrollments_E]
    @Action NVARCHAR(20),
    @E_Id UNIQUEIDENTIFIER = NULL,
    @E_TenantId UNIQUEIDENTIFIER = NULL,
    @E_StudentId UNIQUEIDENTIFIER = NULL,
    @E_AcademicYearId UNIQUEIDENTIFIER = NULL,
    @E_CourseId UNIQUEIDENTIFIER = NULL,
    @E_BatchId UNIQUEIDENTIFIER = NULL,
    @E_EnrollmentNumber NVARCHAR(50) = NULL,
    @E_EnrollmentDate DATE = NULL,
    @E_Status NVARCHAR(20) = NULL,
    @E_CompletionDate DATE = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @AcademicYearId UNIQUEIDENTIFIER = NULL,
    @CourseId UNIQUEIDENTIFIER = NULL,
    @BatchId UNIQUEIDENTIFIER = NULL,
    @Status NVARCHAR(20) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetById'
        BEGIN
            SELECT e.*, s.S_FirstName + ' ' + ISNULL(s.S_MiddleName + ' ', '') + s.S_LastName AS StudentName,
                   c.C_Name AS CourseName, bt.BT_Name AS BatchName, ay.AY_Name AS AcademicYearName
            FROM Enrollments_E e
            LEFT JOIN Students_S s ON e.E_StudentId = s.S_Id
            LEFT JOIN Courses_C c ON e.E_CourseId = c.C_Id
            LEFT JOIN Batches_BT bt ON e.E_BatchId = bt.BT_Id
            LEFT JOIN AcademicYears_AY ay ON e.E_AcademicYearId = ay.AY_Id
            WHERE e.E_Id = @E_Id AND e.E_TenantId = @E_TenantId;
        END

        ELSE IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM Enrollments_E e
            WHERE e.E_TenantId = @E_TenantId
              AND (@SearchTerm IS NULL OR e.E_EnrollmentNumber LIKE '%' + @SearchTerm + '%')
              AND (@AcademicYearId IS NULL OR e.E_AcademicYearId = @AcademicYearId)
              AND (@CourseId IS NULL OR e.E_CourseId = @CourseId)
              AND (@BatchId IS NULL OR e.E_BatchId = @BatchId)
              AND (@Status IS NULL OR e.E_Status = @Status);

            SELECT e.*, s.S_FirstName + ' ' + ISNULL(s.S_MiddleName + ' ', '') + s.S_LastName AS StudentName,
                   c.C_Name AS CourseName, bt.BT_Name AS BatchName, ay.AY_Name AS AcademicYearName
            FROM Enrollments_E e
            LEFT JOIN Students_S s ON e.E_StudentId = s.S_Id
            LEFT JOIN Courses_C c ON e.E_CourseId = c.C_Id
            LEFT JOIN Batches_BT bt ON e.E_BatchId = bt.BT_Id
            LEFT JOIN AcademicYears_AY ay ON e.E_AcademicYearId = ay.AY_Id
            WHERE e.E_TenantId = @E_TenantId
              AND (@SearchTerm IS NULL OR e.E_EnrollmentNumber LIKE '%' + @SearchTerm + '%')
              AND (@AcademicYearId IS NULL OR e.E_AcademicYearId = @AcademicYearId)
              AND (@CourseId IS NULL OR e.E_CourseId = @CourseId)
              AND (@BatchId IS NULL OR e.E_BatchId = @BatchId)
              AND (@Status IS NULL OR e.E_Status = @Status)
            ORDER BY e.E_EnrollmentDate DESC
            OFFSET (@PageNumber - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
        END

        ELSE IF @Action = 'IsDuplicate'
        BEGIN
            SELECT CASE WHEN EXISTS (
                SELECT 1 FROM Enrollments_E WHERE E_TenantId = @E_TenantId AND E_StudentId = @E_StudentId AND E_BatchId = @E_BatchId
                AND (@ExcludeId IS NULL OR E_Id <> @ExcludeId)
            ) THEN 1 ELSE 0 END;
        END

        ELSE IF @Action = 'Insert'
        BEGIN
            IF @E_EnrollmentNumber IS NULL OR @E_EnrollmentNumber = ''
            BEGIN
                DECLARE @YearPart VARCHAR(4) = FORMAT(GETUTCDATE(), 'yy');
                DECLARE @Seq INT = ISNULL((SELECT MAX(CAST(RIGHT(E_EnrollmentNumber, 4) AS INT)) FROM Enrollments_E WHERE E_TenantId = @E_TenantId AND E_EnrollmentNumber LIKE 'ENR-' + @YearPart + '-%'), 0) + 1;
                SET @E_EnrollmentNumber = 'ENR-' + @YearPart + '-' + FORMAT(@Seq, '0000');
            END

            INSERT INTO Enrollments_E (E_Id, E_TenantId, E_StudentId, E_AcademicYearId, E_CourseId, E_BatchId, E_EnrollmentNumber, E_EnrollmentDate, E_Status, E_CompletionDate, E_CreatedAt, E_UpdatedAt)
            VALUES (@E_Id, @E_TenantId, @E_StudentId, @E_AcademicYearId, @E_CourseId, @E_BatchId, @E_EnrollmentNumber, @E_EnrollmentDate, @E_Status, @E_CompletionDate, GETUTCDATE(), GETUTCDATE());
            SELECT @E_Id;
        END

        ELSE IF @Action = 'Update'
        BEGIN
            UPDATE Enrollments_E SET
                E_StudentId = @E_StudentId, E_AcademicYearId = @E_AcademicYearId, E_CourseId = @E_CourseId,
                E_BatchId = @E_BatchId, E_EnrollmentNumber = ISNULL(@E_EnrollmentNumber, E_EnrollmentNumber),
                E_EnrollmentDate = @E_EnrollmentDate, E_Status = @E_Status, E_CompletionDate = @E_CompletionDate,
                E_UpdatedAt = GETUTCDATE()
            WHERE E_Id = @E_Id AND E_TenantId = @E_TenantId;
            SELECT @@ROWCOUNT;
        END

        ELSE IF @Action = 'Delete'
        BEGIN
            DELETE FROM Enrollments_E WHERE E_Id = @E_Id AND E_TenantId = @E_TenantId;
            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_Exams_EX]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- USP_Exams_EX
-- ============================================================
CREATE PROCEDURE [dbo].[USP_Exams_EX]
    @Action NVARCHAR(20),
    @EX_Id UNIQUEIDENTIFIER = NULL,
    @EX_TenantId UNIQUEIDENTIFIER = NULL,
    @EX_AcademicYearId UNIQUEIDENTIFIER = NULL,
    @EX_CourseId UNIQUEIDENTIFIER = NULL,
    @EX_BatchId UNIQUEIDENTIFIER = NULL,
    @EX_ExamTypeId UNIQUEIDENTIFIER = NULL,
    @EX_Name NVARCHAR(150) = NULL,
    @EX_Code NVARCHAR(50) = NULL,
    @EX_StartDate DATE = NULL,
    @EX_EndDate DATE = NULL,
    @EX_Status NVARCHAR(20) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @CourseId UNIQUEIDENTIFIER = NULL,
    @BatchId UNIQUEIDENTIFIER = NULL,
    @Status NVARCHAR(20) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetById'
        BEGIN
            SELECT e.*,
                   ay.AY_Name AS AcademicYearName,
                   c.C_Name AS CourseName,
                   bt.BT_Name AS BatchName,
                   et.ET_Name AS ExamTypeName
            FROM Exams_EX e
            LEFT JOIN AcademicYears_AY ay ON e.EX_AcademicYearId = ay.AY_Id
            LEFT JOIN Courses_C c ON e.EX_CourseId = c.C_Id
            LEFT JOIN Batches_BT bt ON e.EX_BatchId = bt.BT_Id
            LEFT JOIN ExamTypes_ET et ON e.EX_ExamTypeId = et.ET_Id
            WHERE e.EX_Id = @EX_Id AND e.EX_TenantId = @EX_TenantId;
        END

        IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM Exams_EX e
            WHERE e.EX_TenantId = @EX_TenantId
              AND (@SearchTerm IS NULL OR e.EX_Name LIKE '%' + @SearchTerm + '%' OR e.EX_Code LIKE '%' + @SearchTerm + '%')
              AND (@CourseId IS NULL OR e.EX_CourseId = @CourseId)
              AND (@BatchId IS NULL OR e.EX_BatchId = @BatchId)
              AND (@Status IS NULL OR e.EX_Status = @Status);

            SELECT e.*,
                   ay.AY_Name AS AcademicYearName,
                   c.C_Name AS CourseName,
                   bt.BT_Name AS BatchName,
                   et.ET_Name AS ExamTypeName
            FROM Exams_EX e
            LEFT JOIN AcademicYears_AY ay ON e.EX_AcademicYearId = ay.AY_Id
            LEFT JOIN Courses_C c ON e.EX_CourseId = c.C_Id
            LEFT JOIN Batches_BT bt ON e.EX_BatchId = bt.BT_Id
            LEFT JOIN ExamTypes_ET et ON e.EX_ExamTypeId = et.ET_Id
            WHERE e.EX_TenantId = @EX_TenantId
              AND (@SearchTerm IS NULL OR e.EX_Name LIKE '%' + @SearchTerm + '%' OR e.EX_Code LIKE '%' + @SearchTerm + '%')
              AND (@CourseId IS NULL OR e.EX_CourseId = @CourseId)
              AND (@BatchId IS NULL OR e.EX_BatchId = @BatchId)
              AND (@Status IS NULL OR e.EX_Status = @Status)
            ORDER BY e.EX_StartDate DESC
            OFFSET (@PageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END

        IF @Action = 'ExistsByCode'
        BEGIN
            SELECT COUNT(*) FROM Exams_EX
            WHERE EX_TenantId = @EX_TenantId AND EX_Code = @EX_Code
              AND (@ExcludeId IS NULL OR EX_Id <> @ExcludeId);
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO Exams_EX (EX_Id, EX_TenantId, EX_AcademicYearId, EX_CourseId, EX_BatchId, EX_ExamTypeId, EX_Name, EX_Code, EX_StartDate, EX_EndDate, EX_Status, EX_CreatedAt, EX_UpdatedAt)
            VALUES (@EX_Id, @EX_TenantId, @EX_AcademicYearId, @EX_CourseId, @EX_BatchId, @EX_ExamTypeId, @EX_Name, @EX_Code, @EX_StartDate, @EX_EndDate, @EX_Status, SYSUTCDATETIME(), SYSUTCDATETIME());
            SELECT @EX_Id;
        END

        IF @Action = 'Update'
        BEGIN
            UPDATE Exams_EX
            SET EX_AcademicYearId = @EX_AcademicYearId, EX_CourseId = @EX_CourseId, EX_BatchId = @EX_BatchId,
                EX_ExamTypeId = @EX_ExamTypeId, EX_Name = @EX_Name, EX_Code = @EX_Code,
                EX_StartDate = @EX_StartDate, EX_EndDate = @EX_EndDate, EX_Status = @EX_Status,
                EX_UpdatedAt = SYSUTCDATETIME()
            WHERE EX_Id = @EX_Id AND EX_TenantId = @EX_TenantId;
            SELECT @@ROWCOUNT;
        END

        IF @Action = 'Delete'
        BEGIN
            DELETE FROM Exams_EX WHERE EX_Id = @EX_Id AND EX_TenantId = @EX_TenantId;
            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_ExamSubjects_ES]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- USP_ExamSubjects_ES
-- ============================================================
CREATE PROCEDURE [dbo].[USP_ExamSubjects_ES]
    @Action NVARCHAR(20),
    @ES_Id UNIQUEIDENTIFIER = NULL,
    @ES_ExamId UNIQUEIDENTIFIER = NULL,
    @ES_SubjectId UNIQUEIDENTIFIER = NULL,
    @ES_MaxMarks DECIMAL(8,2) = NULL,
    @ES_PassMarks DECIMAL(8,2) = NULL,
    @ES_Weightage DECIMAL(5,2) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetByExam'
        BEGIN
            SELECT es.*, sb.SB_Name AS SubjectName
            FROM ExamSubjects_ES es
            LEFT JOIN Subjects_SB sb ON es.ES_SubjectId = sb.SB_Id
            WHERE es.ES_ExamId = @ES_ExamId;
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO ExamSubjects_ES (ES_Id, ES_ExamId, ES_SubjectId, ES_MaxMarks, ES_PassMarks, ES_Weightage)
            VALUES (@ES_Id, @ES_ExamId, @ES_SubjectId, @ES_MaxMarks, @ES_PassMarks, @ES_Weightage);
        END

        IF @Action = 'DeleteByExam'
        BEGIN
            DELETE FROM ExamSubjects_ES WHERE ES_ExamId = @ES_ExamId;
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
/****** Object:  StoredProcedure [dbo].[USP_ExamTypes_ET]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_ExamTypes_ET]
    @Action NVARCHAR(20),
    @ET_Id UNIQUEIDENTIFIER = NULL,
    @ET_Name NVARCHAR(100) = NULL,
    @ET_Code NVARCHAR(50) = NULL,
    @ET_WeightagePercentage DECIMAL(5,2) = NULL,
    @ET_Description NVARCHAR(MAX) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT
            ET_Id,
            ET_TenantId,
            ET_Name,
            ET_Code,
            ET_WeightagePercentage,
            ET_Description
        FROM dbo.ExamTypes_ET
        WHERE @TenantId IS NULL
           OR ET_TenantId = @TenantId
        ORDER BY ET_Name;

        RETURN;
    END;

    IF @Action = 'GetById'
    BEGIN
        SELECT
            ET_Id,
            ET_TenantId,
            ET_Name,
            ET_Code,
            ET_WeightagePercentage,
            ET_Description
        FROM dbo.ExamTypes_ET
        WHERE ET_Id = @ET_Id
          AND (
                @TenantId IS NULL
                OR ET_TenantId = @TenantId
              );

        RETURN;
    END;

    IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();

        INSERT INTO dbo.ExamTypes_ET
        (
            ET_Id,
            ET_TenantId,
            ET_Name,
            ET_Code,
            ET_WeightagePercentage,
            ET_Description
        )
        VALUES
        (
            @NewId,
            @TenantId,
            LTRIM(RTRIM(@ET_Name)),
            LTRIM(RTRIM(@ET_Code)),
            ISNULL(@ET_WeightagePercentage, 0),
            @ET_Description
        );

        RETURN;
    END;

    IF @Action = 'Update'
    BEGIN
        UPDATE dbo.ExamTypes_ET
        SET
            ET_Name = LTRIM(RTRIM(@ET_Name)),
            ET_Code = LTRIM(RTRIM(@ET_Code)),
            ET_WeightagePercentage = ISNULL(@ET_WeightagePercentage, 0),
            ET_Description = @ET_Description
        WHERE ET_Id = @ET_Id
          AND (
                @TenantId IS NULL
                OR ET_TenantId = @TenantId
              );

        RETURN;
    END;

    IF @Action = 'Deactivate'
    BEGIN
        RAISERROR(
            'ExamTypes_ET does not support soft delete because ET_IsActive does not exist.',
            16,
            1
        );

        RETURN;
    END;

    IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);

        IF @ColumnName NOT IN
        (
            'ET_Name',
            'ET_Code',
            'ET_WeightagePercentage'
        )
        BEGIN
            RAISERROR('Invalid column name.', 16, 1);
            RETURN;
        END;

        IF @ColumnName = 'ET_WeightagePercentage'
        BEGIN
            DECLARE @NumberValue DECIMAL(5,2);

            SET @NumberValue = TRY_CONVERT(DECIMAL(5,2), @Value);

            IF @NumberValue IS NULL
            BEGIN
                RAISERROR('Invalid weightage percentage.', 16, 1);
                RETURN;
            END;

            SET @Sql = N'
                SELECT COUNT(1)
                FROM dbo.ExamTypes_ET
                WHERE ET_WeightagePercentage = @P_Val
                  AND (@P_TenantId IS NULL OR ET_TenantId = @P_TenantId)';

            IF @ExcludeId IS NOT NULL
                SET @Sql += N' AND ET_Id <> @P_ExId';

            EXEC sp_executesql
                @Sql,
                N'@P_Val DECIMAL(5,2),
                  @P_ExId UNIQUEIDENTIFIER,
                  @P_TenantId UNIQUEIDENTIFIER',
                @P_Val = @NumberValue,
                @P_ExId = @ExcludeId,
                @P_TenantId = @TenantId;

            RETURN;
        END;

        SET @Sql = N'
            SELECT COUNT(1)
            FROM dbo.ExamTypes_ET
            WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val
              AND (@P_TenantId IS NULL OR ET_TenantId = @P_TenantId)';

        IF @ExcludeId IS NOT NULL
            SET @Sql += N' AND ET_Id <> @P_ExId';

        EXEC sp_executesql
            @Sql,
            N'@P_Val NVARCHAR(MAX),
              @P_ExId UNIQUEIDENTIFIER,
              @P_TenantId UNIQUEIDENTIFIER',
            @P_Val = @Value,
            @P_ExId = @ExcludeId,
            @P_TenantId = @TenantId;

        RETURN;
    END;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_ExpenseCategories_EC]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- USP_ExpenseCategories_EC
-- ============================================================
CREATE PROCEDURE [dbo].[USP_ExpenseCategories_EC]
    @Action NVARCHAR(20),
    @EC_Id UNIQUEIDENTIFIER = NULL,
    @EC_TenantId UNIQUEIDENTIFIER = NULL,
    @EC_Name NVARCHAR(100) = NULL,
    @EC_Code NVARCHAR(50) = NULL,
    @EC_Description NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetAll'
        BEGIN
            SELECT * FROM ExpenseCategories_EC
            ORDER BY EC_Name;
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
/****** Object:  StoredProcedure [dbo].[USP_Expenses_EXP]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- USP_Expenses_EXP
-- ============================================================
CREATE PROCEDURE [dbo].[USP_Expenses_EXP]
    @Action NVARCHAR(20),
    @EXP_Id UNIQUEIDENTIFIER = NULL,
    @EXP_TenantId UNIQUEIDENTIFIER = NULL,
    @EXP_BranchId UNIQUEIDENTIFIER = NULL,
    @EXP_ExpenseCategoryId UNIQUEIDENTIFIER = NULL,
    @EXP_VendorId UNIQUEIDENTIFIER = NULL,
    @EXP_ExpenseNumber NVARCHAR(50) = NULL,
    @EXP_ExpenseDate DATE = NULL,
    @EXP_Amount DECIMAL(12,2) = NULL,
    @EXP_Description NVARCHAR(MAX) = NULL,
    @EXP_PaymentMethodId UNIQUEIDENTIFIER = NULL,
    @EXP_CreatedBy UNIQUEIDENTIFIER = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @BranchId UNIQUEIDENTIFIER = NULL,
    @ExpenseCategoryId UNIQUEIDENTIFIER = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetById'
        BEGIN
            SELECT e.*, 
                   b.B_Name AS BranchName,
                   ec.EC_Name AS ExpenseCategoryName,
                   v.V_Name AS VendorName,
                   pm.PM_Name AS PaymentMethodName
            FROM Expenses_EXP e
            LEFT JOIN Branches_B b ON e.EXP_BranchId = b.B_Id
            LEFT JOIN ExpenseCategories_EC ec ON e.EXP_ExpenseCategoryId = ec.EC_Id
            LEFT JOIN Vendors_V v ON e.EXP_VendorId = v.V_Id
            LEFT JOIN PaymentMethods_PM pm ON e.EXP_PaymentMethodId = pm.PM_Id
            WHERE e.EXP_Id = @EXP_Id AND e.EXP_TenantId = @EXP_TenantId;
        END

        IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM Expenses_EXP e
            WHERE e.EXP_TenantId = @EXP_TenantId
              AND (@SearchTerm IS NULL OR e.EXP_ExpenseNumber LIKE '%' + @SearchTerm + '%' OR e.EXP_Description LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR e.EXP_BranchId = @BranchId)
              AND (@ExpenseCategoryId IS NULL OR e.EXP_ExpenseCategoryId = @ExpenseCategoryId);

            SELECT e.*, 
                   b.B_Name AS BranchName,
                   ec.EC_Name AS ExpenseCategoryName,
                   v.V_Name AS VendorName,
                   pm.PM_Name AS PaymentMethodName
            FROM Expenses_EXP e
            LEFT JOIN Branches_B b ON e.EXP_BranchId = b.B_Id
            LEFT JOIN ExpenseCategories_EC ec ON e.EXP_ExpenseCategoryId = ec.EC_Id
            LEFT JOIN Vendors_V v ON e.EXP_VendorId = v.V_Id
            LEFT JOIN PaymentMethods_PM pm ON e.EXP_PaymentMethodId = pm.PM_Id
            WHERE e.EXP_TenantId = @EXP_TenantId
              AND (@SearchTerm IS NULL OR e.EXP_ExpenseNumber LIKE '%' + @SearchTerm + '%' OR e.EXP_Description LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR e.EXP_BranchId = @BranchId)
              AND (@ExpenseCategoryId IS NULL OR e.EXP_ExpenseCategoryId = @ExpenseCategoryId)
            ORDER BY e.EXP_ExpenseDate DESC
            OFFSET (@PageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END

        IF @Action = 'GetNextNumber'
        BEGIN
            DECLARE @MaxNum INT;
            SELECT @MaxNum = ISNULL(MAX(CAST(SUBSTRING(e.EXP_ExpenseNumber, 5, LEN(e.EXP_ExpenseNumber) - 4) AS INT)), 0)
            FROM Expenses_EXP e
            WHERE e.EXP_TenantId = @EXP_TenantId;

            SELECT 'EXP-' + RIGHT('0000' + CAST(@MaxNum + 1 AS NVARCHAR), 4);
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO Expenses_EXP (EXP_Id, EXP_TenantId, EXP_BranchId, EXP_ExpenseCategoryId, EXP_VendorId, EXP_ExpenseNumber, EXP_ExpenseDate, EXP_Amount, EXP_Description, EXP_PaymentMethodId, EXP_CreatedBy, EXP_CreatedAt, EXP_UpdatedAt)
            VALUES (@EXP_Id, @EXP_TenantId, @EXP_BranchId, @EXP_ExpenseCategoryId, @EXP_VendorId, @EXP_ExpenseNumber, @EXP_ExpenseDate, @EXP_Amount, @EXP_Description, @EXP_PaymentMethodId, @EXP_CreatedBy, SYSUTCDATETIME(), SYSUTCDATETIME());

            SELECT @EXP_Id;
        END

        IF @Action = 'Update'
        BEGIN
            UPDATE Expenses_EXP
            SET EXP_BranchId = @EXP_BranchId,
                EXP_ExpenseCategoryId = @EXP_ExpenseCategoryId,
                EXP_VendorId = @EXP_VendorId,
                EXP_ExpenseDate = @EXP_ExpenseDate,
                EXP_Amount = @EXP_Amount,
                EXP_Description = @EXP_Description,
                EXP_PaymentMethodId = @EXP_PaymentMethodId,
                EXP_UpdatedAt = SYSUTCDATETIME()
            WHERE EXP_Id = @EXP_Id AND EXP_TenantId = @EXP_TenantId;

            SELECT @@ROWCOUNT;
        END

        IF @Action = 'Delete'
        BEGIN
            DELETE FROM Expenses_EXP WHERE EXP_Id = @EXP_Id AND EXP_TenantId = @EXP_TenantId;
            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_FeeCategories_FC]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- USP_FeeCategories_FC (fix GetAll — no tenant filter for MasterDAL)
-- ============================================================
CREATE PROCEDURE [dbo].[USP_FeeCategories_FC]
    @Action NVARCHAR(20),
    @FC_Id UNIQUEIDENTIFIER = NULL,
    @FC_TenantId UNIQUEIDENTIFIER = NULL,
    @FC_Name NVARCHAR(100) = NULL,
    @FC_Code NVARCHAR(50) = NULL,
    @FC_Description NVARCHAR(MAX) = NULL,
    @FC_IsRefundable BIT = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetAll'
        BEGIN
            SELECT FC_Id, FC_TenantId, FC_Name, FC_Code, FC_Description, FC_IsRefundable
            FROM FeeCategories_FC
            ORDER BY FC_Name;
        END

        IF @Action = 'GetById'
        BEGIN
            SELECT * FROM FeeCategories_FC WHERE FC_Id = @FC_Id;
        END

        IF @Action = 'ExistsByField'
        BEGIN
            DECLARE @Sql NVARCHAR(MAX);
            SET @Sql = N'SELECT COUNT(1) FROM FeeCategories_FC WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val';
            EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX)', @Value;
        END

        IF @Action = 'Insert'
        BEGIN
            SET @NewId = ISNULL(@NewId, NEWID());
            INSERT INTO FeeCategories_FC (FC_Id, FC_TenantId, FC_Name, FC_Code, FC_Description, FC_IsRefundable, FC_CreatedAt, FC_UpdatedAt)
            VALUES (@NewId, @FC_TenantId, @FC_Name, @FC_Code, @FC_Description, ISNULL(@FC_IsRefundable, 0), SYSUTCDATETIME(), SYSUTCDATETIME());
            SELECT @NewId;
        END

        IF @Action = 'Update'
        BEGIN
            UPDATE FeeCategories_FC
            SET FC_Name = @FC_Name, FC_Code = @FC_Code, FC_Description = @FC_Description,
                FC_IsRefundable = ISNULL(@FC_IsRefundable, FC_IsRefundable), FC_UpdatedAt = SYSUTCDATETIME()
            WHERE FC_Id = @FC_Id;
            SELECT @@ROWCOUNT;
        END

        IF @Action = 'Delete'
        BEGIN
            RAISERROR('FeeCategories_FC does not support hard delete.', 16, 1);
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
/****** Object:  StoredProcedure [dbo].[USP_FeeInvoices_FI]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- USP_FeeInvoices_FI
-- ============================================================
CREATE PROCEDURE [dbo].[USP_FeeInvoices_FI]
    @Action NVARCHAR(20),
    @FI_Id UNIQUEIDENTIFIER = NULL,
    @FI_TenantId UNIQUEIDENTIFIER = NULL,
    @FI_StudentId UNIQUEIDENTIFIER = NULL,
    @FI_InvoiceNumber NVARCHAR(50) = NULL,
    @FI_InvoiceDate DATE = NULL,
    @FI_DueDate DATE = NULL,
    @FI_Subtotal DECIMAL(12,2) = NULL,
    @FI_DiscountAmount DECIMAL(12,2) = NULL,
    @FI_TaxAmount DECIMAL(12,2) = NULL,
    @FI_TotalAmount DECIMAL(12,2) = NULL,
    @FI_PaidAmount DECIMAL(12,2) = NULL,
    @FI_BalanceAmount DECIMAL(12,2) = NULL,
    @FI_Status NVARCHAR(20) = NULL,
    @FI_Notes NVARCHAR(MAX) = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @Status NVARCHAR(20) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetById'
        BEGIN
            SELECT fi.*, s.S_FirstName + ' ' + s.S_LastName AS StudentName, s.S_StudentCode AS StudentCode
            FROM FeeInvoices_FI fi
            LEFT JOIN Students_S s ON fi.FI_StudentId = s.S_Id
            WHERE fi.FI_Id = @FI_Id AND fi.FI_TenantId = @FI_TenantId;
        END

        IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM FeeInvoices_FI fi
            WHERE fi.FI_TenantId = @FI_TenantId
              AND (@SearchTerm IS NULL OR fi.FI_InvoiceNumber LIKE '%' + @SearchTerm + '%')
              AND (@Status IS NULL OR fi.FI_Status = @Status);

            SELECT fi.*, s.S_FirstName + ' ' + s.S_LastName AS StudentName, s.S_StudentCode AS StudentCode
            FROM FeeInvoices_FI fi
            LEFT JOIN Students_S s ON fi.FI_StudentId = s.S_Id
            WHERE fi.FI_TenantId = @FI_TenantId
              AND (@SearchTerm IS NULL OR fi.FI_InvoiceNumber LIKE '%' + @SearchTerm + '%')
              AND (@Status IS NULL OR fi.FI_Status = @Status)
            ORDER BY fi.FI_InvoiceDate DESC
            OFFSET (@PageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END

        IF @Action = 'GetNextNumber'
        BEGIN
            DECLARE @MaxNum INT;
            SELECT @MaxNum = ISNULL(MAX(CAST(SUBSTRING(FI_InvoiceNumber, 5, LEN(FI_InvoiceNumber) - 4) AS INT)), 0)
            FROM FeeInvoices_FI WHERE FI_TenantId = @FI_TenantId;
            SELECT 'INV-' + RIGHT('0000' + CAST(@MaxNum + 1 AS NVARCHAR), 4);
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO FeeInvoices_FI (FI_Id, FI_TenantId, FI_StudentId, FI_InvoiceNumber, FI_InvoiceDate, FI_DueDate, FI_Subtotal, FI_DiscountAmount, FI_TaxAmount, FI_TotalAmount, FI_PaidAmount, FI_BalanceAmount, FI_Status, FI_Notes, FI_CreatedAt, FI_UpdatedAt)
            VALUES (@FI_Id, @FI_TenantId, @FI_StudentId, @FI_InvoiceNumber, @FI_InvoiceDate, @FI_DueDate, @FI_Subtotal, @FI_DiscountAmount, @FI_TaxAmount, @FI_TotalAmount, @FI_PaidAmount, @FI_BalanceAmount, @FI_Status, @FI_Notes, SYSUTCDATETIME(), SYSUTCDATETIME());
            SELECT @FI_Id;
        END

        IF @Action = 'UpdatePaidAmount'
        BEGIN
            UPDATE FeeInvoices_FI
            SET FI_PaidAmount = @FI_PaidAmount,
                FI_BalanceAmount = FI_TotalAmount - @FI_PaidAmount,
                FI_Status = CASE
                    WHEN @FI_PaidAmount >= FI_TotalAmount THEN 'Paid'
                    WHEN @FI_PaidAmount > 0 THEN 'Partial'
                    ELSE FI_Status
                END,
                FI_UpdatedAt = SYSUTCDATETIME()
            WHERE FI_Id = @FI_Id;
            SELECT @@ROWCOUNT;
        END

        IF @Action = 'Delete'
        BEGIN
            DELETE FROM FeeInvoices_FI WHERE FI_Id = @FI_Id AND FI_TenantId = @FI_TenantId;
            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_FeeStructureItems_FSI]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- USP_FeeStructureItems_FSI
-- ============================================================
CREATE PROCEDURE [dbo].[USP_FeeStructureItems_FSI]
    @Action NVARCHAR(20),
    @FSI_Id UNIQUEIDENTIFIER = NULL,
    @FSI_FeeStructureId UNIQUEIDENTIFIER = NULL,
    @FSI_FeeCategoryId UNIQUEIDENTIFIER = NULL,
    @FSI_Amount DECIMAL(12,2) = NULL,
    @FSI_DueDays INT = NULL,
    @FSI_IsMandatory BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetByFeeStructure'
        BEGIN
            SELECT fsi.*, fc.FC_Name AS FeeCategoryName
            FROM FeeStructureItems_FSI fsi
            LEFT JOIN FeeCategories_FC fc ON fsi.FSI_FeeCategoryId = fc.FC_Id
            WHERE fsi.FSI_FeeStructureId = @FSI_FeeStructureId;
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO FeeStructureItems_FSI (FSI_Id, FSI_FeeStructureId, FSI_FeeCategoryId, FSI_Amount, FSI_DueDays, FSI_IsMandatory)
            VALUES (@FSI_Id, @FSI_FeeStructureId, @FSI_FeeCategoryId, @FSI_Amount, @FSI_DueDays, @FSI_IsMandatory);
        END

        IF @Action = 'DeleteByFeeStructure'
        BEGIN
            DELETE FROM FeeStructureItems_FSI WHERE FSI_FeeStructureId = @FSI_FeeStructureId;
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
/****** Object:  StoredProcedure [dbo].[USP_FeeStructures_FS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- USP_FeeStructures_FS
-- ============================================================
CREATE PROCEDURE [dbo].[USP_FeeStructures_FS]
    @Action NVARCHAR(20),
    @FS_Id UNIQUEIDENTIFIER = NULL,
    @FS_TenantId UNIQUEIDENTIFIER = NULL,
    @FS_Name NVARCHAR(150) = NULL,
    @FS_Code NVARCHAR(50) = NULL,
    @FS_CourseId UNIQUEIDENTIFIER = NULL,
    @FS_BatchId UNIQUEIDENTIFIER = NULL,
    @FS_AcademicYearId UNIQUEIDENTIFIER = NULL,
    @FS_Description NVARCHAR(MAX) = NULL,
    @FS_IsActive BIT = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @AcademicYearId UNIQUEIDENTIFIER = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetById'
        BEGIN
            SELECT fs.*, c.C_Name AS CourseName, bt.BT_Name AS BatchName, ay.AY_Name AS AcademicYearName
            FROM FeeStructures_FS fs
            LEFT JOIN Courses_C c ON fs.FS_CourseId = c.C_Id
            LEFT JOIN Batches_BT bt ON fs.FS_BatchId = bt.BT_Id
            LEFT JOIN AcademicYears_AY ay ON fs.FS_AcademicYearId = ay.AY_Id
            WHERE fs.FS_Id = @FS_Id AND fs.FS_TenantId = @FS_TenantId;
        END

        IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM FeeStructures_FS fs
            WHERE fs.FS_TenantId = @FS_TenantId
              AND (@SearchTerm IS NULL OR fs.FS_Name LIKE '%' + @SearchTerm + '%' OR fs.FS_Code LIKE '%' + @SearchTerm + '%')
              AND (@AcademicYearId IS NULL OR fs.FS_AcademicYearId = @AcademicYearId);

            SELECT fs.*, c.C_Name AS CourseName, bt.BT_Name AS BatchName, ay.AY_Name AS AcademicYearName
            FROM FeeStructures_FS fs
            LEFT JOIN Courses_C c ON fs.FS_CourseId = c.C_Id
            LEFT JOIN Batches_BT bt ON fs.FS_BatchId = bt.BT_Id
            LEFT JOIN AcademicYears_AY ay ON fs.FS_AcademicYearId = ay.AY_Id
            WHERE fs.FS_TenantId = @FS_TenantId
              AND (@SearchTerm IS NULL OR fs.FS_Name LIKE '%' + @SearchTerm + '%' OR fs.FS_Code LIKE '%' + @SearchTerm + '%')
              AND (@AcademicYearId IS NULL OR fs.FS_AcademicYearId = @AcademicYearId)
            ORDER BY fs.FS_Name
            OFFSET (@PageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END

        IF @Action = 'ExistsByCode'
        BEGIN
            SELECT COUNT(*) FROM FeeStructures_FS
            WHERE FS_TenantId = @FS_TenantId AND FS_Code = @FS_Code
              AND (@ExcludeId IS NULL OR FS_Id <> @ExcludeId);
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO FeeStructures_FS (FS_Id, FS_TenantId, FS_Name, FS_Code, FS_CourseId, FS_BatchId, FS_AcademicYearId, FS_Description, FS_IsActive, FS_CreatedAt, FS_UpdatedAt)
            VALUES (@FS_Id, @FS_TenantId, @FS_Name, @FS_Code, @FS_CourseId, @FS_BatchId, @FS_AcademicYearId, @FS_Description, @FS_IsActive, SYSUTCDATETIME(), SYSUTCDATETIME());
            SELECT @FS_Id;
        END

        IF @Action = 'Update'
        BEGIN
            UPDATE FeeStructures_FS
            SET FS_Name = @FS_Name, FS_Code = @FS_Code, FS_CourseId = @FS_CourseId, FS_BatchId = @FS_BatchId,
                FS_AcademicYearId = @FS_AcademicYearId, FS_Description = @FS_Description, FS_IsActive = @FS_IsActive,
                FS_UpdatedAt = SYSUTCDATETIME()
            WHERE FS_Id = @FS_Id AND FS_TenantId = @FS_TenantId;
            SELECT @@ROWCOUNT;
        END

        IF @Action = 'Delete'
        BEGIN
            DELETE FROM FeeStructures_FS WHERE FS_Id = @FS_Id AND FS_TenantId = @FS_TenantId;
            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_GradeScales_GS]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_GradeScales_GS]
    @Action NVARCHAR(20),
    @GS_Id UNIQUEIDENTIFIER = NULL,
    @GS_Name NVARCHAR(100) = NULL,
    @GS_Code NVARCHAR(50) = NULL,
    @GS_Description NVARCHAR(MAX) = NULL,
    @GS_IsDefault BIT = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT GS_Id, GS_TenantId, GS_Name, GS_Code, GS_Description, GS_IsDefault
        FROM dbo.GradeScales_GS
        ORDER BY GS_Name;
    END

    ELSE IF @Action = 'GetById'
    BEGIN
        SELECT GS_Id, GS_TenantId, GS_Name, GS_Code, GS_Description, GS_IsDefault
        FROM dbo.GradeScales_GS
        WHERE GS_Id = @GS_Id;
    END

    ELSE IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();
        INSERT INTO dbo.GradeScales_GS (GS_Id, GS_TenantId, GS_Name, GS_Code, GS_Description, GS_IsDefault)
        VALUES (@NewId, @TenantId, @GS_Name, @GS_Code, @GS_Description, ISNULL(@GS_IsDefault, 0));
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE dbo.GradeScales_GS
        SET GS_Name = @GS_Name,
            GS_Code = @GS_Code,
            GS_Description = @GS_Description,
            GS_IsDefault = @GS_IsDefault
        WHERE GS_Id = @GS_Id;
    END

    ELSE IF @Action = 'Deactivate'
    BEGIN
        RAISERROR('GradeScales_GS does not support soft delete.', 16, 1);
    END

    ELSE IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'SELECT COUNT(1) FROM dbo.GradeScales_GS WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val';
        IF @ExcludeId IS NOT NULL
            SET @Sql = @Sql + N' AND GS_Id <> @P_ExId';
        EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX), @P_ExId UNIQUEIDENTIFIER', @Value, @ExcludeId;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Marks_M]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- USP_Marks_M
-- ============================================================
CREATE PROCEDURE [dbo].[USP_Marks_M]
    @Action NVARCHAR(20),
    @M_Id UNIQUEIDENTIFIER = NULL,
    @M_ExamSubjectId UNIQUEIDENTIFIER = NULL,
    @M_StudentId UNIQUEIDENTIFIER = NULL,
    @M_MarksObtained DECIMAL(8,2) = NULL,
    @M_Remarks NVARCHAR(MAX) = NULL,
    @ExamId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetByExam'
        BEGIN
            SELECT m.*, s.S_FirstName + ' ' + s.S_LastName AS StudentName, s.S_StudentCode AS StudentCode,
                   CASE WHEN m.M_MarksObtained >= es.ES_PassMarks THEN 'Pass' ELSE 'Fail' END AS Grade
            FROM Marks_M m
            INNER JOIN ExamSubjects_ES es ON m.M_ExamSubjectId = es.ES_Id
            LEFT JOIN Students_S s ON m.M_StudentId = s.S_Id
            WHERE es.ES_ExamId = @ExamId;
        END

        IF @Action = 'Upsert'
        BEGIN
            IF EXISTS (SELECT 1 FROM Marks_M WHERE M_ExamSubjectId = @M_ExamSubjectId AND M_StudentId = @M_StudentId)
            BEGIN
                UPDATE Marks_M SET M_MarksObtained = @M_MarksObtained, M_Remarks = @M_Remarks, M_UpdatedAt = SYSUTCDATETIME()
                WHERE M_ExamSubjectId = @M_ExamSubjectId AND M_StudentId = @M_StudentId;
            END
            ELSE
            BEGIN
                INSERT INTO Marks_M (M_Id, M_ExamSubjectId, M_StudentId, M_MarksObtained, M_Remarks, M_CreatedAt, M_UpdatedAt)
                VALUES (@M_Id, @M_ExamSubjectId, @M_StudentId, @M_MarksObtained, @M_Remarks, SYSUTCDATETIME(), SYSUTCDATETIME());
            END
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
/****** Object:  StoredProcedure [dbo].[USP_NotificationTemplates_NT]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_NotificationTemplates_NT]
    @Action NVARCHAR(20),
    @NT_Id UNIQUEIDENTIFIER = NULL,
    @NT_Name NVARCHAR(150) = NULL,
    @NT_EventKey NVARCHAR(100) = NULL,
    @NT_Channel NVARCHAR(20) = NULL,
    @NT_Subject NVARCHAR(255) = NULL,
    @NT_BodyTemplate NVARCHAR(MAX) = NULL,
    @NT_IsActive BIT = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @NT_Name = NULLIF(LTRIM(RTRIM(@NT_Name)), '');
    SET @NT_EventKey = NULLIF(LTRIM(RTRIM(@NT_EventKey)), '');
    SET @NT_Channel = LOWER(NULLIF(LTRIM(RTRIM(@NT_Channel)), ''));

    -- GET ALL
    IF @Action = 'GetAll'
    BEGIN
        SELECT
            NT_Id,
            NT_TenantId,
            NT_Name,
            NT_EventKey,
            NT_Channel,
            NT_Subject,
            NT_BodyTemplate,
            NT_IsActive,
            NT_CreatedAt,
            NT_UpdatedAt
        FROM dbo.NotificationTemplates_NT
        WHERE NT_IsActive = 1
          AND (
                @TenantId IS NULL
                OR NT_TenantId = @TenantId
              )
        ORDER BY NT_Name;

        RETURN;
    END;

    -- GET BY ID
    IF @Action = 'GetById'
    BEGIN
        SELECT
            NT_Id,
            NT_TenantId,
            NT_Name,
            NT_EventKey,
            NT_Channel,
            NT_Subject,
            NT_BodyTemplate,
            NT_IsActive,
            NT_CreatedAt,
            NT_UpdatedAt
        FROM dbo.NotificationTemplates_NT
        WHERE NT_Id = @NT_Id
          AND (
                @TenantId IS NULL
                OR NT_TenantId = @TenantId
              );

        RETURN;
    END;

    -- INSERT
    IF @Action = 'Insert'
    BEGIN
        IF @NT_Name IS NULL
        BEGIN
            RAISERROR('Template Name is required.', 16, 1);
            RETURN;
        END;

        IF @NT_EventKey IS NULL
        BEGIN
            RAISERROR('Event Key is required.', 16, 1);
            RETURN;
        END;

        IF @NT_Channel IS NULL
        BEGIN
            RAISERROR('Channel is required.', 16, 1);
            RETURN;
        END;

        IF @NT_BodyTemplate IS NULL OR LTRIM(RTRIM(@NT_BodyTemplate)) = ''
        BEGIN
            RAISERROR('Template Content is required.', 16, 1);
            RETURN;
        END;

        IF @NT_Channel NOT IN ('email', 'sms')
        BEGIN
            RAISERROR(
                'Invalid Channel. Allowed values are Email or SMS.',
                16,
                1
            );
            RETURN;
        END;

        -- Subject is required for Email
        IF @NT_Channel = 'email'
           AND (
                @NT_Subject IS NULL
                OR LTRIM(RTRIM(@NT_Subject)) = ''
               )
        BEGIN
            RAISERROR(
                'Email Subject Line is required for Email templates.',
                16,
                1
            );
            RETURN;
        END;

        SET @NewId = NEWID();

        INSERT INTO dbo.NotificationTemplates_NT
        (
            NT_Id,
            NT_TenantId,
            NT_Name,
            NT_EventKey,
            NT_Channel,
            NT_Subject,
            NT_BodyTemplate,
            NT_IsActive
        )
        VALUES
        (
            @NewId,
            @TenantId,
            @NT_Name,
            @NT_EventKey,
            @NT_Channel,
            @NT_Subject,
            @NT_BodyTemplate,
            ISNULL(@NT_IsActive, 1)
        );

        RETURN;
    END;

    -- UPDATE
    IF @Action = 'Update'
    BEGIN
        IF @NT_Name IS NULL
        BEGIN
            RAISERROR('Template Name is required.', 16, 1);
            RETURN;
        END;

        IF @NT_EventKey IS NULL
        BEGIN
            RAISERROR('Event Key is required.', 16, 1);
            RETURN;
        END;

        IF @NT_Channel IS NULL
        BEGIN
            RAISERROR('Channel is required.', 16, 1);
            RETURN;
        END;

        IF @NT_BodyTemplate IS NULL OR LTRIM(RTRIM(@NT_BodyTemplate)) = ''
        BEGIN
            RAISERROR('Template Content is required.', 16, 1);
            RETURN;
        END;

        IF @NT_Channel NOT IN ('email', 'sms')
        BEGIN
            RAISERROR(
                'Invalid Channel. Allowed values are Email or SMS.',
                16,
                1
            );
            RETURN;
        END;

        IF @NT_Channel = 'email'
           AND (
                @NT_Subject IS NULL
                OR LTRIM(RTRIM(@NT_Subject)) = ''
               )
        BEGIN
            RAISERROR(
                'Email Subject Line is required for Email templates.',
                16,
                1
            );
            RETURN;
        END;

        UPDATE dbo.NotificationTemplates_NT
        SET
            NT_Name = @NT_Name,
            NT_EventKey = @NT_EventKey,
            NT_Channel = @NT_Channel,
            NT_Subject = @NT_Subject,
            NT_BodyTemplate = @NT_BodyTemplate,
            NT_IsActive = ISNULL(@NT_IsActive, 1),
            NT_UpdatedAt = SYSUTCDATETIME()
        WHERE NT_Id = @NT_Id
          AND (
                @TenantId IS NULL
                OR NT_TenantId = @TenantId
              );

        RETURN;
    END;

    -- DEACTIVATE
    IF @Action = 'Deactivate'
    BEGIN
        UPDATE dbo.NotificationTemplates_NT
        SET
            NT_IsActive = 0,
            NT_UpdatedAt = SYSUTCDATETIME()
        WHERE NT_Id = @NT_Id
          AND (
                @TenantId IS NULL
                OR NT_TenantId = @TenantId
              );

        RETURN;
    END;

    -- EXISTS BY FIELD
    IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);

        IF @ColumnName NOT IN
        (
            'NT_Name',
            'NT_EventKey',
            'NT_Channel'
        )
        BEGIN
            RAISERROR('Invalid column name.', 16, 1);
            RETURN;
        END;

        SET @Sql = N'
            SELECT COUNT(1)
            FROM dbo.NotificationTemplates_NT
            WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val
              AND NT_IsActive = 1
              AND (
                    @P_TenantId IS NULL
                    OR NT_TenantId = @P_TenantId
                  )';

        IF @ExcludeId IS NOT NULL
            SET @Sql += N'
                AND NT_Id <> @P_ExId';

        EXEC sp_executesql
            @Sql,
            N'
                @P_Val NVARCHAR(MAX),
                @P_ExId UNIQUEIDENTIFIER,
                @P_TenantId UNIQUEIDENTIFIER
            ',
            @P_Val = @Value,
            @P_ExId = @ExcludeId,
            @P_TenantId = @TenantId;

        RETURN;
    END;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_PaymentMethods_PM]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[USP_PaymentMethods_PM]
    @Action NVARCHAR(20),
    @PM_Id UNIQUEIDENTIFIER = NULL,
    @PM_Name NVARCHAR(100) = NULL,
    @PM_Type NVARCHAR(30) = NULL,
    @PM_IsActive BIT = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Normalize input
    SET @PM_Name = NULLIF(LTRIM(RTRIM(@PM_Name)), '');
    SET @PM_Type = LOWER(NULLIF(LTRIM(RTRIM(@PM_Type)), ''));

    -- GET ALL
    IF @Action = 'GetAll'
    BEGIN
        SELECT
            PM_Id,
            PM_TenantId,
            PM_Name,
            PM_Type,
            PM_IsActive
        FROM dbo.PaymentMethods_PM
        WHERE PM_IsActive = 1
          AND (
                @TenantId IS NULL
                OR PM_TenantId = @TenantId
              )
        ORDER BY PM_Name;

        RETURN;
    END;

    -- GET BY ID
    IF @Action = 'GetById'
    BEGIN
        SELECT
            PM_Id,
            PM_TenantId,
            PM_Name,
            PM_Type,
            PM_IsActive
        FROM dbo.PaymentMethods_PM
        WHERE PM_Id = @PM_Id
          AND (
                @TenantId IS NULL
                OR PM_TenantId = @TenantId
              );

        RETURN;
    END;

    -- INSERT
    IF @Action = 'Insert'
    BEGIN
        IF @PM_Name IS NULL
        BEGIN
            RAISERROR('Payment Method Name is required.', 16, 1);
            RETURN;
        END;

        IF @PM_Type IS NULL
        BEGIN
            RAISERROR('Payment Method Type is required.', 16, 1);
            RETURN;
        END;

        -- Allowed values should match CK_PaymentMethods_PM_Type
        IF @PM_Type NOT IN
        (
            'cash',
            'card',
            'online'
        )
        BEGIN
            RAISERROR(
                'Invalid Payment Method Type. Allowed values are: cash, card, online.',
                16,
                1
            );
            RETURN;
        END;

        SET @NewId = NEWID();

        INSERT INTO dbo.PaymentMethods_PM
        (
            PM_Id,
            PM_TenantId,
            PM_Name,
            PM_Type,
            PM_IsActive
        )
        VALUES
        (
            @NewId,
            @TenantId,
            @PM_Name,
            @PM_Type,
            ISNULL(@PM_IsActive, 1)
        );

        RETURN;
    END;

    -- UPDATE
    IF @Action = 'Update'
    BEGIN
        IF @PM_Name IS NULL
        BEGIN
            RAISERROR('Payment Method Name is required.', 16, 1);
            RETURN;
        END;

        IF @PM_Type IS NULL
        BEGIN
            RAISERROR('Payment Method Type is required.', 16, 1);
            RETURN;
        END;

        IF @PM_Type NOT IN
        (
            'cash',
            'card',
            'online'
        )
        BEGIN
            RAISERROR(
                'Invalid Payment Method Type. Allowed values are: cash, card, online.',
                16,
                1
            );
            RETURN;
        END;

        UPDATE dbo.PaymentMethods_PM
        SET
            PM_Name = @PM_Name,
            PM_Type = @PM_Type,
            PM_IsActive = ISNULL(@PM_IsActive, 1)
        WHERE PM_Id = @PM_Id
          AND (
                @TenantId IS NULL
                OR PM_TenantId = @TenantId
              );

        RETURN;
    END;

    -- DEACTIVATE
    IF @Action = 'Deactivate'
    BEGIN
        UPDATE dbo.PaymentMethods_PM
        SET PM_IsActive = 0
        WHERE PM_Id = @PM_Id
          AND (
                @TenantId IS NULL
                OR PM_TenantId = @TenantId
              );

        RETURN;
    END;

    -- EXISTS BY FIELD
    IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);

        IF @ColumnName NOT IN
        (
            'PM_Name',
            'PM_Type'
        )
        BEGIN
            RAISERROR('Invalid column name.', 16, 1);
            RETURN;
        END;

        SET @Sql = N'
            SELECT COUNT(1)
            FROM dbo.PaymentMethods_PM
            WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val
              AND PM_IsActive = 1
              AND (
                    @P_TenantId IS NULL
                    OR PM_TenantId = @P_TenantId
                  )';

        IF @ExcludeId IS NOT NULL
        BEGIN
            SET @Sql += N'
                AND PM_Id <> @P_ExId';
        END;

        EXEC sp_executesql
            @Sql,
            N'
                @P_Val NVARCHAR(MAX),
                @P_ExId UNIQUEIDENTIFIER,
                @P_TenantId UNIQUEIDENTIFIER
            ',
            @P_Val = @Value,
            @P_ExId = @ExcludeId,
            @P_TenantId = @TenantId;

        RETURN;
    END;
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Payments_PAY]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- USP_Payments_PAY
-- ============================================================
CREATE PROCEDURE [dbo].[USP_Payments_PAY]
    @Action NVARCHAR(20),
    @PAY_Id UNIQUEIDENTIFIER = NULL,
    @PAY_TenantId UNIQUEIDENTIFIER = NULL,
    @PAY_StudentId UNIQUEIDENTIFIER = NULL,
    @PAY_PaymentNumber NVARCHAR(50) = NULL,
    @PAY_PaymentDate DATETIME2 = NULL,
    @PAY_Amount DECIMAL(12,2) = NULL,
    @PAY_PaymentMethodId UNIQUEIDENTIFIER = NULL,
    @PAY_Status NVARCHAR(20) = NULL,
    @PAY_TransactionReference NVARCHAR(150) = NULL,
    @PAY_Notes NVARCHAR(MAX) = NULL,
    @PAY_CreatedBy UNIQUEIDENTIFIER = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @Status NVARCHAR(20) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetById'
        BEGIN
            SELECT p.*, s.S_FirstName + ' ' + s.S_LastName AS StudentName, s.S_StudentCode AS StudentCode, pm.PM_Name AS PaymentMethodName
            FROM Payments_PAY p
            LEFT JOIN Students_S s ON p.PAY_StudentId = s.S_Id
            LEFT JOIN PaymentMethods_PM pm ON p.PAY_PaymentMethodId = pm.PM_Id
            WHERE p.PAY_Id = @PAY_Id AND p.PAY_TenantId = @PAY_TenantId;
        END

        IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM Payments_PAY p
            WHERE p.PAY_TenantId = @PAY_TenantId
              AND (@SearchTerm IS NULL OR p.PAY_PaymentNumber LIKE '%' + @SearchTerm + '%')
              AND (@Status IS NULL OR p.PAY_Status = @Status);

            SELECT p.*, s.S_FirstName + ' ' + s.S_LastName AS StudentName, s.S_StudentCode AS StudentCode, pm.PM_Name AS PaymentMethodName
            FROM Payments_PAY p
            LEFT JOIN Students_S s ON p.PAY_StudentId = s.S_Id
            LEFT JOIN PaymentMethods_PM pm ON p.PAY_PaymentMethodId = pm.PM_Id
            WHERE p.PAY_TenantId = @PAY_TenantId
              AND (@SearchTerm IS NULL OR p.PAY_PaymentNumber LIKE '%' + @SearchTerm + '%')
              AND (@Status IS NULL OR p.PAY_Status = @Status)
            ORDER BY p.PAY_PaymentDate DESC
            OFFSET (@PageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END

        IF @Action = 'GetNextNumber'
        BEGIN
            DECLARE @MaxNum INT;
            SELECT @MaxNum = ISNULL(MAX(CAST(SUBSTRING(PAY_PaymentNumber, 5, LEN(PAY_PaymentNumber) - 4) AS INT)), 0)
            FROM Payments_PAY WHERE PAY_TenantId = @PAY_TenantId;
            SELECT 'PAY-' + RIGHT('0000' + CAST(@MaxNum + 1 AS NVARCHAR), 4);
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO Payments_PAY (PAY_Id, PAY_TenantId, PAY_StudentId, PAY_PaymentNumber, PAY_PaymentDate, PAY_Amount, PAY_PaymentMethodId, PAY_Status, PAY_TransactionReference, PAY_Notes, PAY_CreatedBy, PAY_CreatedAt, PAY_UpdatedAt)
            VALUES (@PAY_Id, @PAY_TenantId, @PAY_StudentId, @PAY_PaymentNumber, @PAY_PaymentDate, @PAY_Amount, @PAY_PaymentMethodId, @PAY_Status, @PAY_TransactionReference, @PAY_Notes, @PAY_CreatedBy, SYSUTCDATETIME(), SYSUTCDATETIME());
            SELECT @PAY_Id;
        END

        IF @Action = 'Delete'
        BEGIN
            DELETE FROM Payments_PAY WHERE PAY_Id = @PAY_Id AND PAY_TenantId = @PAY_TenantId;
            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_Programs_P]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_Programs_P]
    @Action NVARCHAR(20),
    @P_Id UNIQUEIDENTIFIER = NULL,
    @P_Name NVARCHAR(200) = NULL,
    @P_Code NVARCHAR(50) = NULL,
    @P_DurationValue INT = NULL,
    @P_DurationUnit NVARCHAR(20) = NULL,
    @P_Description NVARCHAR(MAX) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT P_Id, P_TenantId, P_Name, P_Code, P_DurationValue, P_DurationUnit,
               P_Description, P_Status, P_CreatedAt, P_UpdatedAt
        FROM dbo.Programs_P
        WHERE P_Status = 'active'
        ORDER BY P_Name;
    END

    ELSE IF @Action = 'GetById'
    BEGIN
        SELECT P_Id, P_TenantId, P_Name, P_Code, P_DurationValue, P_DurationUnit,
               P_Description, P_Status, P_CreatedAt, P_UpdatedAt
        FROM dbo.Programs_P
        WHERE P_Id = @P_Id;
    END

    ELSE IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();
        INSERT INTO dbo.Programs_P (P_Id, P_TenantId, P_Name, P_Code, P_DurationValue, P_DurationUnit, P_Description, P_Status)
        VALUES (@NewId, @TenantId, @P_Name, @P_Code, @P_DurationValue, @P_DurationUnit, @P_Description, 'active');
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE dbo.Programs_P
        SET P_Name = @P_Name,
            P_Code = @P_Code,
            P_DurationValue = @P_DurationValue,
            P_DurationUnit = @P_DurationUnit,
            P_Description = @P_Description,
            P_UpdatedAt = SYSUTCDATETIME()
        WHERE P_Id = @P_Id;
    END

    ELSE IF @Action = 'Deactivate'
    BEGIN
        UPDATE dbo.Programs_P
        SET P_Status = 'inactive', P_UpdatedAt = SYSUTCDATETIME()
        WHERE P_Id = @P_Id;
    END

    ELSE IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'SELECT COUNT(1) FROM dbo.Programs_P WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val AND P_Status = ''active''';
        IF @ExcludeId IS NOT NULL
            SET @Sql = @Sql + N' AND P_Id <> @P_ExId';
        EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX), @P_ExId UNIQUEIDENTIFIER', @Value, @ExcludeId;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Results_R]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================
-- USP_Results_R
-- ============================================================
CREATE PROCEDURE [dbo].[USP_Results_R]
    @Action NVARCHAR(20),
    @R_Id UNIQUEIDENTIFIER = NULL,
    @R_ExamId UNIQUEIDENTIFIER = NULL,
    @R_StudentId UNIQUEIDENTIFIER = NULL,
    @R_TotalMarks DECIMAL(10,2) = NULL,
    @R_MarksObtained DECIMAL(10,2) = NULL,
    @R_Percentage DECIMAL(6,2) = NULL,
    @R_Grade NVARCHAR(20) = NULL,
    @R_ResultStatus NVARCHAR(30) = NULL,
    @R_Remarks NVARCHAR(MAX) = NULL,
    @ExamId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetByExam'
        BEGIN
            SELECT r.*, s.S_FirstName + ' ' + s.S_LastName AS StudentName, s.S_StudentCode AS StudentCode
            FROM Results_R r
            LEFT JOIN Students_S s ON r.R_StudentId = s.S_Id
            WHERE r.R_ExamId = @ExamId;
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO Results_R (R_Id, R_ExamId, R_StudentId, R_TotalMarks, R_MarksObtained, R_Percentage, R_Grade, R_ResultStatus, R_Remarks, R_CreatedAt, R_UpdatedAt)
            VALUES (@R_Id, @R_ExamId, @R_StudentId, @R_TotalMarks, @R_MarksObtained, @R_Percentage, @R_Grade, @R_ResultStatus, @R_Remarks, SYSUTCDATETIME(), SYSUTCDATETIME());
        END

        IF @Action = 'DeleteByExam'
        BEGIN
            DELETE FROM Results_R WHERE R_ExamId = @R_ExamId;
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
/****** Object:  StoredProcedure [dbo].[USP_Staff_ST]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- USP_Staff_ST
-- ============================================================
CREATE PROCEDURE [dbo].[USP_Staff_ST]
    @Action NVARCHAR(20),
    @ST_Id UNIQUEIDENTIFIER = NULL,
    @ST_TenantId UNIQUEIDENTIFIER = NULL,
    @ST_BranchId UNIQUEIDENTIFIER = NULL,
    @ST_DepartmentId UNIQUEIDENTIFIER = NULL,
    @ST_DesignationId UNIQUEIDENTIFIER = NULL,
    @ST_EmployeeCode NVARCHAR(50) = NULL,
    @ST_FirstName NVARCHAR(100) = NULL,
    @ST_LastName NVARCHAR(100) = NULL,
    @ST_Email NVARCHAR(255) = NULL,
    @ST_Phone NVARCHAR(30) = NULL,
    @ST_JoiningDate DATE = NULL,
    @ST_Status NVARCHAR(20) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @BranchId UNIQUEIDENTIFIER = NULL,
    @Status NVARCHAR(20) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetAll'
        BEGIN
            SELECT ST_Id, ST_TenantId, ST_FirstName, ST_LastName, ST_EmployeeCode, ST_Email, ST_Status
            FROM Staff_ST
            WHERE ST_DeletedAt IS NULL
            ORDER BY ST_FirstName, ST_LastName;
        END

        IF @Action = 'GetById'
        BEGIN
            SELECT s.*,
                   b.B_Name AS BranchName,
                   d.D_Name AS DepartmentName,
                   ds.DS_Name AS DesignationName
            FROM Staff_ST s
            LEFT JOIN Branches_B b ON s.ST_BranchId = b.B_Id
            LEFT JOIN Departments_D d ON s.ST_DepartmentId = d.D_Id
            LEFT JOIN Designations_DS ds ON s.ST_DesignationId = ds.DS_Id
            WHERE s.ST_Id = @ST_Id AND s.ST_TenantId = @ST_TenantId AND s.ST_DeletedAt IS NULL;
        END

        IF @Action = 'GetPaged'
        BEGIN
            SELECT COUNT(*) AS TotalCount
            FROM Staff_ST s
            WHERE s.ST_TenantId = @ST_TenantId AND s.ST_DeletedAt IS NULL
              AND (@SearchTerm IS NULL OR s.ST_FirstName LIKE '%' + @SearchTerm + '%' OR s.ST_LastName LIKE '%' + @SearchTerm + '%' OR s.ST_EmployeeCode LIKE '%' + @SearchTerm + '%' OR s.ST_Email LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR s.ST_BranchId = @BranchId)
              AND (@Status IS NULL OR s.ST_Status = @Status);

            SELECT s.*,
                   b.B_Name AS BranchName,
                   d.D_Name AS DepartmentName,
                   ds.DS_Name AS DesignationName
            FROM Staff_ST s
            LEFT JOIN Branches_B b ON s.ST_BranchId = b.B_Id
            LEFT JOIN Departments_D d ON s.ST_DepartmentId = d.D_Id
            LEFT JOIN Designations_DS ds ON s.ST_DesignationId = ds.DS_Id
            WHERE s.ST_TenantId = @ST_TenantId AND s.ST_DeletedAt IS NULL
              AND (@SearchTerm IS NULL OR s.ST_FirstName LIKE '%' + @SearchTerm + '%' OR s.ST_LastName LIKE '%' + @SearchTerm + '%' OR s.ST_EmployeeCode LIKE '%' + @SearchTerm + '%' OR s.ST_Email LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR s.ST_BranchId = @BranchId)
              AND (@Status IS NULL OR s.ST_Status = @Status)
            ORDER BY s.ST_FirstName, s.ST_LastName
            OFFSET (@PageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END

        IF @Action = 'ExistsByCode'
        BEGIN
            SELECT COUNT(*) FROM Staff_ST
            WHERE ST_TenantId = @ST_TenantId AND ST_EmployeeCode = @ST_EmployeeCode
              AND (@ExcludeId IS NULL OR ST_Id <> @ExcludeId);
        END

        IF @Action = 'Insert'
        BEGIN
            INSERT INTO Staff_ST (ST_Id, ST_TenantId, ST_BranchId, ST_DepartmentId, ST_DesignationId, ST_EmployeeCode, ST_FirstName, ST_LastName, ST_Email, ST_Phone, ST_JoiningDate, ST_Status, ST_CreatedAt, ST_UpdatedAt)
            VALUES (@ST_Id, @ST_TenantId, @ST_BranchId, @ST_DepartmentId, @ST_DesignationId, @ST_EmployeeCode, @ST_FirstName, @ST_LastName, @ST_Email, @ST_Phone, @ST_JoiningDate, @ST_Status, SYSUTCDATETIME(), SYSUTCDATETIME());

            SELECT @ST_Id;
        END

        IF @Action = 'Update'
        BEGIN
            UPDATE Staff_ST
            SET ST_BranchId = @ST_BranchId,
                ST_DepartmentId = @ST_DepartmentId,
                ST_DesignationId = @ST_DesignationId,
                ST_EmployeeCode = @ST_EmployeeCode,
                ST_FirstName = @ST_FirstName,
                ST_LastName = @ST_LastName,
                ST_Email = @ST_Email,
                ST_Phone = @ST_Phone,
                ST_JoiningDate = @ST_JoiningDate,
                ST_Status = @ST_Status,
                ST_UpdatedAt = SYSUTCDATETIME()
            WHERE ST_Id = @ST_Id AND ST_TenantId = @ST_TenantId;

            SELECT @@ROWCOUNT;
        END

        IF @Action = 'Delete'
        BEGIN
            UPDATE Staff_ST SET ST_DeletedAt = SYSUTCDATETIME(), ST_UpdatedAt = SYSUTCDATETIME()
            WHERE ST_Id = @ST_Id AND ST_TenantId = @ST_TenantId;

            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_Students_S]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  StoredProcedure [dbo].[USP_Subjects_SB]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_Subjects_SB]
    @Action NVARCHAR(20),
    @SB_Id UNIQUEIDENTIFIER = NULL,
    @SB_Name NVARCHAR(200) = NULL,
    @SB_Code NVARCHAR(50) = NULL,
    @SB_Description NVARCHAR(MAX) = NULL,
    @SB_Credits NUMERIC = NULL,
    @SB_MaxMarks NUMERIC = NULL,
    @SB_PassMarks NUMERIC = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT SB_Id, SB_TenantId, SB_Name, SB_Code, SB_Description, SB_Credits, SB_MaxMarks, SB_PassMarks,
               SB_CreatedAt, SB_UpdatedAt
        FROM dbo.Subjects_SB
        ORDER BY SB_Name;
    END

    ELSE IF @Action = 'GetById'
    BEGIN
        SELECT SB_Id, SB_TenantId, SB_Name, SB_Code, SB_Description, SB_Credits, SB_MaxMarks, SB_PassMarks,
               SB_CreatedAt, SB_UpdatedAt
        FROM dbo.Subjects_SB
        WHERE SB_Id = @SB_Id;
    END

    ELSE IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();
        INSERT INTO dbo.Subjects_SB (SB_Id, SB_TenantId, SB_Name, SB_Code, SB_Description, SB_Credits, SB_MaxMarks, SB_PassMarks)
        VALUES (@NewId, @TenantId, @SB_Name, @SB_Code, @SB_Description, @SB_Credits, @SB_MaxMarks, @SB_PassMarks);
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE dbo.Subjects_SB
        SET SB_Name = @SB_Name,
            SB_Code = @SB_Code,
            SB_Description = @SB_Description,
            SB_Credits = @SB_Credits,
            SB_MaxMarks = @SB_MaxMarks,
            SB_PassMarks = @SB_PassMarks,
            SB_UpdatedAt = SYSUTCDATETIME()
        WHERE SB_Id = @SB_Id;
    END

    ELSE IF @Action = 'Deactivate'
    BEGIN
        RAISERROR('Subjects_SB does not support soft delete.', 16, 1);
    END

    ELSE IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'SELECT COUNT(1) FROM dbo.Subjects_SB WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val';
        IF @ExcludeId IS NOT NULL
            SET @Sql = @Sql + N' AND SB_Id <> @P_ExId';
        EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX), @P_ExId UNIQUEIDENTIFIER', @Value, @ExcludeId;
    END
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Timetables_TT]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- ============================================================================
-- 4. USP_Timetables_TT
-- ============================================================================
CREATE PROCEDURE [dbo].[USP_Timetables_TT]
    @Action NVARCHAR(20),
    @TT_Id UNIQUEIDENTIFIER = NULL,
    @TT_TenantId UNIQUEIDENTIFIER = NULL,
    @TT_BranchId UNIQUEIDENTIFIER = NULL,
    @TT_BatchId UNIQUEIDENTIFIER = NULL,
    @TT_SubjectId UNIQUEIDENTIFIER = NULL,
    @TT_StaffId UNIQUEIDENTIFIER = NULL,
    @TT_ClassroomId UNIQUEIDENTIFIER = NULL,
    @TT_DayOfWeek SMALLINT = NULL,
    @TT_StartTime TIME = NULL,
    @TT_EndTime TIME = NULL,
    @TT_EffectiveFrom DATE = NULL,
    @TT_EffectiveTo DATE = NULL,
    @BatchId UNIQUEIDENTIFIER = NULL,
    @BranchId UNIQUEIDENTIFIER = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF @Action = 'GetAll'
        BEGIN
            SELECT tt.*, s.SB_Name AS SubjectName,
                   st.ST_FirstName + ' ' + st.ST_LastName AS StaffName,
                   cr.CR_Name AS ClassroomName,
                   bt.BT_Name AS BatchName
            FROM Timetables_TT tt
            LEFT JOIN Subjects_SB s ON tt.TT_SubjectId = s.SB_Id
            LEFT JOIN Staff_ST st ON tt.TT_StaffId = st.ST_Id
            LEFT JOIN Classrooms_CR cr ON tt.TT_ClassroomId = cr.CR_Id
            LEFT JOIN Batches_BT bt ON tt.TT_BatchId = bt.BT_Id
            WHERE tt.TT_TenantId = @TT_TenantId
              AND (@BatchId IS NULL OR tt.TT_BatchId = @BatchId)
              AND (@BranchId IS NULL OR tt.TT_BranchId = @BranchId)
            ORDER BY tt.TT_DayOfWeek, tt.TT_StartTime;
        END

        ELSE IF @Action = 'GetById'
        BEGIN
            SELECT tt.*, s.SB_Name AS SubjectName,
                   st.ST_FirstName + ' ' + st.ST_LastName AS StaffName,
                   cr.CR_Name AS ClassroomName,
                   bt.BT_Name AS BatchName
            FROM Timetables_TT tt
            LEFT JOIN Subjects_SB s ON tt.TT_SubjectId = s.SB_Id
            LEFT JOIN Staff_ST st ON tt.TT_StaffId = st.ST_Id
            LEFT JOIN Classrooms_CR cr ON tt.TT_ClassroomId = cr.CR_Id
            LEFT JOIN Batches_BT bt ON tt.TT_BatchId = bt.BT_Id
            WHERE tt.TT_Id = @TT_Id AND tt.TT_TenantId = @TT_TenantId;
        END

        ELSE IF @Action = 'CheckConflict'
        BEGIN
            SELECT CASE WHEN EXISTS (
                SELECT 1 FROM Timetables_TT
                WHERE TT_TenantId = @TT_TenantId AND TT_BatchId = @TT_BatchId AND TT_DayOfWeek = @TT_DayOfWeek
                AND TT_StartTime < @TT_EndTime AND TT_EndTime > @TT_StartTime
                AND (@ExcludeId IS NULL OR TT_Id <> @ExcludeId)
            ) THEN 1 ELSE 0 END;
        END

        ELSE IF @Action = 'Insert'
        BEGIN
            INSERT INTO Timetables_TT (TT_Id, TT_TenantId, TT_BranchId, TT_BatchId, TT_SubjectId, TT_StaffId, TT_ClassroomId, TT_DayOfWeek, TT_StartTime, TT_EndTime, TT_EffectiveFrom, TT_EffectiveTo, TT_CreatedAt, TT_UpdatedAt)
            VALUES (@TT_Id, @TT_TenantId, @TT_BranchId, @TT_BatchId, @TT_SubjectId, @TT_StaffId, @TT_ClassroomId, @TT_DayOfWeek, @TT_StartTime, @TT_EndTime, @TT_EffectiveFrom, @TT_EffectiveTo, GETUTCDATE(), GETUTCDATE());
            SELECT @TT_Id;
        END

        ELSE IF @Action = 'Update'
        BEGIN
            UPDATE Timetables_TT SET
                TT_BranchId = @TT_BranchId, TT_BatchId = @TT_BatchId, TT_SubjectId = @TT_SubjectId,
                TT_StaffId = @TT_StaffId, TT_ClassroomId = @TT_ClassroomId, TT_DayOfWeek = @TT_DayOfWeek,
                TT_StartTime = @TT_StartTime, TT_EndTime = @TT_EndTime,
                TT_EffectiveFrom = @TT_EffectiveFrom, TT_EffectiveTo = @TT_EffectiveTo, TT_UpdatedAt = GETUTCDATE()
            WHERE TT_Id = @TT_Id AND TT_TenantId = @TT_TenantId;
            SELECT @@ROWCOUNT;
        END

        ELSE IF @Action = 'Delete'
        BEGIN
            DELETE FROM Timetables_TT WHERE TT_Id = @TT_Id AND TT_TenantId = @TT_TenantId;
            SELECT @@ROWCOUNT;
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
/****** Object:  StoredProcedure [dbo].[USP_Vendors_V]    Script Date: 06-09-2026 20:21:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_Vendors_V]
    @Action NVARCHAR(20),
    @V_Id UNIQUEIDENTIFIER = NULL,
    @V_Name NVARCHAR(200) = NULL,
    @V_Code NVARCHAR(50) = NULL,
    @V_Email NVARCHAR(255) = NULL,
    @V_Phone NVARCHAR(30) = NULL,
    @V_TaxNumber NVARCHAR(100) = NULL,
    @V_Address NVARCHAR(MAX) = NULL,
    @TenantId UNIQUEIDENTIFIER = NULL,
    @CreatedBy UNIQUEIDENTIFIER = NULL,
    @UpdatedBy UNIQUEIDENTIFIER = NULL,
    @ColumnName NVARCHAR(100) = NULL,
    @Value NVARCHAR(MAX) = NULL,
    @ExcludeId UNIQUEIDENTIFIER = NULL,
    @NewId UNIQUEIDENTIFIER = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'GetAll'
    BEGIN
        SELECT V_Id, V_TenantId, V_Name, V_Code, V_Email, V_Phone, V_TaxNumber, V_Address,
               V_CreatedAt, V_UpdatedAt
        FROM dbo.Vendors_V
        ORDER BY V_Name;
    END

    ELSE IF @Action = 'GetById'
    BEGIN
        SELECT V_Id, V_TenantId, V_Name, V_Code, V_Email, V_Phone, V_TaxNumber, V_Address,
               V_CreatedAt, V_UpdatedAt
        FROM dbo.Vendors_V
        WHERE V_Id = @V_Id;
    END

    ELSE IF @Action = 'Insert'
    BEGIN
        SET @NewId = NEWID();
        INSERT INTO dbo.Vendors_V (V_Id, V_TenantId, V_Name, V_Code, V_Email, V_Phone, V_TaxNumber, V_Address)
        VALUES (@NewId, @TenantId, @V_Name, @V_Code, @V_Email, @V_Phone, @V_TaxNumber, @V_Address);
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE dbo.Vendors_V
        SET V_Name = @V_Name,
            V_Code = @V_Code,
            V_Email = @V_Email,
            V_Phone = @V_Phone,
            V_TaxNumber = @V_TaxNumber,
            V_Address = @V_Address,
            V_UpdatedAt = SYSUTCDATETIME()
        WHERE V_Id = @V_Id;
    END

    ELSE IF @Action = 'Deactivate'
    BEGIN
        RAISERROR('Vendors_V does not support soft delete.', 16, 1);
    END

    ELSE IF @Action = 'ExistsByField'
    BEGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'SELECT COUNT(1) FROM dbo.Vendors_V WHERE ' + QUOTENAME(@ColumnName) + N' = @P_Val';
        IF @ExcludeId IS NOT NULL
            SET @Sql = @Sql + N' AND V_Id <> @P_ExId';
        EXEC sp_executesql @Sql, N'@P_Val NVARCHAR(MAX), @P_ExId UNIQUEIDENTIFIER', @Value, @ExcludeId;
    END
END
GO

GO
/****** Object:  StoredProcedure [dbo].[USP_AdmissionApplicationOverview]    Script Date: 06-09-2026 20:30:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- USP_AdmissionApplicationOverview
-- Fetches an overview of applications received by the employer.
-- Returns: Summary stats + recent applications with candidate info.
-- ============================================================
CREATE PROCEDURE [dbo].[USP_AdmissionApplicationOverview]
    @Action NVARCHAR(20),
    @TenantId UNIQUEIDENTIFIER = NULL,
    @BranchId UNIQUEIDENTIFIER = NULL,
    @CourseId UNIQUEIDENTIFIER = NULL,
    @AcademicYearId UNIQUEIDENTIFIER = NULL,
    @Status NVARCHAR(20) = NULL,
    @SearchTerm NVARCHAR(255) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- ============================================================
        -- Action: Summary
        -- Returns total count and per-status counts for dashboard cards
        -- ============================================================
        IF @Action = 'Summary'
        BEGIN
            SELECT
                COUNT(*) AS TotalApplications,
                SUM(CASE WHEN AA_Status = 'submitted' THEN 1 ELSE 0 END) AS SubmittedCount,
                SUM(CASE WHEN AA_Status = 'under_review' THEN 1 ELSE 0 END) AS UnderReviewCount,
                SUM(CASE WHEN AA_Status = 'approved' THEN 1 ELSE 0 END) AS ApprovedCount,
                SUM(CASE WHEN AA_Status = 'rejected' THEN 1 ELSE 0 END) AS RejectedCount,
                SUM(CASE WHEN AA_Status = 'waitlisted' THEN 1 ELSE 0 END) AS WaitlistedCount,
                SUM(CASE WHEN AA_Status = 'enrolled' THEN 1 ELSE 0 END) AS EnrolledCount,
                SUM(CASE WHEN AA_Status = 'cancelled' THEN 1 ELSE 0 END) AS CancelledCount
            FROM AdmissionApplications_AA
            WHERE AA_TenantId = @TenantId
              AND (@BranchId IS NULL OR AA_BranchId = @BranchId)
              AND (@CourseId IS NULL OR AA_CourseId = @CourseId)
              AND (@AcademicYearId IS NULL OR AA_AcademicYearId = @AcademicYearId);
        END

        -- ============================================================
        -- Action: GetPaged
        -- Returns paginated list of applications with candidate and
        -- related entity names (Course, AcademicYear, Branch)
        -- ============================================================
        ELSE IF @Action = 'GetPaged'
        BEGIN
            -- Total count for pagination
            SELECT COUNT(*) AS TotalCount
            FROM AdmissionApplications_AA a
            WHERE a.AA_TenantId = @TenantId
              AND (@SearchTerm IS NULL OR a.AA_ApplicationNumber LIKE '%' + @SearchTerm + '%'
                   OR a.AA_FirstName LIKE '%' + @SearchTerm + '%'
                   OR a.AA_LastName LIKE '%' + @SearchTerm + '%'
                   OR a.AA_Email LIKE '%' + @SearchTerm + '%'
                   OR a.AA_Phone LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR a.AA_BranchId = @BranchId)
              AND (@CourseId IS NULL OR a.AA_CourseId = @CourseId)
              AND (@AcademicYearId IS NULL OR a.AA_AcademicYearId = @AcademicYearId)
              AND (@Status IS NULL OR a.AA_Status = @Status);

            -- Paged results with related entity names
            SELECT
                a.AA_Id,
                a.AA_TenantId,
                a.AA_BranchId,
                a.AA_ApplicationNumber,
                a.AA_FirstName,
                a.AA_LastName,
                a.AA_DateOfBirth,
                a.AA_Gender,
                a.AA_Email,
                a.AA_Phone,
                a.AA_CourseId,
                a.AA_AcademicYearId,
                a.AA_Status,
                a.AA_SubmittedAt,
                a.AA_ReviewedAt,
                a.AA_ReviewedBy,
                a.AA_Notes,
                a.AA_CreatedAt,
                a.AA_UpdatedAt,
                -- Related entity display names
                c.C_Name AS CourseName,
                ay.AY_Name AS AcademicYearName,
                b.B_Name AS BranchName,
                -- Candidate full name (computed)
                a.AA_FirstName + ' ' + a.AA_LastName AS CandidateFullName,
                -- Age computed from DOB
                CASE
                    WHEN a.AA_DateOfBirth IS NOT NULL
                    THEN DATEDIFF(YEAR, a.AA_DateOfBirth, GETUTCDATE())
                         - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, a.AA_DateOfBirth, GETUTCDATE()), a.AA_DateOfBirth) > GETUTCDATE()
                                THEN 1 ELSE 0 END
                    ELSE NULL
                END AS Age
            FROM AdmissionApplications_AA a
            LEFT JOIN Courses_C c ON a.AA_CourseId = c.C_Id
            LEFT JOIN AcademicYears_AY ay ON a.AA_AcademicYearId = ay.AY_Id
            LEFT JOIN Branches_B b ON a.AA_BranchId = b.B_Id
            WHERE a.AA_TenantId = @TenantId
              AND (@SearchTerm IS NULL OR a.AA_ApplicationNumber LIKE '%' + @SearchTerm + '%'
                   OR a.AA_FirstName LIKE '%' + @SearchTerm + '%'
                   OR a.AA_LastName LIKE '%' + @SearchTerm + '%'
                   OR a.AA_Email LIKE '%' + @SearchTerm + '%'
                   OR a.AA_Phone LIKE '%' + @SearchTerm + '%')
              AND (@BranchId IS NULL OR a.AA_BranchId = @BranchId)
              AND (@CourseId IS NULL OR a.AA_CourseId = @CourseId)
              AND (@AcademicYearId IS NULL OR a.AA_AcademicYearId = @AcademicYearId)
              AND (@Status IS NULL OR a.AA_Status = @Status)
            ORDER BY a.AA_CreatedAt DESC
            OFFSET (@PageNumber - 1) * @PageSize ROWS
            FETCH NEXT @PageSize ROWS ONLY;
        END

        -- ============================================================
        -- Action: GetById
        -- Returns single application with full candidate and related
        -- entity details
        -- ============================================================
        ELSE IF @Action = 'GetById'
        BEGIN
            SELECT
                a.*,
                c.C_Name AS CourseName,
                ay.AY_Name AS AcademicYearName,
                b.B_Name AS BranchName,
                a.AA_FirstName + ' ' + a.AA_LastName AS CandidateFullName,
                CASE
                    WHEN a.AA_DateOfBirth IS NOT NULL
                    THEN DATEDIFF(YEAR, a.AA_DateOfBirth, GETUTCDATE())
                         - CASE WHEN DATEADD(YEAR, DATEDIFF(YEAR, a.AA_DateOfBirth, GETUTCDATE()), a.AA_DateOfBirth) > GETUTCDATE()
                                THEN 1 ELSE 0 END
                    ELSE NULL
                END AS Age
            FROM AdmissionApplications_AA a
            LEFT JOIN Courses_C c ON a.AA_CourseId = c.C_Id
            LEFT JOIN AcademicYears_AY ay ON a.AA_AcademicYearId = ay.AY_Id
            LEFT JOIN Branches_B b ON a.AA_BranchId = b.B_Id
            WHERE a.AA_Id = @AA_Id AND a.AA_TenantId = @TenantId;
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

/****** Object:  StoredProcedure [dbo].[USP_GenericDropdown]    Script Date: 07-09-2026 12:00:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================================
-- USP_GenericDropdown
-- Generic dropdown stored procedure for all entities.
-- Builds dynamic SQL from table/column name params.
-- Returns: Value, Text, Code, ParentId, IsActive
-- ============================================================
CREATE PROCEDURE [dbo].[USP_GenericDropdown]
    @TableName NVARCHAR(128),
    @KeyColumn NVARCHAR(128),
    @ValueColumn NVARCHAR(128),
    @TextColumn NVARCHAR(128),
    @CodeColumn NVARCHAR(128) = NULL,
    @ActiveColumn NVARCHAR(128) = NULL,
    @ParentColumn NVARCHAR(128) = NULL,
    @ParentId NVARCHAR(100) = NULL,
    @Search NVARCHAR(255) = NULL,
    @ActiveOnly BIT = 1,
    @OrderByColumn NVARCHAR(128) = NULL,
    @OrderByDirection NVARCHAR(4) = 'ASC'
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Sql NVARCHAR(MAX);
    DECLARE @ParamList NVARCHAR(MAX);
    DECLARE @WhereClause NVARCHAR(MAX) = N'';
    DECLARE @SelectColumns NVARCHAR(MAX);
    DECLARE @ActiveColumnType NVARCHAR(20);

    -- Validate required params
    IF @TableName IS NULL OR @KeyColumn IS NULL OR @ValueColumn IS NULL OR @TextColumn IS NULL
    BEGIN
        RAISERROR('TableName, KeyColumn, ValueColumn, and TextColumn are required.', 16, 1);
        RETURN;
    END

    -- Detect ActiveColumn data type (bit vs nvarchar)
    IF @ActiveColumn IS NOT NULL
    BEGIN
        SELECT @ActiveColumnType = DATA_TYPE
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_NAME = PARSENAME(REPLACE(@TableName, 'dbo.', ''), 1)
          AND COLUMN_NAME = @ActiveColumn;

        -- Default to 'bit' if column not found (will fail gracefully)
        IF @ActiveColumnType IS NULL
            SET @ActiveColumnType = 'bit';
    END

    -- Build SELECT columns
    SET @SelectColumns = QUOTENAME(@ValueColumn) + N' AS [Value], '
                       + QUOTENAME(@TextColumn) + N' AS [Text]';

    IF @CodeColumn IS NOT NULL
        SET @SelectColumns = @SelectColumns + N', ' + QUOTENAME(@CodeColumn) + N' AS [Code]';

    IF @ParentColumn IS NOT NULL
        SET @SelectColumns = @SelectColumns + N', ' + QUOTENAME(@ParentColumn) + N' AS [ParentId]';

    IF @ActiveColumn IS NOT NULL
        SET @SelectColumns = @SelectColumns + N', ' + QUOTENAME(@ActiveColumn) + N' AS [IsActive]';

    -- Build WHERE clause
    -- ActiveOnly filter
    IF @ActiveOnly = 1 AND @ActiveColumn IS NOT NULL
    BEGIN
        IF @ActiveColumnType = 'bit'
            SET @WhereClause = @WhereClause + N' AND ' + QUOTENAME(@ActiveColumn) + N' = 1';
        ELSE
            SET @WhereClause = @WhereClause + N' AND LOWER(' + QUOTENAME(@ActiveColumn) + N') = N''active''';
    END

    -- Parent filter (cascading dropdown)
    IF @ParentColumn IS NOT NULL AND @ParentId IS NOT NULL
    BEGIN
        SET @WhereClause = @WhereClause + N' AND ' + QUOTENAME(@ParentColumn) + N' = @PParentId';
    END

    -- Search filter (LIKE on TextColumn)
    IF @Search IS NOT NULL AND LEN(@Search) > 0
    BEGIN
        SET @WhereClause = @WhereClause + N' AND ' + QUOTENAME(@TextColumn) + N' LIKE N''%' + REPLACE(@Search, '''', '''''') + N'%''';
    END

    -- Build ORDER BY
    DECLARE @OrderBy NVARCHAR(200);
    IF @OrderByColumn IS NOT NULL AND LEN(@OrderByColumn) > 0
        SET @OrderBy = QUOTENAME(@OrderByColumn) + N' ' + CASE WHEN UPPER(@OrderByDirection) = 'DESC' THEN N'DESC' ELSE N'ASC' END;
    ELSE
        SET @OrderBy = QUOTENAME(@TextColumn) + N' ASC';

    -- Build final SQL
    SET @Sql = N'SELECT ' + @SelectColumns
             + N' FROM ' + QUOTENAME(PARSENAME(REPLACE(@TableName, 'dbo.', ''), 2), 'dbo') + N'.' + QUOTENAME(PARSENAME(REPLACE(@TableName, 'dbo.', ''), 1))
             + N' WHERE 1=1'
             + @WhereClause
             + N' ORDER BY ' + @OrderBy;

    -- Execute
    IF @ParentColumn IS NOT NULL AND @ParentId IS NOT NULL
        EXEC sp_executesql @Sql, N'@PParentId NVARCHAR(100)', @ParentId;
    ELSE
        EXEC sp_executesql @Sql;
END
GO