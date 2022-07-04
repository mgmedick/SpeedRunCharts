-- vw_Game
DROP VIEW IF EXISTS vw_Game;

CREATE VIEW vw_Game AS

    SELECT g.ID, g.Name, g.Abbr, gl.CoverImagePath AS CoverImageUrl, g.YearOfRelease, CategoryTypes.Value AS CategoryTypes, Categories.Value AS Categories, Levels.Value AS Levels,
        Variables.Value AS Variables, VariableValues.Value AS VariableValues, Platforms.Value AS Platforms, Moderators.Value AS Moderators, gl.SpeedRunComUrl             
    FROM tbl_Game g
    JOIN tbl_Game_Link gl ON gl.GameID = g.ID
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(ct1.ID,CHAR), '|', ct1.Name) ORDER BY ct1.ID SEPARATOR '^^') Value
		FROM (SELECT ct.ID, ct.Name
		FROM tbl_CategoryType ct
		JOIN tbl_Category c ON c.CategoryTypeID = ct.ID
		WHERE c.GameID = g.ID
		GROUP BY ct.ID, ct.Name) ct1
	) CategoryTypes ON TRUE   
    LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(c.ID,CHAR), '|', CONVERT(c.CategoryTypeID,CHAR), '|', c.Name) ORDER BY c.ID SEPARATOR '^^') Value
        FROM tbl_Category c
        WHERE c.GameID = g.ID
    ) Categories ON TRUE
  	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(l.ID,CHAR), '|', l.Name) ORDER BY l.ID SEPARATOR '^^') Value
        FROM tbl_Level l
        WHERE l.GameID = g.ID
    ) Levels ON TRUE  
  	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(v.ID,CHAR), '|', CASE v.IsSubCategory WHEN 1 THEN 'True' ELSE 'False' END, '|', CONVERT(v.VariableScopeTypeID, CHAR), '|', COALESCE(CONVERT(v.CategoryID, CHAR),''), '|', COALESCE(CONVERT(v.LevelID, CHAR),''), '|', v.Name) ORDER BY v.ID SEPARATOR '^^') Value
        FROM tbl_Variable v
        WHERE v.GameID = g.ID
    ) Variables ON TRUE      
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(v.ID,CHAR), '|', CONVERT(v.VariableID,CHAR), '|', v.Value) ORDER BY v.ID SEPARATOR '^^') Value
        FROM tbl_VariableValue v
        WHERE v.GameID = g.ID
    ) VariableValues ON TRUE    
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(p.ID,CHAR), '|', p.Name) ORDER BY p.ID SEPARATOR '^^') Value
	    FROM tbl_Platform p
	    JOIN tbl_Game_Platform gp ON gp.PlatformID = p.ID 
        WHERE gp.GameID = g.ID
    ) Platforms ON TRUE   
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(u.ID,CHAR), '¦', u.Name, '¦', u.Abbr) ORDER BY gm.ID SEPARATOR '^^') Value
		FROM tbl_User u
		JOIN tbl_Game_Moderator gm ON gm.UserID = u.ID
		WHERE gm.GameID = g.ID
    ) Moderators ON TRUE;
   