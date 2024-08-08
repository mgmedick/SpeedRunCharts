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
        IEnumerable<User> GetUsers(Expression<Func<User, bool>> predicate);
        void SaveUser(User user);
        IEnumerable<UserView> GetUserViews(Expression<Func<UserView, bool>> predicate);
        void SaveUserSetting(UserSetting userSetting);
        IEnumerable<UserSpeedRunListCategory> GetUserSpeedRunListCategories(Expression<Func<UserSpeedRunListCategory, bool>> predicate);
        void SaveUserSpeedRunListCategories(IEnumerable<UserSpeedRunListCategory> userSpeedRunListCategories);
        void DeleteUserSpeedRunListCategories(Expression<Func<UserSpeedRunListCategory, bool>> predicate);
    }
}






