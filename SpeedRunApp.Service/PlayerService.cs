using System;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpeedRunCommon.Extensions;

namespace SpeedRunApp.Service
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepo = null;
        private readonly IGameRepository _gameRepo = null;
        private readonly ISpeedRunService _speedRunService = null;
        private readonly ICacheService _cacheService = null;

        public PlayerService(IPlayerRepository playerRepo, IGameRepository gameRepo, ISpeedRunService speedRunService, ICacheService cacheService)
        {
            _playerRepo = playerRepo;
            _gameRepo = gameRepo;
            _speedRunService = speedRunService;
            _cacheService = cacheService;
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
            var results = new List<SearchResult>();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.SanatizeName();
                var searchItems = searchText.Split(' ');
                results = _cacheService.GetPlayerViews()
                               .Where(i => searchItems.Any(g => i.SantizedName.Contains(g, StringComparison.OrdinalIgnoreCase)))
                               .GroupBy(g => new { g.Name })
                               .Select(i => i.First())
                               .Select(i => new { i.ID, i.Name, i.Abbr, i.ProfileImageUrl, 
                                    ContainsPriority = searchItems.Count() - searchItems.Count(g => i.SantizedName.Contains(g, StringComparison.OrdinalIgnoreCase)),
                                    MatchPriority = searchItems.Count() - searchItems.Intersect(i.SantizedName.Split(' '), StringComparer.OrdinalIgnoreCase).Count(),
                                    RemainderPriority = i.SantizedNameNoSpace.Replace(searchItems, string.Empty, StringComparison.OrdinalIgnoreCase).Length
                               })
                               .OrderBy(i => i.ContainsPriority)
                               .ThenBy(i => i.MatchPriority)
                               .ThenBy(i => i.RemainderPriority)
                               .Select(i => new SearchResult() { Value = i.Abbr, Label = i.Name, ImagePath = i.ProfileImageUrl })
                               .Take(20)
                               .ToList();
            }
            
            return results;
        }         
    }
}
