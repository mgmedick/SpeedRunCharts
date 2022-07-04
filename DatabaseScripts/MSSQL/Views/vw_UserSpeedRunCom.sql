/****** Object:  View [dbo].[vw_UserSpeedRunCom]    Script Date: 4/15/2022 7:08:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vw_UserSpeedRunCom]
AS

    SELECT u.ID,
           uc.SpeedRunComID,  
           u.[Name],
		   lc.[Location],
           ul.SpeedRunComUrl,      
           ul.ProfileImageUrl,      
           ul.TwitchProfileUrl,      
           ul.HitboxProfileUrl,      
           ul.YoutubeProfileUrl,      
           ul.TwitterProfileUrl,      
           ul.SpeedRunsLiveProfileUrl
    FROM dbo.tbl_User u WITH (NOLOCK)
    JOIN dbo.tbl_User_SpeedRunComID uc WITH (NOLOCK) ON uc.UserID = u.ID
    JOIN dbo.tbl_User_Link ul WITH (NOLOCK) ON ul.UserID = u.ID
    LEFT JOIN dbo.tbl_User_Location lc WITH (NOLOCK) ON lc.UserID = u.ID
GO


