 -- vw_GameSpeedRunCom
 DROP VIEW IF EXISTS vw_GameSpeedRunCom;

CREATE VIEW vw_GameSpeedRunCom AS
	
	SELECT g.ID,
	       gc.SpeedRunComID,  
	       g.Name,
	       g.IsRomHack,
	       g.YearOfRelease,
	       gl.CoverImageUrl,      
	       Categories.Value AS CategorySpeedRunComIDs,
	       Levels.Value AS LevelSpeedRunComIDs,
	       Variables.Value AS VariableSpeedRunComIDs,
	       VariableValues.Value AS VariableValueSpeedRunComIDs,
	       Platforms.Value AS PlatformSpeedRunComIDs,
	       Moderators.Value AS ModeratorSpeedRunComIDs,
	       g.CreatedDate,
	       g.ModifiedDate,
	       g.IsChanged
	FROM tbl_Game g
	JOIN tbl_Game_SpeedRunComID gc ON gc.GameID = g.ID
	JOIN tbl_Game_Link gl ON gl.GameID = g.ID 
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(cc.SpeedRunComID ORDER BY c.ID SEPARATOR ',') Value
	    FROM tbl_Category c
	    JOIN tbl_Category_SpeedRunComID cc ON cc.CategoryID=c.ID
	    WHERE c.GameID = g.ID
	) Categories ON TRUE   
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(lc.SpeedRunComID ORDER BY l.ID SEPARATOR ',') Value
	    FROM tbl_Level l
	    JOIN tbl_Level_SpeedRunComID lc ON lc.LevelID = l.ID
	    WHERE l.GameID = g.ID
	) Levels ON TRUE 
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(vc.SpeedRunComID ORDER BY v.ID SEPARATOR ',') Value
	    FROM tbl_Variable v
	    JOIN tbl_Variable_SpeedRunComID vc ON vc.VariableID = v.ID
	    WHERE v.GameID = g.ID
	) Variables ON TRUE  
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(vc.SpeedRunComID ORDER BY v.ID SEPARATOR ',') Value
	    FROM tbl_VariableValue v
	    JOIN tbl_VariableValue_SpeedRunComID vc ON vc.VariableValueID = v.ID
	    WHERE v.GameID = g.ID
	) VariableValues ON TRUE  	    	   
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(pc.SpeedRunComID ORDER BY p.ID SEPARATOR ',') Value
	    FROM tbl_Platform p
	    JOIN tbl_Game_Platform gp ON gp.PlatformID = p.ID
	    JOIN tbl_Platform_SpeedRunComID pc ON pc.PlatformID = p.ID
	    WHERE gp.GameID = g.ID
	) Platforms ON TRUE    	
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(uc.SpeedRunComID ORDER BY gm.ID SEPARATOR ',') Value
		FROM tbl_User u
		JOIN tbl_Game_Moderator gm ON gm.UserID = u.ID
	    JOIN tbl_User_SpeedRunComID uc ON uc.UserID = u.ID 
		WHERE gm.GameID = g.ID
	) Moderators ON TRUE;     
     