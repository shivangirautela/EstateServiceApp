using EstateServiceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateServiceApp.Repository
{
    /// <summary>
    /// This repository manages login and logout features of the application
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        /// <summary>
        /// This constructor takes usermanager and signinmanager as arguments to perform security using identity
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        public AccountRepository(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        /// <summary>
        /// This method returns all roles available for users to select from.
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetUserRoles()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var role in _roleManager.Roles)
            {
                list.Add(new SelectListItem() { Value = role.Name, Text = role.Name});
            }
            return list;
        }
        /// <summary>
        /// This method helps in creating new user with their creditials
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateUserAsync(SignUpUserModel userModel)
        {
            var user = new ApplicationUser()
            {
                FirstName=userModel.FirstName,
                LastName=userModel.LastName,
                UserName = userModel.Email,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber.ToString()
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if(result.Succeeded)
                await _userManager.AddToRoleAsync(user, userModel.RoleName);
            return result;
        }
        /// <summary>
        /// This method helps to sign in the users based on user email and password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SignInResult> PasswordSignInAsync(SignInModel model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        }
        /// <summary>
        /// This method signs out the user from the application
        /// </summary>
        /// <returns></returns>
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
