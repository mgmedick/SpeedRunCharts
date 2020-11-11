using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class PlatformEntity
    {
        public int IDX { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public DateTime ImportedDate { get; set; }
    }
} 
