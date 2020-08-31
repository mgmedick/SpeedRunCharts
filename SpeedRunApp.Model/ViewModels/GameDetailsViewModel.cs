using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDetailsViewModel
    {
        public GameDetailsViewModel(Game game)
        {
            ID = game.ID;
            Name = game.Name;
            JapaneseName = game.JapaneseName;
            Abbreviation = game.Abbreviation;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.Assets?.CoverLarge?.Uri;
            Moderators = game.ModeratorUsers?.Select(i => new IDNamePair { ID = i.ID, Name = i.Name }).OrderBy(i => i.Name);
            Platforms = game.Platforms.OrderBy(i => i.Name);
            SearchGames = new List<GameDisplay>() { new GameDisplay { ID = game.ID, Name = game.Name, CategoryTypeIDs = game.Categories.Select(i => ((int)i.Type).ToString()).Distinct() } };
            SearchCategoryTypes = game.CategoryTypes.Select(i => new IDNamePair { ID = ((int)i).ToString(), Name = i.ToString() }).ToList();
            SearchCategories = game.Categories.Select(i => new CategoryDisplay { ID = i.ID, Name = i.Name, CategoryTypeID = ((int)i.Type).ToString(), GameID = i.GameID }).ToList();
            SearchLevels = game.Levels.Select(i => new LevelDisplay { ID = i.ID, Name = i.Name, GameID = i.GameID }).ToList();

            var subCategoryVariables = game.Variables?.Where(i => i.IsSubCategory).ToList();
            var gameSubCategoryVariables = subCategoryVariables.Where(i => string.IsNullOrWhiteSpace(i.CategoryID)).ToList();
            foreach (var category in SearchCategories.Reverse())
            {
                foreach (var gameSubCategoryVariable in gameSubCategoryVariables)
                {
                    var variable = (Variable)gameSubCategoryVariable.Clone();
                    variable.CategoryID = category.ID;
                    subCategoryVariables.Insert(0, variable);
                }
            }
            subCategoryVariables.RemoveAll(i => string.IsNullOrWhiteSpace(i.CategoryID));
            SearchVariables = GetNestedVariables(subCategoryVariables);
        }

        public IEnumerable<VariableDisplay> GetNestedVariables(IEnumerable<Variable> variables, int count = 0)
        {
            var results = variables.Skip(count).Take(variables.Count() - count).Select((g, i) => new VariableDisplay
            {
                ID = g.ID,
                Name = g.Name,
                GameID = g.GameID,
                CategoryID = g.CategoryID,
                VariableValues = g.Values.Select(h => new VariableValueDisplay
                {
                    ID = h.ID,
                    Name = h.Value,
                    Variables = GetNestedVariables(variables.Where(n => n.GameID == g.GameID && (n.CategoryID == g.CategoryID)), count + 1)
                })
            });

            return results.GroupBy(i => new { i.GameID, i.CategoryID }).Select(i => i.FirstOrDefault());
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string Abbreviation { get; set; }
        public int? YearOfRelease { get; set; }
        public Uri CoverImageUri { get; set; }
        public IEnumerable<IDNamePair> Moderators { get; set; }
        public IEnumerable<Platform> Platforms { get; set; }
        public IEnumerable<IDNamePair> SearchCategoryTypes { get; set; }
        public IEnumerable<GameDisplay> SearchGames { get; set; }
        public IEnumerable<CategoryDisplay> SearchCategories { get; set; }
        public IEnumerable<LevelDisplay> SearchLevels { get; set; }
        public IEnumerable<VariableDisplay> SearchVariables { get; set; }

        public string PlatformsString
        {
            get
            {
                return string.Join(", ", Platforms.Select(i => i.Name));
            }
        }
    }
}
