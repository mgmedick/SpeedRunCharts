/****** Object:  View [dbo].[vw_Game]    Script Date: 4/15/2022 6:46:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vw_Game]
AS

    SELECT g.ID, g.Name, g.Abbr, gl.CoverImagePath AS CoverImageUrl, g.YearOfRelease, CategoryTypes.[Value] AS CategoryTypes, Categories.[Value] AS Categories, Levels.[Value] AS Levels,
        Variables.[Value] AS Variables, VariableValues.[Value] AS VariableValues, Platforms.[Value] AS Platforms, Moderators.Value AS Moderators, gl.SpeedRunComUrl             
    FROM dbo.tbl_Game g WITH (NOLOCK)
    JOIN dbo.tbl_Game_Link gl WITH (NOLOCK) ON gl.GameID = g.ID
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, ct.ID) + '|' + ct.[Name]
                        FROM dbo.tbl_CategoryType ct WITH (NOLOCK)
                        JOIN dbo.tbl_Category c WITH (NOLOCK) ON c.CategoryTypeID = ct.ID
                        WHERE c.GameID = g.ID
                        GROUP BY ct.ID, ct.[Name]
                        ORDER BY ct.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS CategoryTypes        
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, c.ID) + '|' + CONVERT(VARCHAR, c.CategoryTypeID) + '|' + c.[Name]
                        FROM dbo.tbl_Category c WITH (NOLOCK)
                        WHERE c.GameID = g.ID                        
                        ORDER BY c.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Categories   	   
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, l.ID) + '|' + l.[Name]
                        FROM dbo.tbl_Level l WITH (NOLOCK)
                        WHERE l.GameID = g.ID
                        ORDER BY l.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Levels   	
     OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, v.ID) + '|' + CASE v.IsSubCategory WHEN 1 THEN 'True' ELSE 'False' END + '|' + CONVERT(VARCHAR, v.VariableScopeTypeID) + '|' + ISNULL(CONVERT(VARCHAR, v.CategoryID),'') + '|' + ISNULL(CONVERT(VARCHAR, v.LevelID),'') + '|' + v.[Name]
                        FROM dbo.tbl_Variable v WITH (NOLOCK)
                        WHERE v.GameID = g.ID
                        ORDER BY v.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Variables
	OUTER APPLY (SELECT STUFF(
            (   SELECT '^^' + CONVERT(VARCHAR, v.ID) + '|' + CONVERT(VARCHAR, v.VariableID) + '|' + v.[Value]
                FROM dbo.tbl_VariableValue v WITH (NOLOCK)
                WHERE v.GameID = g.ID
                ORDER BY v.ID
                FOR XML PATH (''), root('MyString'), TYPE
            ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
    ) AS VariableValues    
    OUTER APPLY (SELECT STUFF(
                (   SELECT '^^' + CONVERT(VARCHAR, p.ID) + '|' + p.[Name]
                    FROM dbo.tbl_Platform p WITH (NOLOCK)
                    JOIN dbo.tbl_Game_Platform gp WITH (NOLOCK) ON gp.PlatformID = p.ID 
                    WHERE gp.GameID = g.ID
                    ORDER BY p.ID
                    FOR XML PATH (''), root('MyString'), TYPE
                ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
        ) AS Platforms
	OUTER APPLY (SELECT STUFF(
				(   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '¦' + u.[Name] + '¦' + u.[Abbr]
					FROM dbo.tbl_User u WITH (NOLOCK)
					JOIN dbo.tbl_Game_Moderator gm WITH (NOLOCK) ON gm.UserID = u.ID
					WHERE gm.GameID = g.ID
					ORDER BY gm.ID
                    FOR XML PATH (''), root('MyString'), TYPE
                ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
		) AS Moderators

GO


