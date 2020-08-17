namespace SpeedRunApp.Model.Data
{
    public class SpeedRunEmbeds : Embeds
    {
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

        public bool EmbedRegion
        {
            get { return base["region"]; }
            set { base["region"] = value; }
        }

        public bool EmbedPlatform
        {
            get { return base["platform"]; }
            set { base["platform"] = value; }
        }

        public SpeedRunEmbeds(
            bool embedGame = false,
            bool embedCategory = false,
            bool embedLevel = false,
            bool embedPlayers = false,
            bool embedRegion = false,
            bool embedPlatform = false)
        {
            EmbedGame = embedGame;
            EmbedCategory = embedCategory;
            EmbedLevel = embedLevel;
            EmbedPlayers = embedPlayers;
            EmbedRegion = embedRegion;
            EmbedPlatform = embedPlatform;
        }
    }
}
