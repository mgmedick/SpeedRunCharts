using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface ISpeedRunRepository
    {
        IEnumerable<SpeedRunView> GetLatestSpeedRuns(SpeedRunListCategory1 category, int topAmount, int? orderValueOffset);
        SpeedRunView GetSpeedRunView(string speedRunID);
        IEnumerable<IDNamePair> RunStatusTypes();
    }
}






