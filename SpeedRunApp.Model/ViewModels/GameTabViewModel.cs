using System;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Model.ViewModels
{
    public class GameTabViewModel
    {
        public GameTabViewModel(GameView game, IEnumerable<SpeedRun> runs = null, bool hasDataOnly = false)
        {            
            ID = game.ID;
            Name = game.Name;
            Abbr = game.Abbr;
            CoverImageUri = game.CoverImageUrl;
            ShowMilliseconds = game.ShowMilliseconds;
            CategoryTypes = game.GameCategoryTypes.Select(i => new IDNamePair() { ID = i.CategoryTypeID, Name = ((CategoryType)i.CategoryTypeID).ToString() }).ToList();
            Categories = game.Categories;
            GameLevels = game.Levels;
            Variables = game.Variables;

            if (GameLevels != null)
            {
                var levelCategories = Categories?.Where(i => i.CategoryTypeID == (int)CategoryType.Level).ToList();
                if (levelCategories != null)
                {
                    Levels = new List<Level>();
                    foreach (var levelCategory in levelCategories)
                    {
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
            }
  
            if (Variables != null)
            {
                foreach(var variable in Variables)
                {
                    variable.VariableValues = game.VariableValues?.Where(i => i.VariableID == variable.ID).ToList();
                }

                Variables.RemoveAll(i => i.VariableValues == null || !i.VariableValues.Any());
                
                var subVariables = Variables.Where(i => i.IsSubCategory).ToList();
                SubCategoryVariables = GetAdjustedVariables(subVariables, runs);
                SubCategoryVariablesTabs = GetNestedVariables(SubCategoryVariables);

                if (runs != null)
                {
                    SetGameTabHasData(runs);

                    if (hasDataOnly)
                    {
                        FilterGameTabByHasData(true);
                    }
                }
            }           
        }
    
        private List<Variable> GetAdjustedVariables(List<Variable> variables, IEnumerable<SpeedRun> runs = null)
        {       
            if (runs != null)
            {
                Variables.RemoveAll(i => i.VariableScopeTypeID == (int)VariableScopeType.Global && !runs.Any(x => !string.IsNullOrWhiteSpace(x.SubCategoryVariableValueIDs) && x.SubCategoryVariableValueIDs.Split(",").Intersect(i.VariableValues.Select(g => g.ID.ToString())).Any()));
            }
            
            var categoryVariables = variables.Where(i => (i.VariableScopeTypeID == (int)VariableScopeType.Global || i.VariableScopeTypeID == (int)VariableScopeType.FullGame) && i.CategoryID.HasValue && !i.LevelID.HasValue).ToList();
            foreach (var categoryVariable in categoryVariables)
            {
                var category = Categories.FirstOrDefault(i => i.ID == categoryVariable.CategoryID);
                if (category != null && category.CategoryTypeID == (int)CategoryType.FullGame)
                {
                    categoryVariable.IsSingleCategory = true;
                }
            }

            var globalVariables = variables.Where(i => i.VariableScopeTypeID == (int)VariableScopeType.Global && !i.CategoryID.HasValue).Reverse().ToList();            
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

            variables.RemoveAll(i => i.VariableScopeTypeID == (int)VariableScopeType.Global && !i.CategoryID.HasValue);

            var levelCategoryIDs = Categories.Where(i=>i.CategoryTypeID == (int)CategoryType.Level).Select(i=>i.ID).ToList();
            var globalLevelVariables = variables.Where(i => i.VariableScopeTypeID == (int)VariableScopeType.Global && i.CategoryID.HasValue && levelCategoryIDs.Contains(i.CategoryID.Value) && !i.LevelID.HasValue).Reverse().ToList();
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

            variables.RemoveAll(i => i.VariableScopeTypeID == (int)VariableScopeType.Global && i.CategoryID.HasValue && levelCategoryIDs.Contains(i.CategoryID.Value) && !i.LevelID.HasValue);
            
            var gameVariables = variables.Where(i => i.VariableScopeTypeID == (int)VariableScopeType.FullGame && !i.CategoryID.HasValue).Reverse().ToList();
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

            variables.RemoveAll(i => i.VariableScopeTypeID == (int)VariableScopeType.FullGame && !i.CategoryID.HasValue);

            var allLevelVariables = variables.Where(i => i.VariableScopeTypeID == (int)VariableScopeType.AllLevels && !i.LevelID.HasValue).Reverse().ToList();
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

            variables.RemoveAll(i => i.VariableScopeTypeID == (int)VariableScopeType.AllLevels && !i.LevelID.HasValue);

            var singleLevelVariables = variables.Where(i => i.VariableScopeTypeID == (int)VariableScopeType.SingleLevel && !i.CategoryID.HasValue).Reverse().ToList();
            foreach (var singleLevelVariable in singleLevelVariables)
            {
                foreach (var category in levelCategories)
                {
                    var variable = (Variable)singleLevelVariable.Clone();
                    variable.CategoryID = category.ID;
                    variables.Insert(0, variable);
                }
            }

            variables.RemoveAll(i => i.VariableScopeTypeID == (int)VariableScopeType.SingleLevel && !i.CategoryID.HasValue);
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

        private void SetGameTabHasData(IEnumerable<SpeedRun> runs)
        {
            if (Categories != null)
            {
                foreach(var category in Categories)
                {
                    category.HasData = runs.Any(i => i.CategoryID == category.ID);

                    if (!category.HasData) {
                        category.Name += " (empty)";
                    }                    
                }
            }

            if (CategoryTypes != null)
            {
                var categoryTypeIDsToRemove = new List<int>();
                foreach (var categoryType in CategoryTypes)
                {                    
                    if (!Categories.Any(i => i.CategoryTypeID == categoryType.ID && i.HasData))
                    {
                        categoryTypeIDsToRemove.Add(categoryType.ID);
                    }
                }

                CategoryTypes.RemoveAll(i => categoryTypeIDsToRemove.Contains(i.ID));
            }

            if (Levels != null)
            {
                foreach (var level in Levels)
                {
                    level.HasData = runs.Any(i => i.CategoryID == level.CategoryID && i.LevelID == level.ID);

                    if (!level.HasData) {
                        level.Name += " (empty)";
                    }
                }
            }

            if (SubCategoryVariablesTabs != null)
            {
                SetGameTabVariablesHasValue(SubCategoryVariablesTabs, SubCategoryVariablesTabs, runs.ToList());
            }
        }

        private void SetGameTabVariablesHasValue(List<Variable> allVariables, List<Variable> variables, List<SpeedRun> runs, string parentVariableValues = null)
        {
           foreach (var variable in variables)
           {
                foreach (var variableValue in variable.VariableValues)
                {
                    var variableValues = string.IsNullOrWhiteSpace(parentVariableValues) ? variableValue.ID.ToString() : parentVariableValues + "," + variableValue.ID.ToString();                                                            
                    variableValue.HasData = runs.Any(i => i.CategoryID == variable.CategoryID
                                        && i.LevelID == variable.LevelID
                                        && !string.IsNullOrWhiteSpace(i.SubCategoryVariableValueIDs)
                                        && i.SubCategoryVariableValueIDs.StartsWith(variableValues));

                    if (!variableValue.HasData) {
                        variableValue.Name += " (empty)";
                    }

                    var subvars = allVariables.Where(i => i.CategoryID == variable.CategoryID && i.LevelID == variable.LevelID).ToList();
                    foreach(var subvar in subvars)
                    {
                        foreach(var va in subvar.VariableValues)
                        {
                            if (va.ID == variableValue.ID)
                            {
                                va.HasData = variableValue.HasData;
                            }
                        }
                    }

                    if (variableValue.SubVariables != null && variableValue.SubVariables.Any())
                    {
                        SetGameTabVariablesHasValue(allVariables, variableValue.SubVariables.ToList(), runs, variableValues);
                    }
                }

                parentVariableValues = null;    
           }
        }        

        private void FilterGameTabByHasData(bool hasData)
        {
            Categories = Categories?.Where(i => i.HasData == hasData).ToList();
            Levels = Levels?.Where(i => i.HasData == hasData).ToList();

            if(SubCategoryVariablesTabs != null && SubCategoryVariablesTabs.Any())
            {
                FilterGameTabSubCategoryVariableValuesByHasData(SubCategoryVariablesTabs, hasData);
            }
        }

        private List<Variable> FilterGameTabSubCategoryVariablesByHasData(List<Variable> variables, bool hasData)
        {
            variables = variables.Where(x => x.HasData == hasData).ToList();
 
            foreach (var variable in variables)
            {
                foreach (var variableValue in variable.VariableValues)
                {
                    if (variableValue.SubVariables != null && variableValue.SubVariables.Any())
                    {
                        variableValue.SubVariables = FilterGameTabSubCategoryVariablesByHasData(variableValue.SubVariables.ToList(), hasData);
                    }                    
                }
            }

            return variables;
        }

        private void FilterGameTabSubCategoryVariableValuesByHasData(List<Variable> variables, bool hasData)
        {
            foreach(var variable in variables) {
                variable.VariableValues = variable.VariableValues.Where(i => i.HasData == hasData).ToList();

                foreach (var variableValue in variable.VariableValues) {
                    if(variableValue.SubVariables != null && variableValue.SubVariables.Any()) {
                        FilterGameTabSubCategoryVariableValuesByHasData(variableValue.SubVariables.ToList(), hasData);
                    }
                }                 
            } 
        }     

        public int ID { get; set; }
        public string Name { get; set; }
        public string Abbr { get; set; }
        public string CoverImageUri { get; set; }
        public bool ShowMilliseconds { get; set; }
        public List<IDNamePair> CategoryTypes { get; set; }
        public List<Category> Categories { get; set; }
        public List<Level> GameLevels { get; set; }
        public List<Level> Levels { get; set; }
        public List<Variable> Variables { get; set; }
        public List<Variable> SubCategoryVariables { get; set; }
        public List<Variable> SubCategoryVariablesTabs { get; set; }
    }
}

