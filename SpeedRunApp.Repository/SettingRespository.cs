using System;
using System.Collections.Generic;
using NPoco;
using System.Linq;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Interfaces.Repositories;
using System.Linq.Expressions;
//using Microsoft.Extensions.Configuration;

namespace SpeedRunApp.Repository
{
    public class SettingRespository : BaseRepository, ISettingRepository
    {
        public SettingRespository()
        {
        }

        public Setting GetSetting(string name)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<Setting>().Where(i => i.Name == name).FirstOrDefault();
            }
        }

        public IEnumerable<Setting> GetSettings(Expression<Func<Setting, bool>> predicate = null)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<Setting>().Where(predicate ?? (x => true)).ToList();
            }
        }

        public void UpdateSetting(Setting setting)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                db.Update(setting);
            }
        }
    } 
} 
