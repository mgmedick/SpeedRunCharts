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
        IEnumerable<WorldRecordGridViewModel> GetGameWorldRecordGridData(int gameID, string categoryIDs, string levelIDs, string subCategoryVariableValueIDs);
        // IEnumerable<WorldRecordGridViewModel> GetGameWorldRecordGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs);
        IEnumerable<WorldRecordGridViewModel> GetUserPersonalBestGridData(int gameID, int categoryID, int? levelID, int userID);
        IEnumerable<SpeedRunGridViewModel> GetGameSpeedRunGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData);
        IEnumerable<SpeedRunGridViewModel> GetUserSpeedRunGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int userID);
        IEnumerable<SpeedRunListCategory> GetSpeedRunListCategories(int currUserAccountID);
        ImportStatusViewModel GetImportStatus();
    }
}



