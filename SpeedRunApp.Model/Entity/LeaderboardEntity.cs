using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class LeaderboardEntity
    {
        public string ID { get; set; }
        public string GameID { get; set; }
        public string CategoryID { get; set; }
        public string LevelID { get; set; }
        public int? Place { get; set; }
        public string SpeedRunID { get; set; }
        public DateTime ImportedDate { get; set; }
    }
} 
