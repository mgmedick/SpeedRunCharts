using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserService
    {
        Task SendActivationEmail(string email);
        ActivateViewModel GetActivateUser(string email, long expirationTime, string token);
        void CreateUser(string username, string pass);
        IEnumerable<User> GetUsers(Expression<Func<User, bool>> predicate);
        IEnumerable<UserView> GetUserViews(Expression<Func<UserView, bool>> predicate);        
        Task SendResetPasswordEmail(string username);
        ChangePasswordViewModel GetChangePassword(string username, string email, long expirationTime, string token);
        void ChangeUserPassword(string username, string pass);
        bool EmailExists(string email);
        bool PasswordMatches(string password, string username);
        bool UsernameExists(string username, bool activeFilter);
        UserViewModel GetUser(int userID);
        void SaveUser(UserViewModel userVM, int currUserID);
        void UpdateIsDarkTheme(int currUserID, bool isDarkTheme);
        Task SendConfirmRegistrationEmail(string email, string username);
    }
}
