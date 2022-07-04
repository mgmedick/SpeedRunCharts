/****** Object:  View [dbo].[vw_SpeedRun]    Script Date: 4/15/2022 6:47:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_SpeedRun]
AS

    SELECT rn.ID,
           g.ID AS GameID,
           g.[Name] AS GameName,
           st.ID AS StatusTypeID,
           st.[Name] AS StatusTypeName,           
           gl.CoverImageUrl AS GameCoverImageUrl,
           ct.ID AS CategoryTypeID,
           ct.[Name] AS CategoryTypeName,
           c.ID AS CategoryID,
           c.[Name] AS CategoryName,
		   l.ID AS LevelID,
		   l.[Name] AS LevelName,
           pl.ID AS PlatformID,
           pl.[Name] AS PlatformName,
           VariableValues.[Value] AS VariableValues,
           Players.[Value] AS Players,
           Guests.[Value] AS Guests,
           VideoLinks.[Value] AS VideoLinks,
		   EmbeddedVideoLinks.[Value] AS EmbeddedVideoLinks,
           rs.IsEmulated,
           rn.[Rank],
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
    FROM dbo.tbl_SpeedRun rn 
    JOIN dbo.tbl_Game g  ON g.ID = rn.GameID
	JOIN dbo.tbl_Game_Link gl  ON gl.GameID = rn.GameID
    JOIN dbo.tbl_RunStatusType st ON st.ID = rn.StatusTypeID 
    JOIN dbo.tbl_Category c  ON c.ID = rn.CategoryID
    JOIN dbo.tbl_CategoryType ct ON ct.ID = c.CategoryTypeID
    JOIN dbo.tbl_SpeedRun_System rs ON rs.SpeedRunID = rn.ID
    JOIN dbo.tbl_SpeedRun_Time rt ON rt.SpeedRunID = rn.ID
    JOIN dbo.tbl_SpeedRun_Link rl ON rl.SpeedRunID = rn.ID
    LEFT JOIN dbo.tbl_Level l  ON l.ID = rn.LevelID
    LEFT JOIN dbo.tbl_Platform pl on pl.ID = rs.PlatformID
    LEFT JOIN dbo.tbl_SpeedRun_Comment rc ON rc.SpeedRunID = rn.ID
    OUTER APPLY (SELECT STUFF(
                    (    SELECT ',' + CONVERT(VARCHAR, rv.VariableID) + '|' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        WHERE rv.SpeedRunID = rn.ID
                        ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS VariableValues  
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '|' + u.[Name]
                        FROM dbo.tbl_SpeedRun_Player rp  
						JOIN dbo.tbl_User u ON u.ID = rp.UserID
						WHERE rp.SpeedRunID = rn.ID
                        ORDER BY rp.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Players  
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, g.ID) + '|' + g.[Name]
                        FROM dbo.tbl_SpeedRun_Guest rg  
						JOIN dbo.tbl_Guest g ON g.ID = rg.GuestID
						WHERE rg.SpeedRunID = rn.ID
                        ORDER BY rg.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Guests              
    OUTER APPLY (SELECT STUFF(
                    (    SELECT ',' + rd.VideoLinkUrl
                        FROM dbo.tbl_SpeedRun_Video rd 
                        WHERE rd.SpeedRunID = rn.ID
                        ORDER BY rd.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 1, '') AS [Value]
            ) AS VideoLinks
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + rd.EmbeddedVideoLinkUrl
                        FROM dbo.tbl_SpeedRun_Video rd
                        WHERE rd.SpeedRunID = rn.ID
                        AND rd.EmbeddedVideoLinkUrl IS NOT NULL
                        ORDER BY rd.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 1, '') AS [Value] 
            ) AS EmbeddedVideoLinks
GO


