/****** Object:  StoredProcedure [dbo].[ImportRenameFullTables]    Script Date: 4/15/2022 6:41:02 PM ******/
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


