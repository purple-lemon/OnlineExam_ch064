using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApp.ViewModels;
using AutoMapper;
using BAL.Interfaces;
using Model.DB;
using Model.DTO;
using RestSharp;
using Microsoft.Extensions.Configuration;


namespace WebApp.ApiControllers
{
    [Produces("application/json")]
    [Route("api/ExerciseManagementAPI")]
    public class ExerciseManagementAPIController : Controller
    {
        private readonly IExerciseManager exerciseManager;
        private readonly ICourseManager courseManager;
        private readonly UserManager<User> userManager;
        private readonly ICommentManager commentManager;
        private readonly IConfiguration configuration;
        private readonly ICodeManager codeManager;
        private readonly IMapper mapper;

        public ExerciseManagementAPIController(IExerciseManager exerciseManager, ICourseManager courseManager,
                                            UserManager<User> userManager, ICodeManager codeManager, IMapper mapper, ICommentManager commentManager, IConfiguration configuration)
        {
            this.exerciseManager = exerciseManager;
            this.courseManager = courseManager;
            this.userManager = userManager;
            this.commentManager = commentManager;
            this.configuration = configuration;
            this.mapper = mapper;
            this.codeManager = codeManager;
        }


        [HttpGet("{id}")]
        public GetExerciseViewModel Get(int id)
        {
            var task = exerciseManager.GetById(id);

            var client = new RestClient(configuration.GetConnectionString("WebAppRouteAPI"));
            var request = new RestRequest("api/CommentApi/{id}", Method.GET);
            request.AddUrlSegment("id", id);
            IRestResponse<List<CommentDTO>> response = client.Execute<List<CommentDTO>>(request);
           // var commentList = response.Data;
           var all = new GetExerciseViewModel()
           {
               Id = id,
               Course = task.Course,
               CommentList = response.Data,
               TaskName = task.TaskName,
               TaskTextField = task.TaskTextField,
               TaskBaseCodeField = task.TaskBaseCodeField
           };
            return all;
        }
    }
}