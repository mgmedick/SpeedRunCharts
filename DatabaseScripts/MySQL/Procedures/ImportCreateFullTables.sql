-- ImportCreateFullTables
DROP PROCEDURE IF EXISTS ImportCreateFullTables;

DELIMITER $$
CREATE PROCEDURE ImportCreateFullTables()
BEGIN
	-- tbl_Platform_Full
	DROP TABLE IF EXISTS tbl_Platform_Full;
	
	CREATE TABLE tbl_Platform_Full 
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
	    Name varchar (50) NOT NULL,
	    YearOfRelease int NULL,
	    ImportedDate datetime NOT NULL DEFAULT (UTC_TIMESTAMP),
	    PRIMARY KEY (ID)   
	);
		
	-- tbl_Platform_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_Platform_SpeedRunComID_Full;
	
	CREATE TABLE tbl_Platform_SpeedRunComID_Full 
	(
		PlatformID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	    PRIMARY KEY (PlatformID)       
	);

	-- tbl_Platform_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_Platform_SpeedRunComID_Full;
	
	CREATE TABLE tbl_Platform_SpeedRunComID_Full 
	(
		PlatformID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	    PRIMARY KEY (PlatformID)       
	);

	-- tbl_User_Full
	DROP TABLE IF EXISTS tbl_User_Full;
	
	CREATE TABLE tbl_User_Full
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
	    Name varchar (100) NOT NULL,
		UserRoleID int NOT NULL,
	    SignUpDate datetime NULL,  
	    ImportedDate datetime NOT NULL DEFAULT (UTC_TIMESTAMP),
		ModifiedDate datetime NULL,
		Abbr varchar (100) NULL,
	    PRIMARY KEY (ID)
	);
	
	-- tbl_User_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_User_SpeedRunComID_Full;
	
	CREATE TABLE tbl_User_SpeedRunComID_Full
	(
		UserID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	    PRIMARY KEY (UserID)
	);
	
	-- tbl_User_Location_Full
	DROP TABLE IF EXISTS tbl_User_Location_Full;
	
	CREATE TABLE tbl_User_Location_Full
	( 
	    UserID int NOT NULL,
	    Location varchar (100) NULL,
	    PRIMARY KEY (UserID)
	);
			
	-- tbl_User_Link_Full
	DROP TABLE IF EXISTS tbl_User_Link_Full;
	
	CREATE TABLE tbl_User_Link_Full
	( 
	    UserID int NOT NULL,
		SpeedRunComUrl varchar (1000) NULL, 
	    ProfileImageUrl varchar (1000) NULL,
	    TwitchProfileUrl varchar (1000) NULL,
	    HitboxProfileUrl varchar (1000) NULL,
	    YoutubeProfileUrl varchar (1000) NULL,
	    TwitterProfileUrl varchar (1000) NULL,
	    SpeedRunsLiveProfileUrl varchar (1000) NULL,
	    PRIMARY KEY (UserID)    
	);

	-- tbl_Guest_Full
	DROP TABLE IF EXISTS tbl_Guest_Full;
	
	CREATE TABLE tbl_Guest_Full
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
	    Name nvarchar (100) NOT NULL,
	    ImportedDate datetime NOT NULL DEFAULT (UTC_TIMESTAMP),
		Abbr varchar(100) NULL,
		PRIMARY KEY (ID)
	);
	
	-- tbl_Guest_Link_Full
	DROP TABLE IF EXISTS tbl_Guest_Link_Full;
	
	CREATE TABLE tbl_Guest_Link_Full
	( 
	    GuestID int NOT NULL,
		SpeedRunComUrl varchar (1000) NULL,
	    PRIMARY KEY (GuestID)    
	);

	-- tbl_Game_Full
	DROP TABLE IF EXISTS tbl_Game_Full;
	
	CREATE TABLE tbl_Game_Full
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
	    Name varchar (100) NOT NULL,
	    IsRomHack bit NOT NULL,
	    YearOfRelease int NULL,
	    CreatedDate datetime NULL,  
	    ImportedDate datetime NOT NULL DEFAULT (UTC_TIMESTAMP),
	    ModifiedDate datetime NULL,
		Abbr varchar(100) NULL,
		IsChanged bit NULL,
		PRIMARY KEY (ID)
	);

	-- tbl_Game_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_Game_SpeedRunComID_Full;
	
	CREATE TABLE tbl_Game_SpeedRunComID_Full
	(
		GameID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	   	PRIMARY KEY (GameID) 
	);

	-- tbl_Game_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_Game_SpeedRunComID_Full;
	
	CREATE TABLE tbl_Game_SpeedRunComID_Full
	(
		GameID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	   	PRIMARY KEY (GameID) 
	);
	
	-- tbl_Game_Link_Full
	DROP TABLE IF EXISTS tbl_Game_Link_Full;
	
	CREATE TABLE tbl_Game_Link_Full
	(
	    GameID int NOT NULL,
	    SpeedRunComUrl varchar (125) NOT NULL,
	    CoverImageUrl varchar (80) NULL,
	    CoverImagePath varchar (80) NULL,
	   	PRIMARY KEY (GameID)     
	);

	-- tbl_Level_Full
	DROP TABLE IF EXISTS tbl_Level_Full;
	
	CREATE TABLE tbl_Level_Full 
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
	    Name varchar (100) NOT NULL,
	    GameID int NOT NULL,
	    PRIMARY KEY (ID) 
	);

	-- tbl_Level_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_Level_SpeedRunComID_Full;
	
	CREATE TABLE tbl_Level_SpeedRunComID_Full 
	(
		LevelID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	    PRIMARY KEY (LevelID) 
	);

	-- tbl_Level_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_Level_SpeedRunComID_Full;
	
	CREATE TABLE tbl_Level_SpeedRunComID_Full 
	(
		LevelID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	    PRIMARY KEY (LevelID) 
	);

	-- tbl_Level_Rule_Full
	DROP TABLE IF EXISTS tbl_Level_Rule_Full;
	
	CREATE TABLE tbl_Level_Rule_Full 
	( 
	    LevelID int NOT NULL, 
	    Rules varchar (8000) NULL,
	    PRIMARY KEY (LevelID) 
	); 
	
	-- tbl_Category_Full
	DROP TABLE IF EXISTS tbl_Category_Full;
	
	CREATE TABLE tbl_Category_Full
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
	    Name varchar (100) NOT NULL,
	    GameID int NOT NULL,
	    CategoryTypeID int NOT NULL,
	    PRIMARY KEY (ID)     
	);

	-- tbl_Category_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_Category_SpeedRunComID_Full;
	
	CREATE TABLE tbl_Category_SpeedRunComID_Full 
	(
		CategoryID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	    PRIMARY KEY (CategoryID)        
	);

	-- tbl_Category_Rule_Full
	DROP TABLE IF EXISTS tbl_Category_Rule_Full;
	
	CREATE TABLE tbl_Category_Rule_Full 
	( 
	    CategoryID int NOT NULL, 
	    Rules varchar (15500) NULL,
	    PRIMARY KEY (CategoryID)      
	);

	-- tbl_Variable_Full
	DROP TABLE IF EXISTS tbl_Variable_Full;
	
	CREATE TABLE tbl_Variable_Full
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
	    Name varchar (100) NOT NULL,
	    GameID int NOT NULL,
	    VariableScopeTypeID int NOT NULL,
	    CategoryID int NULL,
	    LevelID int NULL,
	    IsSubCategory bit NOT NULL,
	    PRIMARY KEY (ID)        
	);

	-- tbl_Variable_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_Variable_SpeedRunComID_Full;
	
	CREATE TABLE tbl_Variable_SpeedRunComID_Full 
	(
		VariableID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	    PRIMARY KEY (VariableID)       
	);

	-- tbl_VariableValue_Full
	DROP TABLE IF EXISTS tbl_VariableValue_Full;
	
	CREATE TABLE tbl_VariableValue_Full
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
	    GameID int NOT NULL,   
	    VariableID int NOT NULL, 
	    Value varchar (100) NOT NULL, 
	    IsCustomValue bit NOT NULL,
	    PRIMARY KEY (ID)      
	);

	-- tbl_VariableValue_Full
	DROP TABLE IF EXISTS tbl_VariableValue_Full;
	
	CREATE TABLE tbl_VariableValue_Full
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
	    GameID int NOT NULL,   
	    VariableID int NOT NULL, 
	    Value varchar (100) NOT NULL, 
	    IsCustomValue bit NOT NULL,
	    PRIMARY KEY (ID)      
	);

	-- tbl_VariableValue_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_VariableValue_SpeedRunComID_Full;
	
	CREATE TABLE tbl_VariableValue_SpeedRunComID_Full 
	(
		VariableValueID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	    PRIMARY KEY (VariableValueID)          
	);

	-- tbl_Game_Platform_Full
	DROP TABLE IF EXISTS tbl_Game_Platform_Full;
	
	CREATE TABLE tbl_Game_Platform_Full
	(     
	    ID int NOT NULL AUTO_INCREMENT, 
	    GameID int NOT NULL,
	    PlatformID int NOT NULL,
	    PRIMARY KEY (ID)     
	);
	
	-- tbl_Game_Region_Full
	DROP TABLE IF EXISTS tbl_Game_Region_Full;
	
	CREATE TABLE tbl_Game_Region_Full 
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    GameID int NOT NULL,
	    RegionID int NOT NULL,
	    PRIMARY KEY (ID)      
	);

	-- tbl_Game_Moderator_Full
	DROP TABLE IF EXISTS tbl_Game_Moderator_Full;
	
	CREATE TABLE tbl_Game_Moderator_Full 
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    GameID int NOT NULL,
	    UserID int NOT NULL,
	    PRIMARY KEY (ID)      
	);

	-- tbl_Game_TimingMethod_Full
	DROP TABLE IF EXISTS tbl_Game_TimingMethod_Full;
	
	CREATE TABLE tbl_Game_TimingMethod_Full
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    GameID int NOT NULL,
	    TimingMethodID int NOT NULL,
	  	PRIMARY KEY (ID)  
	);

	-- tbl_Game_Ruleset_Full
	DROP TABLE IF EXISTS tbl_Game_Ruleset_Full;
	
	CREATE TABLE tbl_Game_Ruleset_Full 
	( 
	    GameID int NOT NULL,
	    ShowMilliseconds bit NOT NULL,
	    RequiresVerification bit NOT NULL,
	    RequiresVideo bit NOT NULL,
	    DefaultTimingMethodID int NOT NULL,
	    EmulatorsAllowed bit NOT NULL,
	  	PRIMARY KEY (GameID)      
	);

	-- tbl_SpeedRun_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_Full;
	
	CREATE TABLE tbl_SpeedRun_Full 
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
		StatusTypeID int NOT NULL,
		GameID int NOT NULL,
		CategoryID int NOT NULL,
		LevelID int NULL,
		`Rank` int NULL,
		PrimaryTime bigint NOT NULL,
		RunDate datetime NULL,
		DateSubmitted datetime NULL,
		VerifyDate datetime NULL,
		ImportedDate datetime NOT NULL DEFAULT (UTC_TIMESTAMP),
		ModifiedDate datetime NULL,
		IsProcessed bit NULL,
	  	PRIMARY KEY (ID) 	
	);

	-- tbl_SpeedRun_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_Full_Ordered 
	( 
	    ID int NOT NULL AUTO_INCREMENT, 
		StatusTypeID int NOT NULL,
		GameID int NOT NULL,
		CategoryID int NOT NULL,
		LevelID int NULL,
		`Rank` int NULL,
		PrimaryTime bigint NOT NULL,
		RunDate datetime NULL,
		DateSubmitted datetime NULL,
		VerifyDate datetime NULL,
		ImportedDate datetime NOT NULL DEFAULT (UTC_TIMESTAMP),
		ModifiedDate datetime NULL,
	  	PRIMARY KEY (ID) 	
	);

	-- tbl_SpeedRun_SpeedRunComID_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_SpeedRunComID_Full;
	
	CREATE TABLE tbl_SpeedRun_SpeedRunComID_Full 
	(
		SpeedRunID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	  	PRIMARY KEY (SpeedRunID)     
	);

	-- tbl_SpeedRun_SpeedRunComID_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_SpeedRunComID_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_SpeedRunComID_Full_Ordered
	(
		SpeedRunID int NOT NULL,
	    SpeedRunComID varchar (10) NOT NULL,
	  	PRIMARY KEY (SpeedRunID)     
	);

	-- tbl_SpeedRun_System_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_System_Full;
	
	CREATE TABLE tbl_SpeedRun_System_Full 
	( 
	    SpeedRunID int NOT NULL,
		PlatformID int NULL,
		RegionID int NULL,
	 	IsEmulated bit NOT NULL,
	 	PRIMARY KEY (SpeedRunID)   	
	);

	-- tbl_SpeedRun_System_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_System_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_System_Full_Ordered 
	( 
	    SpeedRunID int NOT NULL,
		PlatformID int NULL,
		RegionID int NULL,
	 	IsEmulated bit NOT NULL,
	 	PRIMARY KEY (SpeedRunID)   	
	);

	-- tbl_SpeedRun_Time_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_Time_Full;
	
	CREATE TABLE tbl_SpeedRun_Time_Full 
	( 
	    SpeedRunID int NOT NULL,
		PrimaryTime bigint NOT NULL,
		RealTime bigint NULL,
		RealTimeWithoutLoads bigint NULL,
		GameTime bigint NULL,
	 	PRIMARY KEY (SpeedRunID)   	
	);

	-- tbl_SpeedRun_Time_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_Time_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_Time_Full_Ordered 
	( 
	    SpeedRunID int NOT NULL,
		PrimaryTime bigint NOT NULL,
		RealTime bigint NULL,
		RealTimeWithoutLoads bigint NULL,
		GameTime bigint NULL,
	 	PRIMARY KEY (SpeedRunID)   	
	);

	-- tbl_SpeedRun_Link_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_Link_Full;
	
	CREATE TABLE tbl_SpeedRun_Link_Full 
	( 
	    SpeedRunID int NOT NULL,
		SpeedRunComUrl varchar(1000) NOT NULL,
		SplitsUrl varchar(1000) NULL,
	 	PRIMARY KEY (SpeedRunID) 	
	);

	-- tbl_SpeedRun_Link_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_Link_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_Link_Full_Ordered 
	( 
	    SpeedRunID int NOT NULL,
		SpeedRunComUrl varchar(1000) NOT NULL,
		SplitsUrl varchar(1000) NULL,
	 	PRIMARY KEY (SpeedRunID) 	
	);

	-- tbl_SpeedRun_Comment_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_Comment_Full;
	
	CREATE TABLE tbl_SpeedRun_Comment_Full 
	( 
	    SpeedRunID int NOT NULL,
		Comment varchar(2000) NULL,
	 	PRIMARY KEY (SpeedRunID)	
	);

	-- tbl_SpeedRun_Comment_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_Comment_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_Comment_Full_Ordered 
	( 
	    SpeedRunID int NOT NULL,
		Comment varchar(2000) NULL,
	 	PRIMARY KEY (SpeedRunID)	
	);

	-- tbl_SpeedRun_Player_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_Player_Full;
	
	CREATE TABLE tbl_SpeedRun_Player_Full 
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    SpeedRunID int NOT NULL,
	    UserID int NOT NULL,
		PRIMARY KEY (ID)	 
	);

	-- tbl_SpeedRun_Player_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_Player_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_Player_Full_Ordered 
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    SpeedRunID int NOT NULL,
	    UserID int NOT NULL,
		PRIMARY KEY (ID)	 
	);

	-- tbl_SpeedRun_Guest_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_Guest_Full;
	
	CREATE TABLE tbl_SpeedRun_Guest_Full 
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    SpeedRunID int NOT NULL,
	    GuestID int NOT NULL,
		PRIMARY KEY (ID)	    
	); 

	-- tbl_SpeedRun_Guest_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_Guest_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_Guest_Full_Ordered 
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    SpeedRunID int NOT NULL,
	    GuestID int NOT NULL,
		PRIMARY KEY (ID)	    
	);

	-- tbl_SpeedRun_VariableValue_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_VariableValue_Full;
	
	CREATE TABLE tbl_SpeedRun_VariableValue_Full
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    SpeedRunID int NOT NULL,
	    VariableID int NOT NULL,
	    VariableValueID int NOT NULL,
		PRIMARY KEY (ID)    
	); 

	-- tbl_SpeedRun_VariableValue_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_VariableValue_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_VariableValue_Full_Ordered
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    SpeedRunID int NOT NULL,
	    VariableID int NOT NULL,
	    VariableValueID int NOT NULL,
		PRIMARY KEY (ID)    
	);

	-- tbl_SpeedRun_Video_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_Video_Full;
	
	CREATE TABLE tbl_SpeedRun_Video_Full 
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    SpeedRunID int NOT NULL,
	    VideoLinkUrl varchar (500) NOT NULL,
	    EmbeddedVideoLinkUrl varchar (250) NULL,
		ThumbnailLinkUrl varchar(250) NULL,
		PRIMARY KEY (ID) 	
	);

	-- tbl_SpeedRun_Video_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_Video_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_Video_Full_Ordered 
	( 
	    ID int NOT NULL AUTO_INCREMENT,
	    SpeedRunID int NOT NULL,
	    VideoLinkUrl varchar (500) NOT NULL,
	    EmbeddedVideoLinkUrl varchar (250) NULL,
		ThumbnailLinkUrl varchar(250) NULL,
		PRIMARY KEY (ID) 	
	);

	-- tbl_SpeedRun_Video_Detail_Full
	DROP TABLE IF EXISTS tbl_SpeedRun_Video_Detail_Full;
	
	CREATE TABLE tbl_SpeedRun_Video_Detail_Full(
		SpeedRunVideoID int NOT NULL,
		SpeedRunID int NOT NULL,
		ChannelID varchar(50) NULL,
		ViewCount int NULL,
		PRIMARY KEY (SpeedRunVideoID) 	
	);

	-- tbl_SpeedRun_Video_Detail_Full_Ordered
	DROP TABLE IF EXISTS tbl_SpeedRun_Video_Detail_Full_Ordered;
	
	CREATE TABLE tbl_SpeedRun_Video_Detail_Full_Ordered(
		SpeedRunVideoID int NOT NULL,
		SpeedRunID int NOT NULL,
		ChannelID varchar(50) NULL,
		ViewCount int NULL,
		PRIMARY KEY (SpeedRunVideoID) 	
	);

	CREATE INDEX IDX_tbl_SpeedRun_VariableValue_Full_SpeedRunID ON tbl_SpeedRun_VariableValue_Full (SpeedRunID);
	CREATE INDEX IDX_tbl_SpeedRun_Player_Full_SpeedRunID ON tbl_SpeedRun_Player_Full (SpeedRunID);
	CREATE INDEX IDX_tbl_SpeedRun_Guest_Full_SpeedRunID ON tbl_SpeedRun_Guest_Full (SpeedRunID);
	CREATE INDEX IDX_tbl_SpeedRun_Video_Full_SpeedRunID ON tbl_SpeedRun_Video_Full (SpeedRunID);
	CREATE INDEX IDX_tbl_SpeedRun_Video_Detail_Full_SpeedRunID ON tbl_SpeedRun_Video_Detail_Full (SpeedRunID);
END $$
DELIMITER ;
