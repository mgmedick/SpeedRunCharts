﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class GameView
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string CategoryIDs { get; set; }
        public string CategoryTypeIDs { get; set; }
        public string LevelIDs { get; set; }
        /*
        public string JapaneseName { get; set; }
        public string Abbreviation { get; set; }
        public string RuleSet { get; set; }
        public bool IsRomHack { get; set; }
        public int? YearOfRelease { get; set; }
        public string SpeedRunComUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime ImportedDate { get; set; }
        */
    }
} 
