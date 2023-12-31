﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication1.Helpers;
using WebApplication1.Models;
using WebApplication1.ViewModels.AuthVM;
using WebApplication1.ExternalServices.Interfaces;
using IEmailService = WebApplication1.ExternalServices.Interfaces.IEmailService;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    {
		SignInManager<AppUser> _signInManager { get; }
		UserManager<AppUser> _userManager { get; }
		RoleManager<IdentityRole> _roleManager { get; }
		IEmailService _emailService { get; }

        public AuthController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public IActionResult Login()
        {
            return View();
        }
		[HttpPost]
        public async Task<IActionResult> Login(string? returnUrl, LoginVM vm)
        {
            AppUser user;
            if (vm.UsernameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
            }
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is wrong");
                return View(vm);
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
            vm.IsConfirmed = user.EmailConfirmed;
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Too many attempts wait until " + DateTime.Parse(user.LockoutEnd.ToString()).ToString("HH:mm"));
                }
                else if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("", "Your email isn't confirmed");
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
                return View(vm);
            }
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
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
            var user = new AppUser
            {
                Fullname = vm.Fullname,
                Email = vm.Email,
                UserName = vm.Username
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View(vm);
			}
            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
            if (!roleResult.Succeeded)
            {
                ModelState.AddModelError("", "Something went wrong. Please contact admin");
                return View(vm);
            }
            await _sendConfirmation(user);
            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<bool> CreateRoles()
        {
            foreach (var item in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole
                    { 
                        Name = item.ToString(),
                    });
                    if (!result.Succeeded)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult SendMail()
        {
            _emailService.Send("nihad.ceferov7@gmail.com", "Salam", "Bu bir test mesajidir");
            return Ok();
        }

        public async Task<IActionResult> SendConfirmationEmail(string username)
        {
            await _sendConfirmation(await _userManager.FindByNameAsync(username));
            return Content("Email sent");
        }

        async Task _sendConfirmation(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("EmailConfirmed", "Auth", new
            {
                token = token,
                username = user.UserName
            }, Request.Scheme);
            _emailService.Send(user.Email, "Welcome to Pustok", "Congratulations, your account has been successfully created. - Nihad", true);
        }
        public async Task<IActionResult> EmailConfirmed(string token, string username)
        {
            var result = await _userManager.ConfirmEmailAsync(await _userManager.FindByNameAsync(username), token);
            if (result.Succeeded)
            {
                return Ok();
            } 
            return Problem();
        }
    }
}
