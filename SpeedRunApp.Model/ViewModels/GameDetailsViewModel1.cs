using SpeedRunApp.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameDetailsViewModel1
    {
        public GameDetailsViewModel1(GameView game)
        {
            ID = game.ID;
            Name = game.Name;
            JapaneseName = game.JapaneseName;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.CoverImageUrl;

            if(!string.IsNullOrWhiteSpace(game.CategoryTypes))
            {
                CategoryTypes = new List<IDNamePair>();
                foreach (var categoryType in game.CategoryTypes.Split(","))
                {
                    var values = categoryType.Split("|");
                    CategoryTypes.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Categories))
            {
                Categories = new List<CategoryDisplay1>();
                foreach (var category in game.Categories.Split(","))
                {
                    var values = category.Split("|");
                    Categories.Add(new CategoryDisplay1 { ID = values[0], Name = values[1], CategoryTypeID = Convert.ToInt32(values[2]) });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Levels))
            {
                Levels = new List<IDNamePair>();
                foreach (var level in game.Levels.Split(","))
                {
                    var values = level.Split("|");
                    Levels.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Variables))
            {
                Variables = new List<VariableDisplay1>();
                foreach (var variable in game.Variables.Split(","))
                {
                    var values = variable.Split("|");
                    var variableDisplay = new VariableDisplay1 { ID = values[0], Name = values[1], IsSubCategory = Convert.ToBoolean(values[2]), ScopeTypeID = Convert.ToInt32(values[3]), CategoryID = values[4], LevelID = values[5] };
                    variableDisplay.VariableValues = game.VariableValues?.Split(",").Where(i => i.Split("|")[2] == variableDisplay.ID).Select(i => new VariableValueDisplay1 { ID = i.Split("|")[0], Name = i.Split("|")[1] });
                    Variables.Add(variableDisplay);
                }

                AdjustVariables(Variables);
                var subVariables = Variables.Where(i => i.IsSubCategory).ToList();
                SubCategoryVariables = GetNestedVariables(subVariables);
            }

            if (!string.IsNullOrWhiteSpace(game.Platforms))
            {
                Platforms = new List<IDNamePair>();
                foreach (var platform in game.Platforms.Split(","))
                {
                    var values = platform.Split("|");
                    Platforms.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Moderators))
            {
                Moderators = new List<IDNamePair>();
                foreach (var moderator in game.Moderators.Split(","))
                {
                    var values = moderator.Split("|");
                    Moderators.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }
        }

        public void AdjustVariables(List<VariableDisplay1> variables)
        {
            var globalVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.Global).ToList();
            var categories = Categories.Reverse<CategoryDisplay1>();
            foreach (var globalVariable in globalVariables)
            {
                foreach (var category in categories)
                {
                    if (category.CategoryTypeID == (int)CategoryType.PerLevel)
                    {
                        foreach (var level in Levels)
                        {
                            var variable = (VariableDisplay1)globalVariable.Clone();
                            variable.CategoryID = category.ID;
                            variable.LevelID = level.ID;
                            variables.Insert(0, variable);
                        }
                    }
                    else
                    {
                        var variable = (VariableDisplay1)globalVariable.Clone();
                        variable.CategoryID = category.ID;
                        variables.Insert(0, variable);
                    }
                }
            }

            variables.RemoveAll(i => i.ScopeTypeID == (int)VariableScopeType.Global && string.IsNullOrWhiteSpace(i.CategoryID));

            var allLevelVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.AllLevels).ToList();
            var levelCategories = Categories.Where(i => i.CategoryTypeID == (int)CategoryType.PerLevel).Reverse();
            foreach (var allLevelVariable in allLevelVariables)
            {
                foreach (var category in levelCategories)
                {
                    foreach (var level in Levels)
                    {
                        var variable = (VariableDisplay1)allLevelVariable.Clone();
                        variable.CategoryID = category.ID;
                        variable.LevelID = level.ID;
                        variables.Insert(0, variable);
                    }
                }
            }

            variables.RemoveAll(i => i.ScopeTypeID == (int)VariableScopeType.AllLevels && string.IsNullOrWhiteSpace(i.CategoryID) && string.IsNullOrWhiteSpace(i.LevelID));
        }

        public List<VariableDisplay1> GetNestedVariables(IEnumerable<VariableDisplay1> variables, int count = 0)
        {
            var results = variables.Skip(count).Take(variables.Count() - count).Select((g, i) => new VariableDisplay1
            {
                ID = g.ID,
                Name = g.Name,
                CategoryID = g.CategoryID,
                LevelID = g.LevelID,
                ScopeTypeID = g.ScopeTypeID,
                VariableValues = g.VariableValues.Select(h => new VariableValueDisplay1
                {
                    ID = h.ID,
                    Name = h.Name,
                    SubVariables = GetNestedVariables(variables.Where(n => n.CategoryID == g.CategoryID
                                                                        && n.LevelID == g.LevelID), count + 1)
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
        public List<CategoryDisplay1> Categories { get; set; }
        public List<IDNamePair> Levels { get; set; }
        public List<VariableDisplay1> Variables { get; set; }
        public List<VariableDisplay1> SubCategoryVariables { get; set; }
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
