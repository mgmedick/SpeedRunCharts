using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.Entity
{
    public class UserEntity
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public int UserRoleID { get; set; }
        public string Location { get; set; }
        public string SpeedRunComUrl { get; set; }
        public string ProfileImageUrl { get; set; }
        public string TwitchProfileUrl { get; set; }
        public string HitboxProfileUrl { get; set; }
        public string YoutubeProfileUrl { get; set; }
        public string TwitterProfileUrl { get; set; }
        public string SpeedRunsLiveProfileUrl { get; set; }
        public DateTime? SignUpDate { get; set; }
        public DateTime ImportedDate { get; set; }
    }
} 
