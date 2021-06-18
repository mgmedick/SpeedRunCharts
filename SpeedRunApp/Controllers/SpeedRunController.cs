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

namespace SpeedRunApp.WebUI.Controllers
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
        public JsonResult GetEditSpeedRun(int gameID, int? speedRunID = null, bool isReadOnly = false)
        {
            var results = _speedRunService.GetEditSpeedRun(gameID, speedRunID, isReadOnly);

            return Json(results);
        }

        [HttpGet]
        public JsonResult GetSpeedRunSummary(int speedRunID)
        {
            var results = _speedRunService.GetSpeedRunSummary(speedRunID);

            return Json(results);
        }

        [HttpGet]
        public ActionResult Login()
        {
            var loginVM = new LoginViewModel();

            return PartialView("_Login", loginVM);
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginVM)
        {
            try
            {
                if (ModelState.IsValid)
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
                        loginVM.Success = true;
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Login");
                return Json(new { success = false, message = "Error logging user in." });
            }

            return PartialView("_Login", loginVM);
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
        public ActionResult SignUp(SignUpViewModel signUpVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ = _userAcctService.SendActivationEmail(signUpVM.Email).ContinueWith(t => _logger.Error(t.Exception, "SendActivationEmail"), TaskContinuationOptions.OnlyOnFaulted);
                    signUpVM.Success = true;
                }              
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "SignUp");
                return Json(new { success = false, message = "Error signing up user." });
            }

            return PartialView("_SignUp", signUpVM);
        }

        [HttpGet]
        public ViewResult Activate(string email, long expirationTime, string token)
        {
            var activateUserAcctVM = _userAcctService.GetActivateUserAccount(email, expirationTime, token);
            HttpContext.Session.Set<string>("Email", email);

            return View(activateUserAcctVM);
        }

        [HttpPost]
        public ActionResult Activate(ActivateViewModel activateUserAcctVM)
        {
            if (ModelState.IsValid)
            {
                if (_userAcctService.UsernameExists(activateUserAcctVM.Username, false))
                {
                    ModelState.AddModelError("Activate", "Username already exists");
                }

                if (ModelState.IsValid)
                {
                    _userAcctService.CreateUserAccount(activateUserAcctVM.Username, HttpContext.Session.Get<string>("Email"), activateUserAcctVM.Password);
                    var userAcct = _userAcctService.GetUserAccounts(i => i.Username == activateUserAcctVM.Username).FirstOrDefault();
                    LoginUserAccount(userAcct);

                    return Redirect("SpeedRunList");
                }
            }

            return View(activateUserAcctVM);
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

        [HttpGet]
        public ViewResult ResetPassword()
        {
            var resetPassVM = new ResetPasswordViewModel();
            return View(resetPassVM);
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel resetPassVM)
        {
            if (ModelState.IsValid)
            {
                if (!_userAcctService.UsernameExists(resetPassVM.Username, true))
                {
                    ModelState.AddModelError("Activate", "Username does not exist");
                }

                if (ModelState.IsValid)
                {
                    _ = _userAcctService.SendResetPasswordEmail(resetPassVM.Username).ContinueWith(t => _logger.Error(t.Exception, "SendResetPasswordEmail"), TaskContinuationOptions.OnlyOnFaulted);
                    resetPassVM.Success = true;
                }
            }

            return View(resetPassVM);
        }

        //jqvalidate
        [HttpGet]
        public IActionResult UsernameExists(string username, bool activeFilter)
        {
            var result = _userAcctService.UsernameExists(username, activeFilter);

            return Json(result);
        }

        [HttpGet]
        public IActionResult PasswordMatches(string password, string username)
        {
            var result = _userAcctService.PasswordMatches(password, username);

            return Json(result);
        }

        [HttpGet]
        public IActionResult EmailNotExists(string email)
        {
            var result = !_userAcctService.EmailExists(email);

            return Json(result);
        }
    }
}
