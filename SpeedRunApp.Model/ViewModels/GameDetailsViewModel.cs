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
            SearchVariables = game.Variables?.Where(i => i.IsSubCategory).Select(i => new VariableDisplay { ID = i.ID, Name = i.Name, CategoryID = i.CategoryID, VariableValues = i.Values.Select(g => new IDNamePair { ID = g.ID, Name = g.Value }) }).ToList();

            var subCategoryVariables = game.Variables?.Where(i => i.IsSubCategory).ToList();
            SearchVariables = GetNestedVariables(subCategoryVariables);
        }

        public IEnumerable<VariableDisplay> GetNestedVariables(List<Variable> variables, int count = 0)
        {
            var result = variables.Skip(count).Take(variables.Count - count).Select((g, i) => new VariableDisplay
            {
                ID = g.ID,
                Name = g.Name,
                CategoryID = g.CategoryID,
                VariableValues = g.Values.Select(h => new VariableValueDisplay
                {
                    ID = h.ID,
                    Name = h.Value,
                    Variables = GetNestedVariables(variables.Where(n => n.CategoryID == g.CategoryID).ToList(), count + 1)
                })
            });

            return result.GroupBy(i => i.CategoryID).Select(i => i.FirstOrDefault());
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
