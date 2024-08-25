using System;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpeedRunApp.Service
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepo = null;
        private readonly IGameRepository _gameRepo = null;
        private readonly ISpeedRunService _speedRunService = null;

        public PlayerService(IPlayerRepository playerRepo, IGameRepository gameRepo, ISpeedRunService speedRunService)
        {
            _playerRepo = playerRepo;
            _gameRepo = gameRepo;
            _speedRunService = speedRunService;
        }

        public PlayerDetailsViewModel GetPlayerDetails(string playerAbbr, string speedRunCode)
        {
            var playerDetailsVM = new PlayerDetailsViewModel();
            var playerVW = _playerRepo.GetPlayerViews(i => i.Abbr == playerAbbr).FirstOrDefault();
            if(playerVW != null)
            {
                var playerRunCounts = _playerRepo.GetPlayerSpeedRunCounts(playerVW.ID);
                playerDetailsVM = new PlayerDetailsViewModel(playerVW, playerRunCounts, speedRunCode);
            }
            
            return playerDetailsVM;
        }

        public PlayerDetailsTabViewModel GetPlayerSpeedRunTabsAndData(int playerID)
        {
            var runVMs = _speedRunService.GetPlayerSpeedRunGridData(playerID).ToList();            
            var gameIDs = runVMs.Select(i => i.GameID).Distinct().ToList();
            var games = _gameRepo.GetGameViews(i => gameIDs.Contains(i.ID));
            var runs = runVMs.Select(i=> new SpeedRun() { ID = i.ID, GameID = i.GameID, CategoryID = i.CategoryID, LevelID = i.LevelID, SubCategoryVariableValueIDs = i.SubCategoryVariableValueIDs, Rank = i.Rank }).ToList();
            var tabItems = new List<GameTabViewModel>();
            foreach(var game in games)
            {
                var gameTab = new GameTabViewModel(game, runs, true);
                tabItems.Add(gameTab);
            }

            var categoryTypes = tabItems.SelectMany(i => i.CategoryTypes).GroupBy(g => new {g.ID}).Select(i=>i.First()).OrderBy(i=>i.ID).ToList();                                  
            var tabVM = new PlayerDetailsTabViewModel(tabItems, categoryTypes, runVMs);
                       
            return tabVM;
        }    
         
        public IEnumerable<SearchResult> SearchPlayers(string searchText)
        {
            return _playerRepo.SearchPlayers(searchText);
        }    
    }
}
