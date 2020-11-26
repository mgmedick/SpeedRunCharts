using System;
using System.Collections.Generic;
using NPoco;
using Serilog;
using NPoco.Extensions;
using System.Linq;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Entity;
using SpeedRunApp.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace SpeedRunApp.Repository
{
    public class SpeedRunRespository : BaseRepository, ISpeedRunRepository
    {
        private readonly ILogger _logger;

        public SpeedRunRespository(ILogger logger)
        {
            _logger = logger;
        }

        public IEnumerable<SpeedRunView> GetTop5PercentSpeedRuns()
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SpeedRunEntity>().Where(predicate).ToList();
            }
        }
    }
}
