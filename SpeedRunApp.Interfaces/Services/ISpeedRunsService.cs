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
        EditSpeedRunViewModel GetEditSpeedRun(string runID, string gameID, bool isReadOnly);
        //IEnumerable<SpeedRunViewModel> GetLeaderboards(IEnumerable<SpeedRunGridItem> gridItems);
        IEnumerable<SpeedRunView> GetSpeedRunRecordsByGameID(string gameID);
    }
}



