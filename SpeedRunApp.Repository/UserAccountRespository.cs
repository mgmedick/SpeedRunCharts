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
using System.Collections;

namespace SpeedRunApp.Repository
{
    public class UserAccountRespository : BaseRepository, IUserAccountRepository
    {
        public IEnumerable<UserAccount> GetUserAccounts(Expression<Func<UserAccount, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<UserAccount>().Where(predicate).ToList();
            }
        }

        public void SaveUserAccount(UserAccount userAcct)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    db.Save<UserAccount>(userAcct);

                    tran.Complete();
                }
            }
        }

        public IEnumerable<UserAccountView> GetUserAccountViews(Expression<Func<UserAccountView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<UserAccountView>().Where(predicate).ToList();
            }
        }

        public void SaveUserAccountSetting(UserAccountSetting userAcctSetting)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    db.Save<UserAccountSetting>(userAcctSetting);

                    tran.Complete();
                }
            }
        }

        public IEnumerable<UserAccountSpeedRunListCategory> GetUserAccountSpeedRunListCategories(Expression<Func<UserAccountSpeedRunListCategory, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<UserAccountSpeedRunListCategory>().Where(predicate).ToList();
            }
        }

        public void SaveUserAccountSpeedRunListCategories(IEnumerable<UserAccountSpeedRunListCategory> userAcctSpeedRunListCategories)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    foreach (var userAcctSpeedRunListCategory in userAcctSpeedRunListCategories)
                    {
                        db.Save<UserAccountSpeedRunListCategory>(userAcctSpeedRunListCategory);
                    }

                    tran.Complete();
                }
            }
        }

        public void DeleteUserAccountSpeedRunListCategories(Expression<Func<UserAccountSpeedRunListCategory, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    db.DeleteMany<UserAccountSpeedRunListCategory>().Where(predicate).Execute();
                    tran.Complete();
                }
            }
        }
    }
}

