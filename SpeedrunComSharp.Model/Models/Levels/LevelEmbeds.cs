namespace SpeedrunComSharp.Model
{
    public class LevelEmbeds : Embeds
    {
        //private Embeds embeds;

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
            //embeds = new Embeds();
            EmbedCategories = embedCategories;
            EmbedVariables = embedVariables;
        }

        //public override string ToString()
        //{
        //    return embeds.ToString();
        //}
    }
}
