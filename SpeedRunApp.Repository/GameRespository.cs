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

        public IEnumerable<SearchResult> SearchGames(string searchText)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SearchResult>("SELECT TOP 10 ID AS [Value], [Name] AS Label FROM dbo.tbl_Game WITH (NOLOCK) WHERE [Name] LIKE '%' + @0 + '%'", searchText).ToList();
            }
        }
    }
}

