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

        private IEnumerable<Category> Categories
        {
            get
            {
                var categories = SpeedRuns.Select(i => i.Category)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => i.First())
                                     .OrderBy(i => i.Name);

                return categories;
            }
        }

        public IEnumerable<dynamic> SearchCategoryTypes
        {
            get
            {
                var categoryTypes = SpeedRuns.Select(i => i.Category.Type)
                                     .GroupBy(g => new { g })
                                     .Select(i => new
                                     {
                                         ID = ((int)i.First()).ToString(),
                                         Name = i.First().ToString()
                                     })
                                     .OrderBy(i => (Convert.ToInt32(i.ID)));

                return categoryTypes;
            }
        }

        public IEnumerable<dynamic> SearchGames
        {
            get
            {
                var games = SpeedRuns.Select(i => i.Game)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => new
                                     {
                                         i.First().ID,
                                         i.First().Name,
                                         CategoryTypeIDs = Categories.Where(g => g.GameID == i.First().ID).Select(g => ((int)g.Type).ToString()).Distinct()
                                     })
                                     .OrderBy(i => i.Name);

                return games;
            }
        }

        public IEnumerable<dynamic> SearchCategories
        {
            get
            {
                var categories = SpeedRuns.Select(i => i.Category)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => new
                                     {
                                         i.First().ID,
                                         i.First().Name,
                                         i.First().GameID,
                                         Type = ((int)i.First().Type).ToString()
                                     })
                                     .OrderBy(i => i.Name);

                return categories;
            }
        }

        public IEnumerable<dynamic> SearchLevels
        {
            get
            {
                var levels = SpeedRuns.Where(i => i.Level != null)
                                     .Select(i => i.Level)
                                     .GroupBy(g => new { g.ID })
                                     .Select(i => new
                                     {
                                         i.First().ID,
                                         i.First().Name,
                                         i.First().GameID
                                     })
                                     .OrderBy(i => i.Name);

                return levels;
            }
        }

        //public SpeedRunGridViewModel SpeedRunGridVM { get { return new SpeedRunGridViewModel(ID, SpeedRuns); } }

        //To be removed:
        //public List<Game> Games
        //{
        //    get
        //    {
        //        var games = SpeedRuns.Select(i => i.Game)
        //                             .GroupBy(g => new { g.ID })
        //                             .Select(i => i.First())
        //                             .OrderBy(i => i.Name)
        //                             .ToList();

        //        return games;
        //    }
        //}

        //public List<IDNamePair> CategoryTypes
        //{
        //    get
        //    {
        //        var categoryTypes = SpeedRuns.Select(i => i.Category.Type)
        //                             .GroupBy(g => new { g })
        //                             .Select(i => new IDNamePair { ID = ((int)i.First()).ToString(), Name = i.First().ToString() })
        //                             .OrderBy(i => (Convert.ToInt32(i.ID)))
        //                             .ToList();

        //        return categoryTypes;
        //    }
        //}

        //public List<Category> Categories
        //{
        //    get
        //    {
        //        var categories = SpeedRuns.Select(i => i.Category)
        //                             .GroupBy(g => new { g.ID })
        //                             .Select(i => i.First())
        //                             .OrderBy(i => i.Name)
        //                             .ToList();

        //        return categories;
        //    }
        //}

        //public List<Level> Levels
        //{
        //    get
        //    {
        //        var levels = SpeedRuns.Where(i => i.Level != null)
        //                             .Select(i => i.Level)
        //                             .GroupBy(g => new { g.ID })
        //                             .Select(i => i.First())
        //                             .OrderBy(i => i.Name)
        //                             .ToList();

        //        return levels;
        //    }
        //}
    }
}


