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
                    var login = _userAcctService.ValidateLogin(loginVM);
                    var userAcct = login.Item1;
                    var errorList = login.Item2;

                    if (errorList.Any())
                    {
                        foreach (var error in errorList)
                        {
                            ModelState.AddModelError("BadLogin", error);
                        }
                    }
                    else if (userAcct.PromptToChange)
                    {
                        return RedirectToAction("ChangePassword");
                    }
                    else
                    {
                        LoginUserAccount(userAcct);
                        return Json(new { success = true });
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
                    var errorList = _userAcctService.ValidateSignUp(signUpVM);

                    if (errorList.Any())
                    {
                        foreach (var error in errorList)
                        {
                            ModelState.AddModelError("BadSignUp", error);
                        }
                    }
                    else
                    {
                        _userAcctService.SendActivationEmail(signUpVM.Email);
                        return Json(new { success = true });
                    }
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
                var errorList = _userAcctService.ValidateActivateUserAccount(activateUserAcctVM);

                if (errorList.Any())
                {
                    foreach (var error in errorList)
                    {
                        ModelState.AddModelError("BadActivate", error);
                    }
                }
                else
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
    }
}
