using System;
using SpeedRunApp.Model.ViewModels;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SpeedRunApp.Interfaces.Services
{
    public interface IUserAccountService
    {
        Task SendActivationEmail(string email);
        ActivateViewModel GetActivateUserAccount(string email, long expirationTime, string token);
        void CreateUserAccount(string username, string email, string pass);
        IEnumerable<UserAccount> GetUserAccounts(Expression<Func<UserAccount, bool>> predicate);
        Task SendResetPasswordEmail(string username);
        ResetPasswordViewModel GetResetPassword(string username, string email, long expirationTime, string token);
        bool EmailExists(string email);
        bool PasswordMatches(string password, string username);
        bool UsernameExists(string username, bool activeFilter);
    }
}
