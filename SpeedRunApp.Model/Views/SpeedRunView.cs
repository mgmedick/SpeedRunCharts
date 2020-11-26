using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class SpeedRunView
    {
        public string ID { get; set; }
        public int StatusTypeID { get; set; }
        public string GameID { get; set; }
        public string CategoryID { get; set; }
        public string LevelID { get; set; }
        public string PlatformID { get; set; }
        public string RegionID { get; set; }
        public bool IsEmulated { get; set; }
        public long? PrimaryTime { get; set; }
        public long? RealTime { get; set; }
        public long? RealTimeWithoutLoads { get; set; }
        public long? GameTime { get; set; }
        public string Comment { get; set; }
        public string ExaminerUserID { get; set; }
        public string RejectReason { get; set; }
        public string SpeedRunComUrl { get; set; }
        public string SplitsUrl { get; set; }
        public DateTime? RunDate { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
} 
