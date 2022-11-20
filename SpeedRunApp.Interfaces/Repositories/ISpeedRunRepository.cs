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
        IEnumerable<SpeedRunSummaryView> GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset, int? categoryTypeID);
        IEnumerable<IDNamePair> RunStatusTypes();
        IEnumerable<SpeedRunListCategory> SpeedRunListCategories(Expression<Func<SpeedRunListCategory, bool>> predicate = null);
        IEnumerable<SpeedRunGridView> GetSpeedRunGridViews(Expression<Func<SpeedRunGridView, bool>> predicate);
        IEnumerable<SpeedRunGridTabView> GetSpeedRunGridTabViews(Expression<Func<SpeedRunGridTabView, bool>> predicate);
        IEnumerable<SpeedRunGridTabUserView> GetSpeedRunGridTabUserViews(Expression<Func<SpeedRunGridTabUserView, bool>> predicate);
        IEnumerable<SpeedRunGridView> GetSpeedRunGridViewsByUserID(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int userID);
        IEnumerable<SpeedRunGridView> GetPersonalBestsByUserID(int gameID, int categoryID, int? levelID, int userID);
        IEnumerable<SpeedRunView> GetSpeedRunViews(Expression<Func<SpeedRunView, bool>> predicate);
        IEnumerable<SpeedRunSummaryView> GetSpeedRunSummaryViews(Expression<Func<SpeedRunSummaryView, bool>> predicate);
        int? GetSpeedRunID(string speedRunComID);
    }
}






