using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using thissite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Extensions;
using Braintree;
using Microsoft.EntityFrameworkCore;


namespace thissite
{
    public class AccountController : Controller
    {
        SignInManager<ThisSiteUser> _signInManager;
        EmailService _emailService;
        BraintreeGateway _braintreeGateway;
        private ThisSiteDbContext _thisSiteDbContext;
        

        //using Microsoft.AspNetCore.Identity
        public AccountController(SignInManager<ThisSiteUser> signInManager, EmailService emailService, BraintreeGateway braintreeGateway, ThisSiteDbContext thisSiteDbContext)
        {
            this._signInManager = signInManager;
            this._emailService = emailService;
            _braintreeGateway = braintreeGateway;
            this._thisSiteDbContext = thisSiteDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Responds on GET /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // Responds on POST /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                ThisSiteUser newUser = new ThisSiteUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber
                };

                IdentityResult creationResult = await this._signInManager.UserManager.CreateAsync(newUser);
                if (creationResult.Succeeded)
                {
                    IdentityResult passwordResult = await this._signInManager.UserManager.AddPasswordAsync(newUser, model.Password);
                    if (passwordResult.Succeeded)
                    {

                        var confirmationToken = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(newUser);

                        confirmationToken = System.Net.WebUtility.UrlEncode(confirmationToken);

                        string currentUrl = Request.GetDisplayUrl();    //This will get me the URL for the current request
                        System.Uri uri = new Uri(currentUrl);   //This will wrap it in a "URI" object so I can split it into parts
                        string confirmationUrl = uri.GetLeftPart(UriPartial.Authority); //This gives me just the scheme + authority of the URI
                        confirmationUrl += "/account/confirm?id=" + confirmationToken + "&userId=" + System.Net.WebUtility.UrlEncode(newUser.Id);
                        await this._signInManager.SignInAsync(newUser, false);
                        var emailResult = await this._emailService.SendEmailAsync(
                            model.Email,
                            "Welcome to This Site Sucks!",
                             "<p>Thanks you for signing up, " + model.UserName + "!<br><a href=\"" + confirmationUrl + "\">Please confirm your account<a></p>",
                             "Thanks for signing up, " + model.UserName + "!"
                            );

                        if (!emailResult.Success)
                        {
                            throw new Exception(string.Join(',', emailResult.Errors.Select(x => x.Message)));
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in passwordResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in creationResult.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await this._signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (ModelState.IsValid)
            {

                ThisSiteUser existingUser = await this._signInManager.UserManager.FindByNameAsync(model.UserName);
                if (existingUser != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult passwordResult = await this._signInManager.CheckPasswordSignInAsync(existingUser, model.Password, false);
                    if (passwordResult.Succeeded)
                    {
                        await this._signInManager.SignInAsync(existingUser, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("PasswordIncorrect", "Username or Password is incorrect.");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserDoesNotExist", "Username or Password is incorrect.");

                }
            }
            return View();
        }



        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if ((ModelState.IsValid) && (!string.IsNullOrEmpty(email)))
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var resetToken = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

                    resetToken = System.Net.WebUtility.UrlEncode(resetToken);
                    string currentUrl = Request.GetDisplayUrl();    //This will get me the URL for the current request
                    System.Uri uri = new Uri(currentUrl);   //This will wrap it in a "URI" object so I can split it into parts
                    string resetUrl = uri.GetLeftPart(UriPartial.Authority); //This gives me just the scheme + authority of the URI
                    resetUrl += "/account/resetpassword?id=" + resetToken + "&userId=" + System.Net.WebUtility.UrlEncode(user.Id);
                    string htmlContent = "<a href=\"" + resetUrl + "\">Reset your password</a>";
                    var emailResult = await _emailService.SendEmailAsync(email, "Please reset your password", htmlContent, resetUrl);
                    if (!emailResult.Success)
                    {
                        throw new Exception(string.Join(',', emailResult.Errors.Select(x => x.Message)));

                    }
                    return RedirectToAction("ResetSent");
                }
            }
            ModelState.AddModelError("email", "Email is not valid");
            return View();
        }


        public IActionResult ResetSent()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string userId, string password)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _signInManager.UserManager.ResetPasswordAsync(user, id, password);
                return RedirectToAction("SignIn");
            }
            return BadRequest();
        }

        public async Task<IActionResult> Confirm(string id, string userId)
        {
            var user = await _signInManager.UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _signInManager.UserManager.ConfirmEmailAsync(user, id);
                return RedirectToAction("Index", "Home");
            }
            return BadRequest();


        }
    }
}