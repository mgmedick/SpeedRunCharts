SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[vw_User]
AS
	SELECT u.ID, u.[Name], u.Abbr, u.SignUpDate, uc.[Location],
	ul.SpeedRunComUrl, ul.ProfileImageUrl, ul.TwitchProfileUrl, ul.HitboxProfileUrl, ul.YoutubeProfileUrl, ul.TwitterProfileUrl, ul.SpeedRunsLiveProfileUrl,
	TotalSpeedRuns.Value AS TotalSpeedRuns,
	TotalWorldRecords.Value AS TotalWorldRecords,
	TotalPersonalBests.Value AS TotalPersonalBests
	FROM dbo.tbl_User u WITH (NOLOCK)
	JOIN dbo.tbl_User_Link ul WITH (NOLOCK) ON ul.UserID = u.ID
	LEFT JOIN dbo.tbl_User_Location uc WITH (NOLOCK) ON uc.UserID = u.ID
	OUTER APPLY (SELECT COUNT(*) AS Value
					FROM dbo.tbl_SpeedRun_Player sp
					WHERE sp.UserID = u.ID
		) AS TotalSpeedRuns
	OUTER APPLY (SELECT COUNT(*) AS Value
					FROM dbo.tbl_SpeedRun_Player sp
					JOIN dbo.tbl_SpeedRun sr ON sr.ID=sp.SpeedRunID AND sr.[Rank]=1
					WHERE sp.UserID = u.ID
		) AS TotalWorldRecords
	OUTER APPLY (SELECT COUNT(*) AS Value
					FROM (
					SELECT sr.GameID, sr.CategoryID, sr.LevelID, SubCategoryVariableValueIDs.[Value]
					FROM dbo.tbl_SpeedRun_Player sp
					JOIN dbo.tbl_SpeedRun sr ON sr.ID=sp.SpeedRunID
					OUTER APPLY (SELECT STUFF(
						(SELECT ',' + CONVERT(VARCHAR, rv.VariableValueID)
						FROM dbo.tbl_SpeedRun_VariableValue rv
						JOIN dbo.tbl_Variable v ON v.ID=rv.VariableID AND v.IsSubCategory='1'
						WHERE rv.SpeedRunID = sr.ID                       
						ORDER BY rv.ID
						FOR XML PATH ('')
						), 1, 1, '') AS [Value] 
					) AS SubCategoryVariableValueIDs 
					WHERE sp.UserID = u.ID
					GROUP BY sr.GameID, sr.CategoryID, sr.LevelID, SubCategoryVariableValueIDs.[Value]
				) AS SubQuery
		) AS TotalPersonalBests

GO


