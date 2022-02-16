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
    public class UserRespository : BaseRepository, IUserRepository
    {
        public IEnumerable<UserView> GetUserViews(Expression<Func<UserView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<UserView>().Where(predicate).ToList();
            }
        }

        public IEnumerable<SearchResult> SearchUsers(string searchText)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SearchResult>("SELECT TOP 10 Abbr AS [Value], [Name] AS Label FROM dbo.tbl_User tu WITH (NOLOCK) WHERE [Name] LIKE @0 + '%'", searchText).ToList();
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
                return db.Query<IDNameAbbrPair>("SELECT ID, [Name], [Abbr] FROM dbo.tbl_User WITH (NOLOCK)").ToList();
            }
        }
    }
}

