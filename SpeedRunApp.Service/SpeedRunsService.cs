using Microsoft.Extensions.Configuration;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeedRunApp.Service
{
    public class SpeedRunsService : ISpeedRunsService
    {
        private readonly IConfiguration _config = null;
        private readonly IGamesService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;

        public SpeedRunsService(IConfiguration config, IGamesService gamesService, IUserService userService, ISpeedRunRepository speedRunRepo)
        {
            _config = config;
            _gamesService = gamesService;
            _userService = userService;
            _speedRunRepo = speedRunRepo;
        }

        public SpeedRunListViewModel GetSpeedRunList()
        {
            var elementsPerPage = Convert.ToInt32(_config.GetSection("ApiSettings").GetSection("SpeedRunListElementsPerPage").Value);

            var runListVM = new SpeedRunListViewModel(elementsPerPage);

            return runListVM;
        }

        public IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset)
        {
            var runs = _speedRunRepo.GetLatestSpeedRuns(category, topAmount, orderValueOffset);
            IEnumerable<SpeedRunViewModel> runVMs = runs.Select(i => new SpeedRunViewModel(i));

            return runVMs;
        }

        public EditSpeedRunViewModel GetEditSpeedRun(string runID, string gameID, bool isReadOnly)
        {
            var run = _speedRunRepo.GetSpeedRunView(runID);
            var runVM = new SpeedRunViewModel(run);

            var gameDetails = _gamesService.GetGameDetails(gameID);
            var statusTypes = _speedRunRepo.RunStatusTypes();

            var editSpeedRunVM = new EditSpeedRunViewModel(statusTypes, gameDetails.CategoryTypes, gameDetails.Categories, gameDetails.Levels, gameDetails.Platforms, gameDetails.Variables, runVM, isReadOnly);

            return editSpeedRunVM;
        }

        public IEnumerable<SearchResult> SearchGamesAndUsers(string searchText)
        {
            var games = _gamesService.SearchGames(searchText);
            var users = _userService.SearchUsers(searchText);
            var results = games.Concat(users);

            return results;
        }

        public IEnumerable<SpeedRunViewModel> GetSpeedRunsByGameID(string gameID)
        {
            var runs = _speedRunRepo.GetSpeedRuns(i => i.GameID == gameID);
            var runVMs = runs.Select(i => new SpeedRunViewModel(i)).ToList();

            return runVMs;
        }
    }
}
