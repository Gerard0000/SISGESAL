using Microsoft.AspNetCore.Identity;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Data;
using SISGESAL.web.Enums;
using SISGESAL.web.Models;
using Microsoft.EntityFrameworkCore;

namespace SISGESAL.web.Helpers
{
    public class UserHelper(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : IUserHelper
    {
        private readonly DataContext _context = context;
        private readonly UserManager<User>? _userManager = userManager;
        private readonly RoleManager<IdentityRole>? _roleManager = roleManager;
        private readonly SignInManager<User> _signInManager = signInManager;

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username!,
                model.Password!,
                model.RememberMe,
                //TODO: CUANTOS INTENTOS SE BLOQUEA LA CUENTA
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager!.CreateAsync(user, password);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            bool roleExists = await _roleManager!.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            return user!;
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager!.IsInRoleAsync(user, roleName);
        }

        public async Task AddUserToRoleAsync(User user, UserType userType)
        {
            await _userManager!.AddToRoleAsync(user, userType.ToString());
        }

        public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager!.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await GetUserAsync(username);
            if (user == null)
            {
                return true;
            }
            var response = await _userManager!.DeleteAsync(user);
            return response.Succeeded;
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager!.UpdateAsync(user);
        }

        //public async Task<User> GetUserIdAsync(string id)
        //{
        //    var iduser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        //    return iduser!;
        //}

        //public Task<IdentityResult> RemovePasswordAsync(User user)
        //{
        //    return _userManager!.RemovePasswordAsync(user);
        //}
    }
}