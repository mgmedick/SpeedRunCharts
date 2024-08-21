using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunService
    {
        SpeedRunListViewModel GetSpeedRunList();
        IEnumerable<SpeedRunSummaryList> GetSpeedRunSummaryLists(int currUserID);
        IEnumerable<SpeedRunSummaryViewModel> GetSpeedRunSummaryResults(int category, int topAmount, int? orderValueOffset, int? categoryTypeID);
        SpeedRunSummaryViewModel GetSpeedRunSummary(int speedRunID);
        IEnumerable<SpeedRunGridViewModel> GetLeaderboardGridData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData);
        IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID);
        IEnumerable<SpeedRunGridViewModel> GetGameSummaryChartData(int gameID, int categoryTypeID);  
        IEnumerable<SpeedRunGridViewModel> GetPlayerSpeedRunGridData(int playerID);
        IEnumerable<SpeedRunGridViewModel> GetPlayerSpeedRunChartData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int playerID);  
        ImportStatusViewModel GetImportStatus();
    }
}



