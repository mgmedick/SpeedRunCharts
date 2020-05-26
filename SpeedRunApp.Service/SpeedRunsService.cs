using System.Collections.Generic;
using System.Linq;
using SpeedRunApp.Model;
using SpeedrunComSharp.Client;
using SpeedrunComSharp.Model;
using SpeedRunCommon;
using SpeedRunApp.Interfaces.Services;

namespace SpeedRunApp.Service
{
    public class SpeedRunsService : ISpeedRunsService
    {
        public SpeedRunsService()
        {

        }

        public IEnumerable<SpeedRunDTO> GetLatestSpeedRuns(int? elementsOffset = null)
        {
            var runEmbeds = new RunEmbeds { EmbedGame = true, EmbedPlayers = true, EmbedCategory = true, EmbedLevel = true, EmbedPlatform = true };
            var runs = GetSpeedRuns(status: RunStatusType.New, orderBy: RunsOrdering.DateSubmittedDescending, elementsPerPage: 10, embeds: runEmbeds, elementsOffset: elementsOffset); ; ;

            return runs;
        }

        public IEnumerable<SpeedRunDTO> GetSpeedRuns(
            string userId = null, string guestName = null,
            string examerUserId = null, string gameId = null,
            string levelId = null, string categoryId = null,
            string platformId = null, string regionId = null,
            bool onlyEmulatedRuns = false, RunStatusType? status = null,
            int? elementsPerPage = null,
            RunEmbeds embeds = default(RunEmbeds),
            RunsOrdering orderBy = default(RunsOrdering),
            int? elementsOffset = null)
        {
            ClientContainer clientContainer = new ClientContainer();
            var clientRuns = clientContainer.Runs.GetRuns(userId, guestName, examerUserId, gameId, levelId, categoryId, platformId, regionId, onlyEmulatedRuns,
                                                    status, elementsPerPage, embeds, orderBy, elementsOffset);
            var runs = clientRuns.Select(i => new SpeedRunDTO(i));

            return runs;
        }

        public SpeedRunDTO GetSpeedRun(string runId, RunEmbeds embeds = default(RunEmbeds))
        {
            ClientContainer clientContainer = new ClientContainer();
            var clientRun = clientContainer.Runs.GetRun(runId, embeds);

            return new SpeedRunDTO(clientRun);
        }
    }
}
