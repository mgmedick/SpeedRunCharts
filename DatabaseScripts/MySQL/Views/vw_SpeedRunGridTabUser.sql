-- vw_SpeedRunGridTabUser
DROP VIEW IF EXISTS vw_SpeedRunGridTabUser;

CREATE VIEW vw_SpeedRunGridTabUser AS

    SELECT rn.ID,
           rn.GameID,
           rn.CategoryID,
           rn.LevelID,
           rn.VariableValues,
           rp.UserID,
           rn.`Rank`
    FROM vw_SpeedRunGridTab rn
	JOIN tbl_SpeedRun_Player rp ON rp.SpeedRunID = rn.ID;
