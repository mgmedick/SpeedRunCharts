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
            var runs = _speedRunRepo.GetSpeedRunGridTabViews(i => i.GameID == gameID);
            var runVMs = runs.Select(i => new SpeedRunGridViewModel(i)).ToList();
            var tabItems = new List<GameViewModel>() { new GameViewModel(gamevw, runVMs) };
            var gridVM = new SpeedRunGridTabViewModel(tabItems);

            return gridVM;
        }

        public SpeedRunGridTabViewModel GetSpeedRunGridTabsForUser(int userID)
        {
            var games = _gameRepo.GetGamesByUserID(userID);
            var tabItems = games.Select(i => new GameViewModel(i, null, true));
            var gridVM = new SpeedRunGridTabViewModel(tabItems);

            return gridVM;
        }
    }
}

