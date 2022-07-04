/****** Object:  StoredProcedure [dbo].[ImportGetGamesForSitemap]    Script Date: 4/15/2022 6:43:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[ImportGetGamesForSitemap]
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    SELECT ID, Abbr, ISNULL(ModifiedDate, ImportedDate) AS LastModifiedDate 
    FROM dbo.tbl_Game
    ORDER BY ISNULL(ModifiedDate, ImportedDate) DESC

END



GO


