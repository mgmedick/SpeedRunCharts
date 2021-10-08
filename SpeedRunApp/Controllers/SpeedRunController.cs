using System;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using SpeedRunApp.Interfaces.Services;
using SpeedRunApp.Model;
using SpeedRunApp.Model.Data;
using SpeedRunApp.Model.ViewModels;
using SpeedRunCommon.Extensions;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System.Linq;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace SpeedRunApp.MVC.Controllers
{
    public class SpeedRunController : Controller
    {
        private readonly ISpeedRunService _speedRunService = null;
        private readonly IGameService _gamesService = null;
        private readonly IUserService _userService = null;
        private readonly IUserAccountService _userAcctService = null;
        private readonly ILogger _logger = null;

        public SpeedRunController(ISpeedRunService speedRunService, IGameService gamesService, IUserService userService, IUserAccountService userAcctService, ILogger logger)
        {
            _speedRunService = speedRunService;
            _gamesService = gamesService;
            _userService = userService;
            _userAcctService = userAcctService;
            _logger = logger;
        }

        public ViewResult SpeedRunList()
        {
            var runListVM = _speedRunService.GetSpeedRunList();

            return View(runListVM);
        }

        public ViewResult Error()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetLatestSpeedRuns(SpeedRunListCategory category, int topAmount, int? orderValueOffset)
        {
            var results = _speedRunService.GetLatestSpeedRuns(category, topAmount, orderValueOffset);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetEditSpeedRun(int gameID, int? speedRunID = null)
        {
            var results = _speedRunService.GetEditSpeedRun(gameID, speedRunID);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetSpeedRunSummary(int speedRunID)
        {
            var results = _speedRunService.GetSpeedRunSummary(speedRunID);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetWorldRecordGridData(int gameID, int categoryTypeID, int categoryID, int? userID)
        {
            var results = _speedRunService.GetWorldRecordGridData(gameID, categoryTypeID, categoryID, userID);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetSpeedRunGridData(int gameID, int categoryTypeID, int categoryID, int? levelID, string subCategoryVariableValueIDs, int? userID)
        {
            var results = _speedRunService.GetSpeedRunGridData(gameID, categoryTypeID, categoryID, levelID, subCategoryVariableValueIDs, userID);

            return Json(results);
        }

        [HttpGet]
        public ActionResult Login()
        {
            var loginVM = new LoginViewModel();

            return PartialView("_Login", loginVM);
        }

        [HttpPost]
        public JsonResult Login(LoginViewModel loginVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                if (!_userAcctService.UsernameExists(loginVM.Username, true))
                {
                    ModelState.AddModelError("Login", "Invalid username");
                }

                if (!_userAcctService.PasswordMatches(loginVM.Password, loginVM.Username))
                {
                    ModelState.AddModelError("Login", "Invalid password");
                }

                if(ModelState.IsValid)
                {
                    var userAcct = _userAcctService.GetUserAccounts(i => i.Username == loginVM.Username).FirstOrDefault();
                    LoginUserAccount(userAcct);
                    success = true;
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Login");
                success = false;
                errorMessages = new List<string>() { "Error logging user in." };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            var signUpVM = new SignUpViewModel();

            return PartialView("_SignUp", signUpVM);
        }

        [HttpPost]
        public JsonResult SignUp(SignUpViewModel signUpVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                if (_userAcctService.EmailExists(signUpVM.Email))
                {
                    ModelState.AddModelError("SignUp", "Email already exists for another user");
                }

                if (ModelState.IsValid)
                {
                    _ = _userAcctService.SendActivationEmail(signUpVM.Email).ContinueWith(t => _logger.Error(t.Exception, "SendActivationEmail"), TaskContinuationOptions.OnlyOnFaulted);
                    success = true;
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SignUp");
                return Json(new { success = false, message = "Error signing up user." });
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpGet]
        public ViewResult Activate(string email, long expirationTime, string token)
        {
            var activateUserAcctVM = _userAcctService.GetActivateUserAccount(email, expirationTime, token);
            HttpContext.Session.Set<string>("Email", email);

            return View(activateUserAcctVM);
        }

        [HttpPost]
        public JsonResult Activate(ActivateViewModel activateUserAcctVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                if (_userAcctService.UsernameExists(activateUserAcctVM.Username, false))
                {
                    ModelState.AddModelError("Activate", "Username already exists for another user");
                }

                if (ModelState.IsValid)
                {
                    _userAcctService.CreateUserAccount(activateUserAcctVM.Username, HttpContext.Session.Get<string>("Email"), activateUserAcctVM.Password);
                    var userAcct = _userAcctService.GetUserAccounts(i => i.Username == activateUserAcctVM.Username).FirstOrDefault();
                    LoginUserAccount(userAcct);
                    success = true;
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ResetPassword");
                success = false;
                errorMessages = new List<string>() { "Error resetting password" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            var resetPassVM = new ResetPasswordViewModel();

            return PartialView("_ResetPassword", resetPassVM);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult ResetPassword(ResetPasswordViewModel resetPassVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                if (!_userAcctService.UsernameExists(resetPassVM.Username, true))
                {
                    ModelState.AddModelError("ResetPassword", "Username not found");
                }

                if (ModelState.IsValid)
                {
                    _ = _userAcctService.SendResetPasswordEmail(resetPassVM.Username).ContinueWith(t => _logger.Error(t.Exception, "SendResetPasswordEmail"), TaskContinuationOptions.OnlyOnFaulted);
                    success = true;
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ResetPassword");
                success = false;
                errorMessages = new List<string>() { "Error resetting password" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        [HttpGet]
        public ViewResult ChangePassword(string username, string email, long expirationTime, string token)
        {
            var changePassVM = _userAcctService.GetChangePassword(username, email, expirationTime, token);
            HttpContext.Session.Set<string>("Username", username);

            return View(changePassVM);
        }

        [HttpPost]
        public JsonResult ChangePassword(ChangePasswordViewModel changePassVM)
        {
            var success = false;
            List<string> errorMessages = null;

            try
            {
                var username = HttpContext.Session.Get<string>("Username");
                if (_userAcctService.PasswordMatches(changePassVM.Password, username))
                {
                    ModelState.AddModelError("ChangePassword", "Password must differ from previous password");
                }

                if (ModelState.IsValid)
                {
                    _userAcctService.ChangeUserAcctPassword(username, changePassVM.Password);
                    success = true;
                }
                else
                {
                    success = false;
                    errorMessages = ModelState.Values.SelectMany(i => i.Errors).Select(i => i.ErrorMessage).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "ChangePassword");
                success = false;
                errorMessages = new List<string>() { "Error changing password" };
            }

            return Json(new { success = success, errorMessages = errorMessages });
        }

        private async void LoginUserAccount(UserAccount userAcct)
        {
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, userAcct.ID.ToString()),
                            new Claim(ClaimTypes.Email, userAcct.Email),
                            new Claim(ClaimTypes.Name, userAcct.Username)
                        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
        }

        //jqvalidate
        [AllowAnonymous]
        [HttpGet]
        public IActionResult UsernameExists(string username)
        {
            var result = _userAcctService.UsernameExists(username, false);

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ActiveUsernameExists(string username)
        {
            var result = _userAcctService.UsernameExists(username, true);

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult UsernameNotExists(string username)
        {
            var result = !_userAcctService.UsernameExists(username, false);

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult PasswordNotMatches(string password)
        {
            var username = HttpContext.Session.Get<string>("Username");
            var result = !_userAcctService.PasswordMatches(password, username);

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult PasswordMatches(string password, string username)
        {
            var result = _userAcctService.PasswordMatches(password, username);

            return Json(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult EmailNotExists(string email)
        {
            var result = !_userAcctService.EmailExists(email);

            return Json(result);
        }
    }
}
