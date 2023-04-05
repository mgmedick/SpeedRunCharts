using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunChartView
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int CategoryTypeID { get; set; }    
        public int CategoryID { get; set; }
        public int? LevelID { get; set; }
        public string SubCategoryVariableValueIDs { get; set; }
        public long? PrimaryTime { get; set; }                
        public int? Rank { get; set; }
        public string Players { get; set; }
        public string Guests { get; set; }        
        public DateTime? DateSubmitted { get; set; }
    }
} 
