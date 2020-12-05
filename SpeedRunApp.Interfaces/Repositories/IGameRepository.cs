using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Entity;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface IGameRepository
    {
        GameView GetGameView(string gameID);
    }
}






