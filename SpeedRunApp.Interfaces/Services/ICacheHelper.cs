using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ICacheHelper
    {
        IEnumerable<Platform> GetPlatforms();
    }
}
