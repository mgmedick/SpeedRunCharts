/****** Object:  StoredProcedure [dbo].[GetPersonalBestsByUserID]    Script Date: 4/15/2022 6:43:01 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE PROC [dbo].[GetPersonalBestsByUserID]
(
    @GameID INT,
    @CategoryID INT,
	@LevelID INT,
    @UserID INT    
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
	
	IF OBJECT_ID('tempdb..#ResultsRaw') IS NOT NULL DROP TABLE #ResultsRaw
	SELECT ROW_NUMBER() OVER (PARTITION BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValueIDs ORDER BY rn.PrimaryTime) AS RowNum,
	rn.ID,
	rn.GameID, 
	rn.CategoryID,
	rn.LevelID,
	rn.PlatformID,
	rn.PlatformName,
	rn.SubCategoryVariableValueIDs,
	rn.VariableValues,
	rn.Players,
	rn.Guests, 
	rn.[Rank],
	rn.PrimaryTime,
	rn.Comment,
	rn.DateSubmitted,
	rn.VerifyDate
	INTO #ResultsRaw	
	FROM dbo.vw_SpeedRunGridUser rn
	WHERE rn.GameID = @GameID
    AND rn.CategoryID = @CategoryID
	AND ISNULL(rn.LevelID, '' ) = ISNULL(@LevelID, '')
    AND rn.UserID = @UserID

	SELECT rn.ID,
	rn.GameID,
	rn.CategoryID,
	rn.LevelID,
	rn.PlatformID,
	rn.SubCategoryVariableValueIDs,
	rn.VariableValues,
	rn.Players,
	rn.Guests,
	rn.[Rank],
	rn.PrimaryTime,
	rn.Comment,
	rn.DateSubmitted,
	rn.VerifyDate
	FROM #ResultsRaw rn
	WHERE rn.RowNum = 1
	ORDER BY rn.ID

END


GO


