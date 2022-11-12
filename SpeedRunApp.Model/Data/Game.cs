using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class Game
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsRomHack { get; set; }
        public int? YearOfRelease { get; set; }
        public string Abbr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsChanged { get; set; }
    }
} 
