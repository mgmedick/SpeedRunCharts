using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeedRunApp.Interfaces.Services
{
    public interface ISettingService
    {       
        Setting GetSetting(string name);
        void UpdateSetting(string name, string value);
        void UpdateSetting(string name, DateTime value);
        void UpdateSetting(string name, int value);
        void UpdateSetting(Setting setting); 
    }
}
