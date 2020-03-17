using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Virtual_Store_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Virtual_Store_WebApp.Controllers
{
    [Authorize]
    public class AccountController:Controller
    {
        UserManager<AdminUser> userManager;
        SignInManager<AdminUser> signInManager;
        public AccountController(UserManager<AdminUser> usrmgr, SignInManager<AdminUser> signinmgr) 
        {
            userManager = usrmgr;
            signInManager = signinmgr;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginAdminUser loginAdminUser)
        {
            if (ModelState.IsValid)
            {
                AdminUser adminUser = await userManager.FindByNameAsync(loginAdminUser.UserName);
                if (adminUser != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await signInManager.PasswordSignInAsync(adminUser, loginAdminUser.Password, false, false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                ModelState.AddModelError(nameof(LoginAdminUser.UserName), "Invalid username or password");
                return View(loginAdminUser);
            }
            else return View();
            
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
    }
}
