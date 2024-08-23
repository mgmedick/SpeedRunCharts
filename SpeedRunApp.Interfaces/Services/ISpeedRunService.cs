using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunService
    {
        IEnumerable<SummaryList> GetSummaryLists(int currUserID);
        IEnumerable<SpeedRunSummaryViewModel> GetSummaryListResults(int summaryListID, int topAmount, int? orderValueOffset, int? categoryTypeID);
        IEnumerable<SpeedRunGridViewModel> GetLeaderboardGridData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, bool showAllData);
        IEnumerable<SpeedRunGridViewModel> GetWorldRecordGridData(int gameID, int categoryTypeID, int? categoryID, int? levelID);
        IEnumerable<SpeedRunGridViewModel> GetGameSummaryChartData(int gameID, int categoryTypeID);  
        IEnumerable<SpeedRunGridViewModel> GetPlayerSpeedRunGridData(int playerID);
        IEnumerable<SpeedRunGridViewModel> GetPlayerSpeedRunChartData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int playerID);  
    }
}



