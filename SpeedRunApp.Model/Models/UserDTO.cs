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

            //links
            _runs = user.runs;
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

        //links
        public Lazy<IEnumerable<Run>> _runs { get; set; }
        //public IEnumerable<SpeedRunDTO> SpeedRuns {
        //    get {
        //            List<SpeedRunDTO> runs = new List<SpeedRunDTO>();
        //            foreach (var run in _runs.Value)
        //            {
        //                runs.Add(new SpeedRunDTO(run));
        //            }

        //            return runs.AsEnumerable();
        //        }
        //}
        //private IEnumerable<Run> runs { get { return _runs.Value; } }
        public IEnumerable<SpeedRunDTO> SpeedRuns { get { return _runs.Value.Select(i => new SpeedRunDTO(i)).ToList(); } }
    }
}
