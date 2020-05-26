using System;
using System.Collections.Generic;
using SpeedRunApp.Model;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IGamesService
    {
        GameDTO GetGame(string gameID);
    }
}
