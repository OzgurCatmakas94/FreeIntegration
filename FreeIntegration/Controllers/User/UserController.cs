using FreeIntegration.Data;
using FreeIntegration.Models.Login;
using FreeIntegration.Models.Register;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeIntegration.Web.Controllers.User
{
    public class UserController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        public UserController(SignInManager<IdentityUser> signInManager, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _db = db;
            _userManager = userManager;
        }
        #region Actions
        public IActionResult Login(LoginDT login)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                    _signInManager.SignOutAsync();

                var result = _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, lockoutOnFailure: true).Result;
                if (result.Succeeded)
                {
                    ViewBag.UserName = login.UserName;
                    var exist = _db.Logins.OrderByDescending(x=>x.log_Date).FirstOrDefault(x => x.userName == login.UserName);
                    if (exist==null)
                    {
                        //INSERT
                        LoginUserLogDT loginLog = new LoginUserLogDT()
                        {
                            userName = login.UserName,
                            log_Date = DateTime.Now.ToString()

                        };
                        _db.Logins.Add(loginLog);
                    }
                    else
                    {
                        //INSERT
                        LoginUserLogDT loginLog = new LoginUserLogDT()
                        {
                            userName = login.UserName,
                            log_Date = DateTime.Now.ToString()

                        };
                        _db.Logins.Update(loginLog);
                    }
                    
                    _db.SaveChanges();

                    return Redirect("~/");
                }
            }
            return PartialView("_Login", login);
        }
        public IActionResult ForgotPassword(LoginDT login)
        {
            if (ControlEmailForForgotPassword(login))
                return View("ResetPassword");

            return View("ForgotPassword");
        }
        public IActionResult RegisterUser(RegisterDT registerDT)
        {
            if (ModelState.IsValid)
            {
                string errorMessage = string.Empty;
                var user = new RegisterDT
                {
                    UserName = registerDT.UserName,
                    CompanyName = registerDT.CompanyName,
                    Email = registerDT.Email,
                    City = registerDT.City,
                    StreetAddress = registerDT.StreetAddress,
                    PostalCode = registerDT.PostalCode,
                    PhoneNumber = registerDT.PhoneNumber
                };

                var result = _userManager.CreateAsync(user, registerDT.Password).Result;
                if (result.Succeeded)
                    return PartialView("_Login");
                else
                {
                    for (int i = 0; i < result.Errors.Count(); i++)
                    {
                        errorMessage += $"{result.Errors.ElementAt(i).Description} \n";
                    }
                    ViewBag.ErrorMessages = errorMessage;
                }

            }
            return View("RegisterUser");
        }
        public IActionResult ResetPassword(LoginDT login)
        {
            if (login == null && !string.IsNullOrWhiteSpace(login.Email))
            {
                return View("ResetPassword");
            }

            var user = _userManager.FindByEmailAsync(login.Email).Result;
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return View("ResetPassword");
            }

            var Code = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var result = _userManager.ResetPasswordAsync(user, Code, login.Password).Result;
            if (result.Succeeded)
            {
                return PartialView("_Login");
            }
            return View("ResetPassword");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(true);
            return Redirect("~/");//anasayfaya yönlendir.
        }

        public IActionResult UpdateUser(RegisterDT registerDT)
        {
            //fill fields here.
            if (ModelState.IsValid)
            {
                var user = new RegisterDT
                {
                    UserName = registerDT.UserName,
                    CompanyName = registerDT.CompanyName,
                    Email = registerDT.Email,
                    City = registerDT.City,
                    StreetAddress = registerDT.StreetAddress,
                    PostalCode = registerDT.PostalCode,
                    PhoneNumber = registerDT.PhoneNumber
                };
                var result = _userManager.UpdateAsync(user).Result;
                if (result.Succeeded)
                    return Redirect("~/");
            }
            return View("UpdateUser");
        }
        #endregion

        #region Utility Methods
        private bool ControlEmailForForgotPassword(LoginDT login)
        {
            bool isFinded = false;
            if (login != null && !string.IsNullOrWhiteSpace(login.Email))
            {
                var user = _userManager.FindByEmailAsync(login.Email).Result;
                if (user == null)
                    return isFinded;

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                //var code = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                //var callbackUrl = Url.Page(
                //    "/Account/ResetPassword",
                //    pageHandler: null,
                //    values: new { code },
                //    protocol: Request.Scheme);

                //await _emailSender.SendEmailAsync(
                //    Email,
                //    "Reset Password",
                //    $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                return true;
            }
            return isFinded;
        }

        #endregion
    }
}
