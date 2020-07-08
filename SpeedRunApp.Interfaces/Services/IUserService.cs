using System;
using System.Collections.Generic;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model.Data;
using SpeedRunCommon;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserService
    {
        UserDetailsViewModel GetUser(string userID);
        IEnumerable<SpeedRun> GetUserSpeedRuns(string userID);
        IEnumerable<SpeedRunViewModel> GetUserSpeedRuns(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID);
        SpeedRunGridViewModel SearchUserSpeedRunGrid(string userID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels, string sender);
    }
}
