using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class PlayerView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int PlayerTypeID { get; set; }
        public int PlayerLinkID { get; set; }        
        public string SrcUrl { get; set; }
        public string TwitchUrl { get; set; }
        public string HitboxUrl { get; set; }
        public string YoutubeUrl { get; set; }
        public string TwitterUrl { get; set; }
        public int? PlayerNameStyleID { get; set; }        
        public bool? IsGradient { get; set; }
        public string ColorLight { get; set; }
        public string ColorDark { get; set; }
        public string ColorToLight { get; set; }
        public string ColorToDark { get; set; }
    }
} 
