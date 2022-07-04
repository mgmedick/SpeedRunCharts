/****** Object:  StoredProcedure [dbo].[GetLatestSpeedRuns]    Script Date: 4/15/2022 6:38:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROC [dbo].[GetLatestSpeedRuns]
(
     @SpeedRunListCategoryID INT,
     @TopAmount INT,
     @OrderValueOffset INT = NULL
)
WITH RECOMPILE
AS
BEGIN

     SET NOCOUNT ON;
     SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

     --new
     IF(@SpeedRunListCategoryID = 0)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          ORDER BY rn.ID DESC
     END
	 --top 5%
     ELSE IF(@SpeedRunListCategoryID = 1)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate              
          FROM dbo.vw_SpeedRunSummary rn        
			CROSS APPLy (SELECT MAX(rn1.[Rank]) AS [Value]
								FROM dbo.vw_SpeedRunSummary rn1
								WHERE rn1.GameID = rn.GameID
								AND rn1.CategoryID = rn.CategoryID
								AND ISNULL(rn1.LevelID,'') = ISNULL(rn.LevelID,'')
								AND ISNULL(rn1.SubCategoryVariableValues,'') = ISNULL(rn.SubCategoryVariableValues,'')
								AND rn1.[Rank] IS NOT NULL
					) AS MaxRank
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          AND ((rn.[Rank] / (MaxRank.[Value])) * 100.00) <= 5.00
          ORDER BY rn.ID DESC
     END
	 --first
     ELSE IF(@SpeedRunListCategoryID = 2)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
		  AND rn.[Rank] = 1
		  AND EXISTS (SELECT 1
					  FROM dbo.vw_SpeedRunSummary rn1
					  WHERE rn1.GameID = rn.GameID
					  AND rn1.CategoryID = rn.CategoryID
					  AND ISNULL(rn1.LevelID,'') = ISNULL(rn.LevelID,'')
					  AND ISNULL(rn1.SubCategoryVariableValues,'') = ISNULL(rn.SubCategoryVariableValues,'')
					  AND rn1.[Rank] > 1)
          ORDER BY rn.ID DESC
     END
	 --top 3
     ELSE IF(@SpeedRunListCategoryID = 3)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
		  AND rn.[Rank] <= 3
		  AND EXISTS (SELECT 1
					  FROM dbo.vw_SpeedRunSummary rn1
					  WHERE rn1.GameID = rn.GameID
					  AND rn1.CategoryID = rn.CategoryID
					  AND ISNULL(rn1.LevelID,'') = ISNULL(rn.LevelID,'')
					  AND ISNULL(rn1.SubCategoryVariableValues,'') = ISNULL(rn.SubCategoryVariableValues,'')
					  AND rn1.[Rank] > 3)
          ORDER BY rn.ID DESC
     END
	 --Bests
     ELSE IF(@SpeedRunListCategoryID = 4)
     BEGIN
          SELECT TOP (@TopAmount)
          rn.ID, rn.GameID, rn.GameName, rn.GameAbbr, 
          rn.GameCoverImageUrl, 
          --NULL AS GameCoverImageUrl,          
          rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
          rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate           
          FROM dbo.vw_SpeedRunSummary rn
		  CROSS APPLy (SELECT MIN(rn1.[PrimaryTime]) AS [Value]
								FROM dbo.vw_SpeedRunSummary rn1
								WHERE rn1.GameID = rn.GameID
								AND rn1.CategoryID = rn.CategoryID
								AND ISNULL(rn1.LevelID,'') = ISNULL(rn.LevelID,'')
								AND ISNULL(rn1.SubCategoryVariableValues,'') = ISNULL(rn.SubCategoryVariableValues,'')
								AND ISNULL(rn1.Players,'') = ISNULL(rn.Players,'')
								AND rn1.ID <> rn.ID
			    ) AS MinPrimaryTime
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          AND rn.[PrimaryTime] < MinPrimaryTime.[Value]
          ORDER BY rn.ID DESC     
     END
	 --Popular
     ELSE IF(@SpeedRunListCategoryID = 5)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          AND EXISTS (SELECT 1
                      FROM dbo.tbl_SpeedRun_Video_Detail rd
                      WHERE rd.SpeedRunID = rn.ID
                      AND rd.ViewCount > 1000)
          AND EXISTS (SELECT 1
                      FROM dbo.tbl_SpeedRun_Video rd
                      WHERE rd.SpeedRunID = rn.ID
                      GROUP BY rd.SpeedRunID
                      HAVING COUNT(DISTINCT rd.ID) = 1
                    )
          ORDER BY rn.ID DESC
     END
	 --GDQ
     ELSE IF(@SpeedRunListCategoryID = 7)
     BEGIN
          SELECT TOP (@TopAmount)
               rn.ID, rn.GameID, rn.GameName, rn.GameAbbr, 
               rn.GameCoverImageUrl, 
               --NULL AS GameCoverImageUrl,               
               rn.CategoryTypeID, rn.CategoryTypeName, rn.CategoryID, rn.CategoryName, rn.LevelID, rn.LevelName,
			   rn.SubCategoryVariableValues, rn.Players, rn.EmbeddedVideoLinks, rn.[Rank], rn.PrimaryTime, rn.DateSubmitted, rn.VerifyDate, rn.ImportedDate             
          FROM dbo.vw_SpeedRunSummary rn
          WHERE ((@OrderValueOffset IS NULL) OR (rn.ID < @OrderValueOffset))
          AND rn.EmbeddedVideoLinks IS NOT NULL
          AND EXISTS (SELECT 1
                      FROM dbo.tbl_SpeedRun_Video_Detail rd
                      WHERE rd.SpeedRunID = rn.ID
                      AND rd.ChannelID IN ('22510310','UCI3DTtB-a3fJPjKtQ5kYHfA'))
          ORDER BY rn.ID DESC
     END

END
GO


