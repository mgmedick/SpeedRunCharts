using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Data
{
    public class UserView
    {
        public string OrderValue { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Role { get; set; }
        public DateTime? SignUpDate { get; set; }
        public string Location { get; set; }
        public string TwitchProfileUrl { get; set; }
        public string HitboxProfileUrl { get; set; }
        public string YoutubeProfileUrl { get; set; }
        public string TwitterProfileUrl { get; set; }
        public string SpeedRunsLiveProfileUrl { get; set; }
    }
} 
