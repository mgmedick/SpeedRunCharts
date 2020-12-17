using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunViewModel> GetLatestSpeedRuns(SpeedRunListCategory category, int topAmount, int? orderValueOffset);
        EditSpeedRunViewModel GetEditSpeedRun(string gameID, string speedRunID, bool isReadOnly);
        IEnumerable<SpeedRunViewModel> GetSpeedRunsByGameID(string gameID);
        IEnumerable<SpeedRunViewModel> GetSpeedRunsByUserID(string userID);
    }
}



