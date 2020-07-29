using System;
using System.Collections.Generic;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ICacheHelper
    {
        IEnumerable<Platform> GetPlatforms();
    }
}
