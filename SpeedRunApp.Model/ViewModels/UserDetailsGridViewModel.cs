using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Data;
using SpeedRunCommon;

namespace SpeedRunApp.Model.ViewModels
{
    public class UserDetailsGridViewModel
    {
        public UserDetailsGridViewModel(string userID, IEnumerable<SpeedRun> speedRuns)
        {
            UserID = userID;
            SpeedRuns = speedRuns.ToList();
        }

        public string UserID { get; set; }

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

        public List<IDNamePair> CategoryTypes
        {
            get
            {
                var categoryTypes = SpeedRuns.Select(i => i.Category.Type)
                                     .GroupBy(g => new { g })
                                     .Select(i => new IDNamePair { ID = ((int)i.First()).ToString(), Name = i.First().ToString() })
                                     .OrderBy(i => (Convert.ToInt32(i.ID)))
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
    }
}
