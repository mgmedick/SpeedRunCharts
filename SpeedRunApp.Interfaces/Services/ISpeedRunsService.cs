using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset);
        EditSpeedRunViewModel GetEditSpeedRun(string gameID, string speedRunID, bool isReadOnly);
        IEnumerable<SpeedRunViewModel> GetSpeedRunsByGameID(string gameID);
    }
}



