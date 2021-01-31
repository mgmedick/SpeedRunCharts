using System;
using System.Collections.Generic;
using NPoco;
using Serilog;
using NPoco.Extensions;
using System.Linq;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace SpeedRunApp.Repository
{
    public class GameRespository : BaseRepository, IGameRepository
    {
        public IEnumerable<GameView> GetGameViews(Expression<Func<GameView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<GameView>().Where(predicate).ToList();
            }
        }

        public IEnumerable<GameView> GetGamesByUserID(int userID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<GameView>("EXEC dbo.GetGamesByUserID @0", userID).ToList();
            }
        }

        /*
        public IEnumerable<SearchResult2> SearchGames(string searchText)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SearchResult2>("SELECT ID, Name AS Text, 'Games' AS Category FROM dbo.tbl_Game WITH (NOLOCK) WHERE [Name] LIKE '%' + @0 + '%' ORDER BY [Name]", searchText).ToList();
            }
        }
        */

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SearchResult>("SELECT TOP 30 ID AS [Value], [Name] AS Label FROM dbo.tbl_Game WITH (NOLOCK) WHERE [Name] LIKE '%' + @0 + '%'", searchText).ToList();
            }
        }
    }
}

