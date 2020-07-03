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

        public List<SpeedRun> SpeedRuns { get; set; }

        public List<Game> Games
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

        public List<Category> Categories
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

        public List<Level> Levels
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

        public List<IDNamePair> SearchCategoryTypes
        {
            get
            {
                var categoryTypes = CategoryTypes.Select(i => new IDNamePair { ID = ((int)i).ToString(), Name = i.ToString() }).ToList();
                //categoryTypes.Insert(0, new IDNamePair { ID = "-1", Name = "All CateogryTypes" });
                return categoryTypes;
            }
        }

        public List<IDNamePair> SearchGames
        {
            get
            {
                var games = Games.Select(i => new IDNamePair { ID = i.ID, Name = i.Name }).ToList();
                //games.Insert(0, new IDNamePair { ID = "-1", Name = "All Games" });
                return games;
            }
        }

        public List<IDNamePair> SearchCategories
        {
            get
            {
                var categories = Categories.Select(i => new IDNamePair { ID = i.ID, Name = i.Name }).ToList();
                //categories.Insert(0, new IDNamePair { ID = "-1", Name = "All Categories" });
                return categories;
            }
        }

        public List<IDNamePair> SearchLevels
        {
            get
            {
                var levels = Levels.Select(i => new IDNamePair { ID = i.ID, Name = i.Name }).ToList();
                //levels.Insert(0, new IDNamePair { ID = "-1", Name = "All Levels" });
                return levels;
            }
        }
    }
}
