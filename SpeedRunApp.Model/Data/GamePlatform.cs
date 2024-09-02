using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class GamePlatform
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public int PlatformID { get; set; }
        public bool Deleted { get; set; }
    }
} 
