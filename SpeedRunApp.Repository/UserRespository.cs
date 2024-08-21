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

        public IEnumerable<UserSpeedRunSummaryList> GetUserSpeedRunSummaryLists(Expression<Func<UserSpeedRunSummaryList, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                return db.Query<UserSpeedRunSummaryList>().Where(predicate).ToList();
            }
        }

        public void SaveUserSpeedRunSummaryLists(IEnumerable<UserSpeedRunSummaryList> userSpeedRunSummaryLists)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    foreach (var userSpeedRunSummaryList in userSpeedRunSummaryLists)
                    {
                        db.Save<UserSpeedRunSummaryList>(userSpeedRunSummaryList);
                    }

                    tran.Complete();
                }
            }
        }

        public void DeleteUserSpeedRunSummaryLists(Expression<Func<UserSpeedRunSummaryList, bool>> predicate)
        {
            using (IDatabase db = DBFactory.GetDatabase())
            {
                using (var tran = db.GetTransaction())
                {
                    db.DeleteMany<UserSpeedRunSummaryList>().Where(predicate).Execute();
                    tran.Complete();
                }
            }
        }
    }
}

