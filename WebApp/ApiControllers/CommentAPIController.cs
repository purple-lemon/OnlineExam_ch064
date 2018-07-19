using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApp.Models;
using WebApp.ViewModels;
using WebApp.ViewModels.UserCodeReview;
using Model.DB;
using Model.DTO;
using Microsoft.AspNetCore.Authorization;
using DAL.Interface;
using BAL.Interfaces;
using WebApp.ViewModels.CoursesViewModels;
using AutoMapper;
using RestSharp;
using System.Reflection;
using Newtonsoft.Json;
using NSwag.Annotations;

namespace WebApp.ApiControllers
{
    [Produces("application/json")]
    [Route("api/CommentApi")]
    public class CommentApiController : ControllerBase
    {
        private readonly IExerciseManager exerciseManager;
        private readonly ICourseManager courseManager;
        private readonly UserManager<User> userManager;
        private readonly ICommentManager commentManager;
        private readonly ICodeManager codeManager;
        private readonly IMapper mapper;

        public CommentApiController(IExerciseManager exerciseManager, ICourseManager courseManager,
                                            UserManager<User> userManager, ICodeManager codeManager, IMapper mapper, ICommentManager commentManager)
        {
            this.exerciseManager = exerciseManager;
            this.courseManager = courseManager;
            this.userManager = userManager;
            this.commentManager = commentManager;
            this.mapper = mapper;
            this.codeManager = codeManager;
        }

		[HttpGet]
		[SwaggerResponse(typeof(IEnumerable<CommentDTO>))]
		public IActionResult GetAll(int id)
		{

			var json = commentManager.GetAll();
			return Ok(json);
		}

		// GET: api/CommentAPI
		[HttpGet]
		[Route("excercise/{id}")]
		[SwaggerResponse(typeof(IEnumerable<CommentDTO>))]
        public IActionResult Get(int excerciseId)
        {

            var json = commentManager.Get(c => c.ExerciseId == excerciseId).ToList();
            return Ok(json);
        }
       
        // POST: api/CommentAPI
        [HttpPost]
        public IActionResult Post(CommentDTO comment)
        {
            comment.CreationDateTime = DateTime.Now;
            commentManager.Insert(comment);
			if (comment.Rating != null)
			{
				var commentlist = commentManager.Get(g => g.ExerciseId == comment.ExerciseId && g.Rating != 0).ToList();
				double average = 0;
				foreach (var elem in commentlist)
				{
					if (elem.Rating != 0)
						average += Convert.ToDouble(elem.Rating);
				}

				average = average / commentlist.Count;
				if (average >= 0)
					exerciseManager.UpdateRating(comment.ExerciseId, average);

			}
			return Ok();
		}



    }
}
