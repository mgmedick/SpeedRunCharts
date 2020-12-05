using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGamesService1
    {
        GameDetailsViewModel1 GetGameDetails(string gameID);
    }
}
