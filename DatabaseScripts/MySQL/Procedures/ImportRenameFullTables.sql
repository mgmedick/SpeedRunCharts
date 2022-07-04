-- ImportRenameFullTables
DROP PROCEDURE IF EXISTS ImportRenameFullTables;

DELIMITER $$
CREATE PROCEDURE ImportRenameFullTables()
BEGIN	
	-- Drop Tables
	DROP TABLE tbl_Platform;
	DROP TABLE tbl_Platform_SpeedRunComID;
	DROP TABLE tbl_User;
	DROP TABLE tbl_User_SpeedRunComID;
	DROP TABLE tbl_User_Location;
	DROP TABLE tbl_User_Link;
    DROP TABLE tbl_Guest;
	DROP TABLE tbl_Game;
	DROP TABLE tbl_Game_SpeedRunComID;
	DROP TABLE tbl_Game_Link;
	DROP TABLE tbl_Level;
	DROP TABLE tbl_Level_SpeedRunComID;
	DROP TABLE tbl_Level_Rule;
	DROP TABLE tbl_Category;
	DROP TABLE tbl_Category_SpeedRunComID;
	DROP TABLE tbl_Category_Rule;
	DROP TABLE tbl_Variable;
	DROP TABLE tbl_Variable_SpeedRunComID;
	DROP TABLE tbl_VariableValue;
	DROP TABLE tbl_VariableValue_SpeedRunComID;
	DROP TABLE tbl_Game_Platform;
	DROP TABLE tbl_Game_Region;
	DROP TABLE tbl_Game_Moderator;
	DROP TABLE tbl_Game_TimingMethod;
	DROP TABLE tbl_Game_Ruleset;
	DROP TABLE tbl_SpeedRun;
	DROP TABLE tbl_SpeedRun_SpeedRunComID;
	DROP TABLE tbl_SpeedRun_System;
	DROP TABLE tbl_SpeedRun_Time;
	DROP TABLE tbl_SpeedRun_Link;
	DROP TABLE tbl_SpeedRun_Comment;
	DROP TABLE tbl_SpeedRun_Player;
   	DROP TABLE tbl_SpeedRun_Guest;
	DROP TABLE tbl_SpeedRun_VariableValue;
	DROP TABLE tbl_SpeedRun_Video;
	DROP TABLE tbl_SpeedRun_Video_Detail;
	
    -- Rename tables
	ALTER TABLE tbl_Platform_Full RENAME tbl_Platform;
	ALTER TABLE tbl_Platform_SpeedRunComID_Full RENAME tbl_Platform_SpeedRunComID;
	ALTER TABLE tbl_User_Full RENAME tbl_User;
	ALTER TABLE tbl_User_SpeedRunComID_Full RENAME tbl_User_SpeedRunComID;
	ALTER TABLE tbl_User_Location_Full RENAME tbl_User_Location;
	ALTER TABLE tbl_User_Link_Full RENAME tbl_User_Link;
	ALTER TABLE tbl_Guest_Full RENAME tbl_Guest;
	ALTER TABLE tbl_Game_Full RENAME tbl_Game;
	ALTER TABLE tbl_Game_SpeedRunComID_Full RENAME tbl_Game_SpeedRunComID;
	ALTER TABLE tbl_Game_Link_Full RENAME tbl_Game_Link;
	ALTER TABLE tbl_Level_Full RENAME tbl_Level;
 	ALTER TABLE tbl_Level_SpeedRunComID_Full RENAME tbl_Level_SpeedRunComID;
	ALTER TABLE tbl_Level_Rule_Full RENAME tbl_Level_Rule;
	ALTER TABLE tbl_Category_Full RENAME tbl_Category;
	ALTER TABLE tbl_Category_SpeedRunComID_Full RENAME tbl_Category_SpeedRunComID;
	ALTER TABLE tbl_Category_Rule_Full RENAME tbl_Category_Rule;
	ALTER TABLE tbl_Variable_Full RENAME tbl_Variable;
	ALTER TABLE tbl_Variable_SpeedRunComID_Full RENAME tbl_Variable_SpeedRunComID;
	ALTER TABLE tbl_VariableValue_Full RENAME tbl_VariableValue;
	ALTER TABLE tbl_VariableValue_SpeedRunComID_Full RENAME tbl_VariableValue_SpeedRunComID;
	ALTER TABLE tbl_Game_Platform_Full RENAME tbl_Game_Platform;
	ALTER TABLE tbl_Game_Region_Full RENAME tbl_Game_Region;
	ALTER TABLE tbl_Game_Moderator_Full RENAME tbl_Game_Moderator;
	ALTER TABLE tbl_Game_TimingMethod_Full RENAME tbl_Game_TimingMethod;
	ALTER TABLE tbl_Game_Ruleset_Full RENAME tbl_Game_Ruleset;
	ALTER TABLE tbl_SpeedRun_Full RENAME tbl_SpeedRun;
	ALTER TABLE tbl_SpeedRun_SpeedRunComID_Full RENAME tbl_SpeedRun_SpeedRunComID;
	ALTER TABLE tbl_SpeedRun_System_Full RENAME tbl_SpeedRun_System;
	ALTER TABLE tbl_SpeedRun_Time_Full RENAME tbl_SpeedRun_Time;
	ALTER TABLE tbl_SpeedRun_Link_Full RENAME tbl_SpeedRun_Link;
	ALTER TABLE tbl_SpeedRun_Comment_Full RENAME tbl_SpeedRun_Comment;
	ALTER TABLE tbl_SpeedRun_Player_Full RENAME tbl_SpeedRun_Player;
	ALTER TABLE tbl_SpeedRun_Guest_Full RENAME tbl_SpeedRun_Guest;
	ALTER TABLE tbl_SpeedRun_VariableValue_Full RENAME tbl_SpeedRun_VariableValue;
	ALTER TABLE tbl_SpeedRun_Video_Full RENAME tbl_SpeedRun_Video;
	ALTER TABLE tbl_SpeedRun_Video_Detail_Full RENAME tbl_SpeedRun_Video_Detail;

	-- SearchUsers
	CREATE INDEX IDX_tbl_User_Name_PlusInclude ON tbl_User (Name, Abbr);
	CREATE INDEX IDX_tbl_Guest_Name_PlusInclude ON tbl_Guest (Name, Abbr);
	-- SearchUsers
	CREATE INDEX IDX_tbl_Game_Name_PlusInclude ON tbl_Game (Name, Abbr);
	-- vw_Game
	CREATE INDEX IDX_tbl_Level_GameID_PlusInclude ON tbl_Level (GameID, Name);
	CREATE INDEX IDX_tbl_Category_GameID_CategoryTypeID_PlusInclude ON tbl_Category (GameID, CategoryTypeID, Name);
	CREATE INDEX IDX_tbl_Category_CategoryTypeID_PlusInclude ON tbl_Category (CategoryTypeID, Name);
	CREATE INDEX IDX_tbl_Variable_GameID ON tbl_Variable (GameID, Name);
	CREATE INDEX IDX_tbl_VariableValue_GameID_PlusInclude ON tbl_VariableValue (GameID, VariableID, Value);
	CREATE INDEX IDX_tbl_Game_Platform_GameID_PlatformID ON tbl_Game_Platform (GameID, PlatformID);
	CREATE INDEX IDX_tbl_Game_Moderator_GameID_UserID ON tbl_Game_Moderator (GameID, UserID);
	-- vw_SpeedRunGrid
	CREATE INDEX IDX_tbl_SpeedRun_GameID_CategoryID_LevelID_Rank_PlusInclude ON tbl_SpeedRun (GameID, CategoryID, LevelID, `Rank`);
	CREATE INDEX IDX_tbl_SpeedRun_VariableValue_SpeedRunID_VariableValueID ON tbl_SpeedRun_VariableValue (SpeedRunID, VariableValueID, VariableID);
	CREATE INDEX IDX_tbl_SpeedRun_VariableValue_SpeedRunID_VariableID ON tbl_SpeedRun_VariableValue (SpeedRunID, VariableID, VariableValueID);
	CREATE INDEX IDX_tbl_Variable_IsSubCategory ON tbl_Variable (IsSubCategory);
	CREATE INDEX IDX_tbl_SpeedRun_Player_SpeedRunID_UserID ON tbl_SpeedRun_Player (SpeedRunID, UserID);
	CREATE INDEX IDX_tbl_SpeedRun_Guest_SpeedRunID_GuestID ON tbl_SpeedRun_Guest (SpeedRunID, GuestID);
	-- vw_SpeedRunSummary
	CREATE INDEX IDX_tbl_SpeedRun_Video_SpeedRunID_PlusInclude ON tbl_SpeedRun_Video (SpeedRunID, EmbeddedVideoLinkUrl, ThumbnailLinkUrl);
	CREATE INDEX IDX_tbl_SpeedRun_Video_Detail_SpeedRunID ON tbl_SpeedRun_Video_Detail (SpeedRunID);
	CREATE INDEX IDX_tbl_Category_CategoryTypeID ON tbl_Category (CategoryTypeID);
	-- vw_User
	CREATE INDEX IDX_tbl_SpeedRun_Player_UserID ON tbl_SpeedRun_Player (UserID);
END $$
DELIMITER ;
