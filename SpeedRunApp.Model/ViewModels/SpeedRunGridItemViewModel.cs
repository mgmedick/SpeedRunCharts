using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model.Data;

namespace SpeedRunApp.Model.ViewModels
{
    public class SpeedRunGridItemViewModel
    {
        public SpeedRunGridItemViewModel(SpeedRunGridItem gridItem)
        {
            GameID = gridItem.GameID;
            GameName = gridItem.GameName;

            if (!string.IsNullOrWhiteSpace(gridItem.CategoryTypes))
            {
                CategoryTypes = new List<IDNamePair>();
                foreach (var categoryType in gridItem.CategoryTypes.Split(","))
                {
                    var values = categoryType.Split("|");
                    CategoryTypes.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(gridItem.Categories))
            {
                Categories = new List<Category>();
                foreach (var category in gridItem.Categories.Split(","))
                {
                    var values = category.Split("|");
                    Categories.Add(new Category { ID = values[0], Name = values[1], CategoryTypeID = Convert.ToInt32(values[2]) });
                }
            }

            if (!string.IsNullOrWhiteSpace(gridItem.Levels))
            {
                Levels = new List<IDNamePair>();
                foreach (var level in gridItem.Levels.Split(","))
                {
                    var values = level.Split("|");
                    Levels.Add(new IDNamePair { ID = values[0], Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(gridItem.Variables))
            {
                Variables = new List<Variable>();
                foreach (var variable in gridItem.Variables.Split(","))
                {
                    var values = variable.Split("|");
                    var variableDisplay = new Variable { ID = values[0], Name = values[1], IsSubCategory = Convert.ToBoolean(values[2]), ScopeTypeID = Convert.ToInt32(values[3]), CategoryID = values[4], LevelID = values[5] };
                    variableDisplay.VariableValues = gridItem.VariableValues?.Split(",").Where(i => i.Split("|")[2] == variableDisplay.ID).Select(i => new VariableValue { ID = i.Split("|")[0], Name = i.Split("|")[1] });
                    Variables.Add(variableDisplay);
                }

                var subVariables = Variables.Where(i => i.IsSubCategory).ToList();
                SubCategoryVariables = GetAdjustedVariables(subVariables);
            }
        }

        public List<Variable> GetAdjustedVariables(List<Variable> variables)
        {
            var globalVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.Global).ToList();
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

            var allLevelVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.AllLevels).ToList();
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
                CategoryID = g.CategoryID,
                LevelID = g.LevelID,
                ScopeTypeID = g.ScopeTypeID,
                VariableValues = g.VariableValues.Select(h => new VariableValue
                {
                    ID = h.ID,
                    Name = h.Name,
                    SubVariables = GetNestedVariables(variables.Where(n => n.CategoryID == g.CategoryID
                                                                        && n.LevelID == g.LevelID), count + 1)
                })
            });

            return results.GroupBy(i => new { i.CategoryID, i.LevelID }).Select(i => i.FirstOrDefault()).ToList();
        }

        public string GameID { get; set; }
        public string GameName { get; set; }
        public List<IDNamePair> CategoryTypes { get; set; }
        public List<Category> Categories { get; set; }
        public List<IDNamePair> Levels { get; set; }
        public List<Variable> Variables { get; set; }
        public List<Variable> SubCategoryVariables { get; set; }
    }
}
