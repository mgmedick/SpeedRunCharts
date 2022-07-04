SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[ImportReorderSpeedRuns]
AS
BEGIN
    DECLARE @BatchCount INT = 1000
    DECLARE @RaiseMsg VARCHAR (1000)
    DECLARE @RowCount INT = 0
    DECLARE @MaxRowCount INT

    SELECT @MaxRowCount = COUNT(*)
    FROM dbo.tbl_SpeedRun_Full

    IF OBJECT_ID('tempdb..#IDsToProcess') IS NOT NULL DROP TABLE #IDsToProcess
    CREATE TABLE #IDsToProcess ([ID] INT)

    IF OBJECT_ID('tempdb..#InsertedIDs') IS NOT NULL DROP TABLE #InsertedIDs
    CREATE TABLE #InsertedIDs ([NewID] INT, [OldID] INT)

    IF OBJECT_ID('tempdb..#InsertedVideoIDs') IS NOT NULL DROP TABLE #InsertedVideoIDs
    CREATE TABLE #InsertedVideoIDs ([NewVideoID] INT, [OldVideoID] INT, [NewID] INT)    

    INSERT INTO #IDsToProcess (ID)
    SELECT TOP (@BatchCount) ID
    FROM dbo.tbl_SpeedRun_Full
    WHERE ISNULL(IsProcessed, 0) = 0
    ORDER BY ISNULL(VerifyDate, DateSubmitted)

    WHILE EXISTS (SELECT 1 FROM #IDsToProcess)
    BEGIN
        MERGE INTO dbo.tbl_SpeedRun_Full_Ordered USING (SELECT rn.ID, StatusTypeID, GameID, CategoryID, LevelID, [Rank], PrimaryTime, RunDate, DateSubmitted, VerifyDate FROM dbo.tbl_SpeedRun_Full rn JOIN #IDsToProcess rn1 ON rn1.ID = rn.ID) AS td1 ON 1 = 0
        WHEN NOT MATCHED THEN
        INSERT (StatusTypeID, GameID, CategoryID, LevelID, [Rank], PrimaryTime, RunDate, DateSubmitted, VerifyDate)
        VALUES (td1.StatusTypeID, td1.GameID, td1.CategoryID, td1.LevelID, td1.[Rank], td1.PrimaryTime, td1.RunDate, td1.DateSubmitted, td1.VerifyDate)
        OUTPUT inserted.ID, td1.ID
        INTO #InsertedIDs ([NewID], OldID);

        INSERT INTO dbo.tbl_SpeedRun_SpeedRunComID_Full_Ordered (SpeedRunID, SpeedRunComID)
        SELECT dn.[NewID], rn1.SpeedRunComID
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_SpeedRunComID_Full rn1 ON rn1.SpeedRunID = dn.OldID

        INSERT INTO dbo.tbl_SpeedRun_System_Full_Ordered (SpeedRunID, PlatformID, RegionID, IsEmulated)
        SELECT dn.[NewID], rs.PlatformID, rs.RegionID, rs.IsEmulated
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_System_Full rs ON rs.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Time_Full_Ordered (SpeedRunID, PrimaryTime, RealTime, RealTimeWithoutLoads, GameTime)
        SELECT dn.[NewID], rt.PrimaryTime, rt.RealTime, rt.RealTimeWithoutLoads, rt.GameTime
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Time_Full rt ON rt.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Link_Full_Ordered (SpeedRunID, SpeedRunComUrl, SplitsUrl)
        SELECT dn.[NewID], rl.SpeedRunComUrl, rl.SplitsUrl
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Link_Full rl ON rl.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Comment_Full_Ordered (SpeedRunID, Comment)
        SELECT dn.[NewID], rc.Comment
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Comment_Full rc ON rc.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Player_Full_Ordered (SpeedRunID, UserID)
        SELECT dn.[NewID], rd.UserID
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Player_Full rd ON rd.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_Guest_Full_Ordered (SpeedRunID, GuestID)
        SELECT dn.[NewID], rg.GuestID
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_Guest_Full rg ON rg.SpeedRunID = dn.[OldID]

        INSERT INTO dbo.tbl_SpeedRun_VariableValue_Full_Ordered (SpeedRunID, VariableID, VariableValueID)
        SELECT dn.[NewID], rv.VariableID, rv.VariableValueID
        FROM #InsertedIDs dn
        JOIN dbo.tbl_SpeedRun_VariableValue_Full rv ON rv.SpeedRunID = dn.[OldID] 

        MERGE INTO dbo.tbl_SpeedRun_Video_Full_Ordered USING (SELECT dn.[NewID], rv.ID, rv.VideoLinkUrl, rv.EmbeddedVideoLinkUrl, rv.ThumbnailLinkUrl FROM #InsertedIDs dn JOIN dbo.tbl_SpeedRun_Video_Full rv ON rv.SpeedRunID = dn.[OldID]) AS td1 ON 1 = 0
        WHEN NOT MATCHED THEN
        INSERT (SpeedRunID, VideoLinkUrl, EmbeddedVideoLinkUrl, ThumbnailLinkUrl)
        VALUES (td1.NewID, td1.VideoLinkUrl, td1.EmbeddedVideoLinkUrl, td1.ThumbnailLinkUrl)
        OUTPUT inserted.ID, td1.ID, td1.[NewID]
        INTO #InsertedVideoIDs ([NewVideoID], [OldVideoID], [NewID]);    

        INSERT INTO dbo.tbl_SpeedRun_Video_Detail_Full_Ordered (SpeedRunVideoID, SpeedRunID, ChannelID, ViewCount)
        SELECT dn.[NewVideoID], dn.[NewID], rd.ChannelID, rd.ViewCount
        FROM #InsertedVideoIDs dn
        JOIN dbo.tbl_SpeedRun_Video_Detail_Full rd ON rd.SpeedRunVideoID = dn.[OldVideoID] 

        UPDATE rn SET
        [rn].[IsProcessed] = 1
        FROM dbo.tbl_SpeedRun_Full rn
        JOIN #IDsToProcess rn1 ON rn1.ID = rn.ID

        SELECT @RowCount = @RowCount + COUNT(*)
        FROM #IDsToProcess

        SELECT @RaiseMsg = 'Processed ' + CONVERT(VARCHAR, @RowCount) + '/' + CONVERT(VARCHAR, @MaxRowCount)
        RAISERROR(@RaiseMsg, 0, 1) WITH NOWAIT;

        TRUNCATE TABLE #IDsToProcess
        TRUNCATE TABLE #InsertedIDs
        TRUNCATE TABLE #InsertedVideoIDs

        INSERT INTO #IDsToProcess (ID)
        SELECT TOP (@BatchCount) ID
        FROM dbo.tbl_SpeedRun_Full
        WHERE ISNULL(IsProcessed, 0) = 0
        ORDER BY ISNULL(VerifyDate, DateSubmitted)
    END

	DROP INDEX [IDX_tbl_SpeedRun_VariableValue_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_VariableValue_Full]
	DROP INDEX [IDX_tbl_SpeedRun_Player_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Player_Full]
	DROP INDEX [IDX_tbl_SpeedRun_Guest_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Guest_Full]
	DROP INDEX [IDX_tbl_SpeedRun_Video_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Video_Full]
	DROP INDEX [IDX_tbl_SpeedRun_Video_Detail_Full_SpeedRunID] ON [dbo].[tbl_SpeedRun_Video_Detail_Full]

	DROP TABLE dbo.tbl_SpeedRun_Full
	DROP TABLE dbo.tbl_SpeedRun_SpeedRunComID_Full
	DROP TABLE dbo.tbl_SpeedRun_System_Full
	DROP TABLE dbo.tbl_SpeedRun_Time_Full
	DROP TABLE dbo.tbl_SpeedRun_Link_Full
	DROP TABLE dbo.tbl_SpeedRun_Comment_Full
	DROP TABLE dbo.tbl_SpeedRun_Player_Full
   	DROP TABLE dbo.tbl_SpeedRun_Guest_Full
	DROP TABLE dbo.tbl_SpeedRun_VariableValue_Full
	DROP TABLE dbo.tbl_SpeedRun_Video_Full
	DROP TABLE dbo.tbl_SpeedRun_Video_Detail_Full

	--PK_tbl_SpeedRun_Full_Ordered
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Full_Ordered', 'PK_tbl_SpeedRun_Full'                                
	EXEC sp_rename 'dbo.DF_tbl_SpeedRun_Full_Ordered_ImportedDate', 'DF_tbl_SpeedRun_Full_ImportedDate'
	EXEC sp_rename 'dbo.tbl_SpeedRun_Full_Ordered', 'tbl_SpeedRun_Full'
	--PK_tbl_SpeedRun_SpeedRunComID_Full_Ordered   
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_SpeedRunComID_Full_Ordered', 'PK_tbl_SpeedRun_SpeedRunComID_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_SpeedRunComID_Full_Ordered', 'tbl_SpeedRun_SpeedRunComID_Full'
	--tbl_SpeedRun_System_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_System_Full_Ordered', 'PK_tbl_SpeedRun_System_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_System_Full_Ordered', 'tbl_SpeedRun_System_Full'  
	--tbl_SpeedRun_Time_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Time_Full_Ordered', 'PK_tbl_SpeedRun_Time_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Time_Full_Ordered', 'tbl_SpeedRun_Time_Full' 
	--tbl_SpeedRun_Link_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Link_Full_Ordered', 'PK_tbl_SpeedRun_Link_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Link_Full_Ordered', 'tbl_SpeedRun_Link_Full'   
	--tbl_SpeedRun_Comment_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Comment_Full_Ordered', 'PK_tbl_SpeedRun_Comment_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Comment_Full_Ordered', 'tbl_SpeedRun_Comment_Full'   
	--tbl_SpeedRun_Player_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Player_Full_Ordered', 'PK_tbl_SpeedRun_Player_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Player_Full_Ordered', 'tbl_SpeedRun_Player_Full'   
	--tbl_SpeedRun_Guest_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Guest_Full_Ordered', 'PK_tbl_SpeedRun_Guest_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Guest_Full_Ordered', 'tbl_SpeedRun_Guest_Full'      
	--tbl_SpeedRun_VariableValue_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_VariableValue_Full_Ordered', 'PK_tbl_SpeedRun_VariableValue_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_VariableValue_Full_Ordered', 'tbl_SpeedRun_VariableValue_Full' 
	--tbl_SpeedRun_Video_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Video_Full_Ordered', 'PK_tbl_SpeedRun_Video_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Video_Full_Ordered', 'tbl_SpeedRun_Video_Full'    
	--tbl_SpeedRun_Video_Detail_Full 
	EXEC sp_rename 'dbo.PK_tbl_SpeedRun_Video_Detail_Full_Ordered', 'PK_tbl_SpeedRun_Video_Detail_Full'                                
	EXEC sp_rename 'dbo.tbl_SpeedRun_Video_Detail_Full_Ordered', 'tbl_SpeedRun_Video_Detail_Full'
END
GO


