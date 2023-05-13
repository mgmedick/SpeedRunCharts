using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunListCategory> GetSpeedRunListCategories(int currUserAccountID);
        IEnumerable<SpeedRunSummaryViewModel> GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset, int? categoryTypeID);
        SpeedRunSummaryViewModel GetSpeedRunSummary(int speedRunID);
        IEnumerable<SpeedRunGridViewModel> GetLeaderboardGridData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData);
        IEnumerable<WorldRecordGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID);
        IEnumerable<SpeedRunChartViewModel> GetGameSummaryChartData(int gameID, int categoryTypeID);  
        IEnumerable<SpeedRunChartViewModel> GetLeaderboardChartData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs);     
        IEnumerable<SpeedRunGridUserViewModel> GetUserSpeedRunGridData(int userID);
        IEnumerable<SpeedRunChartViewModel> GetUserSpeedRunChartData(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int userID);  
        IEnumerable<SpeedRunChartViewModel> GetUserSummaryChartData(int userID);              
        ImportStatusViewModel GetImportStatus();
    }
}



