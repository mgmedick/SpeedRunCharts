/****** Object:  View [dbo].[vw_SpeedRunGridTab]    Script Date: 4/15/2022 7:05:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_SpeedRunGridTab]
AS

    SELECT rn.ID,
           rn.GameID,
           rn.CategoryID,
           rn.LevelID,
           VariableValues.[Value] AS VariableValues,
           rn.Rank
    FROM dbo.tbl_SpeedRun rn			               
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rv.VariableID) + '|' + CONVERT(VARCHAR, rv.VariableValueID)
                        FROM dbo.tbl_SpeedRun_VariableValue rv
                        WHERE rv.SpeedRunID = rn.ID
                        ORDER BY rv.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS VariableValues
GO


