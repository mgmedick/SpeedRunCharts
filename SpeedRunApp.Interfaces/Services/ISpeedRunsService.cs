using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Entity;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset);
        EditSpeedRunViewModel GetEditSpeedRun(string runID, string gameID, bool isReadOnly);
        IEnumerable<SearchResult> SearchGamesAndUsers(string searchText);
        IEnumerable<SpeedRunViewModel> GetSpeedRunsByGameID(string gameID);
    }
}



