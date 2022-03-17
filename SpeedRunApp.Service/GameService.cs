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

        public SpeedRunGridTabViewModel GetSpeedRunGridTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank.HasValue);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            var tabItems = new List<GameViewModel>() { new GameViewModel(gamevw, runVMs) };
            var gridVM = new SpeedRunGridTabViewModel(tabItems);

            return gridVM;
        }
        
        public SpeedRunGridTabViewModel GetSpeedRunGridTabsForUser(int userID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridTabUserViews(i => i.UserID == userID && i.Rank.HasValue);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();            
            var gameIDs = runVMs.Select(i => i.GameID).Distinct().ToList();
            var games = _gameRepo.GetGameViews(i => gameIDs.Contains(i.ID));
            var tabItems = games.Select(i => new GameViewModel(i, runVMs.Where(g => g.GameID == i.ID).ToList())).ToList();
            FilterTabsByHasData(tabItems, true);                             
            var gridVM = new SpeedRunGridTabViewModel(tabItems);

            return gridVM;
        } 

        public SpeedRunGridTabViewModel GetWorldRecordGridTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID && i.Rank.HasValue);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            var tabItems = new List<GameViewModel>() { new GameViewModel(gamevw, runVMs) };    
            FilterTabsByHasData(tabItems, true);                             
            var gridVM = new SpeedRunGridTabViewModel(tabItems);

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
    }
}

