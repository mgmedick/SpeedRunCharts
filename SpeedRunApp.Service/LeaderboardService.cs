using System;
using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;
using SpeedrunComSharp.Client;
using SpeedrunComSharp.Model;
using SpeedRunCommon;
using SpeedRunApp.Interfaces.Services;

namespace SpeedRunApp.Service
{
    public class LeaderboardService : ILeaderboardService
    {
        public IEnumerable<SpeedRunRecordViewModel> GetLeaderboardRecords(string gameID, CategoryType categoryType, string categoryID, string levelID = null)
        {
            Leaderboard leaderboard = null;
            ClientContainer clientContainer = new ClientContainer();
            List<SpeedRunRecordDTO> records = new List<SpeedRunRecordDTO>();
            var leaderboardEmbeds = new LeaderboardEmbeds { EmbedGame = true, EmbedCategory = true, EmbedLevel = true, EmbedPlayers = true, EmbedRegions = false, EmbedPlatforms = true, EmbedVariables = false };

            if (categoryType == CategoryType.PerGame)
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForFullGameCategory(gameId: gameID, categoryId: categoryID, embeds: leaderboardEmbeds);
            }
            else
            {
                leaderboard = clientContainer.Leaderboards.GetLeaderboardForLevel(gameId: gameID, levelId: levelID, categoryId: categoryID, embeds: leaderboardEmbeds);
            }

            var dtoRecords = leaderboard.Records.Select(i => new SpeedRunRecordDTO(i));
            var recordVMs = dtoRecords.Select(i => new SpeedRunRecordViewModel(i)).OrderBy(i => i.PrimaryRunTime);

            return recordVMs;
        }
    }
}
