using SpeedRunApp.Client;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Configuration;

namespace SpeedRunApp.Service
{
    public class SpeedRunsService1 : ISpeedRunsService1
    {
        private readonly IConfiguration _config = null;
        private readonly ICacheHelper _cacheHelper = null;
        private readonly IGamesService1 _gamesService = null;
        private readonly ISpeedRunRepository _speedRunRepo = null;

        public SpeedRunsService1(IConfiguration config, ICacheHelper cacheHelper, IGamesService1 gamesService, ISpeedRunRepository speedRunRepo)
        {
            _config = config;
            _cacheHelper = cacheHelper;
            _gamesService = gamesService;
            _speedRunRepo = speedRunRepo;
        }

        public SpeedRunListViewModel1 GetSpeedRunList()
        {
            var loadDateString = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");
            var elementsPerPage = Convert.ToInt32(_config.GetSection("ApiSettings").GetSection("SpeedRunListElementsPerPage").Value);

            var runListVM = new SpeedRunListViewModel1(elementsPerPage, loadDateString);

            return runListVM;
        }

        public IEnumerable<SpeedRunViewModel1> GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset)
        {
            var runs = _speedRunRepo.GetLatestSpeedRuns(category, topAmount, orderValueOffset);
            IEnumerable<SpeedRunViewModel1> runVMs = runs.Select(i => new SpeedRunViewModel1(i));

            return runVMs;
        }

        public EditSpeedRunViewModel1 GetEditSpeedRun(string runID, string gameID, bool isReadOnly)
        {
            var run = _speedRunRepo.GetSpeedRunView(runID);
            var runVM = new SpeedRunViewModel1(run);

            var gameDetails = _gamesService.GetGameDetails(gameID);
            var statusTypes = _speedRunRepo.RunStatusTypes();

            var editSpeedRunVM = new EditSpeedRunViewModel1(statusTypes, gameDetails.CategoryTypes, gameDetails.Categories, gameDetails.Levels, gameDetails.Platforms, gameDetails.Variables, runVM, isReadOnly);

            return editSpeedRunVM;
        }
    }
}
