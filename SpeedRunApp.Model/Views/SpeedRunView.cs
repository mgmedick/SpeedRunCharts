using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class SpeedRunView
    {
        public int OrderValue { get; set; }
        public string ID { get; set; }
        public int StatusTypeID { get; set; }
        public string StatusTypeName { get; set; }
        public string GameID { get; set; }
        public string GameName { get; set; }
        public string GameCoverImageUrl { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryTypeName { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string LevelID { get; set; }
        public string LevelName { get; set; }
        public string PlatformID { get; set; }
        public string PlatformName { get; set; }
        public string VariableValues { get; set; }
        public string Players { get; set; }
        public string VideoLinks { get; set; }
        public bool IsEmulated { get; set; }
        public long? PrimaryTime { get; set; }
        public long? RealTime { get; set; }
        public long? RealTimeWithoutLoads { get; set; }
        public long? GameTime { get; set; }
        public string Comment { get; set; }
        public string ExaminerUserID { get; set; }
        public string ExaminerUserName { get; set; }
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
