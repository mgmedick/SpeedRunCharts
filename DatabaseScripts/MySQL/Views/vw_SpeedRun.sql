-- vw_SpeedRun
DROP VIEW IF EXISTS vw_SpeedRun;

CREATE VIEW vw_SpeedRun AS

    SELECT rn.ID,
           g.ID AS GameID,
           g.Name AS GameName,
           st.ID AS StatusTypeID,
           st.Name AS StatusTypeName,           
           gl.CoverImageUrl AS GameCoverImageUrl,
           ct.ID AS CategoryTypeID,
           ct.Name AS CategoryTypeName,
           c.ID AS CategoryID,
           c.Name AS CategoryName,
		   l.ID AS LevelID,
		   l.Name AS LevelName,
           pl.ID AS PlatformID,
           pl.Name AS PlatformName,
           VariableValues.Value AS VariableValues,
           Players.Value AS Players,
           Guests.Value AS Guests,
           VideoLinks.Value AS VideoLinks,
		   EmbeddedVideoLinks.Value AS EmbeddedVideoLinks,
           rs.IsEmulated,
           rn.Rank,
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
    FROM tbl_SpeedRun rn 
    JOIN tbl_Game g  ON g.ID = rn.GameID
	JOIN tbl_Game_Link gl  ON gl.GameID = rn.GameID
    JOIN tbl_RunStatusType st ON st.ID = rn.StatusTypeID 
    JOIN tbl_Category c  ON c.ID = rn.CategoryID
    JOIN tbl_CategoryType ct ON ct.ID = c.CategoryTypeID
    JOIN tbl_SpeedRun_System rs ON rs.SpeedRunID = rn.ID
    JOIN tbl_SpeedRun_Time rt ON rt.SpeedRunID = rn.ID
    JOIN tbl_SpeedRun_Link rl ON rl.SpeedRunID = rn.ID
    LEFT JOIN tbl_Level l  ON l.ID = rn.LevelID
    LEFT JOIN tbl_Platform pl on pl.ID = rs.PlatformID
    LEFT JOIN tbl_SpeedRun_Comment rc ON rc.SpeedRunID = rn.ID
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(rv.VariableID,CHAR), '|', CONVERT(rv.VariableValueID,CHAR)) ORDER BY rv.ID SEPARATOR ',') Value
        FROM tbl_SpeedRun_VariableValue rv
        WHERE rv.SpeedRunID = rn.ID
	) VariableValues ON TRUE      
 	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(u.ID,CHAR), '|', u.Name) ORDER BY rp.ID SEPARATOR '^^') Value
        FROM tbl_SpeedRun_Player rp  
		JOIN tbl_User u ON u.ID = rp.UserID
		WHERE rp.SpeedRunID = rn.ID
	) Players ON TRUE 
 	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(g.ID,CHAR), '|', g.Name) ORDER BY rg.ID SEPARATOR '^^') Value
	    FROM tbl_SpeedRun_Guest rg  
		JOIN tbl_Guest g ON g.ID = rg.GuestID
		WHERE rg.SpeedRunID = rn.ID
	) Guests ON TRUE 
 	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(rd.VideoLinkUrl ORDER BY rd.ID SEPARATOR ',') Value
	    FROM tbl_SpeedRun_Video rd 
	    WHERE rd.SpeedRunID = rn.ID
	) VideoLinks ON TRUE 
 	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(rd.EmbeddedVideoLinkUrl ORDER BY rd.ID SEPARATOR ',') Value
	    FROM tbl_SpeedRun_Video rd
	    WHERE rd.SpeedRunID = rn.ID
	    AND rd.EmbeddedVideoLinkUrl IS NOT NULL
	) EmbeddedVideoLinks ON TRUE;
