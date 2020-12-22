using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface IUserRepository
    {
        UserView GetUserViews(Expression<Func<UserView, bool>> predicate);
        IEnumerable<SearchResult> SearchUsers(string searchText);
    }
}






