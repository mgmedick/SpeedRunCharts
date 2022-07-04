/****** Object:  StoredProcedure [dbo].[GetSpeedRunsByUserID]    Script Date: 4/15/2022 6:39:11 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROC [dbo].[GetSpeedRunsByUserID]
(
    @GameID INT,
    @CategoryID INT,
    @LevelID INT,
    @VariableValueIDs VARCHAR(MAX),
    @UserID INT
)
AS
BEGIN

     SET NOCOUNT ON;
     SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

	 SELECT rn.ID,
     rn.GameID,
     --rn.CategoryTypeID,
     rn.CategoryID,
     rn.LevelID,
     rn.PlatformID,
     rn.PlatformName,
	 rn.SubCategoryVariableValueIDs,
     --rn.Variables,
     rn.VariableValues,
     rn.Players,
     rn.Guests,
     --rn.IsEmulated,
     rn.[Rank],
     rn.PrimaryTime,
     rn.Comment,
     rn.DateSubmitted,
     rn.VerifyDate 
	 FROM dbo.vw_SpeedRunGrid rn
	 JOIN dbo.tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID AND rp.UserID = @UserID
	 WHERE rn.GameID = @GameID
     AND rn.CategoryID = @CategoryID
     AND ISNULL(rn.LevelID,'') = ISNULL(@LevelID,'')
     AND ISNULL(rn.SubCategoryVariableValueIDs,'') = ISNULL(@VariableValueIDs,'')
	 ORDER BY rn.ID DESC

END


GO


