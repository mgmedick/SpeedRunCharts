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
        public IEnumerable<SpeedRunSummaryView> GetLatestSpeedRuns(int category, int topAmount, int? orderValueOffset, int? categoryTypeID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunSummaryView>("CALL GetLatestSpeedRuns (@0, @1, @2, @3);", category, topAmount, orderValueOffset, categoryTypeID).ToList();

                return results;
            }
        }
        
        public IEnumerable<SpeedRunListCategory> GetSpeedRunListCategories(Expression<Func<SpeedRunListCategory, bool>> predicate = null)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunListCategory>().Where(predicate ?? (x => true)).ToList();
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

        public IEnumerable<SpeedRunGridView> GetSpeedRunGridViews(Expression<Func<SpeedRunGridView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunGridView>().Where(predicate).ToList();
                return results;
            }
        }

        public IEnumerable<SpeedRunChartView> GetSpeedRunChartViews(Expression<Func<SpeedRunChartView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunChartView>().Where(predicate).ToList();
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

        public IEnumerable<SpeedRunChartUserView> GetSpeedRunChartUserViews(Expression<Func<SpeedRunChartUserView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SpeedRunChartUserView>().Where(predicate).ToList();
                return results;
            }
        }           

        public int? GetSpeedRunID(string speedRunComID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<int?>("SELECT Id FROM tbl_SpeedRun WHERE Code = @0;", speedRunComID).FirstOrDefault();
            }
        }               
    }
}
