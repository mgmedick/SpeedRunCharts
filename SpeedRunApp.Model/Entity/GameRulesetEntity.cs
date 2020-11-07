using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class GameRulesetEntity
    {
        public int ID { get; set; }
        public string GameID { get; set; }
        public bool ShowMilliseconds { get; set; }
        public bool RequiresVerification { get; set; }
        public bool RequiresVideo { get; set; }
        public int DefaultTimingMethodID { get; set; }
        public bool EmulatorsAllowed { get; set; }
    }
} 
