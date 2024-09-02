using System;
using System.Collections.Generic;
using System.Linq;


namespace SpeedRunApp.Model.Data
{
    public class SpeedRun
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int CategoryTypeID { get; set; }
        public int CategoryID { get; set; }
        public int? LevelID { get; set; }
        public string SubCategoryVariableValueIDs { get; set; }
         public int? PlatformID { get; set; }       
        public int? Rank { get; set; }
        public long PrimaryTime { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public DateTime CreatedDate { get; set; }   
        public DateTime ModifiedDate { get; set; }        
    }
} 
