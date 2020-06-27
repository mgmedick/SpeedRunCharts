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
            ProfileImageUri = user.ProfileImageUri;
            Role = user.Role;
            SignUpDate = user.SignUpDate;
            Location = user.Location.ToString();
            TwitchProfile = user.TwitchProfile;
            HitboxProfile = user.HitboxProfile;
            YoutubeProfile = user.YoutubeProfile;
            TwitterProfile = user.TwitterProfile;
            SpeedRunsLiveProfile = user.SpeedRunsLiveProfile;
            var speedRuns = user.SpeedRuns.ToList();
            SpeedRuns = speedRuns;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public Uri ProfileImageUri { get; set; }
        public UserRole Role { get; set; }
        public DateTime? SignUpDate { get; set; }
        public string Location { get; set; }
        public Uri TwitchProfile { get; set; }
        public Uri HitboxProfile { get; set; }
        public Uri YoutubeProfile { get; set; }
        public Uri TwitterProfile { get; set; }
        public Uri SpeedRunsLiveProfile { get; set; }

        public List<SpeedRunDTO> SpeedRuns { get; set; }

        public List<GameDTO> Games
        {
            get
            {
                var games = SpeedRuns.Select(i => i.Game)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => i.First())
                                     .OrderBy(i => i.Name)
                                     .ToList();

                return games;
            }
        }

        public List<CategoryType> CategoryTypes
        {
            get
            {
                var categoryTypes = SpeedRuns.Select(i => i.Category.Type)
                                     .GroupBy(g => new { g })
                                     .Select(i => i.First())
                                     .OrderBy(i => i)
                                     .ToList();

                return categoryTypes;
            }
        }

        public List<CategoryDTO> Categories
        {
            get
            {
                var categories = SpeedRuns.Select(i => i.Category)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => i.First())
                                     .OrderBy(i => i.Name)
                                     .ToList();

                return categories;
            }
        }

        public List<LevelDTO> Levels
        {
            get
            {
                var levels = SpeedRuns.Where(i => i.Level != null)
                                     .Select(i => i.Level)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => i.First())
                                     .OrderBy(i => i.Name)
                                     .ToList();

                return levels;
            }
        }
    }
}
