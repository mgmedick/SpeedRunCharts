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
        IEnumerable<SpeedRunSummaryView> GetSummaryListResults(int category, int topAmount, int? orderValueOffset, int? categoryTypeID);
        IEnumerable<SummaryList> GetSummaryLists(Expression<Func<SummaryList, bool>> predicate = null);
        IEnumerable<SpeedRunGridView> GetSpeedRunGridViews(Expression<Func<SpeedRunGridView, bool>> predicate);
        IEnumerable<SpeedRun> GetSpeedRuns(Expression<Func<SpeedRun, bool>> predicate);
        IEnumerable<SpeedRunGridPlayerView> GetSpeedRunGridPlayerViews(Expression<Func<SpeedRunGridPlayerView, bool>> predicate);
        IEnumerable<SpeedRunSummaryView> GetSpeedRunSummaryViews(Expression<Func<SpeedRunSummaryView, bool>> predicate);
    }
}






