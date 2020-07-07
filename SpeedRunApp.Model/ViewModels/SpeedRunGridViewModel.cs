using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Data;
using SpeedRunCommon;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridViewModel
    {
        public SpeedRunGridViewModel(Game game, string sender)
        {
            Games = new List<Game>() { game };
            Categories = game.Categories.ToList();
            Levels = game.Levels.ToList();
            CategoryTypes = game.CategoryTypes.ToList();
            Sender = sender;

            UserID = string.Empty;
            SpeedRuns = new List<SpeedRun>();
        }

        public SpeedRunGridViewModel(string userID, IEnumerable<SpeedRun> speedRuns, string sender)
        {
            UserID = userID;
            SpeedRuns = speedRuns.ToList();
            CategoryTypes = SpeedRuns.Select(i => i.Category.Type).GroupBy(g => new { g }).Select(i => i.First()).OrderBy(i => (int)i).ToList();
            Games = SpeedRuns.Select(i => i.Game).GroupBy(g => new { g.ID }).Select(i => i.First()).OrderBy(i => i.Name).ToList();
            Categories = SpeedRuns.Select(i => i.Category).GroupBy(g => new { g.ID }).Select(i => i.First()).OrderBy(i => i.Name).ToList();
            Levels = SpeedRuns.Where(i => i.Level != null).Select(i => i.Level).GroupBy(g => new { g.ID }).Select(i => i.First()).OrderBy(i => i.Name).ToList();
            Sender = sender;
        }

        public string UserID { get; set; }
        public List<SpeedRun> SpeedRuns { get; set; }
        public List<CategoryType> CategoryTypes { get; set; }
        public List<Game> Games { get; set; }
        public List<Category> Categories { get; set; }
        public List<Level> Levels { get; set; }
        public string Sender { get; set; }
    }
}
