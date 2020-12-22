using SpeedRunApp.Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunCommon;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameViewModel
    {
        public GameViewModel(GameView game)
        {
            ID = game.ID;
            Name = game.Name;
            JapaneseName = game.JapaneseName;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.CoverImageUrl;

            if (!string.IsNullOrWhiteSpace(game.CategoryTypes))
            {
                CategoryTypes = new List<IDNamePair>();
                foreach (var categoryType in game.CategoryTypes.Split("^^"))
                {
                    var values = categoryType.Split("|", 2);
                    CategoryTypes.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Categories))
            {
                Categories = new List<Category>();
                foreach (var category in game.Categories.Split("^^"))
                {
                    var values = category.Split("|", 4);
                    Categories.Add(new Category { ID = values[0], CategoryTypeID = Convert.ToInt32((string)values[1]), HasData = Convert.ToBoolean(values[2]), Name = values[3] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Levels))
            {
                Levels = new List<TabItem>();
                foreach (var level in game.Levels.Split("^^"))
                {
                    var values = level.Split("|", 3);
                    Levels.Add(new TabItem { ID = values[0], HasData = Convert.ToBoolean(values[1]), Name = values[2] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Variables))
            {
                Variables = new List<Variable>();
                foreach (var variable in game.Variables.Split("^^"))
                {
                    var values = variable.Split("|", 6);
                    var variableDisplay = new Variable { ID = values[0], IsSubCategory = Convert.ToBoolean(values[1]), ScopeTypeID = Convert.ToInt32((string)values[2]), CategoryID = values[3], LevelID = values[4], Name = values[5] };
                    variableDisplay.VariableValues = game.VariableValues?.Split("^^").Where(i => i.Split("|", 4)[1] == variableDisplay.ID).Select(i => new VariableValue { ID = i.Split("|", 4)[0], HasData = Convert.ToBoolean(i.Split("|", 4)[2]), Name = i.Split("|", 4)[3] });
                    Variables.Add(variableDisplay);
                }

                var subVariables = Variables.Where(i => i.IsSubCategory).ToList();
                SubCategoryVariables = GetAdjustedVariables(subVariables);
            }

            if (!string.IsNullOrWhiteSpace(game.Platforms))
            {
                Platforms = new List<IDNamePair>();
                foreach (var platform in game.Platforms.Split("^^"))
                {
                    var values = platform.Split("|", 2);
                    Platforms.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Moderators))
            {
                Moderators = new List<IDNamePair>();
                foreach (var moderator in game.Moderators.Split("^^"))
                {
                    var values = moderator.Split("|", 2);
                    Moderators.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }
        }

        public List<Variable> GetAdjustedVariables(List<Variable> variables)
        {
            var globalVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.Global && string.IsNullOrWhiteSpace(i.CategoryID)).ToList();
            var categories = Categories.Reverse<Category>();
            foreach (var globalVariable in globalVariables)
            {
                foreach (var category in categories)
                {
                    if (category.CategoryTypeID == (int)CategoryType.PerLevel)
                    {
                        foreach (var level in Levels)
                        {
                            var variable = (Variable)globalVariable.Clone();
                            variable.CategoryID = category.ID;
                            variable.LevelID = level.ID;
                            variables.Insert(0, variable);
                        }
                    }
                    else
                    {
                        var variable = (Variable)globalVariable.Clone();
                        variable.CategoryID = category.ID;
                        variables.Insert(0, variable);
                    }
                }
            }

            variables.RemoveAll(i => i.ScopeTypeID == (int)VariableScopeType.Global && string.IsNullOrWhiteSpace(i.CategoryID));

            var allLevelVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.AllLevels && string.IsNullOrWhiteSpace(i.CategoryID) && string.IsNullOrWhiteSpace(i.LevelID)).ToList();
            var levelCategories = Categories.Where(i => i.CategoryTypeID == (int)CategoryType.PerLevel).Reverse();
            foreach (var allLevelVariable in allLevelVariables)
            {
                foreach (var category in levelCategories)
                {
                    foreach (var level in Levels)
                    {
                        var variable = (Variable)allLevelVariable.Clone();
                        variable.CategoryID = category.ID;
                        variable.LevelID = level.ID;
                        variables.Insert(0, variable);
                    }
                }
            }

            variables.RemoveAll(i => i.ScopeTypeID == (int)VariableScopeType.AllLevels && string.IsNullOrWhiteSpace(i.CategoryID) && string.IsNullOrWhiteSpace(i.LevelID));

            var nestedVariables = GetNestedVariables(variables);

            return nestedVariables;
        }

        public List<Variable> GetNestedVariables(IEnumerable<Variable> variables, int count = 0)
        {
            var results = variables.Skip(count).Take(variables.Count() - count).Select((g, i) => new Variable
            {
                ID = g.ID,
                Name = g.Name,
                IsSubCategory = g.IsSubCategory,
                ScopeTypeID = g.ScopeTypeID,
                CategoryID = g.CategoryID,
                LevelID = g.LevelID,
                VariableValues = g.VariableValues.Select(h => new VariableValue
                {
                    ID = h.ID,
                    Name = h.Name,
                    HasData = h.HasData,
                    SubVariables = GetNestedVariables(variables.Where(n => n.CategoryID == g.CategoryID && n.LevelID == g.LevelID), count + 1)
                })
            });

            return results.GroupBy(i => new { i.CategoryID, i.LevelID }).Select(i => i.FirstOrDefault()).ToList();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string JapaneseName { get; set; }
        public string CoverImageUri { get; set; }
        public int? YearOfRelease { get; set; }
        public List<IDNamePair> CategoryTypes { get; set; }
        public List<Category> Categories { get; set; }
        public List<TabItem> Levels { get; set; }
        public List<Variable> Variables { get; set; }
        public List<Variable> SubCategoryVariables { get; set; }
        public List<IDNamePair> Platforms { get; set; }
        public List<IDNamePair> Moderators { get; set; }
        public string PlatformsString
        {
            get
            {
                return string.Join(", ", Platforms.Select(i => i.Name));
            }
        }
    }
}
