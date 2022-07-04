-- vw_User
DROP VIEW IF EXISTS vw_User;

CREATE VIEW vw_User AS

	SELECT u.ID, u.Name, u.Abbr, u.SignUpDate, uc.Location,
	ul.SpeedRunComUrl, ul.ProfileImageUrl, ul.TwitchProfileUrl, ul.HitboxProfileUrl, ul.YoutubeProfileUrl, ul.TwitterProfileUrl, ul.SpeedRunsLiveProfileUrl,
	TotalSpeedRuns.Value AS TotalSpeedRuns,
	TotalWorldRecords.Value AS TotalWorldRecords,
	TotalPersonalBests.Value AS TotalPersonalBests
	FROM tbl_User u
	JOIN tbl_User_Link ul ON ul.UserID = u.ID
	LEFT JOIN tbl_User_Location uc ON uc.UserID = u.ID
	LEFT JOIN LATERAL (
		SELECT COUNT(*) AS Value
		FROM tbl_SpeedRun_Player sp
		WHERE sp.UserID = u.ID 
	) TotalSpeedRuns ON TRUE  	
	LEFT JOIN LATERAL (
		SELECT COUNT(*) AS Value				
		FROM tbl_SpeedRun_Player sp
		JOIN tbl_SpeedRun sr ON sr.ID=sp.SpeedRunID AND sr.`Rank`=1
		WHERE sp.UserID = u.ID
	) TotalWorldRecords ON TRUE  		
	LEFT JOIN LATERAL (
		SELECT COUNT(*) AS Value				
		FROM (
			SELECT sr.GameID, sr.CategoryID, sr.LevelID, SubCategoryVariableValueIDs.Value
			FROM tbl_SpeedRun_Player sp
			JOIN tbl_SpeedRun sr ON sr.ID=sp.SpeedRunID
			LEFT JOIN LATERAL (
				SELECT GROUP_CONCAT(CONVERT(rv.VariableValueID,CHAR) SEPARATOR ',') Value
		        FROM tbl_SpeedRun_VariableValue rv
		        JOIN tbl_Variable v ON v.ID=rv.VariableID AND v.IsSubCategory = 1
		        WHERE rv.SpeedRunID = sr.ID     
			) SubCategoryVariableValueIDs ON TRUE  
			WHERE sp.UserID = u.ID
			GROUP BY sr.GameID, sr.CategoryID, sr.LevelID, SubCategoryVariableValueIDs.Value
		) SubQuery
	) TotalPersonalBests ON TRUE;