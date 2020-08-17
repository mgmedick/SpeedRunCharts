using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserService
    {
        UserDetailsViewModel GetUserDetails(string userID);
        IEnumerable<SpeedRunViewModel> GetUserSpeedRuns(string userID, int elementsPerPage, int elementsOffset);
        //IEnumerable<SpeedRun> GetUserSpeedRuns(string userID);
        //IEnumerable<SpeedRunViewModel> GetUserSpeedRuns(string userID, string gameID, CategoryType categoryType, string categoryID, string levelID, DateTime? startDate, DateTime? endDate);
        //SpeedRunGridViewModel SearchUserSpeedRunGrid(string userID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels);
        //SpeedRunGridViewModel GetUserSpeedRunGrid(IEnumerable<SpeedRun> speedRuns);
    }
}
