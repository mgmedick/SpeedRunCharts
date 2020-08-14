using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService
    {
        SpeedRunListViewModel GetLatestSpeedRuns(int? elementsOffset = null);
        SpeedRunViewModel GetSpeedRun(string runID);
        IEnumerable<IDNamePair> SearchGamesAndUsers(string term);
    }
}



