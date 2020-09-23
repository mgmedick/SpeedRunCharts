using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;

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
            Games = new List<GameDisplay>() { new GameDisplay { ID = game.ID, Name = game.Name, CategoryTypeIDs = game.Categories.Select(i => ((int)i.Type).ToString()).Distinct() } };
            Categories = game.Categories.Select(i => new CategoryDisplay { ID = i.ID, Name = i.Name, CategoryTypeID = ((int)i.Type).ToString(), GameID = i.GameID }).ToList();
            Levels = game.Levels.Select(i => new LevelDisplay { ID = i.ID, Name = i.Name, GameID = i.GameID }).ToList();
            CategoryTypes = game.CategoryTypes.Where(i=> Levels.Any() || (int)i != (int)CategoryType.PerLevel).Select(i => new IDNamePair { ID = ((int)i).ToString(), Name = i.ToString() }).ToList();

            if (game.Variables != null && game.Variables.Any())
            {
                var variables = game.Variables.ToList();
                var gameVariables = variables.Where(i => string.IsNullOrWhiteSpace(i.CategoryID)).ToList();
                //var globalVariables = variables.Where(i => i.Scope.Type == VariableScopeType.Global).ToList();
                foreach (var category in Categories.Reverse())
                {
                    foreach (var gameVariable in gameVariables)
                    {
                        var variable = (Variable)gameVariable.Clone();
                        variable.CategoryID = category.ID;
                        variables.Insert(0, variable);
                    }
                }
                //variables = variables.Where(i => !string.IsNullOrWhiteSpace(i.CategoryID)).GroupBy(i => new { i.GameID, i.CategoryID, i.Name, i.Scope.Type }).Where(g => g.Min(i=>i.Scope.Type).Select(y => y.Last()).ToList();
                variables.RemoveAll(i => string.IsNullOrWhiteSpace(i.CategoryID));
                Variables = variables.Where(i => !i.IsSubCategory).Select(i => new VariableDisplay { ID = i.ID, Name = i.Name, GameID = i.GameID, CategoryID = i.CategoryID, VariableValues = i.Values.Select(g => new VariableValueDisplay { ID = g.ID, Name = g.Value }) }).ToList();
                SubCategoryVariables = GetNestedVariables(variables.Where(i => i.IsSubCategory));
            }
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
                    SubVariables = GetNestedVariables(variables.Where(n => n.GameID == g.GameID && (n.CategoryID == g.CategoryID)), count + 1)
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
        public IEnumerable<IDNamePair> CategoryTypes { get; set; }
        public IEnumerable<GameDisplay> Games { get; set; }
        public IEnumerable<CategoryDisplay> Categories { get; set; }
        public IEnumerable<LevelDisplay> Levels { get; set; }
        public IEnumerable<VariableDisplay> SubCategoryVariables { get; set; }
        public IEnumerable<VariableDisplay> Variables { get; set; }
        public string PlatformsString
        {
            get
            {
                return string.Join(", ", Platforms.Select(i => i.Name));
            }
        }
    }
}
