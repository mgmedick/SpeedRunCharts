/****** Object:  View [dbo].[vw_GameSpeedRunCom]    Script Date: 4/15/2022 6:47:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vw_GameSpeedRunCom]
AS

    SELECT g.ID,
           gc.SpeedRunComID,  
           g.Name,
           g.IsRomHack,
           g.YearOfRelease,
           gl.CoverImageUrl,      
           Categories.[Value] AS CategorySpeedRunComIDs,
           Levels.[Value] AS LevelSpeedRunComIDs,
           Variables.[Value] AS VariableSpeedRunComIDs,
           VariableValues.[Value] AS VariableValueSpeedRunComIDs,
           Platforms.[Value] AS PlatformSpeedRunComIDs,
           Moderators.[Value] AS ModeratorSpeedRunComIDs,
           g.CreatedDate,
           g.ModifiedDate,
           g.IsChanged
    FROM dbo.tbl_Game g WITH (NOLOCK)
    JOIN dbo.tbl_Game_SpeedRunComID gc WITH (NOLOCK) ON gc.GameID = g.ID
    JOIN dbo.tbl_Game_Link gl WITH (NOLOCK) ON gl.GameID = g.ID         
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + cc.SpeedRunComID
                        FROM dbo.tbl_Category c WITH (NOLOCK)
                        JOIN dbo.tbl_Category_SpeedRunComID cc WITH (NOLOCK) ON cc.CategoryID=c.ID
                        WHERE c.GameID = g.ID                        
                        ORDER BY c.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS Categories   	   
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + lc.SpeedRunComID
                        FROM dbo.tbl_Level l WITH (NOLOCK)
                        JOIN dbo.tbl_Level_SpeedRunComID lc WITH (NOLOCK) ON lc.LevelID = l.ID
                        WHERE l.GameID = g.ID
                        ORDER BY l.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS Levels   	
     OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + vc.SpeedRunComID
                        FROM dbo.tbl_Variable v WITH (NOLOCK)
                        JOIN dbo.tbl_Variable_SpeedRunComID vc WITH (NOLOCK) ON vc.VariableID = v.ID
                        WHERE v.GameID = g.ID
                        ORDER BY v.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value]
            ) AS Variables
	OUTER APPLY (SELECT STUFF(
            (   SELECT ',' + vc.SpeedRunComID
                FROM dbo.tbl_VariableValue v WITH (NOLOCK)
                JOIN dbo.tbl_VariableValue_SpeedRunComID vc WITH (NOLOCK) ON vc.VariableValueID = v.ID
                WHERE v.GameID = g.ID
                ORDER BY v.ID
                FOR XML PATH ('')
            ), 1, 1, '') AS [Value] 
    ) AS VariableValues    
    OUTER APPLY (SELECT STUFF(
                (   SELECT ',' + pc.SpeedRunComID
                    FROM dbo.tbl_Platform p WITH (NOLOCK)
                    JOIN dbo.tbl_Game_Platform gp WITH (NOLOCK) ON gp.PlatformID = p.ID
                    JOIN dbo.tbl_Platform_SpeedRunComID pc WITH (NOLOCK) ON pc.PlatformID = p.ID
                    WHERE gp.GameID = g.ID
                    ORDER BY p.ID
                    FOR XML PATH ('')
                ), 1, 1, '') AS [Value] 
        ) AS Platforms
	OUTER APPLY (SELECT STUFF(
				(   SELECT ',' + uc.SpeedRunComID
					FROM dbo.tbl_User u WITH (NOLOCK)
					JOIN dbo.tbl_Game_Moderator gm WITH (NOLOCK) ON gm.UserID = u.ID
                    JOIN dbo.tbl_User_SpeedRunComID uc WITH (NOLOCK) ON uc.UserID = u.ID 
					WHERE gm.GameID = g.ID
					ORDER BY gm.ID
					FOR XML PATH ('')
				), 1, 1, '') AS [Value] 
		) AS Moderators
GO


