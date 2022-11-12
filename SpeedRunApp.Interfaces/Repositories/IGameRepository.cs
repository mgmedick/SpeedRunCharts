using System;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<GameView> GetGameViews(Expression<Func<GameView, bool>> predicate);
        IEnumerable<Game> GetGames(Expression<Func<Game, bool>> predicate = null);        
        IEnumerable<SearchResult> SearchGames(string searchText);
        IEnumerable<IDNameAbbrPair> GetGameIDNameAbbrs();
        void UpdateGameIsChanged(Game game);        
    }
}






