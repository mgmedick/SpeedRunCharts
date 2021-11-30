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
        IEnumerable<UserAccountView> GetUserAccountViews(Expression<Func<UserAccountView, bool>> predicate);
        void SaveUserAccountSetting(UserAccountSetting userAcctSetting);
        IEnumerable<UserAccountSpeedRunListCategory> GetUserAccountSpeedRunListCategories(Expression<Func<UserAccountSpeedRunListCategory, bool>> predicate);
        void SaveUserAccountSpeedRunListCategories(IEnumerable<UserAccountSpeedRunListCategory> userAcctSpeedRunListCategories);
        void DeleteUserAccountSpeedRunListCategories(Expression<Func<UserAccountSpeedRunListCategory, bool>> predicate);
    }
}






