using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;
using SpeedrunComSharp.Client;
using SpeedrunComSharp.Model;

namespace SpeedRunApp.Service
{
    public class LeaderboardService
    {
        public IEnumerable<SpeedRunRecordDTO> GetLeaderboardRecordsForCategory(string gameID, string categoryID, string levelID = null)
        {
            ClientContainer clientContainer = new ClientContainer();

            var leaderboard = string.IsNullOrWhiteSpace(levelID) ? clientContainer.Leaderboards.GetLeaderboardForFullGameCategory(gameId: gameID, categoryId: categoryID, embeds: new LeaderboardEmbeds(true, false, false, true, false, false,false)):
                                                                   clientContainer.Leaderboards.GetLeaderboardForLevel(gameId: gameID, levelId: levelID, categoryId: categoryID, embeds: new LeaderboardEmbeds(true, false, false, true, false, false, false));

            return leaderboard.Records.Select(i => new SpeedRunRecordDTO(i));
        }
    }
}
