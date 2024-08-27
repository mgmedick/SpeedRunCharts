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
    public class UserRespository : BaseRepository, IUserRepository
    {
        public IEnumerable<User> GetUsers(Expression<Func<User, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<User>().Where(predicate).ToList();
            }
        }

        public void SaveUser(User user)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    db.Save<User>(user);

                    tran.Complete();
                }
            }
        }

        public IEnumerable<UserView> GetUserViews(Expression<Func<UserView, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<UserView>().Where(predicate).ToList();
            }
        }

        public IEnumerable<UserSetting> GetUserSettings(Expression<Func<UserSetting, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<UserSetting>().Where(predicate).ToList();
            }
        }        

        public void SaveUserSetting(UserSetting userSetting)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    db.Save<UserSetting>(userSetting);

                    tran.Complete();
                }
            }
        }

        public IEnumerable<UserSummaryList> GetUserSummaryLists(Expression<Func<UserSummaryList, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<UserSummaryList>().Where(predicate).ToList();
            }
        }

        public void SaveUserSummaryLists(IEnumerable<UserSummaryList> userSummaryLists)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    foreach (var userSummaryList in userSummaryLists)
                    {
                        db.Save<UserSummaryList>(userSummaryList);
                    }

                    tran.Complete();
                }
            }
        }

        public void DeleteUserSummaryLists(Expression<Func<UserSummaryList, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    db.DeleteMany<UserSummaryList>().Where(predicate).Execute();
                    tran.Complete();
                }
            }
        }
    }
}

