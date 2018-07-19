using AutoMapper;
using BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ApiControllers
{
	[Produces("application/json")]
	[Route("api/Course")]
	public class CourseController : Controller
    {
		private readonly ICourseManager courseManager;
		private readonly UserManager<User> userManager;
		private readonly IMapper mapper;
		private readonly IExerciseManager exerciseManager;

		public CourseController(ICourseManager courseManager, UserManager<User> userManager, IMapper mapper, IExerciseManager exerciseManager)
		{
			this.courseManager = courseManager;
			this.userManager = userManager;
			this.mapper = mapper;
			this.exerciseManager = exerciseManager;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var coursesList = courseManager.GetAll();
			return Ok();
		}

		[HttpPost]
		[Authorize(Roles = "Teacher")]
		public IActionResult Create(CourseDTO model)
		{
			var user = userManager.GetUserAsync(HttpContext.User);

			model.CreationDate = DateTime.Now;
			model.IsActive = true;
			model.UserId = user.Result.Id;
			if (ModelState.IsValid)
			{
				courseManager.Insert(model);
			}
			return Ok(model);
		}

		[Route("delete")]
		public IActionResult Delete(int id)
		{
			courseManager.Delete(new CourseDTO() { Id = id });
			return Ok();
		}


	}
}
