using System;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class GamesService : IGameService
    {
        private readonly IGameRepository _gameRepo = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;

        public GamesService(IGameRepository gameRepo, ISpeedRunRepository speedRunRepo)
        {
            _gameRepo = gameRepo;
            _speedRunRepo = speedRunRepo;
        }

        public GameDetailsViewModel GetGameDetails(string gameAbbr, string speedRunCode) {
            var gameDetailsVM = new GameDetailsViewModel();
            var gameVW = _gameRepo.GetGameViews(i => i.Abbr == gameAbbr).FirstOrDefault();
            if (gameVW != null)
            {
                gameDetailsVM = new GameDetailsViewModel(gameVW, speedRunCode);
            }

            return gameDetailsVM;
        }

        public GameDetailsTabViewModel GetLeaderboardTabs(int gameID, string speedRunCode = null)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = GetGameTabs(new List<GameView>() { gamevw }, runs).ToList();   
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };

            var gridTabVM = new GameDetailsTabViewModel(tabItems, exportTypes);

            if (!string.IsNullOrWhiteSpace(speedRunCode)) {
                var runVW = _speedRunRepo.GetSpeedRunGridViews(i => i.Code == speedRunCode).FirstOrDefault();
                gridTabVM.RunVW = runVW;
            }

            return gridTabVM;
        }
        
        public GameDetailsTabViewModel GetWorldRecordTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = GetGameTabs(new List<GameView>() { gamevw }, runs, true).ToList();   
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };                            
            var tabVM = new GameDetailsTabViewModel(tabItems, exportTypes);

            return tabVM;
        }

        public GameDetailsTabViewModel GetGameChartTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = GetGameTabs(new List<GameView>() { gamevw }, runs, true).ToList();   
            var tabVM = new GameDetailsTabViewModel(tabItems);

            return tabVM;
        }

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            return _gameRepo.SearchGames(searchText);
        }

        public IEnumerable<GameTabViewModel> GetGameTabs(IEnumerable<GameView> games, IEnumerable<SpeedRun> runs = null, bool hasDataOnly = false)
        {
            var gameTabs = new List<GameTabViewModel>();

            foreach(var game in games)
            {
                var gameTab = new GameTabViewModel(game);
                SetGameTabHasData(gameTab, runs);
                if (gameTab.SubCategoryVariablesTabs != null)
                {
                    gameTab.SubCategoryVariablesTabs = FilterGameTabSubCategoryVariablesByHasData(gameTab.SubCategoryVariablesTabs, true);
                }

                if (hasDataOnly)
                {
                    FilterGameTabByHasData(gameTab, true);
                }

                gameTabs.Add(gameTab);
            }

            return gameTabs;
        }

        private void SetGameTabHasData(GameTabViewModel gameTab, IEnumerable<SpeedRun> runs)
        {
            if (gameTab.Categories != null)
            {
                foreach(var category in gameTab.Categories)
                {
                    category.HasData = runs.Any(i => i.CategoryID == category.ID);

                    if (!category.HasData) {
                        category.Name += " (empty)";
                    }                    
                }
            }

            if (gameTab.CategoryTypes != null)
            {
                var categoryTypeIDsToRemove = new List<int>();
                foreach (var categoryType in gameTab.CategoryTypes)
                {                    
                    if (!gameTab.Categories.Any(i => i.CategoryTypeID == categoryType.ID && i.HasData))
                    {
                        categoryTypeIDsToRemove.Add(categoryType.ID);
                    }
                }

                gameTab.CategoryTypes.RemoveAll(i => categoryTypeIDsToRemove.Contains(i.ID));
            }

            if (gameTab.Levels != null)
            {
                foreach (var level in gameTab.Levels)
                {
                    level.HasData = runs.Any(i => i.CategoryID == level.CategoryID && i.LevelID == level.ID);

                    if (!level.HasData) {
                        level.Name += " (empty)";
                    }
                }
            }

            if (gameTab.SubCategoryVariablesTabs != null)
            {
                SetGameTabVariablesHasValue(gameTab.SubCategoryVariablesTabs, gameTab.SubCategoryVariablesTabs, runs.ToList());
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

        private void FilterGameTabByHasData(GameTabViewModel tabItem, bool hasData)
        {
            tabItem.Categories = tabItem.Categories?.Where(i => i.HasData == hasData).ToList();
            tabItem.Levels = tabItem.Levels?.Where(i => i.HasData == hasData).ToList();

            if(tabItem.SubCategoryVariablesTabs != null && tabItem.SubCategoryVariablesTabs.Any())
            {
                tabItem.SubCategoryVariablesTabs = FilterGameTabSubCategoryVariablesByHasData(tabItem.SubCategoryVariablesTabs, hasData);
                FilterGameTabSubCategoryVariableValuesByHasData(tabItem.SubCategoryVariablesTabs, hasData);
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
    }
}

