namespace SpeedrunComSharp.Model
{
    public class CategoryEmbeds : Embeds
    {
        //private Embeds embeds;

        public bool EmbedGame { get { return base["game"]; } set { base["game"] = value; } }
        public bool EmbedVariables { get { return base["variables"]; } set { base["variables"] = value; } }

        public CategoryEmbeds(
            bool embedGame = false, 
            bool embedVariables = false)
        {
            //embeds = new Embeds();
            EmbedGame = embedGame;
            EmbedVariables = embedVariables;
        }

        //public override string ToString()
        //{
        //    return embeds.ToString();
        //}
    }
}
