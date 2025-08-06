using Microsoft.AspNetCore.Mvc;
using SISGESAL.web.Helpers;
using SISGESAL.web.Models;

namespace SISGESAL.web.Controllers
{
    public class AccountController(IUserHelper userHelper/*, DataContext dataContext*/) : Controller
    {
        public bool IsPostBack { get; private set; }
        private readonly IUserHelper _userHelper = userHelper;
        //private readonly DataContext _dataContext = dataContext;

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated!)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First()!);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError(string.Empty, "Usuario o contraseña no valido.");
            model.Password = string.Empty;
            return View(model);
        }

        public async Task<IActionResult> Logout(LoginViewModel model)
        {
            //************agregado por el sistema***********
            ArgumentNullException.ThrowIfNull(model);
            //**********************************************

            await _userHelper.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserAsync(User.Identity!.Name!);
                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword!, model.NewPassword!);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault()!.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario no Encontrado.");
                }
            }

            return View(model);
        }
    }
}