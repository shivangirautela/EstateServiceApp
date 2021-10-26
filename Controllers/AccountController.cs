using EstateServiceApp.Models;
using EstateServiceApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository = null;

        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(IAccountRepository accountRepository,SignInManager<ApplicationUser> signInManager)
        {
            _accountRepository = accountRepository;
            _signInManager = signInManager;
        }
        /// <summary>
        /// This method generates view for sign up of new customers/admins 
        /// </summary>
        /// <returns></returns>
        [Route("signup")]
        [AllowAnonymous]
        public IActionResult Signup(bool isSuccess=false)
        {
            if (isSuccess)
                ViewBag.Status = "You Have Been Successfully Registered. Please Login to continue.";
            else
                ViewBag.Status = null;
            ViewBag.Roles = _accountRepository.GetUserRoles();
            return View();
        }
        /// <summary>
        /// This post method is used to create a new user using identity in asp.net core
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> Signup(SignUpUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(userModel);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    ViewBag.Roles = _accountRepository.GetUserRoles();

                    return View(userModel);
                }

                ModelState.Clear();
                return RedirectToAction(nameof(Signup), new { isSuccess = true });
            }
            ViewBag.Roles = _accountRepository.GetUserRoles();

            return View(userModel);
        }

        /// <summary>
        /// This method is used to get login page for customers
        /// </summary>
        /// <returns></returns>
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// This page authenticates users logged in to the application.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model, string returnUrl,string provider)
        {
            if (ModelState.IsValid)
            {              
                var result = await _accountRepository.PasswordSignInAsync(model);
                if (result.Succeeded)
                {
                    if(!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid Credientials");
            }
            return View(model);
        }

        /// <summary>
        /// This action method used is for logging out from the application
        /// </summary>
        /// <returns></returns>
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}