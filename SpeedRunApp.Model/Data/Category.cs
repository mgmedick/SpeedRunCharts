namespace SpeedRunApp.Model.Data
{
    public class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }     
        public string GameID { get; set; }              
        public int CategoryTypeID { get; set; }         
        public bool IsMiscellaneous { get; set; }
        public bool IsTimerAscending { get; set; }
        public bool Deleted { get; set; }     

        //Transient
        public bool HasData { get; set; }           
    }
}
