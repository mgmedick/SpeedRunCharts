using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridViewModel
    {
        public SpeedRunGridViewModel(Game game, string sender)
        {
            Sender = sender;
            UserID = string.Empty;
            SpeedRuns = new List<SpeedRun>();

            CategoryTypes = game.CategoryTypes
                                .Select(i => new IDNamePair { ID = ((int)i).ToString(), Name = i.ToString() })
                                .ToList();

            Games = new List<GameDisplay>() { new GameDisplay { ID = game.ID, Name = game.Name, CategoryTypeIDs = game.Categories.Select(i=>((int)i.Type).ToString()).Distinct() } };

            Categories = game.Categories
                            .Select(i => new CategoryDisplay { ID = i.ID, Name = i.Name, CategoryTypeID = ((int)i.Type).ToString(), GameID = i.GameID })
                            .ToList();

            Levels = game.Levels
                        .Select(i => new LevelDisplay { ID = i.ID, Name = i.Name, GameID = i.GameID })
                        .ToList();
        }

        public SpeedRunGridViewModel(string userID, IEnumerable<SpeedRun> speedRuns, string sender)
        {
            UserID = userID;
            SpeedRuns = speedRuns.ToList();
            Sender = sender;

            CategoryTypes = SpeedRuns.Select(i => i.Category.Type)
                                    .GroupBy(g => new { g })
                                    .Select(i => new IDNamePair
                                    {
                                        ID = ((int)i.First()).ToString(),
                                        Name = i.First().ToString()
                                    })
                                    .OrderBy(i => Convert.ToInt32(i.ID))
                                    .ToList();

            Categories = SpeedRuns.Select(i => i.Category)
                                .GroupBy(g => new { g.ID })
                                .Select(i => new CategoryDisplay
                                {
                                    ID = i.First().ID,
                                    Name =i.First().Name,
                                    GameID = i.First().GameID,
                                    CategoryTypeID = ((int)i.First().Type).ToString()
                                })
                                .OrderBy(i => i.Name)
                                .ToList();

            Games = SpeedRuns.Select(i => i.Game)
                            .GroupBy(g => new { g.ID })
                            .Select(i => new GameDisplay
                            {
                                ID = i.First().ID,
                                Name = i.First().Name,
                                CategoryTypeIDs = Categories.Where(g => g.GameID == i.First().ID).Select(g => g.CategoryTypeID).Distinct()
                            })
                            .OrderBy(i => i.Name)
                            .ToList();

            Levels = SpeedRuns.Where(i => i.Level != null)
                            .Select(i => i.Level)
                            .GroupBy(g => new { g.ID })
                            .Select(i => new LevelDisplay
                            {
                                ID = i.First().ID,
                                Name = i.First().Name,
                                GameID = i.First().GameID
                            })
                            .OrderBy(i => i.Name)
                            .ToList();

        }

        public string UserID { get; set; }
        public string Sender { get; set; }
        public List<SpeedRun> SpeedRuns { get; set; }
        public List<IDNamePair> CategoryTypes { get; set; }
        public List<GameDisplay> Games { get; set; }
        public List<CategoryDisplay> Categories { get; set; }
        public List<LevelDisplay> Levels { get; set; }
    }
}
