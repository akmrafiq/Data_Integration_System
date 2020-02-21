using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Data_Integration.Core.Entities;
using Data_Integration.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Data_Integration.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ExtendedIdentityUser> _signInManager;
        private readonly UserManager<ExtendedIdentityUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailSender _emailSender;
        public AccountController(
           UserManager<ExtendedIdentityUser> userManager,
           RoleManager<IdentityRole> roleManager,
           SignInManager<ExtendedIdentityUser> signInManager,
           ILogger<AccountController> logger,
           IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            var model = new LoginModel();

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            model.ReturnUrl = returnUrl;

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/admin");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    user.LastLogin = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    if (!string.IsNullOrWhiteSpace(user.City))
                    {
                        await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("City", user.City));
                    }


                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(model.ReturnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            var model = new RegisterModel();
            //model.Roles = (from r in _roleManager.Roles
            //               select new SelectListItem
            //               {
            //                   Value = r.Id,
            //                   Text = r.Name
            //               }).ToList();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            model.ReturnUrl = model.ReturnUrl ?? Url.Content("~/admin");

            if (ModelState.IsValid)
            {
                var user = new ExtendedIdentityUser
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address,
                    City = model.City,
                    Country = model.Country,
                    PhoneNumber = model.PhoneNumber
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var role = await _roleManager.FindByIdAsync(model.Role);
                    var roleResult = await _userManager.AddToRoleAsync(user, role.Name);
                    if (roleResult.Succeeded)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "Account",
                            values: new { userId = user.Id, code = code },
                            protocol: Request.Scheme);


                        await _emailSender.SendEmailAsync(model.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(model.ReturnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public IActionResult ConfirmEmail()
        {
            return View();
        }
    }
}