using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunView
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public string GameName { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryTypeName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int? LevelID { get; set; }
        public string LevelName { get; set; }
        public int? PlatformID { get; set; }
        public string PlatformName { get; set; }
        public string VariableValues { get; set; }
        public string SubCategoryVariableValues { get; set; }
        public string Players { get; set; }
        public string VideoLinks { get; set; }
        public bool IsEmulated { get; set; }
        public int? Rank { get; set; }
        public long? PrimaryTime { get; set; }
        public long? RealTime { get; set; }
        public long? RealTimeWithoutLoads { get; set; }
        public long? GameTime { get; set; }
        public string Comment { get; set; }
        public int? ExaminerUserID { get; set; }
        public string ExaminerUserName { get; set; }
        public string SpeedRunComUrl { get; set; }
        public string SplitsUrl { get; set; }
        public DateTime? RunDate { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
    }
} 
