using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApp.Models;
using WebApp.ViewModels.CommentViewModel;
using WebApp.ViewModels;
using Model.DB;
using Model.DTO;
using Microsoft.AspNetCore.Authorization;
using DAL.Interface;
using BAL.Interfaces;
using WebApp.ViewModels.CoursesViewModels;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestSharp;
using Microsoft.Extensions.Configuration;


namespace WebApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentManager commentManager;
        private readonly IExerciseManager exerciseManager;
        private readonly IConfiguration configuration;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public CommentController(IExerciseManager exerciseManager, ICourseManager courseManager,
                                            UserManager<User> userManager, ICommentManager commentManager, IMapper mapper, IConfiguration configuration)
        {
            this.exerciseManager = exerciseManager;
            this.userManager = userManager;
            this.commentManager = commentManager;
            this.configuration = configuration;
            this.mapper = mapper;
        }


        [HttpPost]
        public ActionResult Create(CreateCommentViewModel model)
        {
            var currentUser = userManager.GetUserAsync(HttpContext.User).Result.Id;

            if (ModelState.IsValid)
            {
                var client = new RestClient(configuration.GetConnectionString("WebAppRouteAPI"));
                var request = new RestRequest("api/CommentApi/", Method.POST);

                CommentDTO comment = new CommentDTO
                {
                    ExerciseId = model.ExerciseId,
                    UserId = currentUser,
                    UserName = User.Identity.Name,
                    CommentText = model.CommentText,
                    Rating = model.Rating,
                };
                request.AddObject(comment);
                client.Execute<CommentDTO>(request);
            }
            return RedirectToAction("TaskView ", "ExerciseManagement", model.ExerciseId);
        }

    }
}