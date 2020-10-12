using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory category, int elementsPerPage, int? elementsOffset);
        EditSpeedRunViewModel GetEditSpeedRun(string runID, string gameID, bool isReadOnly);
        SpeedRunViewModel GetSpeedRun(string runID, SpeedRunEmbeds runEmbeds = null);
        IEnumerable<SpeedRunViewModel> GetSpeedRuns(RunStatusType status, RunsOrdering orderBy, int elementsPerPage, int? elementsOffset, SpeedRunEmbeds runEmbeds = null);
        IEnumerable<SearchResult> SearchGamesAndUsers(string term);
        IEnumerable<SearchResult> SearchGames(string term);
        IEnumerable<SearchResult> SearchUsers(string term);
    }
}



