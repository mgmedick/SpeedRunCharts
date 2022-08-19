using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface ISettingRepository
    {
        Setting GetSetting(string name);
        IEnumerable<Setting> GetSettings(Expression<Func<Setting, bool>> predicate = null);
        void UpdateSetting(Setting setting);
    }
}





