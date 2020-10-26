using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class GameEntity
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string Abbreviation { get; set; }
        public bool IsRomHack { get; set; }
        public int? YearOfRelease { get; set; }
        public DateTime? SpeedRunComCreatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
} 
