-- vw_SpeedRunSummary
DROP VIEW IF EXISTS vw_SpeedRunSummary;

CREATE VIEW vw_SpeedRunSummary AS

    SELECT rn.ID,
    	   rn1.SpeedRunComID,
           g.ID AS GameID,
           g.Name AS GameName,
		   g.Abbr AS GameAbbr,
           gl.CoverImagePath AS GameCoverImageUrl,
           ct.ID AS CategoryTypeID,
           ct.Name AS CategoryTypeName,           
           c.ID AS CategoryID,
           c.Name AS CategoryName,
		   l.ID AS LevelID,
		   l.Name AS LevelName,
           SubCategoryVariableValueIDs.Value AS SubCategoryVariableValueIDs,		   
           SubCategoryVariableValues.Value AS SubCategoryVariableValues,           
           Players.Value AS Players,
 		   EmbeddedVideoLinks.Value AS EmbeddedVideoLinks,
           rn.`Rank`,
           rn.PrimaryTime,
           rn.DateSubmitted,
		   rn.VerifyDate,
           rn.ImportedDate
    FROM tbl_SpeedRun rn
    JOIN tbl_SpeedRun_SpeedRunComID rn1 ON rn1.SpeedRunID = rn.ID
    JOIN tbl_Game g ON g.ID = rn.GameID
	JOIN tbl_Game_Link gl ON gl.GameID = g.ID
    JOIN tbl_Category c ON c.ID = rn.CategoryID
    JOIN tbl_CategoryType ct ON ct.ID = c.CategoryTypeID
    LEFT JOIN tbl_Level l ON l.ID = rn.LevelID
  	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONVERT(rv.VariableValueID,CHAR) ORDER BY rv.ID SEPARATOR ',') Value
	    FROM tbl_SpeedRun_VariableValue rv
	    JOIN tbl_Variable v ON v.ID = rv.VariableID AND v.IsSubCategory = 1
	    WHERE rv.SpeedRunID = rn.ID
	) SubCategoryVariableValueIDs ON TRUE     
  	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(rv.VariableID,CHAR), '¦', CONVERT(rv.VariableValueID,CHAR), '¦', va.Value) ORDER BY rv.ID SEPARATOR '^^') Value
	    FROM tbl_SpeedRun_VariableValue rv
	    JOIN tbl_Variable v ON v.ID = rv.VariableID AND v.IsSubCategory = 1
		JOIN tbl_VariableValue va ON va.ID = rv.VariableValueID
	    WHERE rv.SpeedRunID = rn.ID
	) SubCategoryVariableValues ON TRUE    
 	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(u.ID,CHAR), '¦', u.Name  , '¦', COALESCE (u.Abbr,'')) ORDER BY rp.ID SEPARATOR '^^') Value
	    FROM tbl_SpeedRun_Player rp  
		JOIN tbl_User u ON u.ID = rp.UserID
		WHERE rp.SpeedRunID = rn.ID
	) Players ON TRUE       
  	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(rd.EmbeddedVideoLinkUrl, '|', COALESCE(rd.ThumbnailLinkUrl,'')) ORDER BY rd.ID SEPARATOR ',') Value
	    FROM tbl_SpeedRun_Video rd
	    WHERE rd.SpeedRunID = rn.ID
	    AND rd.EmbeddedVideoLinkUrl IS NOT NULL
	) EmbeddedVideoLinks ON TRUE;