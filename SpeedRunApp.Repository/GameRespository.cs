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
                var results = IsMySQL ? db.Query<SearchResult>("SELECT Abbr AS `Value`, Name AS Label FROM tbl_Game WHERE Name LIKE CONCAT('%', @0, '%') LIMIT 10;", searchText).ToList() :
                                        db.Query<SearchResult>("SELECT TOP 10 Abbr AS [Value], [Name] AS Label FROM dbo.tbl_Game WHERE [Name] LIKE '%' + @0 + '%'", searchText).ToList();
                
                return results;
            }
        }

        public IEnumerable<IDNameAbbrPair> GetGameIDNameAbbrs()
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<IDNameAbbrPair>("SELECT ID, Name, Abbr FROM tbl_Game;").ToList();
            }
        }
    }
}

