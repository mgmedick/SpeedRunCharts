SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vw_UserAccount]
AS

    SELECT ua.ID AS UserAccountID,
	ua.Username,
	ue.IsDarkTheme,
	SpeedRunListCategoryIDs.[Value] AS SpeedRunListCategoryIDs
    FROM dbo.tbl_UserAccount ua
	LEFT JOIN dbo.tbl_UserAccount_Setting ue ON ue.UserAccountID = ua.ID
    OUTER APPLY (SELECT STUFF(
                (   SELECT ',' + CONVERT(VARCHAR, uc.[SpeedRunListCategoryID])
                    FROM dbo.tbl_UserAccount_SpeedRunListCategory uc
                    WHERE uc.UserAccountID = ua.ID
                    ORDER BY uc.UserAccountID
                    FOR XML PATH ('')
                ), 1, 1, '') AS [Value] 
        ) AS SpeedRunListCategoryIDs
GO


