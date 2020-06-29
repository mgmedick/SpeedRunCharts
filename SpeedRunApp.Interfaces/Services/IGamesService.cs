using System;
using System.Collections.Generic;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGamesService
    {
        GameDetailsViewModel GetGame(string gameID);
        IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID);
    }
}
