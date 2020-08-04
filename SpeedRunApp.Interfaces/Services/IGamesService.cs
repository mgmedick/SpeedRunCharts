using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGamesService
    {
        GameDetailsViewModel GetGameDetails(string gameID);
        IEnumerable<SpeedRunRecordViewModel> GetGameSpeedRunRecords(string gameID, CategoryType categoryType, string categoryID, string levelID, IEnumerable<IDNamePair> moderators);
        Game GetGame(string gameID);
    }
}
