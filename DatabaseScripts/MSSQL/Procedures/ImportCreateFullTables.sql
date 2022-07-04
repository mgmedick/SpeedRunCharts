/****** Object:  StoredProcedure [dbo].[ImportCreateFullTables]    Script Date: 4/15/2022 6:39:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[ImportCreateFullTables]
AS
BEGIN

    --tbl_Platform_Full
    IF OBJECT_ID('dbo.tbl_Platform_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Platform_Full
    END 

    CREATE TABLE [dbo].[tbl_Platform_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1), 
        [Name] [varchar] (50) NOT NULL,
        [YearOfRelease] [int] NULL,
        [ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_Platform_Full_ImportedDate] DEFAULT(GETUTCDATE())
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Platform_Full] ADD CONSTRAINT [PK_tbl_Platform_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_Platform_SpeedRunComID_Full
    IF OBJECT_ID('dbo.tbl_Platform_SpeedRunComID_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Platform_SpeedRunComID_Full
    END 

    CREATE TABLE [dbo].[tbl_Platform_SpeedRunComID_Full] 
    (
        [PlatformID] [int] NOT NULL,
        [SpeedRunComID] [varchar] (10) NOT NULL
    )
    ALTER TABLE [dbo].[tbl_Platform_SpeedRunComID_Full] ADD CONSTRAINT [PK_tbl_Platform_SpeedRunComID_Full] PRIMARY KEY CLUSTERED ([PlatformID]) WITH (FILLFACTOR=90) ON [PRIMARY]    
	--CREATE NONCLUSTERED INDEX [IDX_tbl_Platform_SpeedRunComID_Full_SpeedRunComID] ON [dbo].[tbl_Platform_SpeedRunComID_Full] ([SpeedRunComID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_User_Full
    IF OBJECT_ID('dbo.tbl_User_Full') IS NOT NULL
    BEGIN
        DROP TABLE dbo.tbl_User_Full
    END

    CREATE TABLE [dbo].[tbl_User_Full]
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1), 
        [Name] [nvarchar] (100) NOT NULL,
        [UserRoleID] [int] NOT NULL,
        [Abbr] [varchar] (100) NULL,
        [SignUpDate] [datetime] NULL, 
        [ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_User_Full_ImportedDate] DEFAULT(GETUTCDATE()),
        [ModifiedDate] [datetime] NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_User_Full] ADD CONSTRAINT [PK_tbl_User_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_User_SpeedRunComID_Full
    IF OBJECT_ID('dbo.tbl_User_SpeedRunComID_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_User_SpeedRunComID_Full
    END 

    CREATE TABLE [dbo].[tbl_User_SpeedRunComID_Full] 
    (
        [UserID] [int] NOT NULL,
        [SpeedRunComID] [varchar] (10) NOT NULL
    )
    ALTER TABLE [dbo].[tbl_User_SpeedRunComID_Full] ADD CONSTRAINT [PK_tbl_User_SpeedRunComID_Full] PRIMARY KEY CLUSTERED ([UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]                               
	--CREATE NONCLUSTERED INDEX [IDX_tbl_User_SpeedRunComID_Full_SpeedRunComID] ON [dbo].[tbl_User_SpeedRunComID_Full] ([SpeedRunComID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_User_Location_Full
    IF OBJECT_ID('dbo.tbl_User_Location_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_User_Location_Full
    END 

    CREATE TABLE [dbo].[tbl_User_Location_Full]
    ( 
        [UserID] [int] NOT NULL,
        [Location] [varchar] (100) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_User_Location_Full] ADD CONSTRAINT [PK_tbl_User_Location_Full] PRIMARY KEY CLUSTERED ([UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_User_Link_Full
    IF OBJECT_ID('dbo.tbl_User_Link_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_User_Link_Full
    END 

    CREATE TABLE [dbo].[tbl_User_Link_Full] 
    ( 
        [UserID] [int] NOT NULL,
        [SpeedRunComUrl] [varchar] (1000) NULL, 
        [ProfileImageUrl] [varchar] (1000) NULL,
        [TwitchProfileUrl] [varchar] (1000) NULL,
        [HitboxProfileUrl] [varchar] (1000) NULL,
        [YoutubeProfileUrl] [varchar] (1000) NULL,
        [TwitterProfileUrl] [varchar] (1000) NULL,
        [SpeedRunsLiveProfileUrl] [varchar] (1000) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_User_Link_Full] ADD CONSTRAINT [PK_tbl_User_Link_Full] PRIMARY KEY CLUSTERED ([UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_Guest_Full
    IF OBJECT_ID('dbo.tbl_Guest_Full') IS NOT NULL
    BEGIN
        DROP TABLE dbo.tbl_Guest_Full
    END

    CREATE TABLE [dbo].[tbl_Guest_Full]
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1), 
        [Name] [nvarchar] (100) NOT NULL,
        [Abbr] [varchar] (100) NULL,
        [ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_Guest_Full_ImportedDate] DEFAULT(GETUTCDATE())
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Guest_Full] ADD CONSTRAINT [PK_tbl_Guest_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Guest_Link_Full
    IF OBJECT_ID('dbo.tbl_Guest_Link_Full') IS NOT NULL
    BEGIN
        DROP TABLE dbo.tbl_Guest_Link_Full
    END

    CREATE TABLE [dbo].[tbl_Guest_Link_Full](
        [GuestID] [int] NOT NULL,
        [SpeedRunComUrl] [varchar](1000) NULL
    ) ON [PRIMARY]
    ALTER TABLE [dbo].[tbl_Guest_Link_Full] ADD CONSTRAINT [PK_tbl_Guest_Link_Full] PRIMARY KEY CLUSTERED ([GuestID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Game_Full
    IF OBJECT_ID('dbo.tbl_Game_Full') IS NOT NULL
    BEGIN
        DROP TABLE dbo.tbl_Game_Full
    END

    CREATE TABLE [dbo].[tbl_Game_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1), 
        [Name] [varchar] (100) NOT NULL,
        [IsRomHack] [bit] NOT NULL,
        [YearOfRelease] [int] NULL,
        [Abbr] [varchar] (100) NULL,
        [IsChanged] [bit] NULL,
        [CreatedDate] [datetime] NULL,  
        [ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_Game_Full_ImportedDate] DEFAULT(GETUTCDATE()),
        [ModifiedDate] [datetime] NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Game_Full] ADD CONSTRAINT [PK_tbl_Game_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Game_SpeedRunComID_Full
    IF OBJECT_ID('dbo.tbl_Game_SpeedRunComID_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Game_SpeedRunComID_Full
    END 

    CREATE TABLE [dbo].[tbl_Game_SpeedRunComID_Full] 
    (
        [GameID] [int] NOT NULL,
        [SpeedRunComID] [varchar] (10) NOT NULL
    )
    ALTER TABLE [dbo].[tbl_Game_SpeedRunComID_Full] ADD CONSTRAINT [PK_tbl_Game_SpeedRunComID_Full] PRIMARY KEY CLUSTERED ([GameID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	--CREATE NONCLUSTERED INDEX [IDX_tbl_Game_SpeedRunComID_Full_SpeedRunComID] ON [dbo].[tbl_Game_SpeedRunComID_Full] ([SpeedRunComID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Game_Link_Full
    IF OBJECT_ID('dbo.tbl_Game_Link_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Game_Link_Full
    END 

    CREATE TABLE [dbo].[tbl_Game_Link_Full] 
    (
        [GameID] [int] NOT NULL,
        [SpeedRunComUrl] [varchar] (1000) NOT NULL,
        [CoverImageUrl] [varchar] (1000) NULL,
        [CoverImagePath] [varchar] (125) NULL
    )
    ALTER TABLE [dbo].[tbl_Game_Link_Full] ADD CONSTRAINT [PK_tbl_Game_Link_Full] PRIMARY KEY CLUSTERED ([GameID]) WITH (FILLFACTOR=90) ON [PRIMARY]

        --tbl_Level_Full
    IF OBJECT_ID('dbo.tbl_Level_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Level_Full
    END

    CREATE TABLE [dbo].[tbl_Level_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1), 
        [Name] [varchar] (100) NOT NULL,
        [GameID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Level_Full] ADD CONSTRAINT [PK_tbl_Level_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_Level_SpeedRunComID_Full
    IF OBJECT_ID('dbo.tbl_Level_SpeedRunComID_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Level_SpeedRunComID_Full
    END 

    CREATE TABLE [dbo].[tbl_Level_SpeedRunComID_Full] 
    (
        [LevelID] [int] NOT NULL,
        [SpeedRunComID] [varchar] (10) NOT NULL
    )
    ALTER TABLE [dbo].[tbl_Level_SpeedRunComID_Full] ADD CONSTRAINT [PK_tbl_Level_SpeedRunComID_Full] PRIMARY KEY CLUSTERED ([LevelID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	--CREATE NONCLUSTERED INDEX [IDX_tbl_Level_SpeedRunComID_Full_SpeedRunComID] ON [dbo].[tbl_Level_SpeedRunComID_Full] ([SpeedRunComID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Level_Rule_Full
    IF OBJECT_ID('dbo.tbl_Level_Rule_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Level_Rule_Full 
    END 

    CREATE TABLE [dbo].[tbl_Level_Rule_Full] 
    ( 
        [LevelID] [int] NOT NULL, 
        [Rules] [varchar] (MAX) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Level_Rule_Full] ADD CONSTRAINT [PK_tbl_Level_Rule_Full] PRIMARY KEY CLUSTERED ([LevelID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_Category_Full
    IF OBJECT_ID('dbo.tbl_Category_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Category_Full
    END

    CREATE TABLE [dbo].[tbl_Category_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1), 
        [Name] [varchar] (100) NOT NULL,
        [GameID] [int] NOT NULL,
        [CategoryTypeID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Category_Full] ADD CONSTRAINT [PK_tbl_Category_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Category_SpeedRunComID_Full
    IF OBJECT_ID('dbo.tbl_Category_SpeedRunComID_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Category_SpeedRunComID_Full
    END 

    CREATE TABLE [dbo].[tbl_Category_SpeedRunComID_Full] 
    (
        [CategoryID] [int] NOT NULL,
        [SpeedRunComID] [varchar] (10) NOT NULL
    )
    ALTER TABLE [dbo].[tbl_Category_SpeedRunComID_Full] ADD CONSTRAINT [PK_tbl_Category_SpeedRunComID_Full] PRIMARY KEY CLUSTERED ([CategoryID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
	--CREATE NONCLUSTERED INDEX [IDX_tbl_Category_SpeedRunComID_Full_SpeedRunComID] ON [dbo].[tbl_Category_SpeedRunComID_Full] ([SpeedRunComID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Category_Rule_Full
    IF OBJECT_ID('dbo.tbl_Category_Rule_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Category_Rule_Full
    END 

    CREATE TABLE [dbo].[tbl_Category_Rule_Full] 
    ( 
        [CategoryID] [int] NOT NULL, 
        [Rules] [varchar] (MAX) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Category_Rule_Full] ADD CONSTRAINT [PK_tbl_Category_Rule_Full] PRIMARY KEY CLUSTERED ([CategoryID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Variable_Full
    IF OBJECT_ID('dbo.tbl_Variable_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Variable_Full
    END 

    CREATE TABLE [dbo].[tbl_Variable_Full]
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1), 
        [Name] [varchar] (100) NOT NULL,
        [GameID] [int] NOT NULL,
        [VariableScopeTypeID] [int] NOT NULL,
        [CategoryID] [int] NULL,
        [LevelID] [int] NULL,
        [IsSubCategory] [bit] NOT NULL   
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Variable_Full] ADD CONSTRAINT [PK_tbl_Variable_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Variable_SpeedRunComID_Full
    IF OBJECT_ID('dbo.tbl_Variable_SpeedRunComID_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_Variable_SpeedRunComID_Full
    END 

    CREATE TABLE [dbo].[tbl_Variable_SpeedRunComID_Full] 
    (
        [VariableID] [int] NOT NULL,
        [SpeedRunComID] [varchar] (10) NOT NULL
    )
    ALTER TABLE [dbo].[tbl_Variable_SpeedRunComID_Full] ADD CONSTRAINT [PK_tbl_Variable_SpeedRunComID_Full] PRIMARY KEY CLUSTERED ([VariableID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
	--CREATE NONCLUSTERED INDEX [IDX_tbl_Variable_SpeedRunComID_Full_SpeedRunComID] ON [dbo].[tbl_Variable_SpeedRunComID_Full] ([SpeedRunComID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_VariableValue_Full
    IF OBJECT_ID('dbo.tbl_VariableValue_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_VariableValue_Full
    END 

    CREATE TABLE [dbo].[tbl_VariableValue_Full]
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1), 
        [GameID] [int] NOT NULL,   
        [VariableID] [int] NOT NULL, 
        [Value] [varchar] (100) NOT NULL, 
        [IsCustomValue] [bit] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_VariableValue_Full] ADD CONSTRAINT [PK_tbl_VariableValue_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_VariableValue_SpeedRunComID_Full
    IF OBJECT_ID('dbo.tbl_VariableValue_SpeedRunComID_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_VariableValue_SpeedRunComID_Full
    END 

    CREATE TABLE [dbo].[tbl_VariableValue_SpeedRunComID_Full] 
    (
        [VariableValueID] [int] NOT NULL,
        [SpeedRunComID] [varchar] (10) NOT NULL
    )
    ALTER TABLE [dbo].[tbl_VariableValue_SpeedRunComID_Full] ADD CONSTRAINT [PK_tbl_VariableValue_SpeedRunComID_Full] PRIMARY KEY CLUSTERED ([VariableValueID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	--CREATE NONCLUSTERED INDEX [IDX_tbl_VariableValue_SpeedRunComID_Full_SpeedRunComID] ON [dbo].[tbl_VariableValue_SpeedRunComID_Full] ([SpeedRunComID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Game_Platform_Full
    IF OBJECT_ID('dbo.tbl_Game_Platform_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_Game_Platform_Full
    END

    CREATE TABLE [dbo].[tbl_Game_Platform_Full]
    (     
        [ID] [int] NOT NULL IDENTITY(1,1), 
        [GameID] [int] NOT NULL,
        [PlatformID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Game_Platform_Full] ADD CONSTRAINT [PK_tbl_Game_Platform_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Game_Region_Full
    IF OBJECT_ID('dbo.tbl_Game_Region_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_Game_Region_Full
    END

    CREATE TABLE [dbo].[tbl_Game_Region_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [GameID] [int] NOT NULL,
        [RegionID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Game_Region_Full] ADD CONSTRAINT [PK_tbl_Game_Region_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Game_Moderator_Full
    IF OBJECT_ID('dbo.tbl_Game_Moderator_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_Game_Moderator_Full
    END

    CREATE TABLE [dbo].[tbl_Game_Moderator_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [GameID] [int] NOT NULL,
        [UserID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Game_Moderator_Full] ADD CONSTRAINT [PK_tbl_Game_Moderator_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Game_TimingMethod_Full
    IF OBJECT_ID('dbo.tbl_Game_TimingMethod_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_Game_TimingMethod_Full
    END

    CREATE TABLE [dbo].[tbl_Game_TimingMethod_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [GameID] [int] NOT NULL,
        [TimingMethodID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Game_TimingMethod_Full] ADD CONSTRAINT [PK_tbl_Game_TimingMethod_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_Game_Ruleset_Full
    IF OBJECT_ID('dbo.tbl_Game_Ruleset_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_Game_Ruleset_Full
    END

    CREATE TABLE [dbo].[tbl_Game_Ruleset_Full] 
    ( 
        [GameID] [int] NOT NULL,
        [ShowMilliseconds] [bit] NOT NULL,
        [RequiresVerification] [bit] NOT NULL,
        [RequiresVideo] [bit] NOT NULL,
        [DefaultTimingMethodID] [int] NOT NULL,
        [EmulatorsAllowed] [bit] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_Game_Ruleset_Full] ADD CONSTRAINT [PK_tbl_Game_Ruleset_Full] PRIMARY KEY CLUSTERED ([GameID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_SpeedRun_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_Full
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_Full] 
    ( 
		[ID] [int] NOT NULL IDENTITY(1,1), 
		[StatusTypeID] [int] NOT NULL,
		[GameID] [int] NOT NULL,
		[CategoryID] [int] NOT NULL,
		[LevelID] [int] NULL,
		[Rank] [int] NULL,
		[PrimaryTime] [bigint] NOT NULL,
		[RunDate] [datetime] NULL,
		[DateSubmitted] [datetime] NULL,
		[VerifyDate] [datetime] NULL,
		[ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_SpeedRun_Full_ImportedDate] DEFAULT(GETUTCDATE()),
		[ModifiedDate] [datetime] NULL,        
		[IsProcessed] [bit] NULL
    ) ON [PRIMARY]
    ALTER TABLE [dbo].[tbl_SpeedRun_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_SpeedRun_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_Full_Ordered') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_Full_Ordered
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_Full_Ordered] 
    ( 
		[ID] [int] NOT NULL IDENTITY(1,1), 
		[StatusTypeID] [int] NOT NULL,
		[GameID] [int] NOT NULL,
		[CategoryID] [int] NOT NULL,
		[LevelID] [int] NULL,
		[Rank] [int] NULL,
		[PrimaryTime] [bigint] NOT NULL,
		[RunDate] [datetime] NULL,
		[DateSubmitted] [datetime] NULL,
		[VerifyDate] [datetime] NULL,
		[ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_SpeedRun_Full_Ordered_ImportedDate] DEFAULT(GETUTCDATE()),
		[ModifiedDate] [datetime] NULL
    ) ON [PRIMARY]
    ALTER TABLE [dbo].[tbl_SpeedRun_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_Full_Ordered] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_SpeedRun_SpeedRunComID_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_SpeedRunComID_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_SpeedRunComID_Full
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_SpeedRunComID_Full] 
    (
        [SpeedRunID] [int] NOT NULL,
        [SpeedRunComID] [varchar] (10) NOT NULL
    )
    ALTER TABLE [dbo].[tbl_SpeedRun_SpeedRunComID_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_SpeedRunComID_Full] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
	--CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_SpeedRunComID_Full_SpeedRunComID] ON [dbo].[tbl_SpeedRun_SpeedRunComID_Full] ([SpeedRunComID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_SpeedRunComID_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_SpeedRunComID_Full_Ordered') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_SpeedRunComID_Full_Ordered
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_SpeedRunComID_Full_Ordered] 
    (
        [SpeedRunID] [int] NOT NULL,
        [SpeedRunComID] [varchar] (10) NOT NULL
    )
    ALTER TABLE [dbo].[tbl_SpeedRun_SpeedRunComID_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_SpeedRunComID_Full_Ordered] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
	--CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_SpeedRunComID_Full_SpeedRunComID] ON [dbo].[tbl_SpeedRun_SpeedRunComID_Full] ([SpeedRunComID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_System_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_System_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_System_Full
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_System_Full] 
    ( 
        [SpeedRunID] [int] NOT NULL,
        [PlatformID] [int] NULL,
        [RegionID] [int] NULL,
        [IsEmulated] [bit] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_System_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_System_Full] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_System_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_System_Full_Ordered') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_System_Full_Ordered
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_System_Full_Ordered] 
    ( 
        [SpeedRunID] [int] NOT NULL,
        [PlatformID] [int] NULL,
        [RegionID] [int] NULL,
        [IsEmulated] [bit] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_System_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_System_Full_Ordered] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Time_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_Time_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_Time_Full
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_Time_Full] 
    ( 
        [SpeedRunID] [int] NOT NULL,
        [PrimaryTime] [bigint] NOT NULL,
        [RealTime] [bigint] NULL,
        [RealTimeWithoutLoads] [bigint] NULL,
        [GameTime] [bigint] NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Time_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_Time_Full] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Time_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_Time_Full_Ordered') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_Time_Full_Ordered
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_Time_Full_Ordered] 
    ( 
        [SpeedRunID] [int] NOT NULL,
        [PrimaryTime] [bigint] NOT NULL,
        [RealTime] [bigint] NULL,
        [RealTimeWithoutLoads] [bigint] NULL,
        [GameTime] [bigint] NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Time_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_Time_Full_Ordered] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Link_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_Link_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_Link_Full
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_Link_Full] 
    ( 
        [SpeedRunID] [int] NOT NULL,
        [SpeedRunComUrl] [varchar](1000) NOT NULL,
        [SplitsUrl] [varchar](1000) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Link_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_Link_Full] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Link_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_Link_Full_Ordered') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_Link_Full_Ordered
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_Link_Full_Ordered] 
    ( 
        [SpeedRunID] [int] NOT NULL,
        [SpeedRunComUrl] [varchar](1000) NOT NULL,
        [SplitsUrl] [varchar](1000) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Link_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_Link_Full_Ordered] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Comment_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_Comment_Full') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_Comment_Full
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_Comment_Full] 
    ( 
        [SpeedRunID] [int] NOT NULL,
        [Comment] [varchar](MAX) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Comment_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_Comment_Full] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Comment_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_Comment_Full_Ordered') IS NOT NULL 
    BEGIN
        DROP TABLE dbo.tbl_SpeedRun_Comment_Full_Ordered
    END 

    CREATE TABLE [dbo].[tbl_SpeedRun_Comment_Full_Ordered] 
    ( 
        [SpeedRunID] [int] NOT NULL,
        [Comment] [varchar](MAX) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Comment_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_Comment_Full_Ordered] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Player_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_Player_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_Player_Full
    END
    CREATE TABLE [dbo].[tbl_SpeedRun_Player_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [SpeedRunID] [int] NOT NULL,
        [UserID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Player_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_Player_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Player_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_Player_Full_Ordered') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_Player_Full_Ordered
    END
    CREATE TABLE [dbo].[tbl_SpeedRun_Player_Full_Ordered] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [SpeedRunID] [int] NOT NULL,
        [UserID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Player_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_Player_Full_Ordered] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Guest_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_Guest_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_Guest_Full
    END
    CREATE TABLE [dbo].[tbl_SpeedRun_Guest_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [SpeedRunID] [int] NOT NULL,
        [GuestID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Guest_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_Guest_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Guest_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_Guest_Full_Ordered') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_Guest_Full_Ordered
    END
    CREATE TABLE [dbo].[tbl_SpeedRun_Guest_Full_Ordered] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [SpeedRunID] [int] NOT NULL,
        [GuestID] [int] NOT NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Guest_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_Guest_Full_Ordered] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_VariableValue_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_VariableValue_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_VariableValue_Full
    END

    CREATE TABLE [dbo].[tbl_SpeedRun_VariableValue_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [SpeedRunID] [int] NOT NULL,
        [VariableID] [int] NOT NULL,
        [VariableValueID] [int] NOT NULL 
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_VariableValue_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_VariableValue_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_VariableValue_Full_Ordered') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_VariableValue_Full_Ordered
    END

    CREATE TABLE [dbo].[tbl_SpeedRun_VariableValue_Full_Ordered] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [SpeedRunID] [int] NOT NULL,
        [VariableID] [int] NOT NULL,
        [VariableValueID] [int] NOT NULL 
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_VariableValue_Full_Ordered] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 

    --tbl_SpeedRun_Video_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_Video_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_Video_Full
    END

    CREATE TABLE [dbo].[tbl_SpeedRun_Video_Full] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [SpeedRunID] [int] NOT NULL,
        [VideoLinkUrl] [varchar] (1000) NOT NULL,
        [EmbeddedVideoLinkUrl] [varchar] (1000) NULL,
		[ThumbnailLinkUrl] [varchar](1000) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Video_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_Video_Full] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_SpeedRun_Video_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_Video_Full_Ordered') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_Video_Full_Ordered
    END

    CREATE TABLE [dbo].[tbl_SpeedRun_Video_Full_Ordered] 
    ( 
        [ID] [int] NOT NULL IDENTITY(1,1),
        [SpeedRunID] [int] NOT NULL,
        [VideoLinkUrl] [varchar] (1000) NOT NULL,
        [EmbeddedVideoLinkUrl] [varchar] (1000) NULL,
		[ThumbnailLinkUrl] [varchar](1000) NULL
    ) ON [PRIMARY] 
    ALTER TABLE [dbo].[tbl_SpeedRun_Video_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_Video_Full_Ordered] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_SpeedRun_Video_Detail_Full
    IF OBJECT_ID('dbo.tbl_SpeedRun_Video_Detail_Full') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_Video_Detail_Full
    END

    CREATE TABLE [dbo].[tbl_SpeedRun_Video_Detail_Full](
        [SpeedRunVideoID] [int] NOT NULL,
        [SpeedRunID] [int] NOT NULL,
        [ChannelID] [varchar](50) NULL,
        [ViewCount] [int] NULL
    ) ON [PRIMARY]
    ALTER TABLE [dbo].[tbl_SpeedRun_Video_Detail_Full] ADD CONSTRAINT [PK_tbl_SpeedRun_Video_Detail_Full] PRIMARY KEY CLUSTERED ([SpeedRunVideoID]) WITH (FILLFACTOR=90) ON [PRIMARY]

    --tbl_SpeedRun_Video_Detail_Full_Ordered
    IF OBJECT_ID('dbo.tbl_SpeedRun_Video_Detail_Full_Ordered') IS NOT NULL 
    BEGIN 
        DROP TABLE dbo.tbl_SpeedRun_Video_Detail_Full_Ordered
    END

    CREATE TABLE [dbo].[tbl_SpeedRun_Video_Detail_Full_Ordered](
        [SpeedRunVideoID] [int] NOT NULL,
        [SpeedRunID] [int] NOT NULL,
        [ChannelID] [varchar](50) NULL,
        [ViewCount] [int] NULL
    ) ON [PRIMARY]
    ALTER TABLE [dbo].[tbl_SpeedRun_Video_Detail_Full_Ordered] ADD CONSTRAINT [PK_tbl_SpeedRun_Video_Detail_Full_Ordered] PRIMARY KEY CLUSTERED ([SpeedRunVideoID]) WITH (FILLFACTOR=90) ON [PRIMARY]


	-- CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Link_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Link_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	-- CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_System_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_System_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	-- CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Time_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Time_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	-- CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Comment_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Comment_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_VariableValue_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_VariableValue_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Player_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Player_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Guest_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Guest_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Video_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Video_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]
	CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Video_Detail_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Video_Detail_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]	
    --CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_SpeedRunComID_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_SpeedRunComID_Full] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]

END



GO


