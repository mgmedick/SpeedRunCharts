-- GetLatestSpeedRuns
DROP PROCEDURE IF EXISTS GetLatestSpeedRuns;

DELIMITER $$
CREATE PROCEDURE GetLatestSpeedRuns
(
	IN SpeedRunListCategoryID INT,
	IN TopAmount INT,
	IN OrderValueOffset INT
)
BEGIN
     -- new
     IF SpeedRunListCategoryID = 0 THEN
          SELECT rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.`Rank`, rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM vw_SpeedRunSummary rn
          WHERE ((OrderValueOffset IS NULL) OR (rn.ID < OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          ORDER BY rn.ID DESC
          LIMIT TopAmount;
	 -- top 5%
     ELSEIF SpeedRunListCategoryID = 1 THEN
          SELECT rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               -- NULL AS GameCoverImageUrl,
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.`Rank`, rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate              
		  FROM vw_SpeedRunSummary rn,        
		  LATERAL (SELECT MAX(rn1.`Rank`) AS Value
					FROM vw_SpeedRunSummaryLite rn1
					WHERE rn1.GameID = rn.GameID
					AND rn1.CategoryID = rn.CategoryID
					AND COALESCE(rn1.LevelID,'') = COALESCE(rn.LevelID,'')
					AND COALESCE(rn1.SubCategoryVariableValueIDs,'') = COALESCE(rn.SubCategoryVariableValueIDs,'')
					AND rn1.`Rank` IS NOT NULL
				) AS MaxRank
		  WHERE ((OrderValueOffset IS NULL) OR (rn.ID < OrderValueOffset))
		  AND rn.EmbeddedVideoLinks IS NOT NULL
		  AND rn.`Rank` IS NOT NULL
		  AND MaxRank.Value > 1
		  AND rn.`Rank` <= CASE WHEN FLOOR((5 / 100 * (MaxRank.Value + 1))) < 1 THEN 1 ELSE FLOOR((5 / 100 * (MaxRank.Value + 1))) END
		  ORDER BY rn.ID DESC
          LIMIT TopAmount;                  
	 -- first
     ELSEIF SpeedRunListCategoryID = 2 THEN
          SELECT rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               -- NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.`Rank`, rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM vw_SpeedRunSummary rn,
		  LATERAL (SELECT MAX(rn1.`Rank`) AS Value
					FROM vw_SpeedRunSummaryLite rn1
					WHERE rn1.GameID = rn.GameID
					AND rn1.CategoryID = rn.CategoryID
					AND COALESCE(rn1.LevelID,'') = COALESCE(rn.LevelID,'')
					AND COALESCE(rn1.SubCategoryVariableValueIDs,'') = COALESCE(rn.SubCategoryVariableValueIDs,'')
					AND rn1.`Rank` IS NOT NULL
				) AS MaxRank          
          WHERE ((OrderValueOffset IS NULL) OR (rn.ID < OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
		  AND rn.`Rank` = 1
		  AND MaxRank.Value > 1
          ORDER BY rn.ID DESC
          LIMIT TopAmount;         
	 -- top 3
     ELSEIF SpeedRunListCategoryID = 3 THEN
          SELECT rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               -- NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.`Rank`, rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM vw_SpeedRunSummary rn,
		  LATERAL (SELECT MAX(rn1.`Rank`) AS Value
					FROM vw_SpeedRunSummaryLite rn1
					WHERE rn1.GameID = rn.GameID
					AND rn1.CategoryID = rn.CategoryID
					AND COALESCE(rn1.LevelID,'') = COALESCE(rn.LevelID,'')
					AND COALESCE(rn1.SubCategoryVariableValueIDs,'') = COALESCE(rn.SubCategoryVariableValueIDs,'')
					AND rn1.`Rank` IS NOT NULL
				) AS MaxRank             
          WHERE ((OrderValueOffset IS NULL) OR (rn.ID < OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
		  AND rn.`Rank` <= 3
		  AND MaxRank.Value > 3
          ORDER BY rn.ID DESC
          LIMIT TopAmount;            
	 -- Bests
     ELSEIF SpeedRunListCategoryID = 4 THEN
		  SELECT rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
		  rn.GameCoverImageUrl, 
		  -- NULL AS GameCoverImageUrl,          
		  rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
		  rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.`Rank`, rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate           
		  FROM vw_SpeedRunSummary rn,
		  LATERAL (SELECT rn1.ID AS Value
					FROM vw_SpeedRunSummaryLite rn1
					WHERE rn1.GameID = rn.GameID
					AND rn1.CategoryID = rn.CategoryID
					AND COALESCE(rn1.LevelID,'') = COALESCE(rn.LevelID,'')
					AND COALESCE(rn1.SubCategoryVariableValueIDs,'') = COALESCE(rn.SubCategoryVariableValueIDs,'')
					AND rn1.ID <> rn.ID
					LIMIT 1
				) AS OtherRun		  
		  WHERE ((OrderValueOffset IS NULL) OR (rn.ID < OrderValueOffset))
		  AND rn.EmbeddedVideoLinks IS NOT NULL
		  AND rn.`Rank` IS NOT NULL
		  AND OtherRun.Value IS NOT NULL
		  ORDER BY rn.ID DESC
		  LIMIT TopAmount;        
	 -- Popular
     ELSEIF SpeedRunListCategoryID = 5 THEN
          SELECT rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               -- NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.`Rank`, rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM vw_SpeedRunSummary rn,
		  LATERAL (SELECT MAX(rn1.ViewCount) AS Value, COUNT(rn1.SpeedRunVideoID) AS VideoCount
					FROM tbl_SpeedRun_Video_Detail rn1
					WHERE rn1.SpeedRunID = rn.ID
			    ) AS MaxViewCount          
          WHERE ((OrderValueOffset IS NULL) OR (rn.ID < OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          AND MaxViewCount.Value > 1000
          AND MaxViewCount.VideoCount = 1
          ORDER BY rn.ID DESC
          LIMIT TopAmount;           
	 -- GDQ
     ELSEIF SpeedRunListCategoryID = 7 THEN
          SELECT rn.ID, rn.SpeedRunComID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               -- NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.`Rank`, rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM vw_SpeedRunSummary rn
		  JOIN tbl_SpeedRun_Video_Detail rn1 ON rn1.SpeedRunID = rn.ID             
          WHERE ((OrderValueOffset IS NULL) OR (rn.ID < OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
		  AND rn1.ChannelID IN ('22510310','UCI3DTtB-a3fJPjKtQ5kYHfA')
		  ORDER BY rn.ID DESC
          LIMIT TopAmount;           
     END IF;
END $$
DELIMITER ;
