using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class SpeedRunEntity
    {
        public string ID { get; set; }
        public string StatusID { get; set; }
        public string GameID { get; set; }
        public string CategoryID { get; set; }
        public string LevelID { get; set; }
        public string PlatformID { get; set; }
        public string RegionID { get; set; }
        public bool IsEmulated { get; set; }
        public TimeSpan? PrimaryTime { get; set; }
        public TimeSpan? RealTime { get; set; }
        public TimeSpan? RealTimeWithoutLoads { get; set; }
        public TimeSpan? GameTime { get; set; }
        public string SpeedRunComUrl { get; set; }
        public string SplitsUrl { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public DateTime ImportedDate { get; set; }
    }
} 
