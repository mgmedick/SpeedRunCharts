using System;
using System.Collections.Generic;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace SpeedRunApp.Interfaces.Repositories
{
    public interface IUserAccountRepository
    {
        IEnumerable<UserAccount> GetUserAccounts(Expression<Func<UserAccount, bool>> predicate);
        void SaveUserAccount(UserAccount userAcct);
    }
}






