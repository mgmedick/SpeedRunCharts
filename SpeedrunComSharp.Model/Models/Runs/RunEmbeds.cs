namespace SpeedrunComSharp.Model
{
    public class RunEmbeds : Embeds
    {
        //private Embeds embeds;

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

        public RunEmbeds(
            bool embedGame = false,
            bool embedCategory = false,
            bool embedLevel = false,
            bool embedPlayers = false,
            bool embedRegion = false,
            bool embedPlatform = false)
        {
            //embeds = new Embeds();
            EmbedGame = embedGame;
            EmbedCategory = embedCategory;
            EmbedLevel = embedLevel;
            EmbedPlayers = embedPlayers;
            EmbedRegion = embedRegion;
            EmbedPlatform = embedPlatform;
        }

        //public override string ToString()
        //{
        //    return embeds.ToString();
        //}
    }
}
