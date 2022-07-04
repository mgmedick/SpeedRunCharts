-- vw_UserAccount
DROP VIEW IF EXISTS vw_UserAccount;

CREATE VIEW vw_UserAccount AS

    SELECT ua.ID AS UserAccountID,
	ua.Username,
	ue.IsDarkTheme,
	SpeedRunListCategoryIDs.Value AS SpeedRunListCategoryIDs
    FROM tbl_UserAccount ua
	LEFT JOIN tbl_UserAccount_Setting ue ON ue.UserAccountID = ua.ID
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONVERT(uc.SpeedRunListCategoryID,CHAR) ORDER BY uc.UserAccountID SEPARATOR ',') Value
	    FROM tbl_UserAccount_SpeedRunListCategory uc
	    WHERE uc.UserAccountID = ua.ID   
	) SpeedRunListCategoryIDs ON TRUE;
