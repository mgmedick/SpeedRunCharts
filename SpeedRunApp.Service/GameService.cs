using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
//using SpeedRunApp.Interfaces.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class GamesService : IGamesService
    {
        private readonly IGameRepository _gameRepo = null;

        public GamesService(IGameRepository gameRepo)
        {
            _gameRepo = gameRepo;
        }

        public GameViewModel GetGameDetails(string gameID)
        {
            var game = _gameRepo.GetGameView(gameID);
            var gameVM = new GameViewModel(game);

            return gameVM;
        }

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            return _gameRepo.SearchGames(searchText);
        }

        public SpeedRunGridViewModel GetSpeedRunGridModel(string gameID)
        {
            var gridItems = _gameRepo.GetSpeedRunGridItemsByGameID(gameID);
            var gridVM = new SpeedRunGridViewModel("Game", gridItems.Select(i => new SpeedRunGridItemViewModel(i)).ToList());

            return gridVM;
        }
    }
}
