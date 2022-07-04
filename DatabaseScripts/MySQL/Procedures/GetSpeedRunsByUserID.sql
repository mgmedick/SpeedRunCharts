-- GetSpeedRunsByUserID
DROP PROCEDURE IF EXISTS GetSpeedRunsByUserID;

DELIMITER $$
CREATE PROCEDURE GetSpeedRunsByUserID(
	IN GameID INT,
	IN CategoryID INT,
	IN LevelID INT,
	IN VariableValueIDs VARCHAR(8000),
    IN UserID INT	
)
BEGIN
	 SELECT rn.ID,
     rn.GameID,
     -- rn.CategoryTypeID,
     rn.CategoryID,
     rn.LevelID,
     rn.PlatformID,
     rn.PlatformName,
	 rn.SubCategoryVariableValueIDs,
     -- rn.Variables,
     rn.VariableValues,
     rn.Players,
     rn.Guests,
     -- rn.IsEmulated,
     rn.`Rank`,
     rn.PrimaryTime,
     rn.Comment,
     rn.DateSubmitted,
     rn.VerifyDate 
	 FROM vw_SpeedRunGrid rn
	 JOIN tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID AND rp.UserID = UserID
	 WHERE rn.GameID = GameID
     AND rn.CategoryID = CategoryID
     AND COALESCE(rn.LevelID,'') = COALESCE(LevelID,'')
     AND COALESCE(rn.SubCategoryVariableValueIDs,'') = COALESCE(VariableValueIDs,'')
	 ORDER BY rn.ID DESC;
END $$
DELIMITER ;
