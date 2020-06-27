using System;
using System.Collections.Generic;
using System.Text;

namespace SpeedRunApp.Model
{
    public class SpeedRunSystemDTO
    {
        public string PlatformID { get; set; }
        public bool IsEmulated { get; set; }
        public string RegionID { get; set; }


        //embeds
        public PlatformDTO Platform { get; set; }
        public RegionDTO Region { get; set; }
    }
}

