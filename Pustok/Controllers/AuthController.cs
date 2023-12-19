using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication1.Models;
using WebApplication1.ViewModels.AuthVM;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
		SignInManager<AppUser> _signInManager { get; }
		UserManager<AppUser> _userManager { get; }
		RoleManager<IdentityRole> _roleManager { get; }

		public AuthController(SignInManager<AppUser> signInManager,
			UserManager<AppUser> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			var result = await _userManager.CreateAsync(new AppUser
			{
				Fullname = vm.Fullname,
				Email = vm.Email,
				UserName = vm.Username
			}, vm.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View(vm);
			}
			//var roleResult = await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
			//if (!roleResult.Succeeded)
			//{
			//	ModelState.AddModelError("", "Something went wrong. Please contact admin");
			//	return View(vm);
			//}
			return View();
		}
    }
}
