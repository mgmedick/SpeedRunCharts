using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunSummaryViewModel> GetLatestSpeedRuns(SpeedRunListCategory category, int topAmount, int? orderValueOffset);
        EditSpeedRunViewModel GetEditSpeedRun(int gamID, int? speedRunID);
        SpeedRunSummaryViewModel GetSpeedRunSummary(int speedRunID);
    }
}



