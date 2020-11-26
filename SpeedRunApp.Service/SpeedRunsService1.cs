using SpeedRunApp.Client;
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
    public class SpeedRunsService1 : ISpeedRunsService
    {
        private readonly IConfiguration _config = null;
        private readonly ICacheHelper _cacheHelper = null;
        private readonly IGamesService _gamesService = null;

        public SpeedRunsService1(IConfiguration config, ICacheHelper cacheHelper, IGamesService gamesService)
        {
            _config = config;
            _cacheHelper = cacheHelper;
        }

        public IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory category, int elementsPerPage, int? elementsOffset)
        {
            IEnumerable<SpeedRunViewModel> runVMs = null;
            switch (category)
            {
                case SpeedRunListCategory.New:
                    runVMs = GetSpeedRuns(RunStatusType.New, RunsOrdering.DateSubmittedDescending, elementsPerPage, elementsOffset);
                    break;
                case SpeedRunListCategory.Verified:
                    runVMs = GetSpeedRuns(RunStatusType.Verified, RunsOrdering.VerifyDateDescending, elementsPerPage, elementsOffset);
                    break;
                case SpeedRunListCategory.Rejected:
                    runVMs = GetSpeedRuns(RunStatusType.Rejected, RunsOrdering.DateSubmittedDescending, elementsPerPage, elementsOffset);
                    break;
            }

            return runVMs;
        }

        public IEnumerable<SpeedRunViewModel> GetNewSpeedRuns(int elementsPerPage, int? elementsOffset)
        {

        }
    }
}
