using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class LeaderboardEntity
    {
        public int ID { get; set; }
        public string GameID { get; set; }
        public string CategoryID { get; set; }
        public string LevelID { get; set; }
        public int? Rank { get; set; }
        public string SpeedRunID { get; set; }
        public DateTime ImportedDate { get; set; }
    }
} 
