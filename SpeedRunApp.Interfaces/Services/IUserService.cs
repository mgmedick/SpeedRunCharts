using System;
using System.Collections.Generic;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model.Data;
using SpeedRunCommon;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserService
    {
        UserDetailsViewModel GetUserDetails(string userID);
        User GetUser(string userID);
        IEnumerable<SpeedRun> GetUserSpeedRuns(string userID);
        IEnumerable<SpeedRunViewModel> GetUserSpeedRuns(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID, DateTime? startDate, DateTime? endDate);
        SpeedRunGridViewModel SearchUserSpeedRunGrid(string userID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels);
        SpeedRunGridViewModel GetUserSpeedRunGrid(IEnumerable<SpeedRun> speedRuns);
    }
}
