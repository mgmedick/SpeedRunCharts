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

        public GameViewModel GetGame(int gameID)
        {
            var game = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var gameVM = new GameViewModel(game);

            return gameVM;
        }

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            return _gameRepo.SearchGames(searchText);
        }

        //public IEnumerable<SpeedRunGridTabViewModel> GetWorldRecordGridTabs(int gameID)
        //{
        //    var games = _gameRepo.GetGamesByUserID(userID);
        //    var tabItems = games.Select(i => new GameViewModel(i));
        //    var gridVM = new SpeedRunGridTabViewModel(tabItems);

        //    return gridVM;
        //}

        public SpeedRunGridTabViewModel GetSpeedRunGridTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var tabItems = new List<GameViewModel>() { new GameViewModel(gamevw) };
            var gridVM = new SpeedRunGridTabViewModel(tabItems);

            return gridVM;
        }

        public SpeedRunGridTabViewModel GetSpeedRunGridTabsForUser(int userID)
        {
            var games = _gameRepo.GetGamesByUserID(userID);
            var tabItems = games.Select(i => new GameViewModel(i));
            var gridVM = new SpeedRunGridTabViewModel(tabItems);

            return gridVM;
        }

        //public IEnumerable<IDNamePair> GetPersonalBestGridTabs(int userID)
        //{
        //    var games = _gameRepo.GetGamesByUserID(userID);
        //    var tabItems = games.Select(i => new GameViewModel(i));
        //    var categoryTypes = tabItems.SelectMany(i => i.CategoryTypes)
        //                                .GroupBy(g => new { g.ID })
        //                                .Select(i => i.First())
        //                                .ToList();

        //    return categoryTypes;
        //}

        /*
        public SpeedRunGridContainerViewModel GetSpeedRunGrid(int gameID)
        {
            var runs = _speedRunRepo.GetSpeedRunGridViews(i => i.GameID == gameID && i.Rank.HasValue).OrderBy(i => i.Rank);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            var game = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var tabItems = new List<GameViewModel>() { new GameViewModel(game, runVMs) };
            var gridVM = new SpeedRunGridContainerViewModel(new SpeedRunGridTabViewModel("Game", tabItems), runVMs);

            return gridVM;
        } 
        */
    }
}

