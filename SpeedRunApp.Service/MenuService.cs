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
    public class MenuService : IMenuService
    {
        private readonly IGameService _gamesService = null;
        private readonly IPlayerService _playerService = null;
        private readonly ISettingRepository _settingRepo = null;

        public MenuService(IGameService gamesService, IPlayerService playerService, ISettingRepository settingRepo)
        {
            _gamesService = gamesService;
            _playerService = playerService;
            _settingRepo = settingRepo;
        }

        public IEnumerable<SearchResult> Search(string searchText)
        {
            var results = new List<SearchResult>();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                searchText = searchText.Trim();
                var games = _gamesService.SearchGames(searchText).ToList();
                if (games.Any()) {
                    var gamesGroup = new SearchResult { Value = "0", Label = "Games", SubItems = games };
                    results.Add(gamesGroup);
                }
                
                var players = _playerService.SearchPlayers(searchText);
                if (players.Any()){
                    var playersGroup = new SearchResult { Value = "0", Label = "Players", SubItems = players };
                    results.Add(playersGroup);
                }
            }

            return results;
        }

        public ImportStatusViewModel GetImportStatus()
        {
            var importSettings = new List<string>() { "ImporvtLastRunDate", "ImportLastUpdateSpeedRunsDate", "ImportLastBulkReloadDate" };
            var results = _settingRepo.GetSettings(i => importSettings.Contains(i.Name)).ToList();
            var ImportLastRunDate = results.FirstOrDefault(i => i.Name == "ImportLastRunDate")?.Dte;
            var ImportLastUpdateSpeedRunsDate = results.FirstOrDefault(i => i.Name == "ImportLastUpdateSpeedRunsDate")?.Dte;
            var ImportLastBulkReloadDate = results.FirstOrDefault(i => i.Name == "ImportLastBulkReloadDate")?.Dte;

            var importStatusVM = new ImportStatusViewModel(ImportLastRunDate, ImportLastUpdateSpeedRunsDate, ImportLastBulkReloadDate);

            return importStatusVM;
        }           
    }
}
