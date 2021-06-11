using System;
using SpeedRunApp.Interfaces.Repositories;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using SpeedRunCommon.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SpeedRunApp.Service
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAcctRepo = null;
        private readonly ISession _session = null;
        private readonly IConfiguration _config = null;

        public UserAccountService(IUserAccountRepository userAcctRepo, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _userAcctRepo = userAcctRepo;
            _session = httpContextAccessor.HttpContext.Session;
            _config = config;
        }

        public UserAccount GetUserAccountForLogin(string username)
        {
            return _userAcctRepo.GetUserAccounts(i => i.Username == username && i.Active).FirstOrDefault();
        }

        public IEnumerable<UserAccount> GetUserAccounts(Expression<Func<UserAccount, bool>> predicate)
        {
            return _userAcctRepo.GetUserAccounts(predicate);
        }

        public Tuple<UserAccount, IEnumerable<string>> ValidateLogin(LoginViewModel loginVM)
        {
            var errorList = new List<string>();
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.Username == loginVM.Username && i.Active).FirstOrDefault();

            if (userAcct == null)
            {
                errorList.Add("Invalid username/password.");
            }
            else if (userAcct.Locked)
            {
                errorList.Add("The account has been locked due to too many failed login attempts. Please contact support to unlock the account.");
            }
            else if (!loginVM.Password.VerifyPassword(userAcct.Password))
            {
                var attemptCount = _session.Get<int>("PasswordAttempts");
                var maxPasswordAttempts = Convert.ToInt32(_config.GetSection("SiteSettings").GetSection("MaxPasswordAttempts").Value);

                attemptCount++;
                _session.Set<int>("PasswordAttempts", attemptCount);

                if (attemptCount > maxPasswordAttempts)
                {
                    LockUserAccount(userAcct.ID);
                    errorList.Add("The account has been locked due to too many failed login attempts. Please contact support to unlock the account.");
                }
                else
                {
                    errorList.Add("Invalid username/password.");
                }
            }

            return new Tuple<UserAccount, IEnumerable<string>>(userAcct, errorList);
        }

        public void LockUserAccount(int userID)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.ID == userID).FirstOrDefault();
            if (userAcct != null)
            {
                userAcct.Locked = true;
                _userAcctRepo.SaveUserAccount(userAcct);
            }
        }

        public IEnumerable<string> SignUp(SignUpViewModel signUpVM)
        {
            var errorList = ValidateSignUp(signUpVM);

            if (!errorList.Any())
            {
                //send email
            }

            return errorList;
        }

        private IEnumerable<string> ValidateSignUp(SignUpViewModel signUpVM)
        {
            var errorList = new List<string>();
            var existingUserAcct = _userAcctRepo.GetUserAccounts(i => i.Username == signUpVM.Username).FirstOrDefault();
            var usernameRegex = new Regex(@"[^a-zA-Z0-9\s]");

            if (string.IsNullOrWhiteSpace(signUpVM.Username))
            {
                errorList.Add("Username is required.");
            }

            if (string.IsNullOrWhiteSpace(signUpVM.Email))
            {
                errorList.Add("Email is required.");
            }

            if (string.IsNullOrWhiteSpace(signUpVM.ConfrimEmail))
            {
                errorList.Add("Confirm Email is required.");
            }

            if (existingUserAcct != null)
            {
                errorList.Add("Username already exists.");
            }

            if (new EmailAddressAttribute().IsValid(signUpVM.Email))
            {
                errorList.Add("Email is invalid.");
            }

            if (!usernameRegex.IsMatch(signUpVM.Username))
            {
                errorList.Add("Username is invalid.");
            }

            return errorList;
        }
    }
}
