using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class LeaderboardSpeedRunEntity
    {
        public int ID { get; set; }
        public string LeaderboardID { get; set; }
        public int? Rank { get; set; }
        public string SpeedRunID { get; set; }
    }
} 
