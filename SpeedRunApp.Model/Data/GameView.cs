using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class GameView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string CoverImageUrl { get; set; }
        public int? YearOfRelease { get; set; }
        public string CategoryTypes { get; set; }
        public string Categories { get; set; }
        public string Levels { get; set; }
        public string Variables { get; set; }
        public string VariableValues { get; set; }
        public string Platforms { get; set; }
        public string Moderators { get; set; }
        public string SpeedRunComUrl { get; set; }
    }
} 
