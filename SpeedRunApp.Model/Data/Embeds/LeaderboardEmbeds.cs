namespace SpeedRunApp.Model.Data
{
    public class LeaderboardEmbeds : Embeds
    {
        //private Embeds embeds;
        private bool isConstructed;

        public bool EmbedGame
        {
            get { return base["game"]; }
            set { base["game"] = value; }
        }

        public bool EmbedCategory
        {
            get { return base["category"]; }
            set { base["category"] = value; }
        }

        public bool EmbedLevel
        {
            get { return base["level"]; }
            set { base["level"] = value; }
        }

        public bool EmbedPlayers
        {
            get { return base["players"]; }
            set { base["players"] = value; }
        }

        public bool EmbedRegions
        {
            get { return base["regions"]; }
            set { base["regions"] = value; }
        }

        public bool EmbedPlatforms
        {
            get { return base["platforms"]; }
            set { base["platforms"] = value; }
        }

        public bool EmbedVariables
        {
            get { return base["variables"]; }
            set { base["variables"] = value; }
        }

        public LeaderboardEmbeds(
            bool embedGame = false,
            bool embedCategory = false,
            bool embedLevel = false,
            bool embedPlayers = true,
            bool embedRegions = false,
            bool embedPlatforms = false,
            bool embedVariables = false)
        {
            isConstructed = true;

            //embeds = new Embeds();
            EmbedGame = embedGame;
            EmbedCategory = embedCategory;
            EmbedLevel = embedLevel;
            EmbedPlayers = embedPlayers;
            EmbedRegions = embedRegions;
            EmbedPlatforms = embedPlatforms;
            EmbedVariables = embedVariables;
        }

        public override string ToString()
        {
            if (!isConstructed)
            {
                EmbedPlayers = true;
            }

            return base.ToString();
        }
    }
}
