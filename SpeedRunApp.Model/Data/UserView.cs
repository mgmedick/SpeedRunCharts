using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class UserView
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime? SignUpDate { get; set; }
        public string Location { get; set; }
        public string SpeedRunComUrl { get; set; }
        public string ProfileImageUrl { get; set; }
        public string TwitchProfileUrl { get; set; }
        public string HitboxProfileUrl { get; set; }
        public string YoutubeProfileUrl { get; set; }
        public string TwitterProfileUrl { get; set; }
        public string SpeedRunsLiveProfileUrl { get; set; }
        public int TotalWorldRecords { get; set; }
    }
} 
