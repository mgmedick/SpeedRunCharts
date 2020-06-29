using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService
    {
        SpeedRunListViewModel GetLatestSpeedRuns(int? elementsOffset = null);
        SpeedRunViewModel GetSpeedRun(string runId, SpeedRunEmbeds embeds = default(SpeedRunEmbeds));
    }
}
