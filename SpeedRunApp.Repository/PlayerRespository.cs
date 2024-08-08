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
    public class PlayerRespository : BaseRepository, IPlayerRepository
    {
        public IEnumerable<PlayerView> GetPlayerViews(Expression<Func<PlayerView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<PlayerView>().Where(predicate).ToList();
            }
        }        

        public PlayerSpeedRunCountResult GetPlayerSpeedRunCounts(int playerID)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<PlayerSpeedRunCountResult>("CALL GetPlayerSpeedRunCounts (@0);", playerID).FirstOrDefault();
            }
        }                        

        public IEnumerable<SearchResult> SearchPlayers(string searchText)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                var results = db.Query<SearchResult>("SELECT Abbr AS `Value`, Name AS Label FROM tbl_Player WHERE Name LIKE CONCAT('%', @0, '%') LIMIT 10;", searchText).ToList();

                return results;
            }
        }

        public IEnumerable<User> GetUsers(Expression<Func<User, bool>> predicate = null)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<User>().Where(predicate ?? (x => true)).ToList();
            }
        }

        public IEnumerable<IDNameAbbrPair> GetUserIDNameAbbrs()
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<IDNameAbbrPair>("SELECT ID, Name, Abbr FROM tbl_User;").ToList();
            }
        }       
    }
}

