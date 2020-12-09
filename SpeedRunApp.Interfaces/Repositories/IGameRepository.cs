using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Entity;
using SpeedRunApp.Model.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface IGameRepository
    {
        GameView GetGameView(string gameID);
        IEnumerable<SearchResult> SearchGames(string searchText);
        IEnumerable<SpeedRunGridItem> GetSpeedRunGridItemsByGameID(string gameID);
    }
}






