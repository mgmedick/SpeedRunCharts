using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface IGameRepository
    {
        GameView GetGameView(string gameID);
        IEnumerable<GameView> GetGameViews(Expression<Func<GameView, bool>> predicate);
        IEnumerable<SearchResult> SearchGames(string searchText);
    }
}






