-- vw_SpeedRunGridTab
DROP VIEW IF EXISTS vw_SpeedRunGridTab;

CREATE VIEW vw_SpeedRunGridTab AS

    SELECT rn.ID,
           rn.GameID,
           rn.CategoryID,
           rn.LevelID,
           VariableValues.Value AS VariableValues,
           rn.`Rank`
    FROM tbl_SpeedRun rn
	LEFT JOIN LATERAL (
		SELECT GROUP_CONCAT(CONCAT(CONVERT(rv.VariableID,CHAR), '|', CONVERT(rv.VariableValueID,CHAR)) ORDER BY rv.ID SEPARATOR ',') Value
	    FROM tbl_SpeedRun_VariableValue rv
	    WHERE rv.SpeedRunID = rn.ID   
	) VariableValues ON TRUE;