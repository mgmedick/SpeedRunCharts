using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class SpeedRunVideoEntity
    {
        public int ID { get; set; }
        public string SpeedRunID { get; set; }
        public string VideoLinkUrl { get; set; }
        public string VideoLinkEmbededUrl { get; set; }
    }
} 
