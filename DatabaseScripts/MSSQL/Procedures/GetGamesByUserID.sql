/****** Object:  StoredProcedure [dbo].[GetGamesByUserID]    Script Date: 4/15/2022 6:37:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[GetGamesByUserID]
(
     @UserID VARCHAR(20)
)
AS
BEGIN

	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	IF OBJECT_ID('tempdb..#ResultsRaw') IS NOT NULL 
	BEGIN 
		DROP TABLE #ResultsRaw
	END

	CREATE TABLE #ResultsRaw
	( 
			[GameID] INT,
			[CategoryID] INT,
			[LevelID] INT,
			[VariableID] INT,
			[VariableValueID] INT
	)

	INSERT INTO #ResultsRaw (GameID, CategoryID, LevelID, VariableID, VariableValueID)
	SELECT rn.GameID, rn.CategoryID, rn.LevelID, rv.VariableID, rv.VariableValueID
	FROM dbo.tbl_SpeedRun rn
	JOIN dbo.tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID
	LEFT JOIN dbo.tbl_SpeedRun_VariableValue rv ON rv.SpeedRunID = rn.ID
	WHERE rp.UserID = @UserID

	SELECT g.ID, g.[Name], gl.CoverImageUrl, g.YearOfRelease,
	CategoryTypes.[Value] AS CategoryTypes, Categories.[Value] AS Categories, Levels.[Value] AS Levels,
	Variables.[Value] AS Variables, VariableValues.[Value] AS VariableValues, Platforms.[Value] AS Platforms, Moderators.[Value] AS Moderators
	FROM #ResultsRaw r
	JOIN dbo.tbl_Game g  ON g.ID = r.GameID
	JOIN dbo.tbl_Game_Link gl  ON gl.GameID = g.ID
	OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, ct.ID) + '|' + ct.[Name]
						FROM #ResultsRaw r1
						JOIN dbo.tbl_Category c  ON c.ID = r1.CategoryID
						JOIN dbo.tbl_CategoryType ct  ON ct.ID = c.CategoryTypeID
						WHERE r1.GameID = r.GameID
						GROUP BY ct.ID, ct.[Name]
						ORDER BY ct.ID
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS CategoryTypes
	OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, c.ID) + '|' + CONVERT(VARCHAR, c.CategoryTypeID) + '|' + c.[Name]
						FROM #ResultsRaw r1
						JOIN dbo.tbl_Category c  ON c.ID = r1.CategoryID
						WHERE r1.GameID = r.GameID
						GROUP BY c.ID, c.[Name], c.CategoryTypeID
						ORDER BY c.ID
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Categories 
	OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, l.ID) + '|' + l.[Name]
						FROM #ResultsRaw r1
						JOIN dbo.tbl_Level l  ON l.ID = r1.LevelID
						WHERE r1.GameID = r.GameID
						GROUP BY l.ID, l.[Name]
						ORDER BY l.ID                       
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Levels   
		OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, v.ID) + '|' + CASE v.IsSubCategory WHEN 1 THEN 'True' ELSE 'False' END + '|' + CONVERT(VARCHAR, v.VariableScopeTypeID) + '|' + ISNULL(CONVERT(VARCHAR, v.CategoryID),'') + '|' + ISNULL(CONVERT(VARCHAR, v.LevelID),'') + '|' + v.[Name]
						FROM #ResultsRaw r1                       
						JOIN dbo.tbl_Variable v  ON v.ID = r1.VariableID
						WHERE r1.GameID = r.GameID
						GROUP BY v.ID, v.[Name], v.IsSubCategory, v.VariableScopeTypeID, v.CategoryID, v.LevelID
						ORDER BY v.ID                       
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Variables 
		OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, v.ID) + '|' + CONVERT(VARCHAR, v.VariableID) + '|' + v.[Value]
						FROM #ResultsRaw r1                       
						JOIN dbo.tbl_VariableValue v  ON v.ID = r1.VariableValueID
						WHERE r1.GameID = r.GameID
						GROUP BY v.ID, v.[Value], v.VariableID
						ORDER BY v.ID                       
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS VariableValues
		OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, p.ID) + '|' + p.[Name]
						FROM dbo.tbl_Platform p 
						JOIN dbo.tbl_Game_Platform gp  ON gp.PlatformID = p.ID 
						WHERE gp.GameID = r.GameID
						GROUP BY gp.ID, p.ID, p.[Name]
						ORDER BY gp.ID
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Platforms
		OUTER APPLY (SELECT STUFF(
					(   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '|' + u.[Name]
						FROM dbo.tbl_User u 
						JOIN dbo.tbl_Game_Moderator gm  ON gm.UserID = u.ID
						WHERE gm.GameID = r.GameID
						GROUP BY gm.ID, u.ID, u.[Name]
						ORDER BY gm.ID
						FOR XML PATH ('')
					), 1, 2, '') AS [Value] 
			) AS Moderators
	GROUP BY g.ID, g.[Name], gl.CoverImageUrl, g.YearOfRelease, CategoryTypes.[Value], Categories.[Value], Levels.[Value], Variables.[Value], VariableValues.[Value], Platforms.[Value], Moderators.[Value]
	ORDER BY g.[Name]

END
GO


