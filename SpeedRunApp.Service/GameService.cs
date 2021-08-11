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

        public SpeedRunGridTabViewModel GetSpeedRunGridTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var tabItems = new List<GameViewModel>() { new GameViewModel(gamevw) };
            //var game = tabItems.FirstOrDefault();
            //var categoryTypeID = game.CategoryTypes.Select(i => i.ID).FirstOrDefault();
            //var categoryID = game.Categories.Select(i => i.ID).FirstOrDefault();
            //var levelID = game.Levels?.Select(i => i.ID).FirstOrDefault();

            var gridVM = new SpeedRunGridTabViewModel(tabItems);

            return gridVM;
        }

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

