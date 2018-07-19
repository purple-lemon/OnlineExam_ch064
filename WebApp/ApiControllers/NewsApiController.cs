using BAL.Interfaces;
using Common;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.ApiControllers
{
	[Produces("application/json")]
	[Route("api/news")]
	public class NewsApiController : Controller
    {
		private readonly ICourseManager courseManager;
		private readonly INewsManager newsManager;
		public NewsApiController(ICourseManager courseManager, INewsManager newsManager)
		{
			this.newsManager = newsManager;
			this.courseManager = courseManager;
		}

		[HttpGet]
		[SwaggerResponse(typeof(List<NewsDTO>))]
		public IActionResult GetAll()
		{
			return Ok(newsManager.GetAll().ToList());
		}

		[HttpPost]
		public IActionResult Post([FromBody]AddNews item)
		{


			var course = courseManager.Get().Where(e => e.Name == item.Course).FirstOrDefault();

			var newsDTO = new NewsDTO()
			{
				Text = item.Text,
				Title = item.Title,
				ImagePath = string.Empty,
				Day = DateTime.Today.Day,
				Month = Enum.GetName(typeof(MonthEnum),
				DateTime.Today.Month - 1),
				CourseId = course.Id
			};

			newsManager.Insert(newsDTO);

			var news = newsManager.GetAll().ToList();
			var courses = courseManager.GetAll().ToList();
			return Ok(newsDTO);

		}
	}
}
