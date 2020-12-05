using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISpeedRunsService1
    {
        SpeedRunListViewModel1 GetSpeedRunList();
        IEnumerable<SpeedRunViewModel1> GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset);
        EditSpeedRunViewModel1 GetEditSpeedRun(string runID, string gameID, bool isReadOnly);
    }
}



