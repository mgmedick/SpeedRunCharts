SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_SpeedRunGridTabUser]
AS

    SELECT rn.ID,
           rn.GameID,
           rn.CategoryID,
           rn.LevelID,
           rn.VariableValues,
           rp.UserID,
           rn.Rank
    FROM dbo.vw_SpeedRunGridTab rn
	JOIN dbo.tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID
GO


