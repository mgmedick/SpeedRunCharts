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
        public UserView GetUserViews(Expression<Func<UserView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<UserView>().Where(predicate).FirstOrDefault();
            }
        }

        public IEnumerable<SearchResult> SearchUsers(string searchText)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<SearchResult>("SELECT TOP 30 ID AS [Value], [Name] AS Label FROM dbo.tbl_User tu WITH (NOLOCK) WHERE EXISTS (SELECT 1 FROM dbo.tbl_SpeedRun_Player rp WITH (NOLOCK) WHERE rp.UserID = tu.ID) AND UserRoleID <> 0 AND [Name] LIKE @0 + '%' ORDER BY [Name]", searchText).ToList();
            }
        }
    }
}

