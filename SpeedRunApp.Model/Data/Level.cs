namespace SpeedRunApp.Model.Data
{
    public class Level
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int GameID { get; set; }
        public bool Deleted { get; set; }

        //Transient
        public bool HasData { get; set; }
        public int CategoryID { get; set; }        
    }
}
