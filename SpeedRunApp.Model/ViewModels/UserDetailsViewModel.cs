using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Data;
using SpeedRunCommon;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserDetailsViewModel
    {
        public UserDetailsViewModel(User user)
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
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public Uri ProfileImage { get; set; }
        public UserRole Role { get; set; }
        public DateTime? SignUpDate { get; set; }
        public string Location { get; set; }
        public Uri TwitchProfile { get; set; }
        public Uri HitboxProfile { get; set; }
        public Uri YoutubeProfile { get; set; }
        public Uri TwitterProfile { get; set; }
        public Uri SpeedRunsLiveProfile { get; set; }

        public IEnumerable<SpeedRun> SpeedRuns { get; set; }

        public IEnumerable<IDNamePair> SearchCategoryTypes
        {
            get
            {
                var categoryTypes = SpeedRuns.Select(i => i.Category.Type)
                                     .GroupBy(g => new { g })
                                     .Select(i => new IDNamePair
                                     {
                                         ID = ((int)i.First()).ToString(),
                                         Name = i.First().ToString()
                                     })
                                     .OrderBy(i => (Convert.ToInt32(i.ID)));

                return categoryTypes;
            }
        }

        public IEnumerable<CategoryDisplay> SearchCategories
        {
            get
            {
                var categories = SpeedRuns.Select(i => i.Category)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => new CategoryDisplay
                                     {
                                         ID = i.First().ID,
                                         Name = i.First().Name,
                                         GameID = i.First().GameID,
                                         CategoryTypeID = ((int)i.First().Type).ToString()
                                     })
                                     .OrderBy(i => i.Name);

                return categories;
            }
        }

        public IEnumerable<GameDisplay> SearchGames
        {
            get
            {
                var games = SpeedRuns.Select(i => i.Game)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => new GameDisplay
                                     {
                                         ID = i.First().ID,
                                         Name = i.First().Name,
                                         CategoryTypeIDs = SearchCategories.Where(g => g.GameID == i.First().ID).Select(g => g.CategoryTypeID).Distinct()
                                     })
                                     .OrderBy(i => i.Name);

                return games;
            }
        }

        public IEnumerable<LevelDisplay> SearchLevels
        {
            get
            {
                var levels = SpeedRuns.Where(i => i.Level != null)
                                     .Select(i => i.Level)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => new LevelDisplay
                                     {
                                         ID = i.First().ID,
                                         Name = i.First().Name,
                                         GameID = i.First().GameID
                                     })
                                     .OrderBy(i => i.Name);

                return levels;
            }
        }
    }
}


