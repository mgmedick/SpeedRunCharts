namespace SpeedRunApp.Model.Data
{
    public class LeaderboardParam
    {
        public LeaderboardParam(string gameID, string categoryID, string levelID, string variableValues)
        {
            GameID = gameID;
            CategoryID = categoryID;
            LevelID = levelID;
            VariableValues = variableValues;
        }

        public string GameID { get; set; }
        public string CategoryID { get; set; }
        public string LevelID { get; set; }
        public string VariableValues { get; set; }
    }
}
