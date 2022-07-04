USE [SpeedRunApp]

/*********************************************/
--create/alter tables
/*********************************************/
--tbl_UserRole
IF OBJECT_ID('dbo.tbl_UserRole') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_User_tbl_UserRole') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_User] DROP CONSTRAINT [FK_tbl_User_tbl_UserRole]
	END

    DROP TABLE dbo.tbl_UserRole
END 

CREATE TABLE [dbo].[tbl_UserRole]
( 
    [ID] [int] NOT NULL,
    [Name] [varchar] (25) NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_UserRole] ADD CONSTRAINT [PK_tbl_UserRole] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_User
IF OBJECT_ID('dbo.tbl_User') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_Player_tbl_User') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun_Player] DROP CONSTRAINT [FK_tbl_SpeedRun_Player_tbl_User]
	END
	
	IF OBJECT_ID('dbo.FK_tbl_Game_Moderator_tbl_User') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Game_Moderator] DROP CONSTRAINT [FK_tbl_Game_Moderator_tbl_User]
	END

    DROP TABLE dbo.tbl_User
END

CREATE TABLE [dbo].[tbl_User]
( 
    [ID] [int] NOT NULL IDENTITY(1,1), 
    [Name] [nvarchar] (100) NOT NULL,
	[UserRoleID] [int] NOT NULL,
    [SignUpDate] [datetime] NULL,  
    [ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_User_ImportedDate] DEFAULT(GETUTCDATE()),
	[ModifiedDate] [datetime] NULL,
	[Abbr] [varchar](100) NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_User] ADD CONSTRAINT [PK_tbl_User] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_User] ADD CONSTRAINT [FK_tbl_User_tbl_UserRole] FOREIGN KEY ([UserRoleID]) REFERENCES [dbo].[tbl_UserRole] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_User_Name_PlusInclude] ON [dbo].[tbl_User] ([Name]) INCLUDE ([Abbr]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

----tbl_User_SpeedRunComID
IF OBJECT_ID('dbo.tbl_User_SpeedRunComID') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_User_SpeedRunComID
END 

CREATE TABLE [dbo].[tbl_User_SpeedRunComID] 
(
	[UserID] [int] NOT NULL,
    [SpeedRunComID] [varchar] (10) NOT NULL
)
ALTER TABLE [dbo].[tbl_User_SpeedRunComID] ADD CONSTRAINT [PK_tbl_User_SpeedRunComID] PRIMARY KEY CLUSTERED ([UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_User_Location
IF OBJECT_ID('dbo.tbl_User_Location') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_User_Location
END 

CREATE TABLE [dbo].[tbl_User_Location]
( 
    [UserID] [int] NOT NULL,
    [Location] [varchar] (100) NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_User_Location] ADD CONSTRAINT [PK_tbl_User_Location] PRIMARY KEY CLUSTERED ([UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_User_Link
IF OBJECT_ID('dbo.tbl_User_Link') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_User_Link
END 

CREATE TABLE [dbo].[tbl_User_Link] 
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
GO 
ALTER TABLE [dbo].[tbl_User_Link] ADD CONSTRAINT [PK_tbl_User_Link] PRIMARY KEY CLUSTERED ([UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_Guest
IF OBJECT_ID('dbo.tbl_Guest') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_Guest_tbl_Guest') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun_Guest] DROP CONSTRAINT [FK_tbl_SpeedRun_Guest_tbl_Guest]
	END

    DROP TABLE dbo.tbl_Guest
END 

CREATE TABLE [dbo].[tbl_Guest]
( 
    [ID] [int] NOT NULL IDENTITY(1,1), 
    [Name] [nvarchar] (100) NOT NULL,
    [ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_Guest_ImportedDate] DEFAULT(GETUTCDATE()),
	[Abbr] [varchar](100) NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Guest] ADD CONSTRAINT [PK_tbl_Guest] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_Guest_Name_PlusInclude] ON [dbo].[tbl_Guest] ([Name]) INCLUDE (Abbr) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Game 
IF OBJECT_ID('dbo.tbl_Game') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_Level_tbl_Game') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Level] DROP CONSTRAINT [FK_tbl_Level_tbl_Game]
	END

	IF OBJECT_ID('dbo.FK_tbl_Category_tbl_Game') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Category] DROP CONSTRAINT [FK_tbl_Category_tbl_Game]
	END

	IF OBJECT_ID('dbo.FK_tbl_Variable_tbl_Game') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Variable] DROP CONSTRAINT [FK_tbl_Variable_tbl_Game]
	END

	IF OBJECT_ID('dbo.FK_tbl_VariableValue_tbl_Game') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_VariableValue] DROP CONSTRAINT [FK_tbl_VariableValue_tbl_Game]
	END

	IF OBJECT_ID('dbo.FK_tbl_Game_Platform_tbl_Game') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Game_Platform] DROP CONSTRAINT [FK_tbl_Game_Platform_tbl_Game]
	END

	IF OBJECT_ID('dbo.FK_tbl_Game_Region_tbl_Game') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Game_Region] DROP CONSTRAINT [FK_tbl_Game_Region_tbl_Game]
	END

	IF OBJECT_ID('dbo.FK_tbl_Game_Moderator_tbl_Game') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Game_Moderator] DROP CONSTRAINT [FK_tbl_Game_Moderator_tbl_Game]
	END

	IF OBJECT_ID('dbo.FK_tbl_Game_TimingMethod_tbl_Game') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Game_TimingMethod] DROP CONSTRAINT [FK_tbl_Game_TimingMethod_tbl_Game]
	END

	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_tbl_Game') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun] DROP CONSTRAINT [FK_tbl_SpeedRun_tbl_Game]
	END

    DROP TABLE dbo.tbl_Game
END 

CREATE TABLE [dbo].[tbl_Game] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1), 
    [Name] [varchar] (100) NOT NULL,
    [IsRomHack] [bit] NOT NULL,
    [YearOfRelease] [int] NULL,
    [CreatedDate] [datetime] NULL,  
    [ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_Game_ImportedDate] DEFAULT(GETUTCDATE()),
    [ModifiedDate] [datetime] NULL,
	[Abbr] [varchar](100) NULL,
	[IsChanged] [bit] NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Game] ADD CONSTRAINT [PK_tbl_Game] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_Game_Name_PlusInclude] ON [dbo].[tbl_Game] ([Name]) INCLUDE (Abbr) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Game_SpeedRunComID
IF OBJECT_ID('dbo.tbl_Game_SpeedRunComID') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Game_SpeedRunComID
END 

CREATE TABLE [dbo].[tbl_Game_SpeedRunComID] 
(
	[GameID] [int] NOT NULL,
    [SpeedRunComID] [varchar] (10) NOT NULL
)
ALTER TABLE [dbo].[tbl_Game_SpeedRunComID] ADD CONSTRAINT [PK_tbl_Game_SpeedRunComID] PRIMARY KEY CLUSTERED ([GameID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Game_Link
IF OBJECT_ID('dbo.tbl_Game_Link') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Game_Link
END 

CREATE TABLE [dbo].[tbl_Game_Link] 
(
    [GameID] [int] NOT NULL,
    [SpeedRunComUrl] [varchar] (1000) NOT NULL,
    [CoverImageUrl] [varchar] (1000) NULL
)
ALTER TABLE [dbo].[tbl_Game_Link] ADD CONSTRAINT [PK_tbl_Game_Link] PRIMARY KEY CLUSTERED ([GameID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Level
IF OBJECT_ID('dbo.tbl_Level') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_Variable_tbl_Level') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Variable] DROP CONSTRAINT [FK_tbl_Variable_tbl_Level]
	END

	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_tbl_Level') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun] DROP CONSTRAINT [FK_tbl_SpeedRun_tbl_Level]
	END

    DROP TABLE dbo.tbl_Level 
END 

CREATE TABLE [dbo].[tbl_Level] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1), 
    [Name] [varchar] (100) NOT NULL,
    [GameID] [int] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Level] ADD CONSTRAINT [PK_tbl_Level] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_Level] ADD CONSTRAINT [FK_tbl_Level_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_Level_GameID_PlusInclude] ON [dbo].[tbl_Level] ([GameID]) INCLUDE ([Name]) WITH (FILLFACTOR=90) ON [PRIMARY]	
GO

--tbl_Level_SpeedRunComID
IF OBJECT_ID('dbo.tbl_Level_SpeedRunComID') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Level_SpeedRunComID
END 

CREATE TABLE [dbo].[tbl_Level_SpeedRunComID] 
(
	[LevelID] [int] NOT NULL,
    [SpeedRunComID] [varchar] (10) NOT NULL
)
ALTER TABLE [dbo].[tbl_Level_SpeedRunComID] ADD CONSTRAINT [PK_tbl_Level_SpeedRunComID] PRIMARY KEY CLUSTERED ([LevelID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Level_Rule
IF OBJECT_ID('dbo.tbl_Level_Rule') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Level_Rule 
END 

CREATE TABLE [dbo].[tbl_Level_Rule] 
( 
    [LevelID] [int] NOT NULL, 
    [Rules] [varchar] (MAX) NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Level_Rule] ADD CONSTRAINT [PK_tbl_Level_Rule] PRIMARY KEY CLUSTERED ([LevelID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_CategoryType
IF OBJECT_ID('dbo.tbl_CategoryType') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_Category_tbl_CategoryType') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Category] DROP CONSTRAINT [FK_tbl_Category_tbl_CategoryType]
	END

    DROP TABLE dbo.tbl_CategoryType
END 

CREATE TABLE [dbo].[tbl_CategoryType] 
( 
    [ID] [int] NOT NULL,
    [Name] [varchar] (25) NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_CategoryType] ADD CONSTRAINT [PK_tbl_CategoryType] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

IF OBJECT_ID('dbo.tbl_Category') IS NOT NULL 
BEGIN
--[FK_tbl_Variable_tbl_Category]
	IF OBJECT_ID('dbo.FK_tbl_Variable_tbl_Category') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Variable] DROP CONSTRAINT [FK_tbl_Variable_tbl_Category]
	END

	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_tbl_Category') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun] DROP CONSTRAINT [FK_tbl_SpeedRun_tbl_Category]
	END

    DROP TABLE dbo.tbl_Category
END 

CREATE TABLE [dbo].[tbl_Category] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1), 
    [Name] [varchar] (100) NOT NULL,
    [GameID] [int] NOT NULL,
    [CategoryTypeID] [int] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Category] ADD CONSTRAINT [PK_tbl_Category] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_Category] ADD CONSTRAINT [FK_tbl_Category_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
GO
ALTER TABLE [dbo].[tbl_Category] ADD CONSTRAINT [FK_tbl_Category_tbl_CategoryType] FOREIGN KEY ([CategoryTypeID]) REFERENCES [dbo].[tbl_CategoryType] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_Category_GameID_CategoryTypeID_PlusInclude] ON [dbo].[tbl_Category] ([GameID],[CategoryTypeID]) INCLUDE ([Name]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_Category_SpeedRunComID
IF OBJECT_ID('dbo.tbl_Category_SpeedRunComID') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Category_SpeedRunComID
END 

CREATE TABLE [dbo].[tbl_Category_SpeedRunComID] 
(
	[CategoryID] [int] NOT NULL,
    [SpeedRunComID] [varchar] (10) NOT NULL
)
ALTER TABLE [dbo].[tbl_Category_SpeedRunComID] ADD CONSTRAINT [PK_tbl_Category_SpeedRunComID] PRIMARY KEY CLUSTERED ([CategoryID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Category_Rule
IF OBJECT_ID('dbo.tbl_Category_Rule') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Category_Rule
END 

CREATE TABLE [dbo].[tbl_Category_Rule] 
( 
    [CategoryID] [int] NOT NULL, 
    [Rules] [varchar] (MAX) NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Category_Rule] ADD CONSTRAINT [PK_tbl_Category_Rule] PRIMARY KEY CLUSTERED ([CategoryID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_VariableScopeType
IF OBJECT_ID('dbo.tbl_VariableScopeType') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_Variable_tbl_VariableScopeType') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Variable] DROP CONSTRAINT [FK_tbl_Variable_tbl_VariableScopeType]
	END

    DROP TABLE dbo.tbl_VariableScopeType
END 

CREATE TABLE [dbo].[tbl_VariableScopeType] 
( 
    [ID] [int] NOT NULL,
    [Name] [varchar] (25) NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_VariableScopeType] ADD CONSTRAINT [PK_tbl_VariableScopeType] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Variable
IF OBJECT_ID('dbo.tbl_Variable') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Variable
END 

CREATE TABLE [dbo].[tbl_Variable]
( 
    [ID] [int] NOT NULL IDENTITY(1,1), 
    [Name] [varchar] (100) NOT NULL,
    [GameID] [int] NOT NULL,
    [VariableScopeTypeID] [int] NOT NULL,
    [CategoryID] [int] NULL,
    [LevelID] [int] NULL,
    [IsSubCategory] [bit] NOT NULL   
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Variable] ADD CONSTRAINT [PK_tbl_Variable] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_Variable] ADD CONSTRAINT [FK_tbl_Variable_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
GO
ALTER TABLE [dbo].[tbl_Variable] ADD CONSTRAINT [FK_tbl_Variable_tbl_Level] FOREIGN KEY ([LevelID]) REFERENCES [dbo].[tbl_Level] ([ID])
GO
ALTER TABLE [dbo].[tbl_Variable] ADD CONSTRAINT [FK_tbl_Variable_tbl_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[tbl_Category] ([ID])
GO
ALTER TABLE [dbo].[tbl_Variable] ADD CONSTRAINT [FK_tbl_Variable_tbl_VariableScopeType] FOREIGN KEY ([VariableScopeTypeID]) REFERENCES [dbo].[tbl_VariableScopeType] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_Variable_GameID] ON [dbo].[tbl_Variable] ([GameID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_Variable_IsSubCategory] ON [dbo].[tbl_Variable] ([IsSubCategory]) WITH (FILLFACTOR=90) ON [PRIMARY]	
GO

--tbl_Variable
IF OBJECT_ID('dbo.tbl_Variable_SpeedRunComID') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Variable_SpeedRunComID
END 

CREATE TABLE [dbo].[tbl_Variable_SpeedRunComID] 
(
	[VariableID] [int] NOT NULL,
    [SpeedRunComID] [varchar] (10) NOT NULL
)
ALTER TABLE [dbo].[tbl_Variable_SpeedRunComID] ADD CONSTRAINT [PK_tbl_Variable_SpeedRunComID] PRIMARY KEY CLUSTERED ([VariableID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_VariableValue
IF OBJECT_ID('dbo.tbl_VariableValue') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_VariableValue
END 

CREATE TABLE [dbo].[tbl_VariableValue]
( 
    [ID] [int] NOT NULL IDENTITY(1,1), 
    [GameID] [int] NOT NULL,   
    [VariableID] [int] NOT NULL, 
    [Value] [varchar] (100) NOT NULL, 
    [IsCustomValue] [bit] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_VariableValue] ADD CONSTRAINT [PK_tbl_VariableValue] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_VariableValue] ADD CONSTRAINT [FK_tbl_VariableValue_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_VariableValue_GameID_PlusInclude] ON [dbo].[tbl_VariableValue] ([GameID]) INCLUDE ([VariableID],[Value]) WITH (FILLFACTOR=90) ON [PRIMARY]	
GO

--tbl_Variable
IF OBJECT_ID('dbo.tbl_VariableValue_SpeedRunComID') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_VariableValue_SpeedRunComID
END 

CREATE TABLE [dbo].[tbl_VariableValue_SpeedRunComID] 
(
	[VariableValueID] [int] NOT NULL,
    [SpeedRunComID] [varchar] (10) NOT NULL
)
ALTER TABLE [dbo].[tbl_VariableValue_SpeedRunComID] ADD CONSTRAINT [PK_tbl_VariableValue_SpeedRunComID] PRIMARY KEY CLUSTERED ([VariableValueID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Platform
IF OBJECT_ID('dbo.tbl_Platform') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_Game_Platform_tbl_Platform') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Game_Platform] DROP CONSTRAINT [FK_tbl_Game_Platform_tbl_Platform]
	END

	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_System_tbl_Platform') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun_System] DROP CONSTRAINT [FK_tbl_SpeedRun_System_tbl_Platform]
	END

    DROP TABLE dbo.tbl_Platform
END 

CREATE TABLE [dbo].[tbl_Platform] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1), 
    [Name] [varchar] (50) NOT NULL,
    [YearOfRelease] [int] NULL,
    [ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_Platform_ImportedDate] DEFAULT(GETUTCDATE())
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Platform] ADD CONSTRAINT [PK_tbl_Platform] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_Platform_SpeedRunComID
IF OBJECT_ID('dbo.tbl_Platform_SpeedRunComID') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Platform_SpeedRunComID
END 

CREATE TABLE [dbo].[tbl_Platform_SpeedRunComID] 
(
	[PlatformID] [int] NOT NULL,
    [SpeedRunComID] [varchar] (10) NOT NULL
)
ALTER TABLE [dbo].[tbl_Platform_SpeedRunComID] ADD CONSTRAINT [PK_tbl_Platform_SpeedRunComID] PRIMARY KEY CLUSTERED ([PlatformID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Game_Platform
IF OBJECT_ID('dbo.tbl_Game_Platform') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_Game_Platform
END

CREATE TABLE [dbo].[tbl_Game_Platform]
(     
    [ID] [int] NOT NULL IDENTITY(1,1), 
    [GameID] [int] NOT NULL,
    [PlatformID] [int] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Game_Platform] ADD CONSTRAINT [PK_tbl_Game_Platform] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_Game_Platform] ADD CONSTRAINT [FK_tbl_Game_Platform_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
GO
ALTER TABLE [dbo].[tbl_Game_Platform] ADD CONSTRAINT [FK_tbl_Game_Platform_tbl_Platform] FOREIGN KEY ([PlatformID]) REFERENCES [dbo].[tbl_Platform] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_Game_Platform_GameID_PlatformID] ON [dbo].[tbl_Game_Platform] ([GameID],[PlatformID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_Region
IF OBJECT_ID('dbo.tbl_Region') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_Game_Region_tbl_Region') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Game_Region] DROP CONSTRAINT [FK_tbl_Game_Region_tbl_Region]
	END

	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_System_tbl_Region') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun_System] DROP CONSTRAINT [FK_tbl_SpeedRun_System_tbl_Region]
	END

    DROP TABLE dbo.tbl_Region
END 

CREATE TABLE [dbo].[tbl_Region]
( 
    [ID] [int] NOT NULL,
    [Name] [varchar] (25) NOT NULL, 
    [Abbreviation] [varchar] (10) NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Region] ADD CONSTRAINT [PK_tbl_Region] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Region_SpeedRunComID
IF OBJECT_ID('dbo.tbl_Region_SpeedRunComID') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_Region_SpeedRunComID
END 

CREATE TABLE [dbo].[tbl_Region_SpeedRunComID] 
(
	[RegionID] [int] NOT NULL,
    [SpeedRunComID] [varchar] (10) NOT NULL
)
ALTER TABLE [dbo].[tbl_Region_SpeedRunComID] ADD CONSTRAINT [PK_tbl_Region_SpeedRunComID] PRIMARY KEY CLUSTERED ([RegionID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Game_Region
IF OBJECT_ID('dbo.tbl_Game_Region') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_Game_Region
END

CREATE TABLE [dbo].[tbl_Game_Region] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1),
    [GameID] [int] NOT NULL,
    [RegionID] [int] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Game_Region] ADD CONSTRAINT [PK_tbl_Game_Region] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_Game_Region] ADD CONSTRAINT [FK_tbl_Game_Region_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
GO
ALTER TABLE [dbo].[tbl_Game_Region] ADD CONSTRAINT [FK_tbl_Game_Region_tbl_Region] FOREIGN KEY ([RegionID]) REFERENCES [dbo].[tbl_Region] ([ID])
GO

--tbl_Game_Moderator
IF OBJECT_ID('dbo.tbl_Game_Moderator') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_Game_Moderator
END

CREATE TABLE [dbo].[tbl_Game_Moderator] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1),
    [GameID] [int] NOT NULL,
    [UserID] [int] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Game_Moderator] ADD CONSTRAINT [PK_tbl_Game_Moderator] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_Game_Moderator] ADD CONSTRAINT [FK_tbl_Game_Moderator_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
GO
ALTER TABLE [dbo].[tbl_Game_Moderator] ADD CONSTRAINT [FK_tbl_Game_Moderator_tbl_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[tbl_User] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_Game_Moderator_GameID_UserID] ON [dbo].[tbl_Game_Moderator] ([GameID],[UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_TimingMethod
IF OBJECT_ID('dbo.tbl_TimingMethod') IS NOT NULL 
BEGIN 
	IF OBJECT_ID('dbo.FK_tbl_Game_TimingMethod_tbl_TimingMethod') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_Game_TimingMethod] DROP CONSTRAINT [FK_tbl_Game_TimingMethod_tbl_TimingMethod]
	END

    DROP TABLE dbo.tbl_TimingMethod
END

CREATE TABLE [dbo].[tbl_TimingMethod] 
( 
    [ID] [int] NOT NULL,
    [Name] [varchar] (50) NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_TimingMethod] ADD CONSTRAINT [PK_tbl_TimingMethod] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_Game_TimingMethod
IF OBJECT_ID('dbo.tbl_Game_TimingMethod') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_Game_TimingMethod
END

CREATE TABLE [dbo].[tbl_Game_TimingMethod] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1),
    [GameID] [int] NOT NULL,
    [TimingMethodID] [int] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Game_TimingMethod] ADD CONSTRAINT [PK_tbl_Game_TimingMethod] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_Game_TimingMethod] ADD CONSTRAINT [FK_tbl_Game_TimingMethod_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
GO
ALTER TABLE [dbo].[tbl_Game_TimingMethod] ADD CONSTRAINT [FK_tbl_Game_TimingMethod_tbl_TimingMethod] FOREIGN KEY ([TimingMethodID]) REFERENCES [dbo].[tbl_TimingMethod] ([ID])
GO

--tbl_Game_Ruleset
IF OBJECT_ID('dbo.tbl_Game_Ruleset') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_Game_Ruleset
END

CREATE TABLE [dbo].[tbl_Game_Ruleset] 
( 
    [GameID] [int] NOT NULL,
    [ShowMilliseconds] [bit] NOT NULL,
    [RequiresVerification] [bit] NOT NULL,
    [RequiresVideo] [bit] NOT NULL,
    [DefaultTimingMethodID] [int] NOT NULL,
    [EmulatorsAllowed] [bit] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Game_Ruleset] ADD CONSTRAINT [PK_tbl_Game_Ruleset] PRIMARY KEY CLUSTERED ([GameID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO


--tbl_Setting
IF OBJECT_ID('dbo.tbl_Setting') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_Setting
END

CREATE TABLE [dbo].[tbl_Setting] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1),
    [Name] [varchar] (50) NOT NULL,
    [Str] [varchar] (500) NULL,
    [Num] [int] NULL,
    [Dte] [datetime] NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_Setting] ADD CONSTRAINT [PK_tbl_Setting] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO


--tbl_SpeedRunStatusType
IF OBJECT_ID('dbo.tbl_RunStatusType') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_tbl_RunStatusType') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun] DROP CONSTRAINT [FK_tbl_SpeedRun_tbl_RunStatusType]
	END

    DROP TABLE dbo.tbl_RunStatusType
END 

CREATE TABLE [dbo].[tbl_RunStatusType] 
( 
    [ID] [int] NOT NULL,
    [Name] [varchar] (25) NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_RunStatusType] ADD CONSTRAINT [PK_tbl_RunStatusType] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO


--tbl_SpeedRun
IF OBJECT_ID('dbo.tbl_SpeedRun') IS NOT NULL 
BEGIN
	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_Player_tbl_SpeedRun') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun_Player] DROP CONSTRAINT [FK_tbl_SpeedRun_Player_tbl_SpeedRun]
	END

	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_Guest_tbl_SpeedRun') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun_Guest] DROP CONSTRAINT [FK_tbl_SpeedRun_Guest_tbl_SpeedRun]
	END

	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_VariableValue_tbl_SpeedRun') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue] DROP CONSTRAINT [FK_tbl_SpeedRun_VariableValue_tbl_SpeedRun]
	END

	IF OBJECT_ID('dbo.FK_tbl_SpeedRun_Video_tbl_SpeedRun') IS NOT NULL 
	BEGIN
		ALTER TABLE [dbo].[tbl_SpeedRun_Video] DROP CONSTRAINT [FK_tbl_SpeedRun_Video_tbl_SpeedRun]
	END

    DROP TABLE dbo.tbl_SpeedRun
END 

CREATE TABLE [dbo].[tbl_SpeedRun] 
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
	[ImportedDate] [datetime] NOT NULL CONSTRAINT [DF_tbl_SpeedRun_ImportedDate] DEFAULT(GETUTCDATE()),
	[ModifiedDate] [datetime] NULL
) ON [PRIMARY]
GO 
ALTER TABLE [dbo].[tbl_SpeedRun] ADD CONSTRAINT [PK_tbl_SpeedRun] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_SpeedRun] ADD CONSTRAINT [FK_tbl_SpeedRun_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
GO
ALTER TABLE [dbo].[tbl_SpeedRun] ADD CONSTRAINT [FK_tbl_SpeedRun_tbl_RunStatusType] FOREIGN KEY ([StatusTypeID]) REFERENCES [dbo].[tbl_RunStatusType] ([ID])		
GO
ALTER TABLE [dbo].[tbl_SpeedRun] ADD CONSTRAINT [FK_tbl_SpeedRun_tbl_Level] FOREIGN KEY ([LevelID]) REFERENCES [dbo].[tbl_Level] ([ID])
GO
ALTER TABLE [dbo].[tbl_SpeedRun] ADD CONSTRAINT [FK_tbl_SpeedRun_tbl_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[tbl_Category] ([ID])
GO
ALTER TABLE [dbo].[tbl_SpeedRun_Guest] ADD CONSTRAINT [FK_tbl_SpeedRun_Guest_tbl_Guest] FOREIGN KEY ([GuestID]) REFERENCES [dbo].[tbl_Guest] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_GameID_CategoryID_LevelID_Rank_PlusInclude] ON [dbo].[tbl_SpeedRun] ([GameID],[CategoryID],[LevelID],[Rank]) INCLUDE ([PrimaryTime],[DateSubmitted],[VerifyDate]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also GetPersonalBestsByUserID, GetSpeedRunsByUserID, vw_SpeedRunSummary	
GO

--tbl_SpeedRun_SpeedRunComID
IF OBJECT_ID('dbo.tbl_SpeedRun_SpeedRunComID') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_SpeedRun_SpeedRunComID
END 

CREATE TABLE [dbo].[tbl_SpeedRun_SpeedRunComID] 
(
	[SpeedRunID] [int] NOT NULL,
    [SpeedRunComID] [varchar] (10) NOT NULL
)
ALTER TABLE [dbo].[tbl_SpeedRun_SpeedRunComID] ADD CONSTRAINT [PK_tbl_SpeedRun_SpeedRunComID] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_SpeedRun_System
IF OBJECT_ID('dbo.tbl_SpeedRun_System') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_SpeedRun_System
END 

CREATE TABLE [dbo].[tbl_SpeedRun_System] 
( 
    [SpeedRunID] [int] NOT NULL,
	[PlatformID] [int] NULL,
	[RegionID] [int] NULL,
 	[IsEmulated] [bit] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_SpeedRun_System] ADD CONSTRAINT [PK_tbl_SpeedRun_System] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_SpeedRun_System] ADD CONSTRAINT [FK_tbl_SpeedRun_System_tbl_Platform] FOREIGN KEY ([PlatformID]) REFERENCES [dbo].[tbl_Platform] ([ID])
GO
ALTER TABLE [dbo].[tbl_SpeedRun_System] ADD CONSTRAINT [FK_tbl_SpeedRun_System_tbl_Region] FOREIGN KEY ([RegionID]) REFERENCES [dbo].[tbl_Region] ([ID])
GO

--tbl_SpeedRun_Times
IF OBJECT_ID('dbo.tbl_SpeedRun_Time') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_SpeedRun_Time
END 

CREATE TABLE [dbo].[tbl_SpeedRun_Time] 
( 
    [SpeedRunID] [int] NOT NULL,
	[PrimaryTime] [bigint] NOT NULL,
	[RealTime] [bigint] NULL,
	[RealTimeWithoutLoads] [bigint] NULL,
	[GameTime] [bigint] NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_SpeedRun_Time] ADD CONSTRAINT [PK_tbl_SpeedRun_Time] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_SpeedRun_Link
IF OBJECT_ID('dbo.tbl_SpeedRun_Link') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_SpeedRun_Link
END 

CREATE TABLE [dbo].[tbl_SpeedRun_Link] 
( 
    [SpeedRunID] [int] NOT NULL,
	[SpeedRunComUrl] [varchar](1000) NOT NULL,
	[SplitsUrl] [varchar](1000) NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_SpeedRun_Link] ADD CONSTRAINT [PK_tbl_SpeedRun_Link] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_SpeedRun_Comment
IF OBJECT_ID('dbo.tbl_SpeedRun_Comment') IS NOT NULL 
BEGIN
    DROP TABLE dbo.tbl_SpeedRun_Comment
END 

CREATE TABLE [dbo].[tbl_SpeedRun_Comment] 
( 
    [SpeedRunID] [int] NOT NULL,
	[Comment] [varchar](MAX) NULL
) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_SpeedRun_Comment] ADD CONSTRAINT [PK_tbl_SpeedRun_Comment] PRIMARY KEY CLUSTERED ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_SpeedRun_Player
IF OBJECT_ID('dbo.tbl_SpeedRun_Player') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_SpeedRun_Player
END
CREATE TABLE [dbo].[tbl_SpeedRun_Player] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1),
    [SpeedRunID] [int] NOT NULL,
    [UserID] [int] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_SpeedRun_Player] ADD CONSTRAINT [PK_tbl_SpeedRun_Player] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_SpeedRun_Player] ADD CONSTRAINT [FK_tbl_SpeedRun_Player_tbl_SpeedRun] FOREIGN KEY ([SpeedRunID]) REFERENCES [dbo].[tbl_SpeedRun] ([ID])
GO
ALTER TABLE [dbo].[tbl_SpeedRun_Player] ADD CONSTRAINT [FK_tbl_SpeedRun_Player_tbl_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[tbl_User] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Player_SpeedRunID_UserID] ON [dbo].[tbl_SpeedRun_Player] ([SpeedRunID],[UserID]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also vw_SpeedRunGridTabUser, vw_User
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Player_UserID] ON [dbo].[tbl_SpeedRun_Player] ([UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_SpeedRun_Guest
IF OBJECT_ID('dbo.tbl_SpeedRun_Guest') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_SpeedRun_Guest
END
CREATE TABLE [dbo].[tbl_SpeedRun_Guest] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1),
    [SpeedRunID] [int] NOT NULL,
    [GuestID] [int] NOT NULL
) ON [PRIMARY] 
ALTER TABLE [dbo].[tbl_SpeedRun_Guest] ADD CONSTRAINT [PK_tbl_SpeedRun_Guest] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_SpeedRun_Guest] ADD CONSTRAINT [FK_tbl_SpeedRun_Guest_tbl_SpeedRun] FOREIGN KEY ([SpeedRunID]) REFERENCES [dbo].[tbl_SpeedRun] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Guest_SpeedRunID_GuestID] ON [dbo].[tbl_SpeedRun_Guest] ([SpeedRunID],[GuestID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO

--tbl_SpeedRun_VariableValue
IF OBJECT_ID('dbo.tbl_SpeedRun_VariableValue') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_SpeedRun_VariableValue
END

CREATE TABLE [dbo].[tbl_SpeedRun_VariableValue] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1),
    [SpeedRunID] [int] NOT NULL,
    [VariableID] [int] NOT NULL,
    [VariableValueID] [int] NOT NULL 
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue] ADD CONSTRAINT [PK_tbl_SpeedRun_VariableValue] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue] ADD CONSTRAINT [FK_tbl_SpeedRun_VariableValue_tbl_SpeedRun] FOREIGN KEY ([SpeedRunID]) REFERENCES [dbo].[tbl_SpeedRun] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_VariableValue_SpeedRunID_VariableValueID_PlusInclude] ON [dbo].[tbl_SpeedRun_VariableValue] ([SpeedRunID],[VariableValueID]) INCLUDE ([VariableID]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also vw_SpeedRunSummary, ImportUpdateSpeedRunRanks    
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_VariableValue_SpeedRunID_VariableID_VariableValueID] ON [dbo].[tbl_SpeedRun_VariableValue] ([SpeedRunID],[VariableID],[VariableValueID]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also vw_SpeedRunSummary, ImportUpdateSpeedRunRanks    
GO

--tbl_SpeedRun_Video
IF OBJECT_ID('dbo.tbl_SpeedRun_Video') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_SpeedRun_Video
END

CREATE TABLE [dbo].[tbl_SpeedRun_Video] 
( 
    [ID] [int] NOT NULL IDENTITY(1,1),
    [SpeedRunID] [int] NOT NULL,
    [VideoLinkUrl] [varchar] (1000) NOT NULL,
    [EmbeddedVideoLinkUrl] [varchar] (1000) NULL,
	[ThumbnailLinkUrl] [varchar](1000) NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_SpeedRun_Video] ADD CONSTRAINT [PK_tbl_SpeedRun_Video] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
ALTER TABLE [dbo].[tbl_SpeedRun_Video] ADD CONSTRAINT [FK_tbl_SpeedRun_Video_tbl_SpeedRun] FOREIGN KEY ([SpeedRunID]) REFERENCES [dbo].[tbl_SpeedRun] ([ID])
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Video_SpeedRunID_EmbeddedVideoLinkUrl_PlusInclude] ON [dbo].[tbl_SpeedRun_Video] ([SpeedRunID],[EmbeddedVideoLinkUrl]) INCLUDE ([ThumbnailLinkUrl]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also Import Game
GO

--tbl_SpeedRun_Video_Detail
IF OBJECT_ID('dbo.tbl_SpeedRun_Video_Detail') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_SpeedRun_Video_Detail
END

CREATE TABLE [dbo].[tbl_SpeedRun_Video_Detail](
	[SpeedRunVideoID] [int] NOT NULL,
	[SpeedRunID] [int] NOT NULL,
	[ChannelID] [varchar](50) NULL,
	[ViewCount] [int] NULL,
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_SpeedRun_Video_Detail] ADD CONSTRAINT [PK_tbl_SpeedRun_Video_Detail] PRIMARY KEY CLUSTERED ([SpeedRunVideoID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
GO
CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Video_Detail_SpeedRunID] ON [dbo].[tbl_SpeedRun_Video_Detail] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]   
GO

--tbl_SpeedRunListCategory
IF OBJECT_ID('dbo.tbl_SpeedRunListCategory') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_SpeedRunListCategory
END

CREATE TABLE [dbo].[tbl_SpeedRunListCategory](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[DisplayName] [varchar](50) NULL,
	[Description] [varchar](250) NULL,
	[IsDefault] [bit] NOT NULL,
	[DefaultSortOrder] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_SpeedRunListCategory] ADD CONSTRAINT [PK_tbl_SpeedRunListCategory] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_UserAccount
IF OBJECT_ID('dbo.tbl_UserAccount') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_UserAccount
END

CREATE TABLE [dbo].[tbl_UserAccount](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](255) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[PromptToChange] [bit] NOT NULL,
	[Locked] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[Deleted] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_UserAccount] ADD CONSTRAINT [PK_tbl_UserAccount] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_UserAccount_Setting
IF OBJECT_ID('dbo.tbl_UserAccount_Setting') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_UserAccount_Setting
END

CREATE TABLE [dbo].[tbl_UserAccount_Setting](
	[UserAccountID] [int] NOT NULL,
	[IsDarkTheme] [bit] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_UserAccount_Setting] ADD CONSTRAINT [PK_tbl_UserAccount_Setting] PRIMARY KEY CLUSTERED ([UserAccountID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

--tbl_UserAccount_SpeedRunListCategory
IF OBJECT_ID('dbo.tbl_UserAccount_SpeedRunListCategory') IS NOT NULL 
BEGIN 
    DROP TABLE dbo.tbl_UserAccount_SpeedRunListCategory
END

CREATE TABLE [dbo].[tbl_UserAccount_SpeedRunListCategory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserAccountID] [int] NOT NULL,
	[SpeedRunListCategoryID] [int] NOT NULL
) ON [PRIMARY] 
GO 
ALTER TABLE [dbo].[tbl_UserAccount_SpeedRunListCategory] ADD CONSTRAINT [PK_tbl_UserAccount_SpeedRunListCategory] PRIMARY KEY CLUSTERED ([ID]) WITH (FILLFACTOR=90) ON [PRIMARY]
GO

/*********************************************/
--create/alter views
/*********************************************/
--vw_Game
IF OBJECT_ID('dbo.vw_Game') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_Game
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Game]
AS

    SELECT g.ID, g.Name, g.Abbr, gl.CoverImagePath AS CoverImageUrl, g.YearOfRelease, CategoryTypes.[Value] AS CategoryTypes, Categories.[Value] AS Categories, Levels.[Value] AS Levels,
        Variables.[Value] AS Variables, VariableValues.[Value] AS VariableValues, Platforms.[Value] AS Platforms, Moderators.Value AS Moderators, gl.SpeedRunComUrl             
    FROM dbo.tbl_Game g WITH (NOLOCK)
    JOIN dbo.tbl_Game_Link gl WITH (NOLOCK) ON gl.GameID = g.ID
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, ct.ID) + '|' + ct.[Name]
                        FROM dbo.tbl_CategoryType ct WITH (NOLOCK)
                        JOIN dbo.tbl_Category c WITH (NOLOCK) ON c.CategoryTypeID = ct.ID
                        WHERE c.GameID = g.ID
                        GROUP BY ct.ID, ct.[Name]
                        ORDER BY ct.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS CategoryTypes        
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, c.ID) + '|' + CONVERT(VARCHAR, c.CategoryTypeID) + '|' + c.[Name]
                        FROM dbo.tbl_Category c WITH (NOLOCK)
                        WHERE c.GameID = g.ID                        
                        ORDER BY c.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Categories   	   
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, l.ID) + '|' + l.[Name]
                        FROM dbo.tbl_Level l WITH (NOLOCK)
                        WHERE l.GameID = g.ID
                        ORDER BY l.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Levels   	
     OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, v.ID) + '|' + CASE v.IsSubCategory WHEN 1 THEN 'True' ELSE 'False' END + '|' + CONVERT(VARCHAR, v.VariableScopeTypeID) + '|' + ISNULL(CONVERT(VARCHAR, v.CategoryID),'') + '|' + ISNULL(CONVERT(VARCHAR, v.LevelID),'') + '|' + v.[Name]
                        FROM dbo.tbl_Variable v WITH (NOLOCK)
                        WHERE v.GameID = g.ID
                        ORDER BY v.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Variables
	OUTER APPLY (SELECT STUFF(
            (   SELECT '^^' + CONVERT(VARCHAR, v.ID) + '|' + CONVERT(VARCHAR, v.VariableID) + '|' + v.[Value]
                FROM dbo.tbl_VariableValue v WITH (NOLOCK)
                WHERE v.GameID = g.ID
                ORDER BY v.ID
                FOR XML PATH (''), root('MyString'), TYPE
            ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
    ) AS VariableValues    
    OUTER APPLY (SELECT STUFF(
                (   SELECT '^^' + CONVERT(VARCHAR, p.ID) + '|' + p.[Name]
                    FROM dbo.tbl_Platform p WITH (NOLOCK)
                    JOIN dbo.tbl_Game_Platform gp WITH (NOLOCK) ON gp.PlatformID = p.ID 
                    WHERE gp.GameID = g.ID
                    ORDER BY p.ID
                    FOR XML PATH (''), root('MyString'), TYPE
                ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
        ) AS Platforms
	OUTER APPLY (SELECT STUFF(
				(   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '�' + u.[Name] + '�' + u.[Abbr]
					FROM dbo.tbl_User u WITH (NOLOCK)
					JOIN dbo.tbl_Game_Moderator gm WITH (NOLOCK) ON gm.UserID = u.ID
					WHERE gm.GameID = g.ID
					ORDER BY gm.ID
                    FOR XML PATH (''), root('MyString'), TYPE
                ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
		) AS Moderators

GO

--vw_GameSpeedRunCom
IF OBJECT_ID('dbo.vw_GameSpeedRunCom') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_GameSpeedRunCom
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_GameSpeedRunCom]
AS

    SELECT g.ID,
           gc.SpeedRunComID,  
           g.Name,
           g.IsRomHack,
           g.YearOfRelease,
           gl.CoverImageUrl,      
           Categories.[Value] AS CategorySpeedRunComIDs,
           Levels.[Value] AS LevelSpeedRunComIDs,
           Variables.[Value] AS VariableSpeedRunComIDs,
           VariableValues.[Value] AS VariableValueSpeedRunComIDs,
           Platforms.[Value] AS PlatformSpeedRunComIDs,
           Moderators.[Value] AS ModeratorSpeedRunComIDs,
           g.CreatedDate,
           g.ModifiedDate,
           g.IsChanged
    FROM dbo.tbl_Game g WITH (NOLOCK)
    JOIN dbo.tbl_Game_SpeedRunComID gc WITH (NOLOCK) ON gc.GameID = g.ID
    JOIN dbo.tbl_Game_Link gl WITH (NOLOCK) ON gl.GameID = g.ID         
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + cc.SpeedRunComID
                        FROM dbo.tbl_Category c WITH (NOLOCK)
                        JOIN dbo.tbl_Category_SpeedRunComID cc WITH (NOLOCK) ON cc.CategoryID=c.ID
                        WHERE c.GameID = g.ID                        
                        ORDER BY c.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS Categories   	   
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + lc.SpeedRunComID
                        FROM dbo.tbl_Level l WITH (NOLOCK)
                        JOIN dbo.tbl_Level_SpeedRunComID lc WITH (NOLOCK) ON lc.LevelID = l.ID
                        WHERE l.GameID = g.ID
                        ORDER BY l.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS Levels   	
     OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + vc.SpeedRunComID
                        FROM dbo.tbl_Variable v WITH (NOLOCK)
                        JOIN dbo.tbl_Variable_SpeedRunComID vc WITH (NOLOCK) ON vc.VariableID = v.ID
                        WHERE v.GameID = g.ID
                        ORDER BY v.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value]
            ) AS Variables
	OUTER APPLY (SELECT STUFF(
            (   SELECT ',' + vc.SpeedRunComID
                FROM dbo.tbl_VariableValue v WITH (NOLOCK)
                JOIN dbo.tbl_VariableValue_SpeedRunComID vc WITH (NOLOCK) ON vc.VariableValueID = v.ID
                WHERE v.GameID = g.ID
                ORDER BY v.ID
                FOR XML PATH ('')
            ), 1, 1, '') AS [Value] 
    ) AS VariableValues    
    OUTER APPLY (SELECT STUFF(
                (   SELECT ',' + pc.SpeedRunComID
                    FROM dbo.tbl_Platform p WITH (NOLOCK)
                    JOIN dbo.tbl_Game_Platform gp WITH (NOLOCK) ON gp.PlatformID = p.ID
                    JOIN dbo.tbl_Platform_SpeedRunComID pc WITH (NOLOCK) ON pc.PlatformID = p.ID
                    WHERE gp.GameID = g.ID
                    ORDER BY p.ID
                    FOR XML PATH ('')
                ), 1, 1, '') AS [Value] 
        ) AS Platforms
	OUTER APPLY (SELECT STUFF(
				(   SELECT ',' + uc.SpeedRunComID
					FROM dbo.tbl_User u WITH (NOLOCK)
					JOIN dbo.tbl_Game_Moderator gm WITH (NOLOCK) ON gm.UserID = u.ID
                    JOIN dbo.tbl_User_SpeedRunComID uc WITH (NOLOCK) ON uc.UserID = u.ID 
					WHERE gm.GameID = g.ID
					ORDER BY gm.ID
					FOR XML PATH ('')
				), 1, 1, '') AS [Value] 
		) AS Moderators

GO

--vw_SpeedRun
IF OBJECT_ID('dbo.vw_SpeedRun') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_SpeedRun
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_SpeedRun]
AS

    SELECT rn.ID,
        g.ID AS GameID,
        g.[Name] AS GameName,
        st.ID AS StatusTypeID,
        st.[Name] AS StatusTypeName,           
        gl.CoverImageUrl AS GameCoverImageUrl,
        ct.ID AS CategoryTypeID,
        ct.[Name] AS CategoryTypeName,
        c.ID AS CategoryID,
        c.[Name] AS CategoryName,
        l.ID AS LevelID,
        l.[Name] AS LevelName,
        pl.ID AS PlatformID,
        pl.[Name] AS PlatformName,
        VariableValues.[Value] AS VariableValues,
        Players.[Value] AS Players,
        Guests.[Value] AS Guests,
        VideoLinks.[Value] AS VideoLinks,
        EmbeddedVideoLinks.[Value] AS EmbeddedVideoLinks,
        rs.IsEmulated,
        rn.[Rank],
        rt.PrimaryTime,
        rt.RealTime,
        rt.RealTimeWithoutLoads,
        rt.GameTime,
        rc.Comment,
        rl.SpeedRunComUrl,
        rl.SplitsUrl,
        rn.RunDate,
        rn.DateSubmitted,
        rn.VerifyDate
    FROM dbo.tbl_SpeedRun rn 
    JOIN dbo.tbl_Game g  ON g.ID = rn.GameID
    JOIN dbo.tbl_Game_Link gl  ON gl.GameID = rn.GameID
    JOIN dbo.tbl_RunStatusType st ON st.ID = rn.StatusTypeID 
    JOIN dbo.tbl_Category c  ON c.ID = rn.CategoryID
    JOIN dbo.tbl_CategoryType ct ON ct.ID = c.CategoryTypeID
    JOIN dbo.tbl_SpeedRun_System rs ON rs.SpeedRunID = rn.ID
    JOIN dbo.tbl_SpeedRun_Time rt ON rt.SpeedRunID = rn.ID
    JOIN dbo.tbl_SpeedRun_Link rl ON rl.SpeedRunID = rn.ID
    LEFT JOIN dbo.tbl_Level l  ON l.ID = rn.LevelID
    LEFT JOIN dbo.tbl_Platform pl on pl.ID = rs.PlatformID
    LEFT JOIN dbo.tbl_SpeedRun_Comment rc ON rc.SpeedRunID = rn.ID
    OUTER APPLY (SELECT STUFF(
                    (    SELECT ',' + CONVERT(VARCHAR, rv.VariableID) + '|' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        WHERE rv.SpeedRunID = rn.ID
                        ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS VariableValues  
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '|' + u.[Name]
                        FROM dbo.tbl_SpeedRun_Player rp  
                        JOIN dbo.tbl_User u ON u.ID = rp.UserID
                        WHERE rp.SpeedRunID = rn.ID
                        ORDER BY rp.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Players  
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, g.ID) + '|' + g.[Name]
                        FROM dbo.tbl_SpeedRun_Guest rg  
                        JOIN dbo.tbl_Guest g ON g.ID = rg.GuestID
                        WHERE rg.SpeedRunID = rn.ID
                        ORDER BY rg.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Guests              
    OUTER APPLY (SELECT STUFF(
                    (    SELECT ',' + rd.VideoLinkUrl
                        FROM dbo.tbl_SpeedRun_Video rd 
                        WHERE rd.SpeedRunID = rn.ID
                        ORDER BY rd.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 1, '') AS [Value]
            ) AS VideoLinks
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + rd.EmbeddedVideoLinkUrl
                        FROM dbo.tbl_SpeedRun_Video rd
                        WHERE rd.SpeedRunID = rn.ID
                        AND rd.EmbeddedVideoLinkUrl IS NOT NULL
                        ORDER BY rd.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 1, '') AS [Value] 
            ) AS EmbeddedVideoLinks

GO

--vw_SpeedRunGrid
IF OBJECT_ID('dbo.vw_SpeedRunGrid') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_SpeedRunGrid
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_SpeedRunGrid]
AS

    SELECT rn.ID,
           rn.GameID,
           rn.CategoryID,
           rn.LevelID,
           p.ID AS PlatformID,
           p.[Name] AS PlatformName,
           SubCategoryVariableValueIDs.[Value] AS SubCategoryVariableValueIDs,
           VariableValues.[Value] AS VariableValues,
           Players.[Value] AS Players,
		   Guests.[Value] AS Guests,
           rn.[Rank],
           rn.PrimaryTime,
           rc.Comment,
           rn.DateSubmitted,
           rn.VerifyDate
    FROM dbo.tbl_SpeedRun rn
   	JOIN dbo.tbl_SpeedRun_System rs ON rs.SpeedRunID = rn.ID 
   	LEFT JOIN dbo.tbl_SpeedRun_Comment rc ON rc.SpeedRunID = rn.ID
   	LEFT JOIN dbo.tbl_Platform p ON p.ID = rs.PlatformID
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        JOIN dbo.tbl_Variable v ON v.ID=rv.VariableID AND v.IsSubCategory = '1'
                        WHERE rv.SpeedRunID = rn.ID                       
                        --ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS SubCategoryVariableValueIDs             
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rv.VariableID) + '|' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        WHERE rv.SpeedRunID = rn.ID
                        --ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS VariableValues		
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '�' + u.[Name] + '�' + ISNULL(u.[Abbr],'')
                        FROM dbo.tbl_SpeedRun_Player rp  
						JOIN dbo.tbl_User u ON u.ID = rp.UserID
						WHERE rp.SpeedRunID = rn.ID
                        --ORDER BY rp.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Players
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, g.ID) + '�' + g.[Name] + '�' + ISNULL(g.[Abbr],'')
                        FROM dbo.tbl_SpeedRun_Guest rg
						JOIN dbo.tbl_Guest g ON g.ID = rg.GuestID
						WHERE rg.SpeedRunID = rn.ID
                        --ORDER BY rg.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Guests
            
GO

--vw_SpeedRunGridTab
IF OBJECT_ID('dbo.vw_SpeedRunGridTab') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_SpeedRunGridTab
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_SpeedRunGridTab]
AS

    SELECT rn.ID,
           rn.GameID,
           rn.CategoryID,
           rn.LevelID,
           VariableValues.[Value] AS VariableValues,
           rn.Rank
    FROM dbo.tbl_SpeedRun rn			               
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rv.VariableID) + '|' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        WHERE rv.SpeedRunID = rn.ID
                        ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS VariableValues
GO

--vw_SpeedRunGridTabUser
IF OBJECT_ID('dbo.vw_SpeedRunGridTabUser') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_SpeedRunGridTabUser
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_SpeedRunGridTabUser]
AS

    SELECT rn.ID,
           rn.GameID,
           rn.CategoryID,
           rn.LevelID,
           rn.VariableValues,
           rp.UserID,
           rn.Rank
    FROM dbo.vw_SpeedRunGridTab rn
	JOIN dbo.tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID
GO

--vw_SpeedRunGridUser
IF OBJECT_ID('dbo.vw_SpeedRunGridUser') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_SpeedRunGridUser
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_SpeedRunGridUser]
AS

    SELECT rn.ID,
           rn.GameID,
           rn.CategoryID,
           rn.LevelID,
           p.ID AS PlatformID,
           p.[Name] AS PlatformName,
           SubCategoryVariableValueIDs.[Value] AS SubCategoryVariableValueIDs,
           VariableValues.[Value] AS VariableValues,
           Players.[Value] AS Players,
		   Guests.[Value] AS Guests,
           rn.[Rank],
           rn.PrimaryTime,
           rc.Comment,
           rn.DateSubmitted,
           rn.VerifyDate,
           rp.UserID
    FROM dbo.tbl_SpeedRun rn
   	JOIN dbo.tbl_SpeedRun_System rs ON rs.SpeedRunID = rn.ID
    JOIN dbo.tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID
   	LEFT JOIN dbo.tbl_SpeedRun_Comment rc ON rc.SpeedRunID = rn.ID
   	LEFT JOIN dbo.tbl_Platform p ON p.ID = rs.PlatformID
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        JOIN dbo.tbl_Variable v ON v.ID=rv.VariableID AND v.IsSubCategory = '1'
                        WHERE rv.SpeedRunID = rn.ID                       
                        --ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS SubCategoryVariableValueIDs				
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rv.VariableID) + '|' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        WHERE rv.SpeedRunID = rn.ID
                        --ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS VariableValues	
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '�' + u.[Name] + '�' + ISNULL(u.Abbr, '')
                        FROM dbo.tbl_SpeedRun_Player rp  
						JOIN dbo.tbl_User u ON u.ID = rp.UserID
						WHERE rp.SpeedRunID = rn.ID
                        --ORDER BY rp.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Players            	
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, g.ID) + '�' + g.[Name] + '�' + ISNULL(g.Abbr, '')
                        FROM dbo.tbl_SpeedRun_Guest rg
						JOIN dbo.tbl_Guest g ON g.ID = rg.GuestID
						WHERE rg.SpeedRunID = rn.ID
                        --ORDER BY rg.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Guests
            
GO

--vw_SpeedRunSummary
IF OBJECT_ID('dbo.vw_SpeedRunSummary') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_SpeedRunSummary
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_SpeedRunSummary]
AS

    SELECT rn.ID,
           rn1.SpeedRunComID,
           g.ID AS GameID,
           g.[Name] AS GameName,
		   g.[Abbr] AS GameAbbr,
           gl.CoverImagePath AS GameCoverImageUrl,
           ct.ID AS CategoryTypeID,
           ct.[Name] AS CategoryTypeName,           
           c.ID AS CategoryID,
           c.[Name] AS CategoryName,
		   l.ID AS LevelID,
		   l.[Name] AS LevelName,
           SubCategoryVariableValues.[Value] AS SubCategoryVariableValues,
           Players.[Value] AS Players,
		   EmbeddedVideoLinks.[Value] AS EmbeddedVideoLinks,
           rn.[Rank],
           rn.PrimaryTime,
           rn.DateSubmitted,
		   rn.VerifyDate,
           rn.ImportedDate
    FROM dbo.tbl_SpeedRun rn
    JOIN dbo.tbl_SpeedRun_SpeedRunComID rn1 ON rn1.SpeedRunID = rn.ID
    JOIN dbo.tbl_Game g ON g.ID = rn.GameID
	JOIN dbo.tbl_Game_Link gl ON gl.GameID = g.ID
    JOIN dbo.tbl_Category c ON c.ID = rn.CategoryID
    JOIN dbo.tbl_CategoryType ct ON ct.ID = c.CategoryTypeID
    LEFT JOIN dbo.tbl_Level l ON l.ID = rn.LevelID
    OUTER APPLY (SELECT STUFF(
                (   SELECT '^^' + CONVERT(VARCHAR, rv.[VariableID]) + '¦' + CONVERT(VARCHAR, rv.VariableValueID) + '¦' + va.[Value]
                    FROM dbo.tbl_SpeedRun_VariableValue rv
                    JOIN dbo.tbl_Variable v ON v.ID = rv.VariableID AND v.IsSubCategory = 1
					JOIN dbo.tbl_VariableValue va ON va.ID = rv.VariableValueID
                    WHERE rv.SpeedRunID = rn.ID
                    ORDER BY rv.ID
                    FOR XML PATH (''), root('MyString'), TYPE
                ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
        ) AS SubCategoryVariableValues
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '¦' + u.[Name] + '¦' + ISNULL(u.[Abbr],'')
                        FROM dbo.tbl_SpeedRun_Player rp  
						JOIN dbo.tbl_User u ON u.ID = rp.UserID
						WHERE rp.SpeedRunID = rn.ID
                        ORDER BY rp.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Players           
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + rd.EmbeddedVideoLinkUrl + '|' + ISNULL(rd.ThumbnailLinkUrl,'')
                        FROM dbo.tbl_SpeedRun_Video rd
                        WHERE rd.SpeedRunID = rn.ID
                        AND rd.EmbeddedVideoLinkUrl IS NOT NULL
                        ORDER BY rd.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 1, '') AS [Value]
            ) AS EmbeddedVideoLinks 

GO

--vw_User
IF OBJECT_ID('dbo.vw_User') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_User
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_User]
AS
	SELECT u.ID, u.[Name], u.Abbr, u.SignUpDate, uc.[Location],
	ul.SpeedRunComUrl, ul.ProfileImageUrl, ul.TwitchProfileUrl, ul.HitboxProfileUrl, ul.YoutubeProfileUrl, ul.TwitterProfileUrl, ul.SpeedRunsLiveProfileUrl,
	TotalSpeedRuns.Value AS TotalSpeedRuns,
	TotalWorldRecords.Value AS TotalWorldRecords,
	TotalPersonalBests.Value AS TotalPersonalBests
	FROM dbo.tbl_User u WITH (NOLOCK)
	JOIN dbo.tbl_User_Link ul WITH (NOLOCK) ON ul.UserID = u.ID
	LEFT JOIN dbo.tbl_User_Location uc WITH (NOLOCK) ON uc.UserID = u.ID
	OUTER APPLY (SELECT COUNT(*) AS Value
					FROM dbo.tbl_SpeedRun_Player sp
					WHERE sp.UserID = u.ID
		) AS TotalSpeedRuns
	OUTER APPLY (SELECT COUNT(*) AS Value
					FROM dbo.tbl_SpeedRun_Player sp
					JOIN dbo.tbl_SpeedRun sr ON sr.ID=sp.SpeedRunID AND sr.[Rank]=1
					WHERE sp.UserID = u.ID
		) AS TotalWorldRecords
	OUTER APPLY (SELECT COUNT(*) AS Value
					FROM (
					SELECT sr.GameID, sr.CategoryID, sr.LevelID, SubCategoryVariableValueIDs.[Value]
					FROM dbo.tbl_SpeedRun_Player sp
					JOIN dbo.tbl_SpeedRun sr ON sr.ID=sp.SpeedRunID
					OUTER APPLY (SELECT STUFF(
						(SELECT ',' + CONVERT(VARCHAR, rv.VariableValueID)
						FROM dbo.tbl_SpeedRun_VariableValue rv
						JOIN dbo.tbl_Variable v ON v.ID=rv.VariableID AND v.IsSubCategory='1'
						WHERE rv.SpeedRunID = sr.ID                       
						ORDER BY rv.ID
						FOR XML PATH ('')
						), 1, 1, '') AS [Value] 
					) AS SubCategoryVariableValueIDs 
					WHERE sp.UserID = u.ID
					GROUP BY sr.GameID, sr.CategoryID, sr.LevelID, SubCategoryVariableValueIDs.[Value]
				) AS SubQuery
		) AS TotalPersonalBests

GO

--vw_UserAccount
IF OBJECT_ID('dbo.vw_UserAccount') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_UserAccount
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_UserAccount]
AS

    SELECT ua.ID AS UserAccountID,
	ua.Username,
	ue.IsDarkTheme,
	SpeedRunListCategoryIDs.[Value] AS SpeedRunListCategoryIDs
    FROM dbo.tbl_UserAccount ua
	LEFT JOIN dbo.tbl_UserAccount_Setting ue ON ue.UserAccountID = ua.ID
    OUTER APPLY (SELECT STUFF(
                (   SELECT ',' + CONVERT(VARCHAR, uc.[SpeedRunListCategoryID])
                    FROM dbo.tbl_UserAccount_SpeedRunListCategory uc
                    WHERE uc.UserAccountID = ua.ID
                    ORDER BY uc.UserAccountID
                    FOR XML PATH ('')
                ), 1, 1, '') AS [Value] 
        ) AS SpeedRunListCategoryIDs

GO

--vw_UserSpeedRunCom
IF OBJECT_ID('dbo.vw_UserSpeedRunCom') IS NOT NULL 
BEGIN 
    DROP VIEW dbo.vw_UserSpeedRunCom
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_UserSpeedRunCom]
AS

    SELECT u.ID,
           uc.SpeedRunComID,  
           u.[Name],
		   lc.[Location],
           ul.SpeedRunComUrl,      
           ul.ProfileImageUrl,      
           ul.TwitchProfileUrl,      
           ul.HitboxProfileUrl,      
           ul.YoutubeProfileUrl,      
           ul.TwitterProfileUrl,      
           ul.SpeedRunsLiveProfileUrl
    FROM dbo.tbl_User u WITH (NOLOCK)
    JOIN dbo.tbl_User_SpeedRunComID uc WITH (NOLOCK) ON uc.UserID = u.ID
    JOIN dbo.tbl_User_Link ul WITH (NOLOCK) ON ul.UserID = u.ID
    LEFT JOIN dbo.tbl_User_Location lc WITH (NOLOCK) ON lc.UserID = u.ID

GO

/*********************************************/
--create/alter procs
/*********************************************/
--GetGamesByUserID
IF OBJECT_ID('dbo.GetGamesByUserID') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.GetGamesByUserID
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetGamesByUserID]
(
     @UserID VARCHAR(20)
)
AS
BEGIN

	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	IF OBJECT_ID('tempdb..#ResultsRaw') IS NOT NULL 
	BEGIN 
		DROP TABLE #ResultsRaw
	END

	CREATE TABLE #ResultsRaw
	( 
			[GameID] INT,
			[CategoryID] INT,
			[LevelID] INT,
			[VariableID] INT,
			[VariableValueID] INT
	)

	INSERT INTO #ResultsRaw (GameID, CategoryID, LevelID, VariableID, VariableValueID)
	SELECT rn.GameID, rn.CategoryID, rn.LevelID, rv.VariableID, rv.VariableValueID
	FROM dbo.tbl_SpeedRun rn
	JOIN dbo.tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID
	LEFT JOIN dbo.tbl_SpeedRun_VariableValue rv ON rv.SpeedRunID = rn.ID
	WHERE rp.UserID = @UserID

	SELECT g.ID, g.[Name], gl.CoverImageUrl, g.YearOfRelease,
	CategoryTypes.[Value] AS CategoryTypes, Categories.[Value] AS Categories, Levels.[Value] AS Levels,
	Variables.[Value] AS Variables, VariableValues.[Value] AS VariableValues, Platforms.[Value] AS Platforms, Moderators.[Value] AS Moderators
	FROM #ResultsRaw r
	JOIN dbo.tbl_Game g  ON g.ID = r.GameID
	JOIN dbo.tbl_Game_Link gl  ON gl.GameID = g.ID
	OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, ct.ID) + '|' + ct.[Name]
						FROM #ResultsRaw r1
						JOIN dbo.tbl_Category c  ON c.ID = r1.CategoryID
						JOIN dbo.tbl_CategoryType ct  ON ct.ID = c.CategoryTypeID
						WHERE r1.GameID = r.GameID
						GROUP BY ct.ID, ct.[Name]
						ORDER BY ct.ID
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS CategoryTypes
	OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, c.ID) + '|' + CONVERT(VARCHAR, c.CategoryTypeID) + '|' + c.[Name]
						FROM #ResultsRaw r1
						JOIN dbo.tbl_Category c  ON c.ID = r1.CategoryID
						WHERE r1.GameID = r.GameID
						GROUP BY c.ID, c.[Name], c.CategoryTypeID
						ORDER BY c.ID
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Categories 
	OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, l.ID) + '|' + l.[Name]
						FROM #ResultsRaw r1
						JOIN dbo.tbl_Level l  ON l.ID = r1.LevelID
						WHERE r1.GameID = r.GameID
						GROUP BY l.ID, l.[Name]
						ORDER BY l.ID                       
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Levels   
		OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, v.ID) + '|' + CASE v.IsSubCategory WHEN 1 THEN 'True' ELSE 'False' END + '|' + CONVERT(VARCHAR, v.VariableScopeTypeID) + '|' + ISNULL(CONVERT(VARCHAR, v.CategoryID),'') + '|' + ISNULL(CONVERT(VARCHAR, v.LevelID),'') + '|' + v.[Name]
						FROM #ResultsRaw r1                       
						JOIN dbo.tbl_Variable v  ON v.ID = r1.VariableID
						WHERE r1.GameID = r.GameID
						GROUP BY v.ID, v.[Name], v.IsSubCategory, v.VariableScopeTypeID, v.CategoryID, v.LevelID
						ORDER BY v.ID                       
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Variables 
		OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, v.ID) + '|' + CONVERT(VARCHAR, v.VariableID) + '|' + v.[Value]
						FROM #ResultsRaw r1                       
						JOIN dbo.tbl_VariableValue v  ON v.ID = r1.VariableValueID
						WHERE r1.GameID = r.GameID
						GROUP BY v.ID, v.[Value], v.VariableID
						ORDER BY v.ID                       
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS VariableValues
		OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, p.ID) + '|' + p.[Name]
						FROM dbo.tbl_Platform p 
						JOIN dbo.tbl_Game_Platform gp  ON gp.PlatformID = p.ID 
						WHERE gp.GameID = r.GameID
						GROUP BY gp.ID, p.ID, p.[Name]
						ORDER BY gp.ID
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Platforms
		OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '|' + u.[Name]
						FROM dbo.tbl_User u 
						JOIN dbo.tbl_Game_Moderator gm  ON gm.UserID = u.ID
						WHERE gm.GameID = r.GameID
						GROUP BY gm.ID, u.ID, u.[Name]
						ORDER BY gm.ID
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Moderators
	GROUP BY g.ID, g.[Name], gl.CoverImageUrl, g.YearOfRelease, CategoryTypes.[Value], Categories.[Value], Levels.[Value], Variables.[Value], VariableValues.[Value], Platforms.[Value], Moderators.[Value]
	ORDER BY g.[Name]

END
GO

--GetLatestSpeedRuns
IF OBJECT_ID('dbo.GetLatestSpeedRuns') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.GetLatestSpeedRuns
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetLatestSpeedRuns]
(
     @SpeedRunListCategoryID INT,
     @TopAmount INT,
     @OrderValueOffset INT = NULL
)
WITH RECOMPILE
AS
BEGIN

     SET NOCOUNT ON;
     SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

     --new
     IF(@SpeedRunListCategoryID = 0)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          ORDER BY rn.ID DESC
     END
	 --top 5%
     ELSE IF(@SpeedRunListCategoryID = 1)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate              
          FROM dbo.vw_SpeedRunSummary rn        
			CROSS APPLy (SELECT MAX(rn1.[Rank]) AS [Value]
								FROM dbo.vw_SpeedRunSummary rn1
								WHERE rn1.GameID = rn.GameID
								AND rn1.CategoryID = rn.CategoryID
								AND ISNULL(rn1.LevelID,'') = ISNULL(rn.LevelID,'')
								AND ISNULL(rn1.SubCategoryVariableValues,'') = ISNULL(rn.SubCategoryVariableValues,'')
								AND rn1.[Rank] IS NOT NULL
					) AS MaxRank
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          AND ((rn.[Rank] / (MaxRank.[Value])) * 100.00) <= 5.00
          ORDER BY rn.ID DESC
     END
	 --first
     ELSE IF(@SpeedRunListCategoryID = 2)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
		  AND rn.[Rank] = 1
		  AND EXISTS (SELECT 1
					  FROM dbo.vw_SpeedRunSummary rn1
					  WHERE rn1.GameID = rn.GameID
					  AND rn1.CategoryID = rn.CategoryID
					  AND ISNULL(rn1.LevelID,'') = ISNULL(rn.LevelID,'')
					  AND ISNULL(rn1.SubCategoryVariableValues,'') = ISNULL(rn.SubCategoryVariableValues,'')
					  AND rn1.[Rank] > 1)
          ORDER BY rn.ID DESC
     END
	 --top 3
     ELSE IF(@SpeedRunListCategoryID = 3)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
		  AND rn.[Rank] <= 3
		  AND EXISTS (SELECT 1
					  FROM dbo.vw_SpeedRunSummary rn1
					  WHERE rn1.GameID = rn.GameID
					  AND rn1.CategoryID = rn.CategoryID
					  AND ISNULL(rn1.LevelID,'') = ISNULL(rn.LevelID,'')
					  AND ISNULL(rn1.SubCategoryVariableValues,'') = ISNULL(rn.SubCategoryVariableValues,'')
					  AND rn1.[Rank] > 3)
          ORDER BY rn.ID DESC
     END
	 --Bests
     ELSE IF(@SpeedRunListCategoryID = 4)
     BEGIN
          SELECT TOP (@TopAmount)
          rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
          rn.GameCoverImageUrl, 
          --NULL AS GameCoverImageUrl,          
          rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
          rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate           
          FROM dbo.vw_SpeedRunSummary rn
		  CROSS APPLy (SELECT MIN(rn1.[PrimaryTime]) AS [Value]
								FROM dbo.vw_SpeedRunSummary rn1
								WHERE rn1.GameID = rn.GameID
								AND rn1.CategoryID = rn.CategoryID
								AND ISNULL(rn1.LevelID,'') = ISNULL(rn.LevelID,'')
								AND ISNULL(rn1.SubCategoryVariableValues,'') = ISNULL(rn.SubCategoryVariableValues,'')
								AND ISNULL(rn1.Players,'') = ISNULL(rn.Players,'')
								AND rn1.ID <> rn.ID
			    ) AS MinPrimaryTime
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          AND rn.[PrimaryTime] < MinPrimaryTime.[Value]
          ORDER BY rn.ID DESC     
     END
	 --Popular
     ELSE IF(@SpeedRunListCategoryID = 5)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          AND EXISTS (SELECT 1
                      FROM dbo.tbl_SpeedRun_Video_Detail rd
                      WHERE rd.SpeedRunID = rn.ID
                      AND rd.ViewCount > 1000)
          AND EXISTS (SELECT 1
                      FROM dbo.tbl_SpeedRun_Video rd
                      WHERE rd.SpeedRunID = rn.ID
                      GROUP BY rd.SpeedRunID
                      HAVING COUNT(DISTINCT rd.ID) = 1
                    )
          ORDER BY rn.ID DESC
     END
	 --GDQ
     ELSE IF(@SpeedRunListCategoryID = 7)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          AND EXISTS (SELECT 1
                      FROM dbo.tbl_SpeedRun_Video_Detail rd
                      WHERE rd.SpeedRunID = rn.ID
                      AND rd.ChannelID IN ('22510310','UCI3DTtB-a3fJPjKtQ5kYHfA'))
          ORDER BY rn.ID DESC
     END

END
GO

--GetPersonalBestsByUserID
IF OBJECT_ID('dbo.GetPersonalBestsByUserID') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.GetPersonalBestsByUserID
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetPersonalBestsByUserID]
(
    @GameID INT,
    @CategoryID INT,
	@LevelID INT,
    @UserID INT    
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	IF OBJECT_ID('tempdb..#ResultsRaw') IS NOT NULL DROP TABLE #ResultsRaw
	SELECT ROW_NUMBER() OVER (PARTITION BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValueIDs ORDER BY rn.PrimaryTime) AS RowNum,
	rn.ID,
	rn.GameID, 
	rn.CategoryID,
	rn.LevelID,
	rn.PlatformID,
	rn.PlatformName,
	rn.SubCategoryVariableValueIDs,
	rn.VariableValues,
	rn.Players,
	rn.Guests, 
	rn.[Rank],
	rn.PrimaryTime,
	rn.Comment,
	rn.DateSubmitted,
	rn.VerifyDate
	INTO #ResultsRaw	
	FROM dbo.vw_SpeedRunGridUser rn
	WHERE rn.GameID = @GameID
    AND rn.CategoryID = @CategoryID
	AND ISNULL(rn.LevelID, '' ) = ISNULL(@LevelID, '')
    AND rn.UserID = @UserID

	SELECT rn.ID,
	rn.GameID,
	rn.CategoryID,
	rn.LevelID,
	rn.PlatformID,
	rn.SubCategoryVariableValueIDs,
	rn.VariableValues,
	rn.Players,
	rn.Guests,
	rn.[Rank],
	rn.PrimaryTime,
	rn.Comment,
	rn.DateSubmitted,
	rn.VerifyDate
	FROM #ResultsRaw rn
	WHERE rn.RowNum = 1
	ORDER BY rn.ID

END
GO

--GetSpeedRunsByUserID
IF OBJECT_ID('dbo.GetSpeedRunsByUserID') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.GetSpeedRunsByUserID
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetSpeedRunsByUserID]
(
    @GameID INT,
    @CategoryID INT,
    @LevelID INT,
    @VariableValueIDs VARCHAR(MAX),
    @UserID INT
)
AS
BEGIN

     SET NOCOUNT ON;
     SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	 SELECT rn.ID,
     rn.GameID,
     --rn.CategoryTypeID,
     rn.CategoryID,
     rn.LevelID,
     rn.PlatformID,
     rn.PlatformName,
	 rn.SubCategoryVariableValueIDs,
     --rn.Variables,
     rn.VariableValues,
     rn.Players,
     rn.Guests,
     --rn.IsEmulated,
     rn.[Rank],
     rn.PrimaryTime,
     rn.Comment,
     rn.DateSubmitted,
     rn.VerifyDate 
	 FROM dbo.vw_SpeedRunGrid rn
	 JOIN dbo.tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID AND rp.UserID = @UserID
	 WHERE rn.GameID = @GameID
     AND rn.CategoryID = @CategoryID
     AND ISNULL(rn.LevelID,'') = ISNULL(@LevelID,'')
     AND ISNULL(rn.SubCategoryVariableValueIDs,'') = ISNULL(@VariableValueIDs,'')
	 ORDER BY rn.ID DESC

END
GO

--ImportCreateFullTables
IF OBJECT_ID('dbo.ImportCreateFullTables') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.ImportCreateFullTables
END

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

    /*
        DROP TABLE dbo.tbl_Platform_Full
        DROP TABLE dbo.tbl_Platform_SpeedRunComID_Full
        DROP TABLE dbo.tbl_User_Full
        DROP TABLE dbo.tbl_User_SpeedRunComID_Full
        DROP TABLE dbo.tbl_User_Location_Full
        DROP TABLE dbo.tbl_User_Link_Full
        DROP TABLE dbo.tbl_Guest_Full
        DROP TABLE dbo.tbl_Guest_Link_Full
        DROP TABLE dbo.tbl_Game_Full
        DROP TABLE dbo.tbl_Game_SpeedRunComID_Full
        DROP TABLE dbo.tbl_Game_Link_Full
        DROP TABLE dbo.tbl_Level_Full
        DROP TABLE dbo.tbl_Level_SpeedRunComID_Full
        DROP TABLE dbo.tbl_Level_Rule_Full
        DROP TABLE dbo.tbl_Category_Full
        DROP TABLE dbo.tbl_Category_SpeedRunComID_Full
        DROP TABLE dbo.tbl_Category_Rule_Full
        DROP TABLE dbo.tbl_Variable_Full
        DROP TABLE dbo.tbl_Variable_SpeedRunComID_Full
        DROP TABLE dbo.tbl_VariableValue_Full
        DROP TABLE dbo.tbl_VariableValue_SpeedRunComID_Full
        DROP TABLE dbo.tbl_Game_Platform_Full
        DROP TABLE dbo.tbl_Game_Region_Full
        DROP TABLE dbo.tbl_Game_Moderator_Full
        DROP TABLE dbo.tbl_Game_TimingMethod_Full
        DROP TABLE dbo.tbl_Game_Ruleset_Full
        DROP TABLE dbo.tbl_SpeedRun_Full
        DROP TABLE dbo.tbl_SpeedRun_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_SpeedRunComID_Full
        DROP TABLE dbo.tbl_SpeedRun_SpeedRunComID_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_System_Full
        DROP TABLE dbo.tbl_SpeedRun_System_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_Time_Full
        DROP TABLE dbo.tbl_SpeedRun_Time_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_Link_Full
        DROP TABLE dbo.tbl_SpeedRun_Link_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_Comment_Full
        DROP TABLE dbo.tbl_SpeedRun_Comment_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_Player_Full
        DROP TABLE dbo.tbl_SpeedRun_Player_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_Guest_Full
        DROP TABLE dbo.tbl_SpeedRun_Guest_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_VariableValue_Full
        DROP TABLE dbo.tbl_SpeedRun_VariableValue_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_Video_Full
        DROP TABLE dbo.tbl_SpeedRun_Video_Full_Ordered
        DROP TABLE dbo.tbl_SpeedRun_Video_Detail_Full
        DROP TABLE dbo.tbl_SpeedRun_Video_Detail_Full_Ordered
    */

END
GO

--ImportGetGamesForSitemap
IF OBJECT_ID('dbo.ImportGetGamesForSitemap') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.ImportGetGamesForSitemap
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ImportGetGamesForSitemap]
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT ID, Abbr, ISNULL(ModifiedDate, ImportedDate) AS LastModifiedDate 
    FROM dbo.tbl_Game
    ORDER BY ISNULL(ModifiedDate, ImportedDate) DESC

END
GO

--ImportRebuildIndexes
IF OBJECT_ID('dbo.ImportRebuildIndexes') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.ImportRebuildIndexes
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ImportRebuildIndexes]
AS
BEGIN

	IF OBJECT_ID('tempdb..#RebuildIndexes') IS NOT NULL 
	BEGIN 
		DROP TABLE #RebuildIndexes
	END

	CREATE TABLE #RebuildIndexes 
	( 
		[ID] INT IDENTITY(1,1),
		[Schema] VARCHAR (255),
		[Table] VARCHAR (255),
		[Index] VARCHAR (255),
		[Action] VARCHAR (255)
	)

	INSERT INTO #RebuildIndexes ([Schema],[Table],[Index],[Action])
	SELECT S.name, T.name, I.name, CASE WHEN DDIPS.avg_fragmentation_in_percent > 30 THEN 'REBUILD' ELSE 'REORGANIZE' END
	FROM sys.dm_db_index_physical_stats (DB_ID(), NULL, NULL, NULL, NULL) AS DDIPS
	INNER JOIN sys.tables T ON T.object_id = DDIPS.object_id
	INNER JOIN sys.schemas S ON T.schema_id = S.schema_id
	INNER JOIN sys.indexes I ON I.object_id = DDIPS.object_id
	AND DDIPS.index_id = I.index_id
	WHERE DDIPS.database_id = DB_ID()
	AND I.name is not null
	AND DDIPS.avg_fragmentation_in_percent > 5
	ORDER BY DDIPS.avg_fragmentation_in_percent DESC

	DECLARE @Sql NVARCHAR(MAX) = ''
	DECLARE @RowCount INT = 1
	DECLARE @MaxRowCount INT

	SELECT @MaxRowCount = MAX(ID)
	FROM #RebuildIndexes

	WHILE @RowCount <= @MaxRowCount
	BEGIN
		SELECT @Sql += 'ALTER INDEX [' + [Index] + '] ON [' + [Schema] + '].[' + [Table] + '] ' + [Action] + ' '
		FROM #RebuildIndexes
		WHERE ID = @RowCount

		SELECT @RowCount = @RowCount + 1
	END

	EXEC (@Sql)

END
GO

--ImportRenameFullTables
IF OBJECT_ID('dbo.ImportRenameFullTables') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.ImportRenameFullTables
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ImportRenameFullTables]
AS
BEGIN

	--Drop Foreign Keys
	--tbl_Platform
	ALTER TABLE [dbo].[tbl_Game_Platform] DROP CONSTRAINT [FK_tbl_Game_Platform_tbl_Platform]
	ALTER TABLE [dbo].[tbl_SpeedRun_System] DROP CONSTRAINT [FK_tbl_SpeedRun_System_tbl_Platform]
	--tbl_User
	ALTER TABLE [dbo].[tbl_SpeedRun_Player] DROP CONSTRAINT [FK_tbl_SpeedRun_Player_tbl_User]
	ALTER TABLE [dbo].[tbl_Game_Moderator] DROP CONSTRAINT [FK_tbl_Game_Moderator_tbl_User]
   	--tbl_Guest
    ALTER TABLE [dbo].[tbl_SpeedRun_Guest] DROP CONSTRAINT [FK_tbl_SpeedRun_Guest_tbl_Guest]
	--tbl_Game
	ALTER TABLE [dbo].[tbl_Level] DROP CONSTRAINT [FK_tbl_Level_tbl_Game]
	ALTER TABLE [dbo].[tbl_Category] DROP CONSTRAINT [FK_tbl_Category_tbl_Game]
	ALTER TABLE [dbo].[tbl_Variable] DROP CONSTRAINT [FK_tbl_Variable_tbl_Game]
	ALTER TABLE [dbo].[tbl_VariableValue] DROP CONSTRAINT [FK_tbl_VariableValue_tbl_Game]
	ALTER TABLE [dbo].[tbl_Game_Platform] DROP CONSTRAINT [FK_tbl_Game_Platform_tbl_Game]
	ALTER TABLE [dbo].[tbl_Game_Region] DROP CONSTRAINT [FK_tbl_Game_Region_tbl_Game]
	ALTER TABLE [dbo].[tbl_Game_Moderator] DROP CONSTRAINT [FK_tbl_Game_Moderator_tbl_Game]
	ALTER TABLE [dbo].[tbl_Game_TimingMethod] DROP CONSTRAINT [FK_tbl_Game_TimingMethod_tbl_Game]
	ALTER TABLE [dbo].[tbl_SpeedRun] DROP CONSTRAINT [FK_tbl_SpeedRun_tbl_Game]
	--tbl_Level
	ALTER TABLE [dbo].[tbl_Variable] DROP CONSTRAINT [FK_tbl_Variable_tbl_Level]
	ALTER TABLE [dbo].[tbl_SpeedRun] DROP CONSTRAINT [FK_tbl_SpeedRun_tbl_Level]
	--tbl_Category
	ALTER TABLE [dbo].[tbl_Variable] DROP CONSTRAINT [FK_tbl_Variable_tbl_Category]
	ALTER TABLE [dbo].[tbl_SpeedRun] DROP CONSTRAINT [FK_tbl_SpeedRun_tbl_Category]
	--tbl_Variable
	--ALTER TABLE [dbo].[tbl_VariableValue] DROP CONSTRAINT [FK_tbl_VariableValue_tbl_Variable]
	--ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue] DROP CONSTRAINT [FK_tbl_SpeedRun_VariableValue_tbl_Variable]
	--tbl_VariableValue
	--ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue] DROP CONSTRAINT [FK_tbl_SpeedRun_VariableValue_tbl_VariableValue]
	--tbl_SpeedRun
	ALTER TABLE [dbo].[tbl_SpeedRun_Player] DROP CONSTRAINT [FK_tbl_SpeedRun_Player_tbl_SpeedRun]
	ALTER TABLE [dbo].[tbl_SpeedRun_Guest] DROP CONSTRAINT [FK_tbl_SpeedRun_Guest_tbl_SpeedRun]
	ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue] DROP CONSTRAINT [FK_tbl_SpeedRun_VariableValue_tbl_SpeedRun]
	ALTER TABLE [dbo].[tbl_SpeedRun_Video] DROP CONSTRAINT [FK_tbl_SpeedRun_Video_tbl_SpeedRun]

	--Drop Indexes
	-- DROP INDEX [IDX_tbl_SpeedRun_Link_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Link_Full]
	-- DROP INDEX [IDX_tbl_SpeedRun_System_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_System_Full]
	-- DROP INDEX [IDX_tbl_SpeedRun_Time_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Time_Full]
	-- DROP INDEX [IDX_tbl_SpeedRun_Comment_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Comment_Full]

	-- DROP INDEX [IDX_tbl_SpeedRun_VariableValue_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_VariableValue_Full]
	-- DROP INDEX [IDX_tbl_SpeedRun_Player_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Player_Full]
	-- DROP INDEX [IDX_tbl_SpeedRun_Guest_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Guest_Full]
	-- DROP INDEX [IDX_tbl_SpeedRun_Video_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Video_Full]
	-- DROP INDEX [IDX_tbl_SpeedRun_Video_Detail_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Video_Detail_Full]
    
	-- DROP INDEX [IDX_tbl_SpeedRun_SpeedRunComID_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_SpeedRunComID_Full]

	--Drop Tables
	DROP TABLE dbo.tbl_Platform
	DROP TABLE dbo.tbl_Platform_SpeedRunComID
	DROP TABLE dbo.tbl_User
	DROP TABLE dbo.tbl_User_SpeedRunComID
	DROP TABLE dbo.tbl_User_Location
	DROP TABLE dbo.tbl_User_Link
    DROP TABLE dbo.tbl_Guest
	DROP TABLE dbo.tbl_Game
	DROP TABLE dbo.tbl_Game_SpeedRunComID
	DROP TABLE dbo.tbl_Game_Link
	DROP TABLE dbo.tbl_Level
	DROP TABLE dbo.tbl_Level_SpeedRunComID
	DROP TABLE dbo.tbl_Level_Rule
	DROP TABLE dbo.tbl_Category
	DROP TABLE dbo.tbl_Category_SpeedRunComID
	DROP TABLE dbo.tbl_Category_Rule
	DROP TABLE dbo.tbl_Variable
	DROP TABLE dbo.tbl_Variable_SpeedRunComID
	DROP TABLE dbo.tbl_VariableValue
	DROP TABLE dbo.tbl_VariableValue_SpeedRunComID
	DROP TABLE dbo.tbl_Game_Platform
	DROP TABLE dbo.tbl_Game_Region
	DROP TABLE dbo.tbl_Game_Moderator
	DROP TABLE dbo.tbl_Game_TimingMethod
	DROP TABLE dbo.tbl_Game_Ruleset
	DROP TABLE dbo.tbl_SpeedRun
	DROP TABLE dbo.tbl_SpeedRun_SpeedRunComID
	DROP TABLE dbo.tbl_SpeedRun_System
	DROP TABLE dbo.tbl_SpeedRun_Time
	DROP TABLE dbo.tbl_SpeedRun_Link
	DROP TABLE dbo.tbl_SpeedRun_Comment
	DROP TABLE dbo.tbl_SpeedRun_Player
   	DROP TABLE dbo.tbl_SpeedRun_Guest 
	DROP TABLE dbo.tbl_SpeedRun_VariableValue
	DROP TABLE dbo.tbl_SpeedRun_Video
	DROP TABLE dbo.tbl_SpeedRun_Video_Detail        
	
    --Rename tables
	--tbl_Platform
	EXEC sp_rename 'dbo.PK_tbl_Platform_Full', 'PK_tbl_Platform'
	EXEC sp_rename 'dbo.DF_tbl_Platform_Full_ImportedDate', 'DF_tbl_Platform_ImportedDate'
	EXEC sp_rename 'dbo.tbl_Platform_Full', 'tbl_Platform'
	--tbl_Platform_SpeedRunComID
	--EXEC sp_rename 'dbo.IDX_tbl_Platform_SpeedRunComID_Full_SpeedRunComID', 'IDX_tbl_Platform_SpeedRunComID_SpeedRunComID'                                
	EXEC sp_rename 'dbo.PK_tbl_Platform_SpeedRunComID_Full', 'PK_tbl_Platform_SpeedRunComID'                                
	EXEC sp_rename 'dbo.tbl_Platform_SpeedRunComID_Full', 'tbl_Platform_SpeedRunComID'
	--tbl_User
	EXEC sp_rename 'dbo.PK_tbl_User_Full', 'PK_tbl_User'                                
	EXEC sp_rename 'dbo.DF_tbl_User_Full_ImportedDate', 'DF_tbl_User_ImportedDate'
	EXEC sp_rename 'dbo.tbl_User_Full', 'tbl_User'
	--tbl_User_SpeedRunComID
	--EXEC sp_rename 'dbo.IDX_tbl_User_SpeedRunComID_Full_SpeedRunComID', 'IDX_tbl_User_SpeedRunComID_SpeedRunComID'                                
	EXEC sp_rename 'dbo.PK_tbl_User_SpeedRunComID_Full', 'PK_tbl_User_SpeedRunComID'  
	EXEC sp_rename 'dbo.tbl_User_SpeedRunComID_Full', 'tbl_User_SpeedRunComID'
	--tbl_User_Location
	EXEC sp_rename 'dbo.PK_tbl_User_Location_Full', 'PK_tbl_User_Location'  
	EXEC sp_rename 'dbo.tbl_User_Location_Full', 'tbl_User_Location'
	--tbl_User_Link
	EXEC sp_rename 'dbo.PK_tbl_User_Link_Full', 'PK_tbl_User_Link'  
	EXEC sp_rename 'dbo.tbl_User_Link_Full', 'tbl_User_Link'
	--tbl_Guest
	EXEC sp_rename 'dbo.PK_tbl_Guest_Full', 'PK_tbl_Guest'                                
	EXEC sp_rename 'dbo.DF_tbl_Guest_Full_ImportedDate', 'DF_tbl_Guest_ImportedDate'
	EXEC sp_rename 'dbo.tbl_Guest_Full', 'tbl_Guest'    
	--tbl_Guest_Link                                
	EXEC sp_rename 'dbo.PK_tbl_Guest_Link_Full', 'PK_tbl_Guest_Link'                                
	EXEC sp_rename 'dbo.tbl_Guest_Link_Full', 'tbl_Guest_Link'     	
	--tbl_Game
	EXEC sp_rename 'dbo.PK_tbl_Game_Full', 'PK_tbl_Game'                                
	EXEC sp_rename 'dbo.DF_tbl_Game_Full_ImportedDate', 'DF_tbl_Game_ImportedDate'
	EXEC sp_rename 'dbo.tbl_Game_Full', 'tbl_Game'
	--tbl_Game_SpeedRunComID
	--EXEC sp_rename 'dbo.IDX_tbl_Game_SpeedRunComID_Full_SpeedRunComID', 'IDX_tbl_Game_SpeedRunComID_SpeedRunComID'                                
	EXEC sp_rename 'dbo.PK_tbl_Game_SpeedRunComID_Full', 'PK_tbl_Game_SpeedRunComID'                                
	EXEC sp_rename 'dbo.tbl_Game_SpeedRunComID_Full', 'tbl_Game_SpeedRunComID'
	--tbl_Game_Link                                
	EXEC sp_rename 'dbo.PK_tbl_Game_Link_Full', 'PK_tbl_Game_Link'                                
	EXEC sp_rename 'dbo.tbl_Game_Link_Full', 'tbl_Game_Link'
	--tbl_Level
	EXEC sp_rename 'dbo.PK_tbl_Level_Full', 'PK_tbl_Level'                                
	EXEC sp_rename 'dbo.tbl_Level_Full', 'tbl_Level'
	--tbl_Level_SpeedRunComID
  	--EXEC sp_rename 'dbo.IDX_tbl_Level_SpeedRunComID_Full_SpeedRunComID', 'IDX_tbl_Level_SpeedRunComID_SpeedRunComID'                                  
	EXEC sp_rename 'dbo.PK_tbl_Level_SpeedRunComID_Full', 'PK_tbl_Level_SpeedRunComID'  
	EXEC sp_rename 'dbo.tbl_Level_SpeedRunComID_Full', 'tbl_Level_SpeedRunComID'
	--tbl_Level_Rule
	EXEC sp_rename 'dbo.PK_tbl_Level_Rule_Full', 'PK_tbl_Level_Rule'  
	EXEC sp_rename 'dbo.tbl_Level_Rule_Full', 'tbl_Level_Rule'
	--tbl_Category
	EXEC sp_rename 'dbo.PK_tbl_Category_Full', 'PK_tbl_Category'  
	EXEC sp_rename 'dbo.tbl_Category_Full', 'tbl_Category'
	--tbl_Category_SpeedRunComID
    --EXEC sp_rename 'dbo.IDX_tbl_Category_SpeedRunComID_Full_SpeedRunComID', 'IDX_tbl_Category_SpeedRunComID_SpeedRunComID'                                   
	EXEC sp_rename 'dbo.PK_tbl_Category_SpeedRunComID_Full', 'PK_tbl_Category_SpeedRunComID'  
	EXEC sp_rename 'dbo.tbl_Category_SpeedRunComID_Full', 'tbl_Category_SpeedRunComID'
	--tbl_Category_Rule
	EXEC sp_rename 'dbo.PK_tbl_Category_Rule_Full', 'PK_tbl_Category_Rule'  
	EXEC sp_rename 'dbo.tbl_Category_Rule_Full', 'tbl_Category_Rule'
	--tbl_Variable
	EXEC sp_rename 'dbo.PK_tbl_Variable_Full', 'PK_tbl_Variable'  
	EXEC sp_rename 'dbo.tbl_Variable_Full', 'tbl_Variable'
	--tbl_Variable_SpeedRunComID
    --EXEC sp_rename 'dbo.IDX_tbl_Variable_SpeedRunComID_Full_SpeedRunComID', 'IDX_tbl_Variable_SpeedRunComID_SpeedRunComID'                                    
	EXEC sp_rename 'dbo.PK_tbl_Variable_SpeedRunComID_Full', 'PK_tbl_Variable_SpeedRunComID'  
	EXEC sp_rename 'dbo.tbl_Variable_SpeedRunComID_Full', 'tbl_Variable_SpeedRunComID'
	--tbl_VariableValue
	EXEC sp_rename 'dbo.PK_tbl_VariableValue_Full', 'PK_tbl_VariableValue'  
	EXEC sp_rename 'dbo.tbl_VariableValue_Full', 'tbl_VariableValue'
	--tbl_VariableValue_SpeedRunComID
    --EXEC sp_rename 'dbo.IDX_tbl_VariableValue_SpeedRunComID_Full_SpeedRunComID', 'IDX_tbl_VariableValue_SpeedRunComID_SpeedRunComID'                                       
	EXEC sp_rename 'dbo.PK_tbl_VariableValue_SpeedRunComID_Full', 'PK_tbl_VariableValue_SpeedRunComID'  
	EXEC sp_rename 'dbo.tbl_VariableValue_SpeedRunComID_Full', 'tbl_VariableValue_SpeedRunComID'
	--tbl_Game_Platform
	EXEC sp_rename 'dbo.PK_tbl_Game_Platform_Full', 'PK_tbl_Game_Platform'  
	EXEC sp_rename 'dbo.tbl_Game_Platform_Full', 'tbl_Game_Platform'
	--tbl_Game_Region
	EXEC sp_rename 'dbo.PK_tbl_Game_Region_Full', 'PK_tbl_Game_Region'  
	EXEC sp_rename 'dbo.tbl_Game_Region_Full', 'tbl_Game_Region'
	--tbl_Game_Moderator
	EXEC sp_rename 'dbo.PK_tbl_Game_Moderator_Full', 'PK_tbl_Game_Moderator'  
	EXEC sp_rename 'dbo.tbl_Game_Moderator_Full', 'tbl_Game_Moderator'
	--tbl_Game_TimingMethod
	EXEC sp_rename 'dbo.PK_tbl_Game_TimingMethod_Full', 'PK_tbl_Game_TimingMethod'  
	EXEC sp_rename 'dbo.tbl_Game_TimingMethod_Full', 'tbl_Game_TimingMethod'
	--tbl_Game_Ruleset
	EXEC sp_rename 'dbo.PK_tbl_Game_Ruleset_Full', 'PK_tbl_Game_Ruleset'  
	EXEC sp_rename 'dbo.tbl_Game_Ruleset_Full', 'tbl_Game_Ruleset'
	--tbl_SpeedRun
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Full', 'PK_tbl_SpeedRun'                                
	EXEC sp_rename 'dbo.DF_tbl_SpeedRun_Full_ImportedDate', 'DF_tbl_SpeedRun_ImportedDate'
	EXEC sp_rename 'dbo.tbl_SpeedRun_Full', 'tbl_SpeedRun'
	--tbl_SpeedRun_SpeedRunComID   
    --EXEC sp_rename 'dbo.IDX_tbl_SpeedRun_SpeedRunComID_Full_SpeedRunComID', 'IDX_tbl_SpeedRun_SpeedRunComID_SpeedRunComID'                                       
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_SpeedRunComID_Full', 'PK_tbl_SpeedRun_SpeedRunComID'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_SpeedRunComID_Full', 'tbl_SpeedRun_SpeedRunComID'
	--tbl_SpeedRun_System
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_System_Full', 'PK_tbl_SpeedRun_System'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_System_Full', 'tbl_SpeedRun_System'  
	--tbl_SpeedRun_Time
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Time_Full', 'PK_tbl_SpeedRun_Time'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Time_Full', 'tbl_SpeedRun_Time' 
	--tbl_SpeedRun_Link
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Link_Full', 'PK_tbl_SpeedRun_Link'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Link_Full', 'tbl_SpeedRun_Link'   
	--tbl_SpeedRun_Comment
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Comment_Full', 'PK_tbl_SpeedRun_Comment'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Comment_Full', 'tbl_SpeedRun_Comment'   
	--tbl_SpeedRun_Player
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Player_Full', 'PK_tbl_SpeedRun_Player'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Player_Full', 'tbl_SpeedRun_Player'   
	--tbl_SpeedRun_Guest
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Guest_Full', 'PK_tbl_SpeedRun_Guest'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Guest_Full', 'tbl_SpeedRun_Guest'      
	--tbl_SpeedRun_VariableValue
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_VariableValue_Full', 'PK_tbl_SpeedRun_VariableValue'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_VariableValue_Full', 'tbl_SpeedRun_VariableValue' 
	--tbl_SpeedRun_Video
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Video_Full', 'PK_tbl_SpeedRun_Video'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Video_Full', 'tbl_SpeedRun_Video'    
	--tbl_SpeedRun_Video_Detail
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Video_Detail_Full', 'PK_tbl_SpeedRun_Video_Detail'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Video_Detail_Full', 'tbl_SpeedRun_Video_Detail'  

	--Add Foreign Keys
	ALTER TABLE [dbo].[tbl_Game_Platform] ADD CONSTRAINT [FK_tbl_Game_Platform_tbl_Platform] FOREIGN KEY ([PlatformID]) REFERENCES [dbo].[tbl_Platform] ([ID])
	ALTER TABLE [dbo].[tbl_SpeedRun_System] ADD CONSTRAINT [FK_tbl_SpeedRun_System_tbl_Platform] FOREIGN KEY ([PlatformID]) REFERENCES [dbo].[tbl_Platform] ([ID])
	ALTER TABLE [dbo].[tbl_User] ADD CONSTRAINT [FK_tbl_User_tbl_UserRole] FOREIGN KEY ([UserRoleID]) REFERENCES [dbo].[tbl_UserRole] ([ID])
	ALTER TABLE [dbo].[tbl_Game_Moderator] ADD CONSTRAINT [FK_tbl_Game_Moderator_tbl_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[tbl_User] ([ID])
	ALTER TABLE [dbo].[tbl_Level] ADD CONSTRAINT [FK_tbl_Level_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
	ALTER TABLE [dbo].[tbl_Category] ADD CONSTRAINT [FK_tbl_Category_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
	ALTER TABLE [dbo].[tbl_Variable] ADD CONSTRAINT [FK_tbl_Variable_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
	ALTER TABLE [dbo].[tbl_VariableValue] ADD CONSTRAINT [FK_tbl_VariableValue_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
	ALTER TABLE [dbo].[tbl_Game_Platform] ADD CONSTRAINT [FK_tbl_Game_Platform_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
	ALTER TABLE [dbo].[tbl_Game_Region] ADD CONSTRAINT [FK_tbl_Game_Region_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
	ALTER TABLE [dbo].[tbl_Game_Moderator] ADD CONSTRAINT [FK_tbl_Game_Moderator_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
	ALTER TABLE [dbo].[tbl_Game_TimingMethod] ADD CONSTRAINT [FK_tbl_Game_TimingMethod_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
	ALTER TABLE [dbo].[tbl_SpeedRun] ADD CONSTRAINT [FK_tbl_SpeedRun_tbl_Game] FOREIGN KEY ([GameID]) REFERENCES [dbo].[tbl_Game] ([ID])
	ALTER TABLE [dbo].[tbl_Variable] ADD CONSTRAINT [FK_tbl_Variable_tbl_Level] FOREIGN KEY ([LevelID]) REFERENCES [dbo].[tbl_Level] ([ID])
	ALTER TABLE [dbo].[tbl_SpeedRun] ADD CONSTRAINT [FK_tbl_SpeedRun_tbl_RunStatusType] FOREIGN KEY ([StatusTypeID]) REFERENCES [dbo].[tbl_RunStatusType] ([ID])		
	ALTER TABLE [dbo].[tbl_SpeedRun] ADD CONSTRAINT [FK_tbl_SpeedRun_tbl_Level] FOREIGN KEY ([LevelID]) REFERENCES [dbo].[tbl_Level] ([ID])
	ALTER TABLE [dbo].[tbl_Category] ADD CONSTRAINT [FK_tbl_Category_tbl_CategoryType] FOREIGN KEY ([CategoryTypeID]) REFERENCES [dbo].[tbl_CategoryType] ([ID])
	ALTER TABLE [dbo].[tbl_Variable] ADD CONSTRAINT [FK_tbl_Variable_tbl_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[tbl_Category] ([ID])
	ALTER TABLE [dbo].[tbl_SpeedRun] ADD CONSTRAINT [FK_tbl_SpeedRun_tbl_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[tbl_Category] ([ID])
	ALTER TABLE [dbo].[tbl_Variable] ADD CONSTRAINT [FK_tbl_Variable_tbl_VariableScopeType] FOREIGN KEY ([VariableScopeTypeID]) REFERENCES [dbo].[tbl_VariableScopeType] ([ID])
	--ALTER TABLE [dbo].[tbl_VariableValue] ADD CONSTRAINT [FK_tbl_VariableValue_tbl_Variable] FOREIGN KEY ([VariableID]) REFERENCES [dbo].[tbl_Variable] ([ID])
	--ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue] ADD CONSTRAINT [FK_tbl_SpeedRun_VariableValue_tbl_Variable] FOREIGN KEY ([VariableID]) REFERENCES [dbo].[tbl_Variable] ([ID])
	--ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue] ADD CONSTRAINT [FK_tbl_SpeedRun_VariableValue_tbl_VariableValue] FOREIGN KEY ([VariableValueID]) REFERENCES [dbo].[tbl_VariableValue] ([ID])
    ALTER TABLE [dbo].[tbl_Game_Region] ADD CONSTRAINT [FK_tbl_Game_Region_tbl_Region] FOREIGN KEY ([RegionID]) REFERENCES [dbo].[tbl_Region] ([ID])
    ALTER TABLE [dbo].[tbl_Game_TimingMethod] ADD CONSTRAINT [FK_tbl_Game_TimingMethod_tbl_TimingMethod] FOREIGN KEY ([TimingMethodID]) REFERENCES [dbo].[tbl_TimingMethod] ([ID])
	ALTER TABLE [dbo].[tbl_SpeedRun_Player] ADD CONSTRAINT [FK_tbl_SpeedRun_Player_tbl_SpeedRun] FOREIGN KEY ([SpeedRunID]) REFERENCES [dbo].[tbl_SpeedRun] ([ID])                           
    ALTER TABLE [dbo].[tbl_SpeedRun_Player] ADD CONSTRAINT [FK_tbl_SpeedRun_Player_tbl_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[tbl_User] ([ID])
	ALTER TABLE [dbo].[tbl_SpeedRun_Guest] ADD CONSTRAINT [FK_tbl_SpeedRun_Guest_tbl_SpeedRun] FOREIGN KEY ([SpeedRunID]) REFERENCES [dbo].[tbl_SpeedRun] ([ID])                           
    ALTER TABLE [dbo].[tbl_SpeedRun_Guest] ADD CONSTRAINT [FK_tbl_SpeedRun_Guest_tbl_Guest] FOREIGN KEY ([GuestID]) REFERENCES [dbo].[tbl_Guest] ([ID])
    ALTER TABLE [dbo].[tbl_SpeedRun_VariableValue] ADD CONSTRAINT [FK_tbl_SpeedRun_VariableValue_tbl_SpeedRun] FOREIGN KEY ([SpeedRunID]) REFERENCES [dbo].[tbl_SpeedRun] ([ID])
	ALTER TABLE [dbo].[tbl_SpeedRun_Video] ADD CONSTRAINT [FK_tbl_SpeedRun_Video_tbl_SpeedRun] FOREIGN KEY ([SpeedRunID]) REFERENCES [dbo].[tbl_SpeedRun] ([ID])
	ALTER TABLE [dbo].[tbl_SpeedRun_System] ADD CONSTRAINT [FK_tbl_SpeedRun_System_tbl_Region] FOREIGN KEY ([RegionID]) REFERENCES [dbo].[tbl_Region] ([ID])

    --SearchUsers
    CREATE NONCLUSTERED INDEX [IDX_tbl_User_Name_PlusInclude] ON [dbo].[tbl_User] ([Name]) INCLUDE ([Abbr]) WITH (FILLFACTOR=90) ON [PRIMARY]
    CREATE NONCLUSTERED INDEX [IDX_tbl_Guest_Name_PlusInclude] ON [dbo].[tbl_Guest] ([Name]) INCLUDE ([Abbr]) WITH (FILLFACTOR=90) ON [PRIMARY]	    
    --SearchGames
    CREATE NONCLUSTERED INDEX [IDX_tbl_Game_Name_PlusInclude] ON [dbo].[tbl_Game] ([Name]) INCLUDE ([Abbr]) WITH (FILLFACTOR=90) ON [PRIMARY]	
    --vw_Game
    CREATE NONCLUSTERED INDEX [IDX_tbl_Level_GameID_PlusInclude] ON [dbo].[tbl_Level] ([GameID]) INCLUDE ([Name]) WITH (FILLFACTOR=90) ON [PRIMARY]	
    CREATE NONCLUSTERED INDEX [IDX_tbl_Category_GameID_CategoryTypeID_PlusInclude] ON [dbo].[tbl_Category] ([GameID],[CategoryTypeID]) INCLUDE ([Name]) WITH (FILLFACTOR=90) ON [PRIMARY]
    CREATE NONCLUSTERED INDEX [IDX_tbl_Variable_GameID] ON [dbo].[tbl_Variable] ([GameID]) WITH (FILLFACTOR=90) ON [PRIMARY]
    CREATE NONCLUSTERED INDEX [IDX_tbl_VariableValue_GameID_PlusInclude] ON [dbo].[tbl_VariableValue] ([GameID]) INCLUDE ([VariableID],[Value]) WITH (FILLFACTOR=90) ON [PRIMARY]	
    CREATE NONCLUSTERED INDEX [IDX_tbl_Game_Platform_GameID_PlatformID] ON [dbo].[tbl_Game_Platform] ([GameID],[PlatformID]) WITH (FILLFACTOR=90) ON [PRIMARY]
    CREATE NONCLUSTERED INDEX [IDX_tbl_Game_Moderator_GameID_UserID] ON [dbo].[tbl_Game_Moderator] ([GameID],[UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]
    --vw_SpeedRunGrid
	CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_GameID_CategoryID_LevelID_Rank_PlusInclude] ON [dbo].[tbl_SpeedRun] ([GameID],[CategoryID],[LevelID],[Rank]) INCLUDE ([PrimaryTime],[DateSubmitted],[VerifyDate]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also GetPersonalBestsByUserID, GetSpeedRunsByUserID, vw_SpeedRunSummary	
	CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_VariableValue_SpeedRunID_VariableValueID_PlusInclude] ON [dbo].[tbl_SpeedRun_VariableValue] ([SpeedRunID],[VariableValueID]) INCLUDE ([VariableID]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also vw_SpeedRunSummary, ImportUpdateSpeedRunRanks    
	CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_VariableValue_SpeedRunID_VariableID_VariableValueID] ON [dbo].[tbl_SpeedRun_VariableValue] ([SpeedRunID],[VariableID],[VariableValueID]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also vw_SpeedRunSummary, ImportUpdateSpeedRunRanks    
    CREATE NONCLUSTERED INDEX [IDX_tbl_Variable_IsSubCategory] ON [dbo].[tbl_Variable] ([IsSubCategory]) WITH (FILLFACTOR=90) ON [PRIMARY]	
    CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Player_SpeedRunID_UserID] ON [dbo].[tbl_SpeedRun_Player] ([SpeedRunID],[UserID]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also vw_SpeedRunGridTabUser, vw_User
    CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Guest_SpeedRunID_GuestID] ON [dbo].[tbl_SpeedRun_Guest] ([SpeedRunID],[GuestID]) WITH (FILLFACTOR=90) ON [PRIMARY] 
    --vw_SpeedRunSummary
    CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Video_SpeedRunID_EmbeddedVideoLinkUrl_PlusInclude] ON [dbo].[tbl_SpeedRun_Video] ([SpeedRunID],[EmbeddedVideoLinkUrl]) INCLUDE ([ThumbnailLinkUrl]) WITH (FILLFACTOR=90) ON [PRIMARY] --Also Import Game
    CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Video_Detail_SpeedRunID] ON [dbo].[tbl_SpeedRun_Video_Detail] ([SpeedRunID]) WITH (FILLFACTOR=90) ON [PRIMARY]   
    --vw_User
    CREATE NONCLUSTERED INDEX [IDX_tbl_SpeedRun_Player_UserID] ON [dbo].[tbl_SpeedRun_Player] ([UserID]) WITH (FILLFACTOR=90) ON [PRIMARY]
    
END
GO

--ImportReorderSpeedRuns
IF OBJECT_ID('dbo.ImportReorderSpeedRuns') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.ImportReorderSpeedRuns
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ImportReorderSpeedRuns]
AS
BEGIN
    DECLARE @BatchCount INT = 1000
    DECLARE @RaiseMsg VARCHAR (1000)
    DECLARE @RowCount INT = 0
    DECLARE @MaxRowCount INT

    SELECT @MaxRowCount = COUNT(*)
    FROM dbo.tbl_SpeedRun_Full

    IF OBJECT_ID('tempdb..#IDsToProcess') IS NOT NULL DROP TABLE #IDsToProcess
    CREATE TABLE #IDsToProcess ([ID] INT)

    IF OBJECT_ID('tempdb..#InsertedIDs') IS NOT NULL DROP TABLE #InsertedIDs
    CREATE TABLE #InsertedIDs ([NewID] INT, [OldID] INT)

    IF OBJECT_ID('tempdb..#InsertedVideoIDs') IS NOT NULL DROP TABLE #InsertedVideoIDs
    CREATE TABLE #InsertedVideoIDs ([NewVideoID] INT, [OldVideoID] INT, [NewID] INT)    

    INSERT INTO #IDsToProcess (ID)
    SELECT TOP (@BatchCount) ID
    FROM dbo.tbl_SpeedRun_Full
    WHERE ISNULL(IsProcessed, 0) = 0
    ORDER BY ISNULL(VerifyDate, DateSubmitted)

    WHILE EXISTS (SELECT 1 FROM #IDsToProcess)
    BEGIN
        MERGE INTO dbo.tbl_SpeedRun_Full_Ordered USING (SELECT rn.ID, StatusTypeID, GameID, CategoryID, LevelID, [Rank], PrimaryTime, RunDate, DateSubmitted, VerifyDate FROM dbo.tbl_SpeedRun_Full rn JOIN #IDsToProcess rn1 ON rn1.ID = rn.ID) AS td1 ON 1 = 0
        WHEN NOT MATCHED THEN
        INSERT (StatusTypeID, GameID, CategoryID, LevelID, [Rank], PrimaryTime, RunDate, DateSubmitted, VerifyDate)
        VALUES (td1.StatusTypeID, td1.GameID, td1.CategoryID, td1.LevelID, td1.[Rank], td1.PrimaryTime, td1.RunDate, td1.DateSubmitted, td1.VerifyDate)
        OUTPUT inserted.ID, td1.ID
        INTO #InsertedIDs ([NewID], OldID);

        INSERT INTO dbo.tbl_SpeedRun_SpeedRunComID_Full_Ordered (SpeedRunID, SpeedRunComID)
        SELECT dn.[NewID], rn1.SpeedRunComID
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_SpeedRunComID_Full rn1 ON rn1.SpeedRunID = dn.OldID

        INSERT INTO dbo.tbl_SpeedRun_System_Full_Ordered (SpeedRunID, PlatformID, RegionID, IsEmulated)
        SELECT dn.[NewID], rs.PlatformID, rs.RegionID, rs.IsEmulated
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_System_Full rs ON rs.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Time_Full_Ordered (SpeedRunID, PrimaryTime, RealTime, RealTimeWithoutLoads, GameTime)
        SELECT dn.[NewID], rt.PrimaryTime, rt.RealTime, rt.RealTimeWithoutLoads, rt.GameTime
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Time_Full rt ON rt.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Link_Full_Ordered (SpeedRunID, SpeedRunComUrl, SplitsUrl)
        SELECT dn.[NewID], rl.SpeedRunComUrl, rl.SplitsUrl
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Link_Full rl ON rl.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Comment_Full_Ordered (SpeedRunID, Comment)
        SELECT dn.[NewID], rc.Comment
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Comment_Full rc ON rc.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Player_Full_Ordered (SpeedRunID, UserID)
        SELECT dn.[NewID], rd.UserID
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Player_Full rd ON rd.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Guest_Full_Ordered (SpeedRunID, GuestID)
        SELECT dn.[NewID], rg.GuestID
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Guest_Full rg ON rg.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_VariableValue_Full_Ordered (SpeedRunID, VariableID, VariableValueID)
        SELECT dn.[NewID], rv.VariableID, rv.VariableValueID
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_VariableValue_Full rv ON rv.SpeedRunID = dn.[OldID] 

        MERGE INTO dbo.tbl_SpeedRun_Video_Full_Ordered USING (SELECT dn.[NewID], rv.ID, rv.VideoLinkUrl, rv.EmbeddedVideoLinkUrl, rv.ThumbnailLinkUrl FROM #InsertedIDs dn JOIN dbo.tbl_SpeedRun_Video_Full rv ON rv.SpeedRunID = dn.[OldID]) AS td1 ON 1 = 0
        WHEN NOT MATCHED THEN
        INSERT (SpeedRunID, VideoLinkUrl, EmbeddedVideoLinkUrl, ThumbnailLinkUrl)
        VALUES (td1.NewID, td1.VideoLinkUrl, td1.EmbeddedVideoLinkUrl, td1.ThumbnailLinkUrl)
        OUTPUT inserted.ID, td1.ID, td1.[NewID]
        INTO #InsertedVideoIDs ([NewVideoID], [OldVideoID], [NewID]);    

        INSERT INTO dbo.tbl_SpeedRun_Video_Detail_Full_Ordered (SpeedRunVideoID, SpeedRunID, ChannelID, ViewCount)
        SELECT dn.[NewVideoID], dn.[NewID], rd.ChannelID, rd.ViewCount
        FROM #InsertedVideoIDs dn
        JOIN dbo.tbl_SpeedRun_Video_Detail_Full rd ON rd.SpeedRunVideoID = dn.[OldVideoID] 

        UPDATE rn SET
        [rn].[IsProcessed] = 1
        FROM dbo.tbl_SpeedRun_Full rn
        JOIN #IDsToProcess rn1 ON rn1.ID = rn.ID

        /*
        DELETE rl
        FROM dbo.tbl_SpeedRun_Link_Full rl
        JOIN #IDsToProcess rn1 ON rn1.ID = rl.SpeedRunID

        DELETE rs
        FROM dbo.tbl_SpeedRun_System_Full rs
        JOIN #IDsToProcess rn1 ON rn1.ID = rs.SpeedRunID

        DELETE rt
        FROM dbo.tbl_SpeedRun_Time_Full rt
        JOIN #IDsToProcess rn1 ON rn1.ID = rt.SpeedRunID

        DELETE rc
        FROM dbo.tbl_SpeedRun_Comment_Full rc
        JOIN #IDsToProcess rn1 ON rn1.ID = rc.SpeedRunID

        DELETE rv
        FROM dbo.tbl_SpeedRun_VariableValue_Full rv
        JOIN #IDsToProcess rn1 ON rn1.ID = rv.SpeedRunID

        DELETE rp
        FROM dbo.tbl_SpeedRun_Player_Full rp
        JOIN #IDsToProcess rn1 ON rn1.ID = rp.SpeedRunID

        DELETE rg
        FROM dbo.tbl_SpeedRun_Guest_Full rg
        JOIN #IDsToProcess rn1 ON rn1.ID = rg.SpeedRunID

        DELETE rd
        FROM dbo.tbl_SpeedRun_Video_Detail_Full rd
        JOIN dbo.tbl_SpeedRun_Video_Full rv ON rv.ID = rd.SpeedRunVideoID
        JOIN #IDsToProcess rn1 ON rn1.ID = rv.SpeedRunID

        DELETE rv
        FROM dbo.tbl_SpeedRun_Video_Full rv
        JOIN #IDsToProcess rn1 ON rn1.ID = rv.SpeedRunID

        DELETE rc
        FROM dbo.tbl_SpeedRun_SpeedRunComID_Full rc
        JOIN #IDsToProcess rn1 ON rn1.ID = rc.SpeedRunID        

        DELETE rn
        FROM dbo.tbl_SpeedRun_Full rn
        JOIN #IDsToProcess rn1 ON rn1.ID = rn.ID     
        */
        
        -- SELECT @RowCount = @RowCount + COUNT(*)
        -- FROM #IDsToProcess

        -- SELECT @RaiseMsg = 'Processed ' + CONVERT(VARCHAR, @RowCount) + '/' + CONVERT(VARCHAR, @MaxRowCount)
        -- RAISERROR(@RaiseMsg, 0, 1) WITH NOWAIT;

        TRUNCATE TABLE #IDsToProcess
        TRUNCATE TABLE #InsertedIDs
        TRUNCATE TABLE #InsertedVideoIDs

        INSERT INTO #IDsToProcess (ID)
        SELECT TOP (@BatchCount) ID
        FROM dbo.tbl_SpeedRun_Full
        WHERE ISNULL(IsProcessed, 0) = 0
        ORDER BY ISNULL(VerifyDate, DateSubmitted)
    END

	DROP TABLE dbo.tbl_SpeedRun_Full
	DROP TABLE dbo.tbl_SpeedRun_SpeedRunComID_Full
	DROP TABLE dbo.tbl_SpeedRun_System_Full
	DROP TABLE dbo.tbl_SpeedRun_Time_Full
	DROP TABLE dbo.tbl_SpeedRun_Link_Full
	DROP TABLE dbo.tbl_SpeedRun_Comment_Full
	DROP TABLE dbo.tbl_SpeedRun_Player_Full
   	DROP TABLE dbo.tbl_SpeedRun_Guest_Full
	DROP TABLE dbo.tbl_SpeedRun_VariableValue_Full
	DROP TABLE dbo.tbl_SpeedRun_Video_Full
	DROP TABLE dbo.tbl_SpeedRun_Video_Detail_Full
    
	--PK_tbl_SpeedRun_Full_Ordered
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Full_Ordered', 'PK_tbl_SpeedRun_Full'                                
	EXEC sp_rename 'dbo.DF_tbl_SpeedRun_Full_Ordered_ImportedDate', 'DF_tbl_SpeedRun_Full_ImportedDate'
	EXEC sp_rename 'dbo.tbl_SpeedRun_Full_Ordered', 'tbl_SpeedRun_Full'
	--PK_tbl_SpeedRun_SpeedRunComID_Full_Ordered   
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_SpeedRunComID_Full_Ordered', 'PK_tbl_SpeedRun_SpeedRunComID_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_SpeedRunComID_Full_Ordered', 'tbl_SpeedRun_SpeedRunComID_Full'
	--tbl_SpeedRun_System_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_System_Full_Ordered', 'PK_tbl_SpeedRun_System_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_System_Full_Ordered', 'tbl_SpeedRun_System_Full'  
	--tbl_SpeedRun_Time_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Time_Full_Ordered', 'PK_tbl_SpeedRun_Time_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Time_Full_Ordered', 'tbl_SpeedRun_Time_Full' 
	--tbl_SpeedRun_Link_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Link_Full_Ordered', 'PK_tbl_SpeedRun_Link_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Link_Full_Ordered', 'tbl_SpeedRun_Link_Full'   
	--tbl_SpeedRun_Comment_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Comment_Full_Ordered', 'PK_tbl_SpeedRun_Comment_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Comment_Full_Ordered', 'tbl_SpeedRun_Comment_Full'   
	--tbl_SpeedRun_Player_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Player_Full_Ordered', 'PK_tbl_SpeedRun_Player_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Player_Full_Ordered', 'tbl_SpeedRun_Player_Full'   
	--tbl_SpeedRun_Guest_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Guest_Full_Ordered', 'PK_tbl_SpeedRun_Guest_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Guest_Full_Ordered', 'tbl_SpeedRun_Guest_Full'      
	--tbl_SpeedRun_VariableValue_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_VariableValue_Full_Ordered', 'PK_tbl_SpeedRun_VariableValue_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_VariableValue_Full_Ordered', 'tbl_SpeedRun_VariableValue_Full' 
	--tbl_SpeedRun_Video_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Video_Full_Ordered', 'PK_tbl_SpeedRun_Video_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Video_Full_Ordered', 'tbl_SpeedRun_Video_Full'    
	--tbl_SpeedRun_Video_Detail_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Video_Detail_Full_Ordered', 'PK_tbl_SpeedRun_Video_Detail_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Video_Detail_Full_Ordered', 'tbl_SpeedRun_Video_Detail_Full'
END
GO

--ImportUpdateSpeedRunRanks
IF OBJECT_ID('dbo.ImportUpdateSpeedRunRanks') IS NOT NULL 
BEGIN 
    DROP PROCEDURE dbo.ImportUpdateSpeedRunRanks
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[ImportUpdateSpeedRunRanks]
(
    @LastImportDate DATETIME
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    DECLARE @CurrDate DATETIME = GETDATE()  
    DECLARE @Debug BIT = '0'

    IF OBJECT_ID('tempdb..#LeaderboardKeys') IS NOT NULL 
    BEGIN 
        DROP TABLE #LeaderboardKeys
    END

    CREATE TABLE #LeaderboardKeys 
    ( 
          [GameID] INT,
          [CategoryID] INT,
          [LevelID] INT,
          [SubCategoryVariableValues] [varchar] (150)
    )
	CREATE NONCLUSTERED INDEX [IDX_LeaderboardKeys_GameID_CategoryID] ON #LeaderboardKeys ([GameID], [CategoryID]) INCLUDE ([LevelID])

    IF OBJECT_ID('tempdb..#SpeedRunsToUpdate') IS NOT NULL 
    BEGIN 
        DROP TABLE #SpeedRunsToUpdate
    END

    CREATE TABLE #SpeedRunsToUpdate 
    ( 
          [ID] INT,
          [GameID] INT,
          [CategoryID] INT,
          [LevelID] INT,
          [SubCategoryVariableValues] [varchar] (150),
          [PlayerIDs] [varchar] (150),
          [GuestIDs] [varchar] (150),
          [PrimaryTime] [bigint],
          [RankPriority] [int]
    )

    IF OBJECT_ID('tempdb..#SpeedRunsRanked') IS NOT NULL 
    BEGIN 
        DROP TABLE #SpeedRunsRanked
    END

    CREATE TABLE #SpeedRunsRanked 
    ( 
          [ID] INT,
          [Rank] INT
    )

	INSERT INTO #LeaderboardKeys (GameID, CategoryID, LevelID)
	SELECT rn.GameID, rn.CategoryID, rn.LevelID
	FROM dbo.tbl_SpeedRun rn WITH (NOLOCK)
	WHERE ISNULL(rn.ModifiedDate, rn.ImportedDate) >= @LastImportDate
	GROUP BY rn.GameID, rn.CategoryID, rn.LevelID

	INSERT INTO #LeaderboardKeys (GameID, CategoryID, LevelID)
	SELECT g.ID, c.ID, l.ID
	FROM dbo.tbl_Game g WITH (NOLOCK)
	JOIN dbo.tbl_Category c WITH (NOLOCK) ON c.GameID = g.ID
	LEFT JOIN dbo.tbl_Level l WITH (NOLOCK) ON l.GameID = g.ID  
	WHERE ISNULL(g.ModifiedDate, g.ImportedDate) >= @LastImportDate
    AND NOT EXISTS (SELECT 1 FROM #LeaderboardKeys WHERE GameID = g.ID AND CategoryID = c.ID AND ISNULL(LevelID,'') = ISNULL(l.ID,''))
	GROUP BY g.ID, c.ID, l.ID

    INSERT INTO #SpeedRunsToUpdate(ID, GameID, CategoryID, LevelID, SubCategoryVariableValues, PlayerIDs, GuestIDs, PrimaryTime, RankPriority)
    SELECT rn.ID, rn.GameID, rn.CategoryID, rn.LevelID, SubCategoryVariableValues.[Value], PlayerIDs.[Value], GuestIDs.[Value], rn.PrimaryTime,
    ROW_NUMBER() OVER (PARTITION BY rn.GameID, rn.CategoryID, rn.LevelID, SubCategoryVariableValues.[Value], PlayerIDs.[Value], GuestIDs.[Value] ORDER BY rn.PrimaryTime)
    FROM dbo.tbl_SpeedRun rn WITH (NOLOCK)
    OUTER APPLY (SELECT STUFF(
            (   SELECT ',' + CONVERT(VARCHAR, rv.[VariableID]) + '|' + CONVERT(VARCHAR, rv.VariableValueID)
                FROM dbo.tbl_SpeedRun_VariableValue rv WITH (NOLOCK)
                JOIN dbo.tbl_Variable v WITH (NOLOCK) ON v.ID = rv.VariableID AND v.IsSubCategory = 1
                WHERE rv.SpeedRunID = rn.ID
                ORDER BY rv.ID
                FOR XML PATH ('')
            ), 1, 1, '') AS [Value] 
    ) AS SubCategoryVariableValues 
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rp.UserID)
                        FROM dbo.tbl_SpeedRun_Player rp WITH (NOLOCK)
                        WHERE rp.SpeedRunID = rn.ID
                        ORDER BY rp.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS PlayerIDs
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rg.GuestID)
                        FROM dbo.tbl_SpeedRun_Guest rg WITH (NOLOCK)
                        WHERE rg.SpeedRunID = rn.ID
                        ORDER BY rg.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS GuestIDs                                
    WHERE EXISTS (SELECT 1 FROM #LeaderboardKeys lb WHERE lb.GameID = rn.GameID AND lb.CategoryID = rn.CategoryID AND ISNULL(lb.LevelID,'') = ISNULL(rn.LevelID,''))
    
    INSERT INTO #SpeedRunsRanked(ID, [Rank])
    SELECT ID, RANK() OVER (PARTITION BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues ORDER BY rn.PrimaryTime)
    FROM #SpeedRunsToUpdate rn
    WHERE rn.RankPriority = 1
    AND ISNULL(PlayerIDs, GuestIDs) IS NOT NULL
        
    IF(@Debug = 0)
    BEGIN
        UPDATE rn SET
        [Rank] = NULL
        FROM dbo.tbl_SpeedRun rn         
        WHERE EXISTS (SELECT 1 FROM #SpeedRunsToUpdate rn1 WHERE rn1.ID = rn.ID)

        UPDATE rn SET
        [Rank] = rn1.[Rank]
        FROM dbo.tbl_SpeedRun rn
        JOIN #SpeedRunsRanked rn1 ON rn1.ID = rn.ID
    END
    ELSE
    BEGIN
        SELECT rn.RankPriority, rn.ID, rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn.PlayerIDs, rn.GuestIDs, rn.PrimaryTime
        FROM #SpeedRunsToUpdate rn
        where rn.SubCategoryVariableValues is not null
        ORDER BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn.PlayerIDs, rn.GuestIDs, rn.RankPriority

        SELECT rn1.[Rank], rn.ID, rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn.PlayerIDs, rn.GuestIDs, rn.PrimaryTime
        FROM #SpeedRunsToUpdate rn
        JOIN #SpeedRunsRanked rn1 ON rn1.ID = rn.ID
        where rn.SubCategoryVariableValues is not null
        ORDER BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn1.[Rank]
    END

END
GO

/*********************************************/
--populate tables
/*********************************************/
DECLARE @currdate DATETIME = GETDATE()

INSERT INTO tbl_CategoryType (ID, [Name])
SELECT '0', 'PerGame'
UNION ALL
SELECT '1', 'PerLevel'

INSERT INTO tbl_VariableScopeType (ID, [Name])
SELECT '0', 'Global'
UNION ALL
SELECT '1', 'FullGame'
UNION ALL
SELECT '2', 'AllLevels'
UNION ALL
SELECT '3', 'SingleLevel'

INSERT INTO tbl_Region (ID, [Name], Abbreviation)
SELECT '1','BRA / PAL','BRA'
UNION ALL
SELECT '2','CHN / PAL','CHN'
UNION ALL
SELECT '3','EUR / PAL','PAL'
UNION ALL
SELECT '4','JPN / NTSC','NTSC-J'
UNION ALL
SELECT '5','KOR / NTSC','KOR'
UNION ALL
SELECT '6','USA / NTSC','NTSC-U'

INSERT INTO tbl_Region_SpeedRunComID(RegionID, SpeedRunComID)
SELECT '1', 'ypl25l47'
UNION ALL
SELECT '2', 'mol4z19n'
UNION ALL
SELECT '3', 'e6lxy1dz'
UNION ALL
SELECT '4', 'o316x197'
UNION ALL
SELECT '5', 'p2g50lnk'
UNION ALL
SELECT '6', 'pr184lqn'

INSERT INTO tbl_Setting([Name], [Str], Num, Dte)
SELECT 'GameLastImportDate', NULL, NULL, NULL
UNION ALL
SELECT 'UserLastImportDate', NULL, NULL, NULL
UNION ALL
SELECT 'SpeedRunLastImportDate', NULL, NULL, NULL
UNION ALL
SELECT 'LeaderboardLastImportDate', NULL, NULL, NULL
UNION ALL
SELECT 'PlatformLastImportDate', NULL, NULL, NULL
UNION ALL
SELECT 'ImportLastRunDate', NULL, NULL, NULL
UNION ALL
SELECT 'UpdateSpeedRunsTime', '07:00', NULL, NULL
UNION ALL
SELECT 'TwitchToken', '0tu8l8tiw6pwwaa064wnuel47bn1fu', NULL, NULL
UNION ALL
SELECT 'TwitchAPIEnabled', NULL, '1', NULL
UNION ALL
SELECT 'YouTubeAPIEnabled', NULL, '1', NULL

INSERT INTO tbl_TimingMethod (ID, [Name])
SELECT '0', 'GameTime'
UNION ALL
SELECT '1', 'RealTime'
UNION ALL
SELECT '2', 'RealTimeWithoutLoads'

INSERT INTO tbl_UserRole (ID, [Name])
SELECT '0', 'Banned'
UNION ALL
SELECT '1', 'User'
UNION ALL
SELECT '2', 'Trusted'
UNION ALL
SELECT '3', 'Moderator'
UNION ALL
SELECT '4', 'Admin'
UNION ALL
SELECT '5', 'Programmer'
UNION ALL
SELECT '6', 'ContentModerator'

INSERT INTO tbl_RunStatusType (ID, [Name])
SELECT '0', 'New'
UNION ALL
SELECT '1', 'Verified'
UNION ALL
SELECT '2', 'Rejected'



