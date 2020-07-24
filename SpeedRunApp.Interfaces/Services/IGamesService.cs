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
        SpeedRunGridViewModel GetGameSpeedRunGrid(string gameID);
        IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, bool includeExaminer = false);
        IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(Game game, bool includeExaminer = false);
        IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID, bool includeExaminer = false);
        Game GetGame(string gameID);
    }
}
