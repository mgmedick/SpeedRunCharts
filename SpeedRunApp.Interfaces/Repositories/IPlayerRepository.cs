using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface IPlayerRepository
    {
        IEnumerable<PlayerView> GetPlayerViews(Expression<Func<PlayerView, bool>> predicate);
        PlayerSpeedRunCountResult GetPlayerSpeedRunCounts(int playerID);
        IEnumerable<SearchResult> SearchPlayers(string searchText);
    }
}






