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

        public async Task SendActivationEmail(string email)
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

            await _emailService.SendEmailTemplate(email, "Create your speedruncharts.com account", Template.ActivateUserAccount.ToString(), activateUserAcct);
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

        public async Task SendResetPasswordEmail(string username)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.Username == username).FirstOrDefault();
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var baseUrl = string.Format("{0}://{1}{2}", _context.HttpContext.Request.Scheme, _context.HttpContext.Request.Host, _context.HttpContext.Request.PathBase);
            var queryParams = string.Format("username={0}&email={1}&expirationTime={2}", userAcct.Username, userAcct.Email, DateTime.UtcNow.AddHours(48).Ticks);
            var token = string.Format("{0}&password={1}", queryParams, userAcct.Password).GetHMACSHA256Hash(hashKey);

            var passwordReset = new
            {
                Username = userAcct.Username,
                ResetPassLink = string.Format("{0}/SpeedRun/ChangePassword?{1}&token={2}", baseUrl, queryParams, token)
            };

            await _emailService.SendEmailTemplate(userAcct.Email, "Reset your speedruncharts.com password", Template.ResetUserAccountPassword.ToString(), passwordReset);
        }
        
        public ChangePasswordViewModel GetChangePassword(string username, string email, long expirationTime, string token)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.Username == username).FirstOrDefault();
            var hashKey = _config.GetSection("SiteSettings").GetSection("HashKey").Value;
            var strToHash = string.Format("username={0}&email={1}&expirationTime={2}&password={3}", username, email, expirationTime, userAcct.Password);
            var hash = strToHash.GetHMACSHA256Hash(hashKey);
            var expirationDate = new DateTime(expirationTime);
            var isValid = (hash == token) && expirationDate > DateTime.UtcNow;
            var changePassVM = new ChangePasswordViewModel() { IsValid = isValid };

            return changePassVM;
        }

        public IEnumerable<UserAccount> GetUserAccounts(Expression<Func<UserAccount, bool>> predicate)
        {
            return _userAcctRepo.GetUserAccounts(predicate);
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

        public void ChangeUserAcctPassword(string username, string pass)
        {
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.Username == username).FirstOrDefault();
            userAcct.Password = pass.HashString();
            userAcct.ModifiedBy = userAcct.ID;
            userAcct.ModifiedDate = DateTime.UtcNow;

            _userAcctRepo.SaveUserAccount(userAcct);
        }

        //jqvalidate
        public bool EmailExists(string email)
        {
            var result = _userAcctRepo.GetUserAccounts(i => i.Email == email).Any();

            return result;
        }

        public bool PasswordMatches(string password, string username)
        {
            var result = false;
            var userAcct = _userAcctRepo.GetUserAccounts(i => i.Username == username && i.Active).FirstOrDefault();

            if (userAcct != null)
            {
                result = password.VerifyHash(userAcct.Password);
            }

            return result;
        }

        public bool UsernameExists(string username, bool activeFilter)
        {
            var result = _userAcctRepo.GetUserAccounts(i => i.Username == username && (i.Active || i.Active == activeFilter)).Any();

            return result;
        }
    }
}
