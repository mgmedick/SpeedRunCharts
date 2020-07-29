using SpeedRunApp.Model;
using SpeedRunApp.Client;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Interfaces.Services;
using SpeedRunCommon;
using System.Collections.Generic;
using System.Linq;
using System;

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
