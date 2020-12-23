﻿using System;
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
        public IEnumerable<SpeedRunView> GetLatestSpeedRuns(SpeedRunListCategory category, int topAmount, int? orderValueOffset)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SpeedRunView>("EXEC dbo.GetLatestSpeedRuns @0, @1, @2", (int)category, topAmount, orderValueOffset).ToList();
            }
        }

        public IEnumerable<IDNamePair> RunStatusTypes()
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<IDNamePair>("SELECT ID, Name FROM dbo.tbl_RunStatusType").ToList();
            }
        }

        public IEnumerable<SpeedRunView> GetSpeedRunViews(Expression<Func<SpeedRunView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SpeedRunView>().Where(predicate).ToList();
            }
        }

        public IEnumerable<SpeedRunView> GetSpeedRunsByUserID(string userID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SpeedRunView>("EXEC dbo.GetSpeedRunsByUserID @0", userID).ToList();
            }
        }
    }
}