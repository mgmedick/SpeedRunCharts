-- vw_UserSpeedRunCom
DROP VIEW IF EXISTS vw_UserSpeedRunCom;

CREATE VIEW vw_UserSpeedRunCom AS

    SELECT u.ID,
           uc.SpeedRunComID,  
           u.Name,
		   lc.Location,
           ul.SpeedRunComUrl,      
           ul.ProfileImageUrl,      
           ul.TwitchProfileUrl,      
           ul.HitboxProfileUrl,      
           ul.YoutubeProfileUrl,      
           ul.TwitterProfileUrl,      
           ul.SpeedRunsLiveProfileUrl
    FROM tbl_User u
    JOIN tbl_User_SpeedRunComID uc ON uc.UserID = u.ID
    JOIN tbl_User_Link ul ON ul.UserID = u.ID
    LEFT JOIN tbl_User_Location lc ON lc.UserID = u.ID;