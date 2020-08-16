using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService
    {
        SpeedRunListViewModel GetLatestSpeedRuns(RunStatusType status, int? elementsOffset);
        SpeedRunViewModel GetSpeedRun(string runID);
        IEnumerable<SearchResult> SearchGamesAndUsers(string term);
    }
}



