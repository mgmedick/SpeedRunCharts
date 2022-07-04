/****** Object:  StoredProcedure [dbo].[ImportRebuildIndexes]    Script Date: 4/15/2022 6:40:42 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[ImportRebuildIndexes]
AS
BEGIN

	IF OBJECT_ID('tempdb..#RebuildIndexes') IS NOT NULL 
	BEGIN 
		DROP TABLE #RebuildIndexes
	END

	CREATE TABLE #RebuildIndexes 
	( 
		[ID] INT IDENTITY(1,1),
		[Schema] VARCHAR (255),
		[Table] VARCHAR (255),
		[Index] VARCHAR (255),
		[Action] VARCHAR (255)
	)

	INSERT INTO #RebuildIndexes ([Schema],[Table],[Index],[Action])
	SELECT S.name, T.name, I.name, CASE WHEN DDIPS.avg_fragmentation_in_percent > 30 THEN 'REBUILD' ELSE 'REORGANIZE' END
	FROM sys.dm_db_index_physical_stats (DB_ID(), NULL, NULL, NULL, NULL) AS DDIPS
	INNER JOIN sys.tables T ON T.object_id = DDIPS.object_id
	INNER JOIN sys.schemas S ON T.schema_id = S.schema_id
	INNER JOIN sys.indexes I ON I.object_id = DDIPS.object_id
	AND DDIPS.index_id = I.index_id
	WHERE DDIPS.database_id = DB_ID()
	AND I.name is not null
	AND DDIPS.avg_fragmentation_in_percent > 5
	ORDER BY DDIPS.avg_fragmentation_in_percent DESC

	DECLARE @Sql NVARCHAR(MAX) = ''
	DECLARE @RowCount INT = 1
	DECLARE @MaxRowCount INT

	SELECT @MaxRowCount = MAX(ID)
	FROM #RebuildIndexes

	WHILE @RowCount <= @MaxRowCount
	BEGIN
		SELECT @Sql += 'ALTER INDEX [' + [Index] + '] ON [' + [Schema] + '].[' + [Table] + '] ' + [Action] + ' '
		FROM #RebuildIndexes
		WHERE ID = @RowCount

		SELECT @RowCount = @RowCount + 1
	END

	EXEC (@Sql)

END
GO


