using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedrunComSharp.Model;
using SpeedRunCommon;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService
    {
        IEnumerable<SpeedRunDTO> GetLatestSpeedRuns(int? elementsOffset = null);
        IEnumerable<SpeedRunDTO> GetSpeedRuns(string userId = null, string guestName = null, string examerUserId = null, string gameId = null, string levelId = null, string categoryId = null, string platformId = null, string regionId = null, bool onlyEmulatedRuns = false, RunStatusType? status = null, int? elementsPerPage = null, RunEmbeds embeds = default(RunEmbeds), RunsOrdering orderBy = default(RunsOrdering), int? elementsOffset = null);
        SpeedRunDTO GetSpeedRun(string runId, RunEmbeds embeds = default(RunEmbeds));
    }
}
