namespace SpeedrunComSharp.Model
{
    public class SeriesEmbeds : Embeds
    {
        //private Embeds embeds;

        public bool EmbedModerators
        {
            get { return base["moderators"]; }
            set { base["moderators"] = value; }
        }

        public SeriesEmbeds(
            bool embedModerators = false)
        {
            //embeds = new Embeds();
            EmbedModerators = embedModerators;
        }

        //public override string ToString()
        //{
        //    return embeds.ToString();
        //}
    }
}
