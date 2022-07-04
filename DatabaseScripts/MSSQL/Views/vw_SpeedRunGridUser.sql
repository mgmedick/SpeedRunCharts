/****** Object:  View [dbo].[vw_SpeedRunGridUser]    Script Date: 4/15/2022 7:06:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vw_SpeedRunGridUser]
AS

    SELECT rn.ID,
           rn.GameID,
           rn.CategoryID,
           rn.LevelID,
           p.ID AS PlatformID,
           p.[Name] AS PlatformName,
           SubCategoryVariableValueIDs.[Value] AS SubCategoryVariableValueIDs,
           VariableValues.[Value] AS VariableValues,
           Players.[Value] AS Players,
		   Guests.[Value] AS Guests,
           rn.[Rank],
           rn.PrimaryTime,
           rc.Comment,
           rn.DateSubmitted,
           rn.VerifyDate,
           rp.UserID
    FROM dbo.tbl_SpeedRun rn
   	JOIN dbo.tbl_SpeedRun_System rs ON rs.SpeedRunID = rn.ID
    JOIN dbo.tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID
   	LEFT JOIN dbo.tbl_SpeedRun_Comment rc ON rc.SpeedRunID = rn.ID
   	LEFT JOIN dbo.tbl_Platform p ON p.ID = rs.PlatformID
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        JOIN dbo.tbl_Variable v ON v.ID=rv.VariableID AND v.IsSubCategory = '1'
                        WHERE rv.SpeedRunID = rn.ID                       
                        --ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS SubCategoryVariableValueIDs				
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rv.VariableID) + '|' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        WHERE rv.SpeedRunID = rn.ID
                        --ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS VariableValues	
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, u.ID) + '¦' + u.[Name] + '¦' + ISNULL(u.Abbr, '')
                        FROM dbo.tbl_SpeedRun_Player rp  
						JOIN dbo.tbl_User u ON u.ID = rp.UserID
						WHERE rp.SpeedRunID = rn.ID
                        --ORDER BY rp.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Players            	
    OUTER APPLY (SELECT STUFF(
                    (   SELECT '^^' + CONVERT(VARCHAR, g.ID) + '¦' + g.[Name] + '¦' + ISNULL(g.Abbr, '')
                        FROM dbo.tbl_SpeedRun_Guest rg
						JOIN dbo.tbl_Guest g ON g.ID = rg.GuestID
						WHERE rg.SpeedRunID = rn.ID
                        --ORDER BY rg.ID
                        FOR XML PATH (''), root('MyString'), TYPE
                    ).value('/MyString[1]','varchar(max)'), 1, 2, '') AS [Value]
            ) AS Guests
            
GO


