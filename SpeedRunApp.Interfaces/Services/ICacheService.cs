using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ICacheService
    {
        IEnumerable<User> GetUsers();
        IEnumerable<IDNameAbbrPair> GetGameIDNameAbbrs();
        IEnumerable<IDNameAbbrPair> GetUserIDNameAbbrs();
        IEnumerable<IDNamePair> GetRunStatusTypes();
    }
}
