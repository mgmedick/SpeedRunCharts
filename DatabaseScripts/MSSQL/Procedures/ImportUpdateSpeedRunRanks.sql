/****** Object:  StoredProcedure [dbo].[ImportUpdateSpeedRunRanks]    Script Date: 4/15/2022 6:41:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[ImportUpdateSpeedRunRanks]
(
    @LastImportDate DATETIME
)
AS
BEGIN

    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    DECLARE @CurrDate DATETIME = GETDATE()  
    DECLARE @Debug BIT = '0'

    IF OBJECT_ID('tempdb..#LeaderboardKeys') IS NOT NULL 
    BEGIN 
        DROP TABLE #LeaderboardKeys
    END

    CREATE TABLE #LeaderboardKeys 
    ( 
          [GameID] INT,
          [CategoryID] INT,
          [LevelID] INT,
          [SubCategoryVariableValues] [varchar] (150)
    )
	CREATE NONCLUSTERED INDEX [IDX_LeaderboardKeys_GameID_CategoryID] ON #LeaderboardKeys ([GameID], [CategoryID]) INCLUDE ([LevelID])

    IF OBJECT_ID('tempdb..#SpeedRunsToUpdate') IS NOT NULL 
    BEGIN 
        DROP TABLE #SpeedRunsToUpdate
    END

    CREATE TABLE #SpeedRunsToUpdate 
    ( 
          [ID] INT,
          [GameID] INT,
          [CategoryID] INT,
          [LevelID] INT,
          [SubCategoryVariableValues] [varchar] (150),
          [PlayerIDs] [varchar] (150),
          [GuestIDs] [varchar] (150),
          [PrimaryTime] [bigint],
          [RankPriority] [int]
    )

    IF OBJECT_ID('tempdb..#SpeedRunsRanked') IS NOT NULL 
    BEGIN 
        DROP TABLE #SpeedRunsRanked
    END

    CREATE TABLE #SpeedRunsRanked 
    ( 
          [ID] INT,
          [Rank] INT
    )

	INSERT INTO #LeaderboardKeys (GameID, CategoryID, LevelID)
	SELECT rn.GameID, rn.CategoryID, rn.LevelID
	FROM dbo.tbl_SpeedRun rn WITH (NOLOCK)
	WHERE ISNULL(rn.ModifiedDate, rn.ImportedDate) >= @LastImportDate
	GROUP BY rn.GameID, rn.CategoryID, rn.LevelID

	INSERT INTO #LeaderboardKeys (GameID, CategoryID, LevelID)
	SELECT g.ID, c.ID, l.ID
	FROM dbo.tbl_Game g WITH (NOLOCK)
	JOIN dbo.tbl_Category c WITH (NOLOCK) ON c.GameID = g.ID
	LEFT JOIN dbo.tbl_Level l WITH (NOLOCK) ON l.GameID = g.ID  
	WHERE ISNULL(g.ModifiedDate, g.ImportedDate) >= @LastImportDate
    AND NOT EXISTS (SELECT 1 FROM #LeaderboardKeys WHERE GameID = g.ID AND CategoryID = c.ID AND ISNULL(LevelID,'') = ISNULL(l.ID,''))
	GROUP BY g.ID, c.ID, l.ID

    INSERT INTO #SpeedRunsToUpdate(ID, GameID, CategoryID, LevelID, SubCategoryVariableValues, PlayerIDs, GuestIDs, PrimaryTime, RankPriority)
    SELECT rn.ID, rn.GameID, rn.CategoryID, rn.LevelID, SubCategoryVariableValues.[Value], PlayerIDs.[Value], GuestIDs.[Value], rn.PrimaryTime,
    ROW_NUMBER() OVER (PARTITION BY rn.GameID, rn.CategoryID, rn.LevelID, SubCategoryVariableValues.[Value], PlayerIDs.[Value], GuestIDs.[Value] ORDER BY rn.PrimaryTime)
    FROM dbo.tbl_SpeedRun rn WITH (NOLOCK)
    OUTER APPLY (SELECT STUFF(
            (   SELECT ',' + CONVERT(VARCHAR, rv.[VariableID]) + '|' + CONVERT(VARCHAR, rv.VariableValueID)
                FROM dbo.tbl_SpeedRun_VariableValue rv WITH (NOLOCK)
                JOIN dbo.tbl_Variable v WITH (NOLOCK) ON v.ID = rv.VariableID AND v.IsSubCategory = 1
                WHERE rv.SpeedRunID = rn.ID
                ORDER BY rv.ID
                FOR XML PATH ('')
            ), 1, 1, '') AS [Value] 
    ) AS SubCategoryVariableValues 
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rp.UserID)
                        FROM dbo.tbl_SpeedRun_Player rp WITH (NOLOCK)
                        WHERE rp.SpeedRunID = rn.ID
                        ORDER BY rp.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS PlayerIDs
    OUTER APPLY (SELECT STUFF(
                    (   SELECT ',' + CONVERT(VARCHAR, rg.GuestID)
                        FROM dbo.tbl_SpeedRun_Guest rg WITH (NOLOCK)
                        WHERE rg.SpeedRunID = rn.ID
                        ORDER BY rg.ID
                        FOR XML PATH ('')
                    ), 1, 1, '') AS [Value] 
            ) AS GuestIDs                                
    WHERE EXISTS (SELECT 1 FROM #LeaderboardKeys lb WHERE lb.GameID = rn.GameID AND lb.CategoryID = rn.CategoryID AND ISNULL(lb.LevelID,'') = ISNULL(rn.LevelID,''))
    
    INSERT INTO #SpeedRunsRanked(ID, [Rank])
    SELECT ID, RANK() OVER (PARTITION BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues ORDER BY rn.PrimaryTime)
    FROM #SpeedRunsToUpdate rn
    WHERE rn.RankPriority = 1
    AND ISNULL(PlayerIDs, GuestIDs) IS NOT NULL
        
    IF(@Debug = 0)
    BEGIN
        UPDATE rn SET
        [Rank] = NULL
        FROM dbo.tbl_SpeedRun rn         
        WHERE EXISTS (SELECT 1 FROM #SpeedRunsToUpdate rn1 WHERE rn1.ID = rn.ID)

        UPDATE rn SET
        [Rank] = rn1.[Rank]
        FROM dbo.tbl_SpeedRun rn
        JOIN #SpeedRunsRanked rn1 ON rn1.ID = rn.ID
    END
    ELSE
    BEGIN
        SELECT rn.RankPriority, rn.ID, rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn.PlayerIDs, rn.GuestIDs, rn.PrimaryTime
        FROM #SpeedRunsToUpdate rn
        where rn.SubCategoryVariableValues is not null
        ORDER BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn.PlayerIDs, rn.GuestIDs, rn.RankPriority

        SELECT rn1.[Rank], rn.ID, rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn.PlayerIDs, rn.GuestIDs, rn.PrimaryTime
        FROM #SpeedRunsToUpdate rn
        JOIN #SpeedRunsRanked rn1 ON rn1.ID = rn.ID
        where rn.SubCategoryVariableValues is not null
        ORDER BY rn.GameID, rn.CategoryID, rn.LevelID, rn.SubCategoryVariableValues, rn1.[Rank]
    END

END
GO


