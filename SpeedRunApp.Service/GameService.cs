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

        public GameViewModel GetGame(string gameID)
        {
            var game = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var gameVM = new GameViewModel(game);

            return gameVM;
        }

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            return _gameRepo.SearchGames(searchText);
        }

        public Tuple<SpeedRunGridViewModel, IEnumerable<SpeedRunViewModel>> GetSpeedRunGrid(string gameID)
        {
            var runs = _speedRunRepo.GetSpeedRunViews(i => i.GameID == gameID && i.StatusTypeID == (int)RunStatusType.Verified && i.Rank.HasValue).OrderBy(i => i.Rank);
            var runVMs = runs.Select(i => new SpeedRunViewModel(i));
            var game = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var gridItems = new List<SpeedRunGridItem>() { new SpeedRunGridItem(game) };
            var gridVM = new SpeedRunGridViewModel("Game", gridItems);

            return new Tuple<SpeedRunGridViewModel, IEnumerable<SpeedRunViewModel>>(gridVM, runVMs);
        }
    }
}
