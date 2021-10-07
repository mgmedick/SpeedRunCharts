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
        IEnumerable<SpeedRunSummaryView> GetLatestSpeedRuns(SpeedRunListCategory category, int topAmount, int? orderValueOffset);
        IEnumerable<IDNamePair> RunStatusTypes();
        IEnumerable<SpeedRunGridView> GetSpeedRunGridViews(Expression<Func<SpeedRunGridView, bool>> predicate);
        IEnumerable<SpeedRunGridView> GetSpeedRunGridViewsByUserID(int gameID, int categoryTypeID, int categoryID, int? levelID, string variableValueIDs, int userID);
        IEnumerable<SpeedRunGridView> GetPersonalBestsByUserID(int userID, int categoryTypeID);
        IEnumerable<SpeedRunView> GetSpeedRunViews(Expression<Func<SpeedRunView, bool>> predicate);
        IEnumerable<SpeedRunSummaryView> GetSpeedRunSummaryViews(Expression<Func<SpeedRunSummaryView, bool>> predicate);
    }
}






