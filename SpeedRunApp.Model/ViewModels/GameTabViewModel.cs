using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameTabViewModel
    {
        public GameTabViewModel(GameView game)
        {            
            ID = game.ID;
            Name = game.Name;
            CoverImageUri = game.CoverImageUrl;
            ShowMilliseconds = game.ShowMilliseconds;

            if (!string.IsNullOrWhiteSpace(game.CategoryTypes))
            {
                CategoryTypes = new List<IDNamePair>();
                foreach (var categoryType in game.CategoryTypes.Split("^^"))
                {
                    var values = categoryType.Split("|", 2);
                    var categoryTypeTab = new IDNamePair() { ID = Convert.ToInt32(values[0]), Name = values[1] };
                    CategoryTypes.Add(categoryTypeTab);
                }
            }

            if (!string.IsNullOrWhiteSpace(game.Categories))
            {
                Categories = new List<Category>();
                foreach (var categoryString in game.Categories.Split("^^"))
                {
                    var values = categoryString.Split("|", 4);
                    var category = new Category
                    {
                        ID = Convert.ToInt32(values[0]),
                        CategoryTypeID = Convert.ToInt32((string)values[1]),
                        IsTimerAsc = Convert.ToBoolean((string)values[2]),
                        Name = values[3]
                    };
                    Categories.Add(category);
                }         
            }

            if (!string.IsNullOrWhiteSpace(game.Levels))
            {
                GameLevels = new List<IDNamePair>();
                foreach (var levelString in game.Levels.Split("^^"))
                {
                    var values = levelString.Split("|", 2);
                    var gameLevel = new IDNamePair
                    {
                        ID = Convert.ToInt32(values[0]),
                        Name = values[1]
                    };
                    GameLevels.Add(gameLevel);
                }

                Levels = new List<Level>();
                var levelCategories = Categories.Where(i => i.CategoryTypeID == (int)CategoryType.Level).ToList();
                foreach (var levelCategory in levelCategories) {
                    foreach (var gameLevel in GameLevels)
                    {
                        var level = new Level
                        {
                            ID = gameLevel.ID,
                            Name = gameLevel.Name,
                            CategoryID = levelCategory.ID
                        };
                        Levels.Add(level);
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

                    Variables.Add(variable);
                }

                Variables.RemoveAll(i => i.VariableValues == null || !i.VariableValues.Any());

                var subVariables = Variables.Where(i => i.IsSubCategory).ToList();
                SubCategoryVariables = GetAdjustedVariables(subVariables);
                SubCategoryVariablesTabs = GetNestedVariables(SubCategoryVariables);
            }
        }
    
        private List<Variable> GetAdjustedVariables(List<Variable> variables)
        {       
            var categoryVariables = variables.Where(i => (i.ScopeTypeID == (int)VariableScopeType.Global || i.ScopeTypeID == (int)VariableScopeType.FullGame) && i.CategoryID.HasValue && !i.LevelID.HasValue).ToList();
            foreach (var categoryVariable in categoryVariables)
            {
                var category = Categories.FirstOrDefault(i => i.ID == categoryVariable.CategoryID);
                if (category != null && category.CategoryTypeID == (int)CategoryType.FullGame)
                {
                    categoryVariable.IsSingleCategory = true;
                }
            }

            var globalVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.Global && !i.CategoryID.HasValue).Reverse().ToList();            
            var categories = Categories.Reverse<Category>();
            foreach (var globalVariable in globalVariables)
            {
                foreach (var category in categories)
                {
                    if (category.CategoryTypeID == (int)CategoryType.Level && GameLevels != null)
                    {
                        foreach (var gameLevel in GameLevels)
                        {
                            var variable = (Variable)globalVariable.Clone();
                            variable.CategoryID = category.ID;
                            variable.LevelID = gameLevel.ID;
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

            var levelCategoryIDs = Categories.Where(i=>i.CategoryTypeID == (int)CategoryType.Level).Select(i=>i.ID).ToList();
            var globalLevelVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.Global && i.CategoryID.HasValue && levelCategoryIDs.Contains(i.CategoryID.Value) && !i.LevelID.HasValue).Reverse().ToList();
            if (GameLevels != null && GameLevels.Any()) {
                foreach (var globalLevelVariable in globalLevelVariables)
                {
                    foreach (var gameLevel in GameLevels)
                    {
                        var variable = (Variable)globalLevelVariable.Clone();
                        variable.LevelID = gameLevel.ID;
                        variables.Insert(0, variable);
                    }
                }
            }

            variables.RemoveAll(i => i.ScopeTypeID == (int)VariableScopeType.Global && i.CategoryID.HasValue && levelCategoryIDs.Contains(i.CategoryID.Value) && !i.LevelID.HasValue);
            
            var gameVariables = variables.Where(i => i.ScopeTypeID == (int)VariableScopeType.FullGame && !i.CategoryID.HasValue).Reverse().ToList();
            var gameCategories = Categories.Where(i => i.CategoryTypeID == (int)CategoryType.FullGame).Reverse();
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
            var levelCategories = Categories.Where(i => i.CategoryTypeID == (int)CategoryType.Level).Reverse();
            if (GameLevels != null && GameLevels.Any()) {
                foreach (var allLevelVariable in allLevelVariables)
                {
                    if(!allLevelVariable.CategoryID.HasValue){
                        foreach (var category in levelCategories)
                        {
                            foreach (var gameLevel in GameLevels)
                            {
                                var variable = (Variable)allLevelVariable.Clone();
                                variable.CategoryID = category.ID;
                                variable.LevelID = gameLevel.ID;
                                variables.Insert(0, variable);
                            }
                        } 
                    } else {
                        foreach (var gameLevel in GameLevels)
                        {
                            var variable = (Variable)allLevelVariable.Clone();                          
                            variable.LevelID = gameLevel.ID;
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

        private List<Variable> GetNestedVariables(List<Variable> variables)
        {
            var results = new List<Variable>();

            foreach(var variable in variables)
            {
                if (!results.Any(i=>i.CategoryID == variable.CategoryID && i.LevelID == variable.LevelID))
                {
                    var variableCopy = (Variable)variable.Clone();
                    var subVariables = variables.Where(n => n.CategoryID == variableCopy.CategoryID && n.LevelID == variableCopy.LevelID && n.ID > variableCopy.ID).ToList();

                    if (subVariables.Any())
                    {
                        foreach(var variableValue in variableCopy.VariableValues)
                        {
                            variableValue.SubVariables = GetNestedVariables(subVariables);
                        }
                    }

                    results.Add(variableCopy);
                }
            }

            return results;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string CoverImageUri { get; set; }
        public bool ShowMilliseconds { get; set; }
        public List<IDNamePair> CategoryTypes { get; set; }
        public List<Category> Categories { get; set; }
        public List<IDNamePair> GameLevels { get; set; }
        public List<Level> Levels { get; set; }
        public List<Variable> Variables { get; set; }
        public List<Variable> SubCategoryVariables { get; set; }
        public List<Variable> SubCategoryVariablesTabs { get; set; }
    }
}

