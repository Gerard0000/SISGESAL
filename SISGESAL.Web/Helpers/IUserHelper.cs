using Microsoft.AspNetCore.Identity;
using SISGESAL.web.Data.Entities;
using SISGESAL.web.Enums;
using SISGESAL.web.Models;

namespace SISGESAL.web.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string username);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task CheckRoleAsync(string roleName);

        Task AddUserToRoleAsync(User user, UserType userType);

        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();

        Task<bool> DeleteUserAsync(string username);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        Task<IdentityResult> RemovePasswordAsync(User user);

        Task<IdentityResult> AddPasswordAsync(User user, string newPassword);

        //resetear password
        Task<string> GeneratePasswordResetTokenAsync(User user);

        Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

        //Task RemovePasswordAsync(string? userName);

        //object RemovePasswordAsync(int id);

        //object AddPasswordAsync(int id, string? password);

        //object AddPasswordAsync(ResetPasswordViewModel model1, ResetPasswordViewModel model2);

        //Task<User> GetUserIdAsync(string id);
    }
}