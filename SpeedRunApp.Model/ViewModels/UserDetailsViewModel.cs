using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon;

namespace SpeedRunApp.Model
{
    public class UserDetailsViewModel
    {
        public UserDetailsViewModel(UserDTO user)
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
            SpeedRuns = user.SpeedRuns;
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

        public List<GameDTO> Games
        {
            get
            {
                return SpeedRuns.Select(i => i.Game).OrderBy(i => i.Name).Distinct().ToList();
                //return SpeedRuns.GroupBy(g => new { g.GameID, g.Game.Name })
                //                .Select(i => new IDNamePair { ID = i.Key.GameID, Name = i.Key.Name })
                //                .OrderBy(i => i.Name);
            }
        }

        public List<CategoryType> CategoryTypes
        {
            get
            {
                return SpeedRuns.Select(i => i.Category.Type).OrderBy(i => i).Distinct().ToList();
            }
        }

        public List<CategoryDTO> Categories
        {
            get
            {
                return SpeedRuns.Select(i => i.Category).OrderBy(i => i.Name).Distinct().ToList();
            }
        }

        public List<LevelDTO> Levels
        {
            get
            {
                return SpeedRuns.Select(i => i.Level).OrderBy(i => i.ID).Distinct().ToList();
            }
        }
    }
}
