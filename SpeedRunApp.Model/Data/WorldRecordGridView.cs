﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class WorldRecordGridView
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int CategoryTypeID { get; set; }
        public int? LevelID { get; set; }
        public string LevelName { get; set; }        
        public int? PlatformID { get; set; }
        public string PlatformName { get; set; }
        public string SubCategoryVariableValueIDs { get; set; }
        public string VariableValues { get; set; }
        public string Players { get; set; }
        public string Guests { get; set; }
        public int? Rank { get; set; }
        public long? PrimaryTime { get; set; }
        public string Comment { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }

    }
} 
