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

        public GameDetailsViewModel GetGameDetails(string gameAbbr, string speedRunCode) {
            var gameDetailsVM = new GameDetailsViewModel();
            var gameVW = _gameRepo.GetGameViews(i => i.Abbr == gameAbbr).FirstOrDefault();
            if (gameVW != null)
            {
                gameDetailsVM = new GameDetailsViewModel(gameVW, speedRunCode);
            }

            return gameDetailsVM;
        }

        public GameDetailsTabViewModel GetLeaderboardTabs(int gameID, string speedRunCode = null)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = new List<GameTabViewModel>() { new GameTabViewModel(gamevw, runs) };
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };

            var gridTabVM = new GameDetailsTabViewModel(tabItems, exportTypes);

            if (!string.IsNullOrWhiteSpace(speedRunCode)) {
                var runVW = _speedRunRepo.GetSpeedRunGridViews(i => i.Code == speedRunCode).FirstOrDefault();
                gridTabVM.RunVW = runVW;
            }

            return gridTabVM;
        }
        
        public GameDetailsTabViewModel GetWorldRecordTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = new List<GameTabViewModel>() { new GameTabViewModel(gamevw, runs, true) };
            var exportTypes = new List<IDNamePair>() { new IDNamePair() { ID = (int)ExportType.csv, Name = ExportType.csv.ToString() },
                                                       new IDNamePair() { ID = (int)ExportType.json, Name = ExportType.json.ToString() } };                            
            var tabVM = new GameDetailsTabViewModel(tabItems, exportTypes);

            return tabVM;
        }

        public GameDetailsTabViewModel GetGameChartTabs(int gameID)
        {
            var gamevw = _gameRepo.GetGameViews(i => i.ID == gameID).FirstOrDefault();
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID && i.Rank == 1).ToList();
            var tabItems = new List<GameTabViewModel>() { new GameTabViewModel(gamevw, runs, true) };
            var tabVM = new GameDetailsTabViewModel(tabItems);

            return tabVM;
        }

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            return _gameRepo.SearchGames(searchText);
        }
    }
}

