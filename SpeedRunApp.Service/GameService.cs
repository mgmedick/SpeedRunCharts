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
        private readonly ISpeedRunsService _speedRunsService = null;

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
            var gridItems = new List<SpeedRunGridItem>() { _gameRepo.GetSpeedRunGridItemByGameID(gameID) };
            var gridVM = new SpeedRunGridViewModel("Game", gridItems);

            return gridVM;
        }
    }
}
