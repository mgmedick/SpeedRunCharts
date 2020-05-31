using System;
using System.Collections.Generic;
using System.Linq;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class UserDTO
    {
        public UserDTO(User user)
        {
            ID = user.ID;
            Name = user.Name;
            JapaneseName = user.JapaneseName;
            Role = user.Role;
            SignUpDate = user.SignUpDate;
            Location = user.Location.ToString();
            TwitchProfile = user.TwitchProfile;
            HitboxProfile = user.HitboxProfile;
            YoutubeProfile = user.YoutubeProfile;
            TwitterProfile = user.TwitterProfile;
            SpeedRunsLiveProfile = user.SpeedRunsLiveProfile;
            SpeedRuns = user.Runs.Select(i => new SpeedRunDTO(i));
            //PersonalBests = user.PersonalBests.Select(i => new SpeedRunRecordDTO(i));
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public UserRole Role { get; set; }
        public DateTime? SignUpDate { get; set; }
        public string Location { get; set; }
        public Uri TwitchProfile { get; set; }
        public Uri HitboxProfile { get; set; }
        public Uri YoutubeProfile { get; set; }
        public Uri TwitterProfile { get; set; }
        public Uri SpeedRunsLiveProfile { get; set; }
        public IEnumerable<SpeedRunDTO> SpeedRuns { get; set; }
        //public IEnumerable<SpeedRunRecordDTO> PersonalBests { get; set; }
    }
}
