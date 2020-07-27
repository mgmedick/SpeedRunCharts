using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService
    {
        SpeedRunListViewModel GetLatestSpeedRuns(int? elementsOffset = null);
        SpeedRunViewModel GetSpeedRun(string runID);
    }
}
