using System;
using System.Collections.Generic;
using NPoco;
using Serilog;
using NPoco.Extensions;
using System.Linq;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace SpeedRunApp.Repository
{
    public class SpeedRunRespository : BaseRepository, ISpeedRunRepository
    {
        public IEnumerable<SpeedRunSummaryView> GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = IsMySQL ? db.Query<SpeedRunSummaryView>("CALL GetLatestSpeedRuns (@0, @1, @2);", category, topAmount, orderValueOffset).ToList() :
                                        db.Query<SpeedRunSummaryView>("EXEC dbo.GetLatestSpeedRuns @0, @1, @2", category, topAmount, orderValueOffset).ToList();

                return results;
            }
        }

        public IEnumerable<IDNamePair> RunStatusTypes()
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<IDNamePair>("SELECT ID, Name FROM tbl_RunStatusType;").ToList();
            }
        }
        
        public IEnumerable<SpeedRunListCategory> SpeedRunListCategories(Expression<Func<SpeedRunListCategory, bool>> predicate = null)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunListCategory>().Where(predicate ?? (x => true)).ToList();
                return results;
            }
        }
        
        public IEnumerable<SpeedRunGridView> GetSpeedRunGridViews(Expression<Func<SpeedRunGridView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunGridView>().Where(predicate).ToList();
                return results;
            }
        }

        public IEnumerable<SpeedRunGridTabView> GetSpeedRunGridTabViews(Expression<Func<SpeedRunGridTabView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunGridTabView>().Where(predicate).ToList();
                return results;
            }
        }

        public IEnumerable<SpeedRunGridTabUserView> GetSpeedRunGridTabUserViews(Expression<Func<SpeedRunGridTabUserView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunGridTabUserView>().Where(predicate).ToList();
                return results;
            }
        }

        public IEnumerable<SpeedRunGridView> GetSpeedRunGridViewsByUserID(int gameID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int userID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = IsMySQL ? db.Query<SpeedRunGridView>("CALL GetSpeedRunsByUserID (@0, @1, @2, @3, @4);", gameID, categoryID, levelID, subCategoryVariableValueIDs, userID).ToList() :
                                        db.Query<SpeedRunGridView>("EXEC dbo.GetSpeedRunsByUserID @0, @1, @2, @3, @4", gameID, categoryID, levelID, subCategoryVariableValueIDs, userID).ToList();

                return results;
            }
        }

        public IEnumerable<SpeedRunGridView> GetPersonalBestsByUserID(int gameID, int categoryID, int? levelID, int userID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = IsMySQL ? db.Query<SpeedRunGridView>("CALL GetPersonalBestsByUserID (@0, @1, @2, @3);", gameID, categoryID, levelID, userID).ToList() :
                                        db.Query<SpeedRunGridView>("EXEC dbo.GetPersonalBestsByUserID @0, @1, @2, @3", gameID, categoryID, levelID, userID).ToList();

                return results;
            }
        }

        public IEnumerable<SpeedRunView> GetSpeedRunViews(Expression<Func<SpeedRunView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SpeedRunView>().Where(predicate).ToList();
            }
        }

        public IEnumerable<SpeedRunSummaryView> GetSpeedRunSummaryViews(Expression<Func<SpeedRunSummaryView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SpeedRunSummaryView>().Where(predicate).ToList();
            }
        }

        public int? GetSpeedRunID(string speedRunComID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<int?>("SELECT SpeedRunID FROM tbl_SpeedRun_SpeedRunComID WHERE SpeedRunComID = @0;", speedRunComID).FirstOrDefault();
            }
        }        
    }
}
