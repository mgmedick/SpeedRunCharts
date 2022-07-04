-- ImportUpdateSpeedRunRanks
DROP PROCEDURE IF EXISTS ImportUpdateSpeedRunRanks;

DELIMITER $$
CREATE PROCEDURE ImportUpdateSpeedRunRanks(
	IN LastImportDate DATETIME
)
BEGIN	
    DECLARE CurrDate DATETIME DEFAULT UTC_TIMESTAMP;
    DECLARE BatchCount INT DEFAULT 1000;
	DECLARE RowCount INT DEFAULT 0;
	DECLARE MaxRowCount INT;     
    DECLARE Debug BIT DEFAULT 0;

   	DROP TEMPORARY TABLE IF EXISTS LeaderboardKeysFromRuns;
	CREATE TEMPORARY TABLE LeaderboardKeysFromRuns
	(
		GameID INT,
		CategoryID INT,
		LevelID INT,
		SubCategoryVariableValues VARCHAR(50)
	);
	CREATE INDEX IDX_LeaderboardKeysFromRuns_GameID_CategoryID ON LeaderboardKeysFromRuns (GameID, CategoryID, LevelID);

   	DROP TEMPORARY TABLE IF EXISTS LeaderboardKeys;
	CREATE TEMPORARY TABLE LeaderboardKeys
	(
		GameID INT,
		CategoryID INT,
		LevelID INT,
		SubCategoryVariableValues VARCHAR(50)
	);
	CREATE INDEX IDX_LeaderboardKeys_GameID_CategoryID ON LeaderboardKeys (GameID, CategoryID, LevelID);

   	DROP TEMPORARY TABLE IF EXISTS SpeedRunsToUpdate;
	CREATE TEMPORARY TABLE SpeedRunsToUpdate
	(
	      RowNum INT AUTO_INCREMENT,	
          ID INT,
          GameID INT,
          CategoryID INT,
          LevelID INT,
          SubCategoryVariableValues VARCHAR(150),
          PlayerIDs VARCHAR(150),
          GuestIDs VARCHAR(150),
          PrimaryTime BIGINT,
          RankPriority INT,
		  PRIMARY KEY (RowNum)          
	);

   	DROP TEMPORARY TABLE IF EXISTS SpeedRunsRanked;
	CREATE TEMPORARY TABLE SpeedRunsRanked
	(
		RowNum INT AUTO_INCREMENT,
		ID INT,
		`Rank` INT,
		PRIMARY KEY (RowNum)		
	);

   	DROP TEMPORARY TABLE IF EXISTS SpeedRunsToUpdateBatch;
	CREATE TEMPORARY TABLE SpeedRunsToUpdateBatch
	(
		ID INT
	);

   	DROP TEMPORARY TABLE IF EXISTS SpeedRunsRankedBatch;
	CREATE TEMPORARY TABLE SpeedRunsRankedBatch
	(
		ID INT,
		`Rank` INT	
	);

	IF LastImportDate > '1753-01-01 00:00:00' THEN	
		INSERT INTO LeaderboardKeysFromRuns (GameID, CategoryID, LevelID)
		SELECT rn.GameID, rn.CategoryID, rn.LevelID
		FROM tbl_SpeedRun rn
		WHERE COALESCE(rn.ModifiedDate, rn.ImportedDate) >= LastImportDate
		GROUP BY rn.GameID, rn.CategoryID, rn.LevelID;
 	END IF;
 
	INSERT INTO LeaderboardKeys (GameID, CategoryID)
	SELECT g.ID, c.ID
	FROM tbl_Game g
	JOIN tbl_Category c ON c.GameID = g.ID
	WHERE COALESCE(g.ModifiedDate, g.ImportedDate) >= LastImportDate
    AND NOT EXISTS (SELECT 1 FROM LeaderboardKeysFromRuns WHERE GameID = g.ID AND CategoryID = c.ID)
	GROUP BY g.ID, c.ID;

	INSERT INTO LeaderboardKeys (GameID, CategoryID, LevelID)
	SELECT rn.GameID, rn.CategoryID, rn.LevelID
	FROM LeaderboardKeysFromRuns rn;
	
	INSERT INTO SpeedRunsToUpdate(ID, GameID, CategoryID, LevelID, SubCategoryVariableValues, PlayerIDs, GuestIDs, PrimaryTime, RankPriority)
    SELECT rn.ID, rn.GameID, rn.CategoryID, rn.LevelID, SubCategoryVariableValues.Value, PlayerIDs.Value, GuestIDs.Value, rn.PrimaryTime,
    ROW_NUMBER() OVER (PARTITION BY rn.GameID, rn.CategoryID, rn.LevelID, SubCategoryVariableValues.Value, PlayerIDs.Value, GuestIDs.Value ORDER BY rn.PrimaryTime)
    FROM tbl_SpeedRun rn    
  	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONVERT(rv.VariableValueID,CHAR) ORDER BY rv.ID SEPARATOR ',') Value
	    FROM tbl_SpeedRun_VariableValue rv
	    JOIN tbl_Variable v ON v.ID = rv.VariableID AND v.IsSubCategory = 1
	    WHERE rv.SpeedRunID = rn.ID
	) SubCategoryVariableValues ON TRUE
 	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONVERT(rp.UserID,CHAR) ORDER BY rp.ID SEPARATOR ',') Value
	    FROM tbl_SpeedRun_Player rp
		WHERE rp.SpeedRunID = rn.ID
	) PlayerIDs ON TRUE      
 	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONVERT(rg.GuestID,CHAR) ORDER BY rg.ID SEPARATOR ',') Value
	    FROM tbl_SpeedRun_Guest rg
		WHERE rg.SpeedRunID = rn.ID
	) GuestIDs ON TRUE 	                               
    WHERE EXISTS (SELECT 1 FROM LeaderboardKeys lb WHERE lb.GameID = rn.GameID AND lb.CategoryID = rn.CategoryID AND COALESCE(lb.LevelID,'') = COALESCE(rn.LevelID,''));
  
    INSERT INTO SpeedRunsRanked(ID, `Rank`)
    SELECT ID, RANK() OVER (PARTITION BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues ORDER BY rn.PrimaryTime)
    FROM SpeedRunsToUpdate rn
    WHERE rn.RankPriority = 1
    AND COALESCE(PlayerIDs, GuestIDs) IS NOT NULL;
   
    IF Debug = 0 THEN        
    	SELECT COUNT(*) INTO MaxRowCount FROM SpeedRunsToUpdate;      
        WHILE RowCount < MaxRowCount DO
            INSERT INTO SpeedRunsToUpdateBatch (ID)
		    SELECT ID
		    FROM SpeedRunsToUpdate
		    WHERE RowNum > RowCount
		    ORDER BY RowNum
		    LIMIT BatchCount; 
		   
			UPDATE tbl_SpeedRun rn
		  	JOIN SpeedRunsToUpdateBatch rn1 ON rn1.ID = rn.ID
		  	SET rn.`Rank` = NULL;	
		  
			SET RowCount = RowCount + BatchCount;
	        TRUNCATE TABLE SpeedRunsToUpdateBatch;
	    END WHILE;

	   	SET RowCount = 0;
    	SELECT COUNT(*) INTO MaxRowCount FROM SpeedRunsRanked;  	   
        WHILE RowCount < MaxRowCount DO	   
            INSERT INTO SpeedRunsRankedBatch (ID, `Rank`)
		    SELECT ID, `Rank`
		    FROM SpeedRunsRanked
		    WHERE RowNum > RowCount
		    ORDER BY RowNum
		    LIMIT BatchCount;         
        
			UPDATE tbl_SpeedRun rn
		  	JOIN SpeedRunsRankedBatch rn1 ON rn1.ID = rn.ID
		  	SET rn.`Rank` = rn1.`Rank`;
		  
  			SET RowCount = RowCount + BatchCount;
	        TRUNCATE TABLE SpeedRunsRankedBatch;  		
	    END WHILE;		  
    ELSE
		SELECT rn.RankPriority, rn.ID, rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn.PlayerIDs, rn.GuestIDs, rn.PrimaryTime
        FROM SpeedRunsToUpdate rn
        WHERE rn.SubCategoryVariableValues IS NOT NULL
        ORDER BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn.PlayerIDs, rn.GuestIDs, rn.RankPriority;
        
        SELECT rn1.`Rank`, rn.ID, rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn.PlayerIDs, rn.GuestIDs, rn.PrimaryTime
        FROM SpeedRunsToUpdate rn
        JOIN SpeedRunsRanked rn1 ON rn1.ID = rn.ID
        WHERE rn.SubCategoryVariableValues IS NOT NULL
        ORDER BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn1.`Rank`;       
    END IF;
END $$
DELIMITER ;
