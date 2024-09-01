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
        public IEnumerable<SpeedRunSummaryView> GetSummaryListResults(int summaryListID, int topAmount, int? orderValueOffset, int? categoryTypeID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunSummaryView>("CALL GetSummaryListResults (@0, @1, @2, @3);", summaryListID, topAmount, orderValueOffset, categoryTypeID).ToList();

                return results;
            }
        }
        
        public IEnumerable<SummaryList> GetSummaryLists(Expression<Func<SummaryList, bool>> predicate = null)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SummaryList>().Where(predicate ?? (x => true)).ToList();
                return results;
            }
        }
        
        public IEnumerable<SpeedRun> GetSpeedRuns(Expression<Func<SpeedRun, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRun>().Where(predicate).ToList();
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

        public IEnumerable<SpeedRunGridPlayerView> GetSpeedRunGridPlayerViews(Expression<Func<SpeedRunGridPlayerView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunGridPlayerView>().Where(predicate).ToList();
                return results;
            }
        }

        public IEnumerable<SpeedRunSummaryView> GetSpeedRunSummaryViews(Expression<Func<SpeedRunSummaryView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SpeedRunSummaryView>().Where(predicate).ToList();
            }
        }
        
        public IEnumerable<SpeedRunDetailView> GetSpeedRunDetailViews(Expression<Func<SpeedRunDetailView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunDetailView>().Where(predicate).ToList();
                return results;
            }
        }                  
    }
}
