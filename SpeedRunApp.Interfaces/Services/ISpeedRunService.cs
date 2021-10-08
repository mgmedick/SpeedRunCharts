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
        //IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID);
        //IEnumerable<SpeedRunGridViewModel> GetPersonalBestGridData(int userID, int categoryTypeID);
        IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID, int categoryID, int? userID);
        IEnumerable<SpeedRunGridViewModel> GetSpeedRunGridData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int? userID);
    }
}



