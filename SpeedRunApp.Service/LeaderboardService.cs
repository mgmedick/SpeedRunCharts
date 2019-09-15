using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;
using SpeedrunComSharp.Client;

namespace SpeedRunApp.Service
{
    public class LeaderboardService
    {
        public IEnumerable<SpeedRunRecordDTO> GetLeaderboardRecordsForCategory(string gameID, string categoryID, int? elementsOffset = null)
        {
            ClientContainer clientContainer = new ClientContainer();
            var leaderboard = clientContainer.Leaderboards.GetLeaderboardForFullGameCategory(gameId: gameID, categoryId: categoryID);
            return leaderboard.Records.Select(i => new SpeedRunRecordDTO(i));
        }
    }
}
