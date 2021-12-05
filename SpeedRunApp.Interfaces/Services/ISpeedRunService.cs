using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunSummaryViewModel> GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset);
        EditSpeedRunViewModel GetEditSpeedRun(int gamID, int? speedRunID);
        SpeedRunSummaryViewModel GetSpeedRunSummary(int speedRunID);
        IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID, int categoryID, int? levelID, int? userID);
        IEnumerable<SpeedRunGridViewModel> GetSpeedRunGridData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int? userID);
        IEnumerable<SpeedRunListCategory> GetSpeedRunListCategories(int currUserAccountID);
    }
}



