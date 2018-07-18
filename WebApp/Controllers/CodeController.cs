using BAL.Interfaces;
using BAL.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DB;
using WebApp.ViewModels;
using WebApp.ViewModels.UserCodeReview;
using Model.DTO.CodeDTO;
using Microsoft.AspNetCore.Authorization;
using RestSharp;
using Microsoft.Extensions.Configuration;

namespace WebApp.Controllers
{
    [Authorize]
    public class CodeController : BaseWebController
	{
        private CodeManager codeManager;
        private IExerciseManager exerciseManager;
        private UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public CodeController(CodeManager codeManager, IExerciseManager exerciseManager, UserManager<User> userManager, IConfiguration configuration)
        {
            this.codeManager = codeManager;
            this.exerciseManager = exerciseManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }
        public IActionResult Index(UserCodeDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("../Home/Index");
            }
            var exercise = exerciseManager.GetById(model.ExerciseId);
            return View(new CodeStartViewModel() {
                UserCodeDTO = codeManager.BuildCodeModel(model),
                ExerciseTaskText = exercise.TaskTextField
        });
        }

        [HttpPost]
        public string GetCode(UserCodeDTO model)
        {
            return model.CodeText == null ? "Write some code" : codeManager.ExecuteCode(model);
        }


        [HttpPost]
        public string ExecuteOnFlyCode(UserCodeDTO model)
        {
            return model.CodeText == null ? "Write some codeeeee" : codeManager.GetOnFlyCode(model);
        }

        [HttpPost]
        public string TestRun(TestRunViewModel code)
        {
            return code == null ? "Write some codeeeee" : codeManager.ExecuteOnFlyCode(code.TestRunCode);
        }

        [HttpPost]
        public IActionResult SetCodeStatus(UserCodeViewModel model)
        {
            var code = codeManager.UserCodeByExId(model.UserId, model.ExerciseId);
            if (code != null)
            {
                codeManager.SetCodeStatus(code.Id, code.UserId);
            }
            return RedirectToAction("Index", "CourseManagement");
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public IActionResult CodeReview(int id)
        {
            var client = new RestClient(configuration.GetConnectionString("WebAppRouteAPI"));
            var request = new RestRequest("api/CodeAPI/{id}", Method.GET);
            request.AddUrlSegment("id", id);
            IRestResponse<UserCodeReviewViewModel> response = client.Execute<UserCodeReviewViewModel>(request);
            return View(response.Data);
        }

        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public IActionResult CodeMarking(UserCodeReviewViewModel model)
        {
            var client = new RestClient(configuration.GetConnectionString("WebAppRouteAPI"));
            var request = new RestRequest("api/CodeAPI", Method.POST);
            request.AddParameter("userCodeId", model.UserCodeDTO.Id);
            request.AddParameter("userCodeMark", model.UserCodeDTO.Mark);
            request.AddParameter("userCodeTeachersComment", model.UserCodeDTO.TeachersComment);
            request.AddParameter("userCodeDTOUserId", model.UserCodeDTO.UserId);
            IRestResponse response = client.Execute(request);
            return RedirectToAction("Index", "ExerciseManagement");
        }

    }
}