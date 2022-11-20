using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunSummaryViewModel> GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset, int? categoryTypeID);
        EditSpeedRunViewModel GetEditSpeedRun(int gamID, int? speedRunID);
        SpeedRunSummaryViewModel GetSpeedRunSummary(int speedRunID);
        IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryID, int? levelID, int? userID);
        IEnumerable<SpeedRunGridViewModel> GetSpeedRunGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int? userID, bool showAllData);
        IEnumerable<SpeedRunListCategory> GetSpeedRunListCategories(int currUserAccountID);
        ImportStatusViewModel GetImportStatus();
    }
}



