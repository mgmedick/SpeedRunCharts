/****** Object:  View [dbo].[vw_SpeedRunSummary]    Script Date: 4/15/2022 7:07:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_SpeedRunSummary]
AS

    SELECT rn.ID,
           g.ID AS GameID,
           g.[Name] AS GameName,
		   g.[Abbr] AS GameAbbr,
           gl.CoverImagePath AS GameCoverImageUrl,
           ct.ID AS CategoryTypeID,
           ct.[Name] AS CategoryTypeName,           
           c.ID AS CategoryID,
           c.[Name] AS CategoryName,
		   l.ID AS LevelID,
		   l.[Name] AS LevelName,
           SubCategoryVariableValues.[Value] AS SubCategoryVariableValues,
           Players.[Value] AS Players,
		   EmbeddedVideoLinks.[Value] AS EmbeddedVideoLinks,
           rn.[Rank],
           rn.PrimaryTime,
           rn.DateSubmitted,
		   rn.VerifyDate,
           rn.ImportedDate
    FROM dbo.tbl_SpeedRun rn
    JOIN dbo.tbl_Game g ON g.ID = rn.GameID
	JOIN dbo.tbl_Game_Link gl ON gl.GameID = g.ID
    JOIN dbo.tbl_Category c ON c.ID = rn.CategoryID
    JOIN dbo.tbl_CategoryType ct ON ct.ID = c.CategoryTypeID
    LEFT JOIN dbo.tbl_Level l ON l.ID = rn.LevelID
    OUTER APPLY (SELECT STUFF(
                (   SELECT '^^' + CONVERT(VARCHAR, rv.[VariableID]) + '¦' + CONVERT(VARCHAR, rv.VariableValueID) + '¦' + va.[Value]
                    FROM dbo.tbl_SpeedRun_VariableValue rv
                    JOIN dbo.tbl_Variable v ON v.ID = rv.VariableID AND v.IsSubCategory = 1
					JOIN dbo.tbl_VariableValue va ON va.ID = rv.VariableValueID
                    WHERE rv.SpeedRunID = rn.ID
                    ORDER BY rv.ID
                    FOR XML PATH (''), root('MyString'), TYPE
                ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
        ) AS SubCategoryVariableValues
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '¦' + u.[Name] + '¦' + ISNULL(u.[Abbr],'')
                        FROM dbo.tbl_SpeedRun_Player rp  
						JOIN dbo.tbl_User u ON u.ID = rp.UserID
						WHERE rp.SpeedRunID = rn.ID
                        ORDER BY rp.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Players           
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + rd.EmbeddedVideoLinkUrl + '|' + ISNULL(rd.ThumbnailLinkUrl,'')
                        FROM dbo.tbl_SpeedRun_Video rd
                        WHERE rd.SpeedRunID = rn.ID
                        AND rd.EmbeddedVideoLinkUrl IS NOT NULL
                        ORDER BY rd.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 1, '') AS [Value]
            ) AS EmbeddedVideoLinks 

GO


