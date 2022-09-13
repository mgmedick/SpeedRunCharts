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
        private readonly ISpeedRunRepository _speedRunRepo = null;
        private readonly IGameRepository _gameRepo = null;
        private readonly ICacheService _cacheService = null;

        public GamesService(IGameRepository gameRepo, ISpeedRunRepository speedRunRepo, ICacheService cacheService)
        {
            _gameRepo = gameRepo;
            _speedRunRepo = speedRunRepo;
            _cacheService = cacheService;
        }
        public GameDetailsViewModel GetGameDetails(string gameAbbr, string speedRunComID) {
            var gameVM = GetGame(gameAbbr);
            var speedRunID = string.IsNullOrWhiteSpace(speedRunComID) ? (int?)null : _speedRunRepo.GetSpeedRunID(speedRunComID);
            var gameDetailsVM = new GameDetailsViewModel(gameVM, speedRunID);

            return gameDetailsVM;
        }

        public GameViewModel GetGame(string gameAbbr)
        {
            var game = _gameRepo.GetGameViews(i => i.Abbr == gameAbbr).FirstOrDefault();
            var gameVM = game != null ? new GameViewModel(game) : null;

            return gameVM;
        }

        public GameViewModel GetGame(int gameID)
        {
            var game = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var gameVM = game != null ? new GameViewModel(game) : null;

            return gameVM;
        }

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            return _gameRepo.SearchGames(searchText);
        }

        public SpeedRunGridTabViewModel GetSpeedRunGridTabs(int gameID, int? speedRunID = null)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank.HasValue).ToList();
            var tabItems = new List<GameTabViewModel>() { new GameTabViewModel(gamevw, runs) };
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };

            var gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes);

            if (speedRunID.HasValue) {
                var run = runs.FirstOrDefault(i => i.ID == speedRunID);
                if (run == null) {
                    run = _speedRunRepo.GetSpeedRunGridTabViews(i => i.ID == speedRunID).FirstOrDefault();
                }

                var game = tabItems.FirstOrDefault(i => i.ID == run.GameID);
                if (run != null && game != null) {
                    var categoryTypeID = game.Categories.Where(i=>i.ID == run.CategoryID).Select(i=>i.CategoryTypeID).FirstOrDefault();
                    var subCategoryVariableValueIDs = run.SubCategoryVariableValueIDs.Split(',').Select(i => Convert.ToInt32(i)).ToList();
                    var subCategoryVariableValueNames = GetSubCategoryVariableValueNames(subCategoryVariableValueIDs, game.SubCategoryVariables);
                    var showAllData = !run.Rank.HasValue;

                    gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes, run.GameID, categoryTypeID, run.CategoryID, run.LevelID, subCategoryVariableValueNames, showAllData);
                }
            }

            return gridVM;
        }
        
        public SpeedRunGridTabViewModel GetSpeedRunGridTabsForUser(int userID, int? speedRunID = null)
        {
            var runs = _speedRunRepo.GetSpeedRunGridTabUserViews(i => i.UserID == userID && i.Rank.HasValue).Cast<SpeedRunGridTabView>().ToList();
            var gameIDs = runs.Select(i => i.GameID).Distinct().ToList();
            var games = _gameRepo.GetGameViews(i => gameIDs.Contains(i.ID));
            var tabItems = games.Select(i => new GameTabViewModel(i, runs.Where(g => g.GameID == i.ID).ToList())).OrderBy(i => i.Name).ToList();
            FilterTabsByHasData(tabItems, true); 
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };                            
            
            var gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes);

            if (speedRunID.HasValue) {
                var run = runs.FirstOrDefault(i => i.ID == speedRunID);
                if (run == null) {
                    run = _speedRunRepo.GetSpeedRunGridTabUserViews(i => i.ID == speedRunID).FirstOrDefault();
                }
                                
                var game = tabItems.FirstOrDefault(i => i.ID == run.GameID);
                if (run != null && game != null) {
                    var categoryTypeID = game.Categories.Where(i=>i.ID == run.CategoryID).Select(i=>i.CategoryTypeID).FirstOrDefault();
                    var subCategoryVariableValueIDs = run.SubCategoryVariableValueIDs.Split(',').Select(i => Convert.ToInt32(i)).ToList();
                    var subCategoryVariableValueNames = GetSubCategoryVariableValueNames(subCategoryVariableValueIDs, game.SubCategoryVariables);
                    var showAllData = true;

                    gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes, run.GameID, categoryTypeID, run.CategoryID, run.LevelID, subCategoryVariableValueNames, showAllData);
                }
            }
            
            return gridVM;
        }
        
        public SpeedRunGridTabViewModel GetWorldRecordGridTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = new List<GameTabViewModel>() { new GameTabViewModel(gamevw, runs) };    
            FilterTabsByHasData(tabItems, true);                             
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };                            
            var gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes);

            return gridVM;
        }

        private void FilterTabsByHasData(List<GameTabViewModel> tabItems, bool hasData) {
            foreach (var tabItem in tabItems) {
                tabItem.Categories = tabItem.Categories?.Where(i => i.HasData == hasData).ToList();
                tabItem.Levels = tabItem.Levels?.Where(i => i.HasData == hasData).ToList();

                if(tabItem.SubCategoryVariablesTabs != null && tabItem.SubCategoryVariablesTabs.Any())
                {
                    FilterSubCategoryVariablesTabsByHasData(tabItem.SubCategoryVariablesTabs, hasData);
                }
            }   
        }

        private void FilterSubCategoryVariablesTabsByHasData(List<Variable> variables, bool hasData) {
            foreach(var variable in variables){
                variable.VariableValues = variable.VariableValues.Where(i => i.HasData == hasData).ToList();
                
                foreach (var variableValue in variable.VariableValues) {
                    if(variableValue.SubVariables != null && variableValue.SubVariables.Any()) {
                        FilterSubCategoryVariablesTabsByHasData(variableValue.SubVariables.ToList(), hasData);
                    }
                }      
            } 
        }

        private Dictionary<string, string> GetSubCategoryVariableValueNames(List<int> runSubCategoryVariableValueIDs, List<Variable> gameSubCategoryVariables)
        {                
            var SubCategoryVariableValueNames = new Dictionary<string, string>();

            var variableCount = 0;
            foreach (var runSubCategoryVariableValueID in runSubCategoryVariableValueIDs) {
                var variable = gameSubCategoryVariables.FirstOrDefault(i => i.VariableValues.Any(g => g.ID == runSubCategoryVariableValueID));
                var variableValue = variable?.VariableValues?.FirstOrDefault(i => i.ID == runSubCategoryVariableValueID);
                
                if (variable != null && variableValue != null) {
                    SubCategoryVariableValueNames.Add(variable.Name + variableCount, variableValue.Name);
                }

                variableCount++;
            }

            return SubCategoryVariableValueNames;
        }  

        /*
        private Dictionary<string, string> GetSubCategoryVariableValueNames(Dictionary<int, int> runVariableValueIDs, List<Variable> gameSubCategoryVariables)
        {                
                var SubCategoryVariableValueNames = new Dictionary<string, string>();

                if (runVariableValueIDs != null && gameSubCategoryVariables != null) {
                    var runSubCategoryVariableValueIDs = runVariableValueIDs.Where(i => gameSubCategoryVariables.Any(g => g.ID == i.Key)).ToDictionary(i => i.Key, i => i.Value);
                
                    var variableCount = 0;
                    foreach (var runSubCategoryVariableValueID in runSubCategoryVariableValueIDs) {
                        var variable = gameSubCategoryVariables.FirstOrDefault(i => i.ID == runSubCategoryVariableValueID.Key);
                        var variableValue = variable?.VariableValues?.FirstOrDefault(i => i.ID == runSubCategoryVariableValueID.Value);
                        
                        if (variable != null && variableValue != null) {
                            SubCategoryVariableValueNames.Add(variable.Name + variableCount, variableValue.Name);
                        }

                        variableCount++;
                    }
                }

                return SubCategoryVariableValueNames;
        }    
        */            
    }
}

