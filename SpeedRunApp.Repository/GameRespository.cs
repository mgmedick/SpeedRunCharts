using System;
using System.Collections.Generic;
using NPoco;
using Serilog;
using NPoco.Extensions;
using System.Linq;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Entity;
using SpeedRunApp.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace SpeedRunApp.Repository
{
    public class GameRespository : BaseRepository, IGameRepository
    {
        public GameView GetGameView(string gameID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<GameView>().Where(i => i.ID == gameID).FirstOrDefault();
            }
        }
    }
}

