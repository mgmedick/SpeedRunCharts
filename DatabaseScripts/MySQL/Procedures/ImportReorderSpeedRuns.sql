-- ImportReorderSpeedRuns
DROP PROCEDURE IF EXISTS ImportReorderSpeedRuns;

DELIMITER $$
CREATE PROCEDURE ImportReorderSpeedRuns()
BEGIN	
    DECLARE BatchCount INT DEFAULT 1000;
    DECLARE RowCount INT DEFAULT 0;
    DECLARE MaxRowCount INT;
    DECLARE LastInsertID INT;

    SELECT COUNT(*) INTO MaxRowCount FROM tbl_SpeedRun_Full;
   
   	DROP TEMPORARY TABLE IF EXISTS IDsToProcess;
	CREATE TEMPORARY TABLE IDsToProcess
	(
		RowNum INT AUTO_INCREMENT,
		ID INT,
		PRIMARY KEY (RowNum)	
	); 

   	DROP TEMPORARY TABLE IF EXISTS InsertedIDs;
	CREATE TEMPORARY TABLE InsertedIDs
	(
		RowNum INT AUTO_INCREMENT,
		NewID INT,
		OldID INT,
		PRIMARY KEY (RowNum)
	);

   	DROP TEMPORARY TABLE IF EXISTS InsertedVideoIDs;
	CREATE TEMPORARY TABLE InsertedVideoIDs
	(
		RowNum INT AUTO_INCREMENT,	
		NewVideoID INT,
		NewID INT,
		OldVideoID INT,
		PRIMARY KEY (RowNum)		
	);

   	DROP TEMPORARY TABLE IF EXISTS OldVideoIDs;
	CREATE TEMPORARY TABLE OldVideoIDs
	(
		RowNum INT AUTO_INCREMENT,	
		OldVideoID INT,
		PRIMARY KEY (RowNum)		
	);

	WHILE RowCount < MaxRowCount DO
        INSERT INTO IDsToProcess (ID)
	    SELECT ID
	    FROM tbl_SpeedRun_Full
	    WHERE COALESCE(IsProcessed, 0) = 0
	    ORDER BY COALESCE(VerifyDate, DateSubmitted)
	    LIMIT BatchCount;  
    
        INSERT INTO tbl_SpeedRun_Full_Ordered (StatusTypeID, GameID, CategoryID, LevelID, `Rank`, PrimaryTime, RunDate, DateSubmitted, VerifyDate)
        SELECT StatusTypeID, GameID, CategoryID, LevelID, `Rank`, PrimaryTime, RunDate, DateSubmitted, VerifyDate
        FROM tbl_SpeedRun_Full rn
        JOIN IDsToProcess rn1 ON rn1.ID = rn.ID;
              
        SELECT LAST_INSERT_ID() INTO LastInsertID;
       
       	INSERT INTO InsertedIDs (NewID)
       	SELECT rn.ID
       	FROM tbl_SpeedRun_Full_Ordered rn
       	WHERE rn.ID >= LastInsertID
        ORDER BY rn.ID;
       	
		UPDATE InsertedIDs rn
	  	JOIN IDsToProcess rn1 ON rn1.RowNum = rn.RowNum
	  	SET rn.OldID = rn1.ID;
	  
        INSERT INTO tbl_SpeedRun_SpeedRunComID_Full_Ordered (SpeedRunID, SpeedRunComID)
        SELECT dn.NewID, rn1.SpeedRunComID
        FROM InsertedIDs dn
        JOIN tbl_SpeedRun_SpeedRunComID_Full rn1 ON rn1.SpeedRunID = dn.OldID;

        INSERT INTO tbl_SpeedRun_System_Full_Ordered (SpeedRunID, PlatformID, RegionID, IsEmulated)
        SELECT dn.NewID, rs.PlatformID, rs.RegionID, rs.IsEmulated
        FROM InsertedIDs dn
        JOIN tbl_SpeedRun_System_Full rs ON rs.SpeedRunID = dn.OldID;

        INSERT INTO tbl_SpeedRun_Time_Full_Ordered (SpeedRunID, PrimaryTime, RealTime, RealTimeWithoutLoads, GameTime)
        SELECT dn.NewID, rt.PrimaryTime, rt.RealTime, rt.RealTimeWithoutLoads, rt.GameTime
        FROM InsertedIDs dn
        JOIN tbl_SpeedRun_Time_Full rt ON rt.SpeedRunID = dn.OldID;

        INSERT INTO tbl_SpeedRun_Link_Full_Ordered (SpeedRunID, SpeedRunComUrl, SplitsUrl)
        SELECT dn.NewID, rl.SpeedRunComUrl, rl.SplitsUrl
        FROM InsertedIDs dn
        JOIN tbl_SpeedRun_Link_Full rl ON rl.SpeedRunID = dn.OldID;

        INSERT INTO tbl_SpeedRun_Comment_Full_Ordered (SpeedRunID, Comment)
        SELECT dn.NewID, rc.Comment
        FROM InsertedIDs dn
        JOIN tbl_SpeedRun_Comment_Full rc ON rc.SpeedRunID = dn.OldID;

        INSERT INTO tbl_SpeedRun_Player_Full_Ordered (SpeedRunID, UserID)
        SELECT dn.NewID, rd.UserID
        FROM InsertedIDs dn
        JOIN tbl_SpeedRun_Player_Full rd ON rd.SpeedRunID = dn.OldID;

        INSERT INTO tbl_SpeedRun_Guest_Full_Ordered (SpeedRunID, GuestID)
        SELECT dn.NewID, rg.GuestID
        FROM InsertedIDs dn
        JOIN tbl_SpeedRun_Guest_Full rg ON rg.SpeedRunID = dn.OldID;

        INSERT INTO tbl_SpeedRun_VariableValue_Full_Ordered (SpeedRunID, VariableID, VariableValueID)
        SELECT dn.NewID, rv.VariableID, rv.VariableValueID
        FROM InsertedIDs dn
        JOIN tbl_SpeedRun_VariableValue_Full rv ON rv.SpeedRunID = dn.OldID;  
        
        INSERT INTO tbl_SpeedRun_Video_Full_Ordered (SpeedRunID, VideoLinkUrl, EmbeddedVideoLinkUrl, ThumbnailLinkUrl)
		SELECT dn.NewID, rv.VideoLinkUrl, rv.EmbeddedVideoLinkUrl, rv.ThumbnailLinkUrl
		FROM InsertedIDs dn
		JOIN tbl_SpeedRun_Video_Full rv ON rv.SpeedRunID = dn.OldID      
		ORDER BY dn.NewID;	
	
		SELECT LAST_INSERT_ID() INTO LastInsertID;	
	
       	INSERT INTO InsertedVideoIDs (NewVideoID, NewID)
        SELECT rv.ID, rv.SpeedRunID
		FROM tbl_SpeedRun_Video_Full_Ordered rv
		WHERE rv.ID >= LastInsertID
		ORDER BY rv.ID;
	
       	INSERT INTO OldVideoIDs (OldVideoID)
        SELECT rv.ID
		FROM InsertedIDs dn
		JOIN tbl_SpeedRun_Video_Full rv ON rv.SpeedRunID = dn.OldID
		ORDER BY dn.NewID;	
	
		UPDATE InsertedVideoIDs rn
	  	JOIN OldVideoIDs rn1 ON rn1.RowNum = rn.RowNum
	  	SET rn.OldVideoID = rn1.OldVideoID;	
	
        INSERT INTO tbl_SpeedRun_Video_Detail_Full_Ordered (SpeedRunVideoID, SpeedRunID, ChannelID, ViewCount)
		SELECT dn.NewVideoID, dn.NewID, rv.ChannelID, rv.ViewCount
		FROM InsertedVideoIDs dn
		JOIN tbl_SpeedRun_Video_Detail_Full rv ON rv.SpeedRunVideoID = dn.OldVideoID;

		UPDATE tbl_SpeedRun_Full rn
	  	JOIN IDsToProcess rn1 ON rn1.ID = rn.ID
	  	SET rn.IsProcessed = 1;
	  
	  	SELECT COUNT(*) INTO RowCount FROM tbl_SpeedRun_Full WHERE COALESCE(IsProcessed, 0) = 1;
	  
        TRUNCATE TABLE IDsToProcess;
        TRUNCATE TABLE InsertedIDs;
        TRUNCATE TABLE InsertedVideoIDs;  
        TRUNCATE TABLE OldVideoIDs;         
   	END WHILE;
   
	DROP TABLE tbl_SpeedRun_Full;
	DROP TABLE tbl_SpeedRun_SpeedRunComID_Full;
	DROP TABLE tbl_SpeedRun_System_Full;
	DROP TABLE tbl_SpeedRun_Time_Full;
	DROP TABLE tbl_SpeedRun_Link_Full;
	DROP TABLE tbl_SpeedRun_Comment_Full;
	DROP TABLE tbl_SpeedRun_Player_Full;
   	DROP TABLE tbl_SpeedRun_Guest_Full;
	DROP TABLE tbl_SpeedRun_VariableValue_Full;
	DROP TABLE tbl_SpeedRun_Video_Full;
	DROP TABLE tbl_SpeedRun_Video_Detail_Full;

	ALTER TABLE tbl_SpeedRun_Full_Ordered RENAME tbl_SpeedRun_Full;
	ALTER TABLE tbl_SpeedRun_SpeedRunComID_Full_Ordered RENAME tbl_SpeedRun_SpeedRunComID_Full;
	ALTER TABLE tbl_SpeedRun_System_Full_Ordered RENAME tbl_SpeedRun_System_Full;	
	ALTER TABLE tbl_SpeedRun_Time_Full_Ordered RENAME tbl_SpeedRun_Time_Full;	
	ALTER TABLE tbl_SpeedRun_Link_Full_Ordered RENAME tbl_SpeedRun_Link_Full;
	ALTER TABLE tbl_SpeedRun_Comment_Full_Ordered RENAME tbl_SpeedRun_Comment_Full;
	ALTER TABLE tbl_SpeedRun_Player_Full_Ordered RENAME tbl_SpeedRun_Player_Full;
	ALTER TABLE tbl_SpeedRun_Guest_Full_Ordered RENAME tbl_SpeedRun_Guest_Full;
	ALTER TABLE tbl_SpeedRun_VariableValue_Full_Ordered RENAME tbl_SpeedRun_VariableValue_Full;
	ALTER TABLE tbl_SpeedRun_Video_Full_Ordered RENAME tbl_SpeedRun_Video_Full;
	ALTER TABLE tbl_SpeedRun_Video_Detail_Full_Ordered RENAME tbl_SpeedRun_Video_Detail_Full;
END $$
DELIMITER ;
