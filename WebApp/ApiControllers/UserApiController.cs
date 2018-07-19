using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.ApiControllers
{
	[Produces("application/json")]
	[Route("api/CodeAPI")]
	public class UserApiController  :Controller
    {
		private readonly UserManager<User> userManager;
		private readonly IMapper mapper;

		public UserApiController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager)
		{
			this.userManager = userManager;
			this.mapper = mapper;
		}

		[HttpPost]
		public IActionResult ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			User user = userManager.FindByIdAsync(model.Id).Result;
			if (user == null)
			{
				return BadRequest("User with provided id can't be found");
			}

			IdentityResult result = userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword).Result;
			if (result.Succeeded)
			{
				return Ok(new
				{
					Id = model.Id,
					Status = "Success"
				});
			} else
			{
				return StatusCode(500, new { Status = "error changing pass" });
			}
		}

		[HttpPost]
		public IActionResult ChangeUserName(ChangeUserNameViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			User user = userManager.FindByIdAsync(model.Id).Result;
			IdentityResult userNameResult = userManager.SetUserNameAsync(user, model.NewUserName).Result;

			if (!userNameResult.Succeeded)
			{
				foreach (var error in userNameResult.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
					return BadRequest(model);
				}
			}

			bool passwordResult = userManager.CheckPasswordAsync(user, model.Password).Result;
			if (!passwordResult)
			{
				return BadRequest("Incorrect password");
			}
			return Ok(new { Status = "Success" });
		}

		[HttpPost]
		public IActionResult ChangeEmail(ChangeEmailViewModel model)
		{
			User user = userManager.FindByIdAsync(model.Id).Result;
			IdentityResult emailResult = userManager.SetEmailAsync(user, model.NewEmail).Result;
			if (!emailResult.Succeeded)
			{
				foreach (var error in emailResult.Errors)
				{
					return BadRequest(model);
				}
			}
			bool passwordResult = userManager.CheckPasswordAsync(user, model.Password).Result;
			if (!passwordResult)
			{
				return BadRequest("Incorrect password");
			}

			return Ok(new { Status = "Success" });
		}
	}
}
