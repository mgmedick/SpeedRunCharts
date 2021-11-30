﻿using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface ISpeedRunRepository
    {
        IEnumerable<SpeedRunSummaryView> GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset);
        IEnumerable<IDNamePair> RunStatusTypes();
        IEnumerable<SpeedRunListCategory> SpeedRunListCategories(Expression<Func<SpeedRunListCategory, bool>> predicate = null);
        IEnumerable<SpeedRunGridView> GetSpeedRunGridViews(Expression<Func<SpeedRunGridView, bool>> predicate);
        IEnumerable<SpeedRunGridView> GetSpeedRunGridViewsByUserID(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int userID);
        IEnumerable<SpeedRunGridView> GetPersonalBestsByUserID(int gameID, int categoryTypeID, int categoryID, int userID);
        IEnumerable<SpeedRunView> GetSpeedRunViews(Expression<Func<SpeedRunView, bool>> predicate);
        IEnumerable<SpeedRunSummaryView> GetSpeedRunSummaryViews(Expression<Func<SpeedRunSummaryView, bool>> predicate);
    }
}






