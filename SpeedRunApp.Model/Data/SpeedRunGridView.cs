using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class SpeedRunGridView : SpeedRunGridTabView
    {
        public int? PlatformID { get; set; }
        public string PlatformName { get; set; }
        public string VariableValues { get; set; }
        public string Players { get; set; }
        public string Guests { get; set; }
        public long? PrimaryTime { get; set; }
        public string Comment { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public DateTime? VerifyDate { get; set; }

    }
} 
