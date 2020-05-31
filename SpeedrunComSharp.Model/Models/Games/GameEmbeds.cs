namespace SpeedrunComSharp.Model
{
    public class GameEmbeds : Embeds
    {
        //private Embeds embeds;

        public bool EmbedLevels
        {
            get { return base["levels"]; }
            set { base["levels"] = value; }
        }
        public bool EmbedCategories
        {
            get { return base["categories"]; }
            set { base["categories"] = value; }
        }
        public bool EmbedModerators
        {
            get { return base["moderators"]; }
            set { base["moderators"] = value; }
        }
        public bool EmbedPlatforms
        {
            get { return base["platforms"]; }
            set { base["platforms"] = value; }
        }
        public bool EmbedRegions
        {
            get { return base["regions"]; }
            set { base["regions"] = value; }
        }
        public bool EmbedVariables
        {
            get { return base["variables"]; }
            set { base["variables"] = value; }
        }

        public GameEmbeds(
            bool embedLevels = false,
            bool embedCategories = false,
            bool embedModerators = false,
            bool embedPlatforms = false,
            bool embedRegions = false,
            bool embedVariables = false)
        {
            //embeds = new Embeds();
            EmbedLevels = embedLevels;
            EmbedCategories = embedCategories;
            EmbedModerators = embedModerators;
            EmbedPlatforms = embedPlatforms;
            EmbedRegions = embedRegions;
            EmbedVariables = embedVariables;
        }

        //public override string ToString()
        //{
        //    return embeds.ToString();
        //}
    }
}
