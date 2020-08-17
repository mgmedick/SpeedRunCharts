using SpeedRunApp.Client;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;

namespace SpeedRunApp.Service
{
    public class PlatformService : IPlatformService
    {
        public IEnumerable<Platform> GetPlatforms()
        {
            ClientContainer clientContainer = new ClientContainer();
            var platforms = clientContainer.Platforms.GetPlatforms(200);

            return platforms;
        }
    }
}
