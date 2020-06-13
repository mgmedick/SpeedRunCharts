using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ILeaderboardService
    {
        IEnumerable<SpeedRunRecordDTO> GetLeaderboardRecords(string gameID, CategoryType categoryType, string categoryID, string levelID = null);
    }
}
