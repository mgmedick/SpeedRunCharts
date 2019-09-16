using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;
using SpeedrunComSharp.Client;

namespace SpeedRunApp.Service
{
    public class LeaderboardService
    {
        public IEnumerable<SpeedRunRecordDTO> GetLeaderboardRecordsForCategory(string gameID, string categoryID, string levelID = null)
        {
            ClientContainer clientContainer = new ClientContainer();
            var leaderboard = string.IsNullOrWhiteSpace(levelID) ? clientContainer.Leaderboards.GetLeaderboardForFullGameCategory(gameId: gameID, categoryId: categoryID) :
                                                                   clientContainer.Leaderboards.GetLeaderboardForLevel(gameId: gameID, levelId: levelID, categoryId: categoryID);

            return leaderboard.Records.Select(i => new SpeedRunRecordDTO(i));
        }
    }
}
