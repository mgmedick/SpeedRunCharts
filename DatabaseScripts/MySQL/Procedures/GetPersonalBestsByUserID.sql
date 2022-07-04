-- GetPersonalBestsByUserID
DROP PROCEDURE IF EXISTS GetPersonalBestsByUserID;

DELIMITER $$
CREATE PROCEDURE GetPersonalBestsByUserID(
	IN GameID INT,
	IN CategoryID INT,
	IN LevelID INT,
	IN UserID INT
)
BEGIN
	DROP TEMPORARY TABLE IF EXISTS ResultsRaw;
	CREATE TEMPORARY TABLE ResultsRaw 
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
	rn.`Rank`,
	rn.PrimaryTime,
	rn.Comment,
	rn.DateSubmitted,
	rn.VerifyDate
	FROM vw_SpeedRunGridUser rn
	WHERE rn.GameID = GameID
    AND rn.CategoryID = CategoryID
	AND COALESCE(rn.LevelID, '' ) = COALESCE(LevelID, '')
    AND rn.UserID = UserID;
   
	SELECT rn.ID,
	rn.GameID,
	rn.CategoryID,
	rn.LevelID,
	rn.PlatformID,
	rn.SubCategoryVariableValueIDs,
	rn.VariableValues,
	rn.Players,
	rn.Guests,
	rn.`Rank`,
	rn.PrimaryTime,
	rn.Comment,
	rn.DateSubmitted,
	rn.VerifyDate
	FROM ResultsRaw rn
	WHERE rn.RowNum = 1
	ORDER BY rn.ID;
END $$
DELIMITER ;
