-- GetGamesByUserID
DROP PROCEDURE IF EXISTS GetGamesByUserID;

DELIMITER &&
CREATE PROCEDURE GetGamesByUserID
(
	IN UserID VARCHAR(20)
)
BEGIN
	DROP TEMPORARY TABLE IF EXISTS ResultsRaw;

	CREATE TEMPORARY TABLE ResultsRaw
	( 
		GameID INT,
		CategoryID INT,
		LevelID INT,
		VariableID INT,
		VariableValueID INT
	);

	INSERT INTO ResultsRaw (GameID, CategoryID, LevelID, VariableID, VariableValueID)
	SELECT rn.GameID, rn.CategoryID, rn.LevelID, rv.VariableID, rv.VariableValueID
	FROM tbl_SpeedRun rn
	JOIN tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID
	LEFT JOIN tbl_SpeedRun_VariableValue rv ON rv.SpeedRunID = rn.ID
	WHERE rp.UserID = UserID;

	SELECT g.ID, g.Name, gl.CoverImageUrl, g.YearOfRelease,
	CategoryTypes.Value AS CategoryTypes, Categories.Value AS Categories, Levels.Value AS Levels,
	Variables.Value AS Variables, VariableValues.Value AS VariableValues, Platforms.Value AS Platforms, Moderators.Value AS Moderators
	FROM ResultsRaw r
	JOIN tbl_Game g  ON g.ID = r.GameID
	JOIN tbl_Game_Link gl  ON gl.GameID = g.ID
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(ct.ID,CHAR), '|', ct.Name) ORDER BY ct.ID SEPARATOR '^^') Value
		FROM ResultsRaw r1
		JOIN tbl_Category c  ON c.ID = r1.CategoryID
		JOIN tbl_CategoryType ct  ON ct.ID = c.CategoryTypeID
		WHERE r1.GameID = r.GameID
	) CategoryTypes ON TRUE   	
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(c.ID,CHAR), '|', CONVERT(c.CategoryTypeID,CHAR), '|', ct.Name) ORDER BY ct.ID SEPARATOR '^^') Value
		FROM ResultsRaw r1
		JOIN tbl_Category c  ON c.ID = r1.CategoryID
		WHERE r1.GameID = r.GameID
	) Categories ON TRUE  
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(l.ID,CHAR), '|', l.Name) ORDER BY l.ID SEPARATOR '^^') Value
		FROM ResultsRaw r1
		JOIN tbl_Level l  ON l.ID = r1.LevelID
		WHERE r1.GameID = r.GameID
	) Levels ON TRUE  
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(v.ID,CHAR), '|', CASE v.IsSubCategory WHEN 1 THEN 'True' ELSE 'False' END, '|', CONVERT(v.VariableScopeTypeID, CHAR), '|', COALESCE(CONVERT(v.CategoryID, CHAR),''), '|', COALESCE(CONVERT(v.LevelID, CHAR),''), '|', v.Name) ORDER BY v.ID SEPARATOR '^^') Value
		FROM ResultsRaw r1                       
		JOIN tbl_Variable v  ON v.ID = r1.VariableID
		WHERE r1.GameID = r.GameID
	) Variables ON TRUE  
 	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(v.ID,CHAR), '|', CONVERT(v.VariableID,CHAR), '|', v.Value) ORDER BY v.ID SEPARATOR '^^') Value
		FROM ResultsRaw r1                       
		JOIN tbl_VariableValue v  ON v.ID = r1.VariableValueID
		WHERE r1.GameID = r.GameID
	) VariableValues ON TRUE  
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(p.ID,CHAR), '|', p.Name) ORDER BY p.ID SEPARATOR '^^') Value
	    FROM tbl_Platform p
	    JOIN tbl_Game_Platform gp ON gp.PlatformID = p.ID 
        WHERE gp.GameID = r.GameID
    ) Platforms ON TRUE   
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(u.ID,CHAR), '¦', u.Name, '¦', u.Abbr) ORDER BY gm.ID SEPARATOR '^^') Value
		FROM tbl_User u
		JOIN tbl_Game_Moderator gm ON gm.UserID = u.ID
		WHERE gm.GameID = r.GameID
    ) Moderators ON TRUE
	GROUP BY g.ID, g.Name, gl.CoverImageUrl, g.YearOfRelease, CategoryTypes.Value, Categories.Value, Levels.Value, Variables.Value, VariableValues.Value, Platforms.Value, Moderators.Value
	ORDER BY g.Name;

END $$
DELIMITER ;
