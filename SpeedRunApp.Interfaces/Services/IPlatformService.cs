using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IPlatformService
    {
        IEnumerable<Platform> GetPlatforms();
    }
}
