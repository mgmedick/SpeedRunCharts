using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface ISpeedRunRepository
    {
        IEnumerable<SpeedRunView> GetLatestSpeedRuns(SpeedRunListCategory category, int topAmount, int? orderValueOffset);
        SpeedRunView GetSpeedRunView(string speedRunID);
        IEnumerable<IDNamePair> RunStatusTypes();
        IEnumerable<SpeedRunView> GetSpeedRuns(Expression<Func<SpeedRunView, bool>> predicate);
    }
}






