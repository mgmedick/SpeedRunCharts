using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameDetailsViewModel(Game game)
        {
            ID = game.ID;
            Name = game.Name;
            Abbreviation = game.Abbreviation;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.Assets?.CoverLarge?.Uri;
            SearchGames = new List<GameDisplay>() { new GameDisplay { ID = game.ID, Name = game.Name, CategoryTypeIDs = game.Categories.Select(i => ((int)i.Type).ToString()).Distinct() } };
            SearchCategoryTypes = game.CategoryTypes.Select(i => new IDNamePair { ID = ((int)i).ToString(), Name = i.ToString() }).ToList();
            SearchCategories = game.Categories.Select(i => new CategoryDisplay { ID = i.ID, Name = i.Name, CategoryTypeID = ((int)i.Type).ToString(), GameID = i.GameID }).ToList();
            SearchLevels = game.Levels.Select(i => new LevelDisplay { ID = i.ID, Name = i.Name, GameID = i.GameID }).ToList();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public int? YearOfRelease { get; set; }
        public Uri CoverImageUri { get; set; }
        public IEnumerable<IDNamePair> SearchCategoryTypes { get; set; }
        public IEnumerable<GameDisplay> SearchGames { get; set; }
        public IEnumerable<CategoryDisplay> SearchCategories { get; set; }
        public IEnumerable<LevelDisplay> SearchLevels { get; set; }
    }
}
