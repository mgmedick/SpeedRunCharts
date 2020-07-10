using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGamesService
    {
        GameDetailsViewModel GetGameDetails(string gameID);
        IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID, DateTime? startDate, DateTime? endDate);
        SpeedRunGridViewModel SearchGameSpeedRunGrid(string gameID, List<string> drpCategoryTypes, List<string> drpGames, List<string> drpCategories, List<string> drpLevels);
        SpeedRunGridViewModel GetGameSpeedRunGrid(Game game);
        Game GetGame(string gameID);
    }
}
