using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameViewModel
    {
        public GameViewModel(GameView game, IEnumerable<SpeedRunGridViewModel> runVMs = null)
        {
            ID = game.ID;
            Name = game.Name;
            Abbr = game.Abbr;
            YearOfRelease = game.YearOfRelease;
            CoverImageUri = game.CoverImageUrl;
            SpeedRunComLink = game.SpeedRunComUrl;

            if (!string.IsNullOrWhiteSpace(game.CategoryTypes))
            {
                CategoryTypes = new List<TabItem>();
                foreach (var categoryType in game.CategoryTypes.Split("^^"))
                {
                    var values = categoryType.Split("|", 2);
                    var categoryTypeTab = new TabItem() { ID = Convert.ToInt32(values[0]), Name = values[1] };
                    CategoryTypes.Add(categoryTypeTab);
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Categories))
            {
                Categories = new List<Category>();
                foreach (var categoryString in game.Categories.Split("^^"))
                {
                    var values = categoryString.Split("|", 3);
                    var category = new Category
                    {
                        ID = Convert.ToInt32(values[0]),
                        CategoryTypeID = Convert.ToInt32((string)values[1]),
                        Name = values[2]
                    };
                    category.HasData = runVMs == null || runVMs.Any(i => i.CategoryID == category.ID);
                    Categories.Add(category);
                }

                foreach (var category in Categories)
                {
                    if (!category.HasData)
                    {
                        category.Name += " (empty)";
                    }
                }             
            }

            if (CategoryTypes != null) {
                var categoryTypeIDsToRemove = new List<int>();
                foreach (var categoryType in CategoryTypes)
                {
                    categoryType.HasData = Categories.Any(i => i.CategoryTypeID == categoryType.ID && i.HasData);
                    
                    if (!categoryType.HasData)
                    {
                        categoryTypeIDsToRemove.Add(categoryType.ID);
                    }
                }

                CategoryTypes.RemoveAll(i => categoryTypeIDsToRemove.Contains(i.ID));
            }
                                                
            if (!string.IsNullOrWhiteSpace(game.Levels))
            {
                Levels = new List<IDNamePair>();
                foreach (var levelString in game.Levels.Split("^^"))
                {
                    var values = levelString.Split("|", 2);
                    var level = new IDNamePair
                    {
                        ID = Convert.ToInt32(values[0]),
                        Name = values[1]
                    };
                    Levels.Add(level);
                }

                LevelTabs = new List<LevelTab>();
                var levelCategories = Categories.Where(i => i.CategoryTypeID == (int)CategoryType.PerLevel).ToList();
                foreach (var levelCategory in levelCategories) {
                    foreach (var level in Levels)
                    {
                        var levelTab = new LevelTab
                        {
                            ID = level.ID,
                            Name = level.Name,
                            CategoryID = levelCategory.ID,
                            HasData = runVMs == null || runVMs.Any(i => i.CategoryID == levelCategory.ID && i.LevelID == level.ID)
                        };
                        LevelTabs.Add(levelTab);
                    }
                }

                foreach(var levelTab in LevelTabs){
                    if (!levelTab.HasData) {
                        levelTab.Name += " (empty)";
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Variables))
            {
                Variables = new List<Variable>();
                foreach (var variableString in game.Variables.Split("^^"))
                {
                    var values = variableString.Split("|", 6);
                    var variable = new Variable
                    {
                        ID = Convert.ToInt32(values[0]),
                        IsSubCategory = Convert.ToBoolean(values[1]),
                        ScopeTypeID = Convert.ToInt32((string)values[2]),
                        CategoryID = !String.IsNullOrWhiteSpace((string)values[3]) ? Convert.ToInt32((string)values[3]) : (int?)null,
                        LevelID = !String.IsNullOrWhiteSpace((string)values[4]) ? Convert.ToInt32((string)values[4]) : (int?)null,
                        Name = values[5],
                    };

                    variable.VariableValues = game.VariableValues?.Split("^^")
                                                  .Where(i => i.Split("|", 3)[1] == variable.ID.ToString())
                                                  .Select(i => new VariableValue
                                                  {
                                                      ID = Convert.ToInt32(i.Split("|", 3)[0]),
                                                      Name = i.Split("|", 3)[2]
                                                  }).ToList();

                    if (variable.VariableValues != null)
                    {
                        foreach (var variableValue in variable.VariableValues)
                        {
                            variableValue.HasData = runVMs == null || runVMs.Any(i => i.VariableValues != null && i.VariableValues.Any(g => g.Value == variableValue.ID));
                        }
                    }

                    Variables.Add(variable);
                }

                Variables.RemoveAll(i => i.VariableValues == null || !i.VariableValues.Any());

                var subVariables = Variables.Where(i => i.IsSubCategory).ToList();
                SubCategoryVariables = GetAdjustedVariables(subVariables);
                SubCategoryVariablesTabs = GetNestedVariables(SubCategoryVariables);
                SetVariablesHasValue(SubCategoryVariablesTabs, runVMs);          
                //SetParentVariablesHasValue(SubCategoryVariablesTabs);
            }

            if (!string.IsNullOrWhiteSpace(game.Platforms))
            {
                Platforms = new List<IDNamePair>();
                foreach (var platform in game.Platforms.Split("^^"))
                {
                    var values = platform.Split("|", 2);
                    Platforms.Add(new IDNamePair { ID = Convert.ToInt32(values[0]), Name = values[1] });
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Moderators))
            {
                Moderators = new List<IDNameAbbrPair>();
                foreach (var moderator in game.Moderators.Split("^^"))
                {
                    var values = moderator.Split("¦", 3);
                    Moderators.Add(new IDNameAbbrPair { ID = Convert.ToInt32(values[0]), Name = values[1], Abbr = values[2] });
                }
            }
        }

        public List<Variable> GetAdjustedVariables(List<Variable> variables)
        {
            var globalVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.Global && !i.CategoryID.HasValue).Reverse().ToList();
            var categories = Categories.Reverse<Category>();
            foreach (var globalVariable in globalVariables)
            {
                foreach (var category in categories)
                {
                    if (category.CategoryTypeID == (int)CategoryType.PerLevel && Levels != null)
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

            variables.RemoveAll(i => i.ScopeTypeID == (int)VariableScopeType.Global && !i.CategoryID.HasValue);

            var gameVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.FullGame && !i.CategoryID.HasValue).Reverse().ToList();
            var gameCategories = Categories.Where(i => i.CategoryTypeID == (int)CategoryType.PerGame).Reverse();
            foreach (var gameVariable in gameVariables)
            {
                foreach (var category in gameCategories)
                {
                    var variable = (Variable)gameVariable.Clone();
                    variable.CategoryID = category.ID;
                    variables.Insert(0, variable);
                }
            }

            variables.RemoveAll(i => i.ScopeTypeID == (int)VariableScopeType.FullGame && !i.CategoryID.HasValue);

            var allLevelVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.AllLevels && !i.LevelID.HasValue).Reverse().ToList();
            var levelCategories = Categories.Where(i => i.CategoryTypeID == (int)CategoryType.PerLevel).Reverse();
            if (Levels != null && Levels.Any()) {
                foreach (var allLevelVariable in allLevelVariables)
                {
                    if(!allLevelVariable.CategoryID.HasValue){
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
                    } else {
                        foreach (var level in Levels)
                        {
                            var variable = (Variable)allLevelVariable.Clone();
                            variable.LevelID = level.ID;
                            variables.Insert(0, variable);
                        }
                    }
                }
            }

            variables.RemoveAll(i => i.ScopeTypeID == (int)VariableScopeType.AllLevels && !i.LevelID.HasValue);

            var singleLevelVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.SingleLevel && !i.CategoryID.HasValue).Reverse().ToList();
            foreach (var singleLevelVariable in singleLevelVariables)
            {
                foreach (var category in levelCategories)
                {
                    var variable = (Variable)singleLevelVariable.Clone();
                    variable.CategoryID = category.ID;
                    variables.Insert(0, variable);
                }
            }

            variables.RemoveAll(i => i.ScopeTypeID == (int)VariableScopeType.SingleLevel && !i.CategoryID.HasValue);
            variables = variables.OrderBy(i => i.ID).ToList();

            return variables;
        }

/*
        public Variable[] GetNestedVariables(Variable[] variables, int count = 0)
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
                    SubVariables = GetNestedVariables(variables.Where(n => n.CategoryID == g.CategoryID && n.LevelID == g.LevelID).ToArray(), count + 1)
                }).ToArray()
            }).ToArray();

            return results;
        }
*/
        public List<Variable> GetNestedVariables(List<Variable> variables, int count = 0)
        {
            var results = new List<Variable>();
            var variablesBatch = variables.Skip(count).Take(variables.Count() - count).ToList();
            foreach(var variable in variablesBatch)
            {
                var variableCopy = new Variable
                {   
                    ID = variable.ID,
                    Name = variable.Name,
                    IsSubCategory = variable.IsSubCategory,
                    ScopeTypeID = variable.ScopeTypeID,
                    CategoryID = variable.CategoryID,
                    LevelID = variable.LevelID,
                    VariableValues = variable.VariableValues
                                             .Select(h => new VariableValue
                                                {
                                                    ID = h.ID,
                                                    Name = h.Name,
                                                    HasData = h.HasData,
                                                    SubVariables = new List<Variable>()
                                                }).ToList()
                };

                var subVariables = variables.Where(n => n.CategoryID == variable.CategoryID && n.LevelID == variable.LevelID).ToList();
                if (subVariables.Any())
                {
                    foreach(var variableValue in variableCopy.VariableValues)
                    {
                        variableValue.SubVariables = GetNestedVariables(subVariables, count + 1);
                    }
                }

                results.Add(variableCopy);
            }

            return results.GroupBy(i => new { i.CategoryID, i.LevelID }).Select(i => i.FirstOrDefault()).ToList();
        }

        /*
        public List<Variable> GetNestedVariables(List<Variable> variables, int count = 0)
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
                    SubVariables = GetNestedVariables(variables.Where(n => n.CategoryID == g.CategoryID && n.LevelID == g.LevelID).ToList(), count + 1)
                }).ToList()
            }).ToList();

            return results.GroupBy(i => new { i.CategoryID, i.LevelID }).Select(i => i.FirstOrDefault()).ToList();
        } 
        */

        public void SetVariablesHasValue(IEnumerable<Variable> variables, IEnumerable<SpeedRunGridViewModel> runVMs, string parentVariableValues = null)
        {
           foreach (var variable in variables)
           {
                foreach (var variableValue in variable.VariableValues)
                {
                    var variableValues = string.IsNullOrWhiteSpace(parentVariableValues) ? variableValue.ID.ToString() : parentVariableValues + "," + variableValue.ID.ToString();                                                            
                    variableValue.HasData = runVMs == null || (runVMs.Any(i => i.CategoryID == variable.CategoryID
                                        && i.LevelID == variable.LevelID
                                        && !string.IsNullOrWhiteSpace(i.SubCategoryVariableValueIDs)
                                        && i.SubCategoryVariableValueIDs.StartsWith(variableValues)));

                    if (!variableValue.HasData) {
                        variableValue.Name += " (empty)";
                    }

                    if (variableValue.SubVariables != null && variableValue.SubVariables.Any())
                    {
                        parentVariableValues = string.IsNullOrWhiteSpace(parentVariableValues) ? variableValue.ID.ToString() : parentVariableValues + "," + variableValue.ID.ToString();
                        SetVariablesHasValue(variableValue.SubVariables, runVMs, parentVariableValues);
                        parentVariableValues = null;
                    }
                }
           }
        }

        /*
        public void SetVariablesHasValue(IEnumerable<Variable> variables, IEnumerable<SpeedRunGridViewModel> runVMs, List<Tuple<int,int>> parentVariableValues = null)
        {
           foreach (var variable in variables)
           {
                if (parentVariableValues == null)
                {
                    parentVariableValues = new List<Tuple<int, int>>();
                }

                foreach (var variableValue in variable.VariableValues)
                {
                    variableValue.HasData = runVMs == null || (runVMs.Any(i => i.CategoryID == variable.CategoryID
                                        && i.LevelID == variable.LevelID
                                        && i.VariableValues != null
                                        && parentVariableValues.Count(g => i.VariableValues.Any(h => h.Key.ToString() == g.Item1.ToString() && h.Value.ToString() == g.Item2.ToString())) == parentVariableValues.Count()
                                        && i.VariableValues.Any(g => g.Key.ToString() == variable.ID.ToString() && g.Value.ToString() == variableValue.ID.ToString())));
                    
                    if (!variableValue.HasData) {
                        variableValue.Name += " (empty)";
                    }

                    if (variableValue.SubVariables != null && variableValue.SubVariables.Any())
                    {
                        var parentVariableValues1 = new List<Tuple<int, int>>();
                        parentVariableValues1.AddRange(parentVariableValues);
                        parentVariableValues1.Add(new Tuple<int, int>(variable.ID, variableValue.ID));
                        SetVariablesHasValue(variableValue.SubVariables, runVMs, parentVariableValues1);
                    }
                }

                parentVariableValues = null;
           }
        }
        */

        public void SetParentVariablesHasValue(List<Variable> variables) {
            foreach(var variable in variables) {
                foreach (var variableValue in variable.VariableValues) {
                    if (variableValue.SubVariables.Any() && variableValue.SubVariables.SelectMany(i => i.VariableValues).Count(i => !i.HasData) == variableValue.SubVariables.SelectMany(i => i.VariableValues).Count()) {
                        variableValue.HasData = false;
                        
                        if (!variableValue.HasData && !variableValue.Name.EndsWith(" (empty)")) {
                            variableValue.Name += " (empty)";
                        }
                    }
                    
                    if (variableValue.SubVariables != null && variableValue.SubVariables.Any())
                    {
                        SetParentVariablesHasValue(variableValue.SubVariables.ToList());
                    }                      
                }
            }
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string CoverImageUri { get; set; }
        public string SpeedRunComLink { get; set; }
        public int? YearOfRelease { get; set; }
        public List<TabItem> CategoryTypes { get; set; }
        public List<Category> Categories { get; set; }
        public List<IDNamePair> Levels { get; set; }
        public List<LevelTab> LevelTabs { get; set; }
        public List<Variable> Variables { get; set; }
        public List<Variable> SubCategoryVariables { get; set; }
        public List<Variable> SubCategoryVariablesTabs { get; set; }
        public List<IDNamePair> Platforms { get; set; }
        public List<IDNameAbbrPair> Moderators { get; set; }
        public string PlatformsString
        {
            get
            {
                return Platforms != null ? string.Join(", ", Platforms.Select(i => i.Name)) : null;
            }
        }
    }
}

