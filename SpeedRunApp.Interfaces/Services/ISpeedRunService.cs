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
        IEnumerable<SpeedRunGridViewModel> GetLeaderboardGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData);
        IEnumerable<WorldRecordGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID);
        IEnumerable<SpeedRunGridViewModel> GetUserSpeedRunGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int userID);        
        IEnumerable<WorldRecordGridViewModel> GetPersonalBestGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID, int userID);
        IEnumerable<SpeedRunListCategory> GetSpeedRunListCategories(int currUserAccountID);
        ImportStatusViewModel GetImportStatus();
    }
}



