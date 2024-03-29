﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunSummaryView
    {
        public int ID { get; set; }
        public string SpeedRunComID { get; set; }
        public int GameID { get; set; }
        public string GameName { get; set; }
        public string GameAbbr { get; set; }
        public string GameCoverImageUrl { get; set; }
        public bool ShowMilliseconds { get; set; }
        public int CategoryTypeID { get; set; }
        public string CategoryTypeName { get; set; }            
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }        
        public int? LevelID { get; set; }
        public string LevelName { get; set; }
        public string SubCategoryVariableValues { get; set; }
        public string Players { get; set; }
        public string EmbeddedVideoLinks { get; set; }
        public int? Rank { get; set; }
        public long? PrimaryTime { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }
        public DateTime ImportedDate { get; set; }
    }
} 
