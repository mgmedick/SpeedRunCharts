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

        public PlayerService(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }

        public PlayerDetailsViewModel GetPlayerDetails(string playerName, string speedRunCode)
        {
            var playerVW = _playerRepo.GetPlayerViews(i => i.Name == playerName).FirstOrDefault();
            var playerRunCounts = _playerRepo.GetPlayerSpeedRunCounts(playerVW.ID);
            var playerDetailsVM = new PlayerDetailsViewModel(playerVW, playerRunCounts, speedRunCode);

            return playerDetailsVM;
        }

        public IEnumerable<SearchResult> SearchPlayers(string searchText)
        {
            return _playerRepo.SearchPlayers(searchText);
        }       
    }
}
