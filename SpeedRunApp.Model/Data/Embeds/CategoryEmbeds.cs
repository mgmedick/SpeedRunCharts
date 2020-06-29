namespace SpeedRunApp.Model.Data
{
    public class CategoryEmbeds : Embeds
    {
        public bool EmbedGame { get { return base["game"]; } set { base["game"] = value; } }
        public bool EmbedVariables { get { return base["variables"]; } set { base["variables"] = value; } }

        public CategoryEmbeds(
            bool embedGame = false,
            bool embedVariables = false)
        {
            EmbedGame = embedGame;
            EmbedVariables = embedVariables;
        }
    }
}
