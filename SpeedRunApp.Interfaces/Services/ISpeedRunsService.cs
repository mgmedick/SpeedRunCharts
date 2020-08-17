using SpeedRunApp.Model;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory category, int elementsPerPage, int? elementsOffset);
        IEnumerable<SpeedRunViewModel> GetSpeedRuns(RunStatusType status, RunsOrdering orderBy, int elementsPerPage, int? elementsOffset);
        SpeedRunViewModel GetSpeedRun(string runID);
        IEnumerable<SearchResult> SearchGamesAndUsers(string term);
    }
}



