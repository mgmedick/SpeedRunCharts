namespace SpeedRunApp.Model.Data
{
    public class LevelEmbeds : Embeds
    {
        public bool EmbedCategories
        {
            get { return base["categories"]; }
            set { base["categories"] = value; }
        }
        public bool EmbedVariables
        {
            get { return base["variables"]; }
            set { base["variables"] = value; }
        }

        public LevelEmbeds(
            bool embedCategories = false,
            bool embedVariables = false)
        {
            EmbedCategories = embedCategories;
            EmbedVariables = embedVariables;
        }
    }
}
