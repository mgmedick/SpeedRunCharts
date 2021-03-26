﻿using System;
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
                return db.Query<SearchResult>("SELECT TOP 10 UserID AS [Value], [Name] AS Label FROM dbo.tbl_Player tu WITH (NOLOCK) WHERE [Name] LIKE @0 + '%'", searchText).ToList();
            }
        }
    }
}

