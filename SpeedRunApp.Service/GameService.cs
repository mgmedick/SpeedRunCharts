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
        public GameDetailsViewModel GetGameDetails(string gameAbbr, int? speedRunID) {
            var gameVM = GetGame(gameAbbr);
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
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank.HasValue);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            var tabItems = new List<GameViewModel>() { new GameViewModel(gamevw, runVMs) };
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };

            var gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes);

            if (speedRunID.HasValue) {
                var runVM = runVMs.FirstOrDefault(i => i.ID == speedRunID);
                if(runVM == null) {
                    var runvw = _speedRunRepo.GetSpeedRunGridTabViews(i => i.ID == speedRunID).FirstOrDefault();

                    if (runvw != null) {
                        runVM = new SpeedRunGridViewModel(runvw);
                    }
                }

                var gameVM = tabItems.FirstOrDefault(i => i.ID == runVM.GameID);
                if (runVM != null && gameVM != null) {
                    var categoryTypeID = gameVM.Categories.Where(i=>i.ID == runVM.CategoryID).Select(i=>i.CategoryTypeID).FirstOrDefault();
                    var subCategoryVariableValueNames = GetSubCategoryVariableValueNames(runVM.VariableValues, gameVM.SubCategoryVariables);
                    var showAllData = !runVM.Rank.HasValue;

                    gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes, runVM.GameID, categoryTypeID, runVM.CategoryID, runVM.LevelID, subCategoryVariableValueNames, showAllData);
                }
            }

            return gridVM;
        }    

        public SpeedRunGridTabViewModel GetSpeedRunGridTabsForUser(int userID, int? speedRunID = null)
        {
            var runs = _speedRunRepo.GetSpeedRunGridTabUserViews(i => i.UserID == userID && i.Rank.HasValue);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();            
            var gameIDs = runVMs.Select(i => i.GameID).Distinct().ToList();
            var games = _gameRepo.GetGameViews(i => gameIDs.Contains(i.ID));
            var tabItems = games.Select(i => new GameViewModel(i, runVMs.Where(g => g.GameID == i.ID).ToList())).ToList();
            FilterTabsByHasData(tabItems, true); 
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };                            
            
            var gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes);

            if (speedRunID.HasValue) {
                var runVM = runVMs.FirstOrDefault(i => i.ID == speedRunID);
                if(runVM == null) {
                    var runvw = _speedRunRepo.GetSpeedRunGridTabViews(i => i.ID == speedRunID).FirstOrDefault();

                    if (runvw != null) {
                        runVM = new SpeedRunGridViewModel(runvw);
                    }
                }
                                
                var gameVM = tabItems.FirstOrDefault(i => i.ID == runVM.GameID);
                if (runVM != null && gameVM != null) {
                    var categoryTypeID = gameVM.Categories.Where(i=>i.ID == runVM.CategoryID).Select(i=>i.CategoryTypeID).FirstOrDefault();
                    var subCategoryVariableValueNames = GetSubCategoryVariableValueNames(runVM.VariableValues, gameVM.SubCategoryVariables);
                    var showAllData = true;

                    gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes, runVM.GameID, categoryTypeID, runVM.CategoryID, runVM.LevelID, subCategoryVariableValueNames, showAllData);
                }
            }
            
            return gridVM;
        }
        
        public SpeedRunGridTabViewModel GetWorldRecordGridTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank.HasValue);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            var tabItems = new List<GameViewModel>() { new GameViewModel(gamevw, runVMs) };    
            FilterTabsByHasData(tabItems, true);                             
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };                            
            var gridVM = new SpeedRunGridTabViewModel(tabItems, exportTypes);

            return gridVM;
        }

        private void FilterTabsByHasData(List<GameViewModel> tabItems, bool hasData) {
            foreach (var tabItem in tabItems) {
                tabItem.CategoryTypes = tabItem.CategoryTypes?.Where(i => i.HasData == hasData).ToList();
                tabItem.Categories = tabItem.Categories?.Where(i => i.HasData == hasData).ToList();
                tabItem.LevelTabs = tabItem.LevelTabs?.Where(i => i.HasData == hasData).ToList();

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
    }
}

