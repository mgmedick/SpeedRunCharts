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
        private readonly IEmailService _emailService = null;
        private readonly IHttpContextAccessor _context = null;
        private readonly IConfiguration _config = null;

        public UserAccountService(IUserAccountRepository userAcctRepo, IEmailService emailService, IHttpContextAccessor context, IConfiguration config)
        {
            _userAcctRepo = userAcctRepo;
            _emailService = emailService;
            _context = context;
            _config = config;
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
            else if (!loginVM.Password.VerifyHash(userAcct.Password))
            {
                var attemptCount = _context.HttpContext.Session.Get<int>("PasswordAttempts");
                var maxPasswordAttempts = Convert.ToInt32(_config.GetSection("SiteSettings").GetSection("MaxPasswordAttempts").Value);

                attemptCount++;
                _context.HttpContext.Session.Set<int>("PasswordAttempts", attemptCount);

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

        public IEnumerable<string> ValidateSignUp(SignUpViewModel signUpVM)
        {
            var errorList = new List<string>();

            if (string.IsNullOrWhiteSpace(signUpVM.Email))
            {
                errorList.Add("Email is required.");
            }

            if (!new EmailAddressAttribute().IsValid(signUpVM.Email))
            {
                errorList.Add("Email is invalid.");
            }

            if (!string.IsNullOrWhiteSpace(signUpVM.Email) && !string.IsNullOrWhiteSpace(signUpVM.ConfrimEmail) && signUpVM.Email != signUpVM.ConfrimEmail)
            {
                errorList.Add("Email does not match Confirm Email.");
            }

            var existingEmail = _userAcctRepo.GetUserAccounts(i => i.Email == signUpVM.Email).FirstOrDefault();
            if (existingEmail != null)
            {
                errorList.Add("Email already exists.");
            }

            return errorList;
        }

        public void SendActivationEmail(string email)
        {
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var baseUrl = string.Format("{0}://{1}{2}", _context.HttpContext.Request.Scheme, _context.HttpContext.Request.Host, _context.HttpContext.Request.PathBase);
            var queryParams = string.Format("email={0}&expirationTime={1}", email, DateTime.UtcNow.AddHours(48).Ticks);
            var token = queryParams.GetHMACSHA256Hash(hashKey);

            var activateUserAcct = new
            {
                Email = email,
                ActivateLink = string.Format("{0}/SpeedRun/Activate?{1}&token={2}", baseUrl, queryParams, token)
            };

            _emailService.SendEmailTemplate(email, "Create your speedruncharts.com account", Template.ActivateUserAccount.ToString(), activateUserAcct);
        }

        public ActivateViewModel GetActivateUserAccount(string email, long expirationTime, string token)
        {
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var strToHash = string.Format("email={0}&expirationTime={1}", email, expirationTime);
            var hash = strToHash.GetHMACSHA256Hash(hashKey);
            var expirationDate = new DateTime(expirationTime);
            var emailExists = _userAcctRepo.GetUserAccounts(i => i.Email == email).Any();
            var isValid = (hash == token) && expirationDate > DateTime.UtcNow && !emailExists;
            var activateUserAcctVM = new ActivateViewModel() { IsValid = isValid };

            return activateUserAcctVM;
        }

        public IEnumerable<string> ValidateActivateUserAccount(ActivateViewModel activateUserAcctVM)
        {
            var errorList = new List<string>();

            if (string.IsNullOrWhiteSpace(activateUserAcctVM.Username))
            {
                errorList.Add("Username is required.");
            }

            if (string.IsNullOrWhiteSpace(activateUserAcctVM.Password))
            {
                errorList.Add("Password is required.");
            }

            if (string.IsNullOrWhiteSpace(activateUserAcctVM.ConfirmPassword))
            {
                errorList.Add("Confirm Password is required.");
            }

            if (!string.IsNullOrWhiteSpace(activateUserAcctVM.Password) && !string.IsNullOrWhiteSpace(activateUserAcctVM.ConfirmPassword) && activateUserAcctVM.Password != activateUserAcctVM.ConfirmPassword)
            {
                errorList.Add("Password does not match Confirm Password.");
            }

            var usernameRegex = new Regex(@"^(?=.{3,15}$)([A-Za-z0-9][._()\[\]-]?)*$");
            if (!usernameRegex.IsMatch(activateUserAcctVM.Username))
            {
                errorList.Add("Username is invalid.");
            }

            var existingUserAcct = _userAcctRepo.GetUserAccounts(i => i.Username == activateUserAcctVM.Username).FirstOrDefault();
            if (existingUserAcct != null)
            {
                errorList.Add("Username already exists, please select another.");
            }

            return errorList;
        }

        public void CreateUserAccount(string username, string email, string pass)
        {
            var userAcct = new UserAccount()
            {
                Username = username,
                Password = pass.HashString(),
                Email = email,
                Active = true,
                CreatedBy = 1,
                CreatedDate = DateTime.UtcNow
            };

            _userAcctRepo.SaveUserAccount(userAcct);
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

        public IEnumerable<UserAccount> GetUserAccounts(Expression<Func<UserAccount, bool>> predicate)
        {
            return _userAcctRepo.GetUserAccounts(predicate);
        }
    }
}
