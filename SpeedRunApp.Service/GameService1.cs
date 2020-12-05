using SpeedRunApp.Client;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
//using SpeedRunApp.Interfaces.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class GamesService1 : IGamesService1
    {
        private readonly IGameRepository _gameRepo = null;

        public GamesService1(IGameRepository gameRepo)
        {
            _gameRepo = gameRepo;
        }

        public GameDetailsViewModel1 GetGameDetails(string gameID)
        {
            var game = _gameRepo.GetGameView(gameID);
            var gameVM = new GameDetailsViewModel1(game);

            return gameVM;
        }
    }
}
