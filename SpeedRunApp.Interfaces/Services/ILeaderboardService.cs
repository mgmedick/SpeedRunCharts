using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ILeaderboardService
    {
        IEnumerable<SpeedRunRecordViewModel> GetLeaderboardRecords(string gameID, CategoryType categoryType, string categoryID, string levelID = null);
    }
}
