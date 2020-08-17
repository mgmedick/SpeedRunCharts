namespace SpeedRunApp.Model.Data
{
    public class SpeedRunSystem
    {
        public string PlatformID { get; set; }
        public string RegionID { get; set; }
        public bool IsEmulated { get; set; }

        //embeds
        public Platform Platform { get; set; }
        public Region Region { get; set; }
    }
}

