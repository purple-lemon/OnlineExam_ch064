using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApp.ViewModels.UserCodeReview;

namespace WebApp.ApiControllers
{
    [Produces("application/json")]
    [Route("api/CodeAPI")]
    public class CodeAPIController : Controller
    {
        private readonly ICodeManager codeManager;
        private readonly IExerciseManager exerciseManager;

        public CodeAPIController(ICodeManager codeManager, IExerciseManager exerciseManager)
        {
            this.codeManager = codeManager;
            this.exerciseManager = exerciseManager;
        }

        [HttpGet("{id}")]
        public UserCodeReviewViewModel Get(int id)
        {
            var userCode = codeManager.Get(c => c.Id == id).First();
            var exercise = exerciseManager.GetById(userCode.ExerciseId);
            return new UserCodeReviewViewModel()
            {
                UserCodeDTO = userCode,
                ExerciseName = exercise.TaskName,
                ExerciseTask = exercise.TaskTextField
            };
        }

        [HttpPost]
        public void Post(int userCodeId, int userCodeMark, string userCodeTeachersComment, string userCodeDTOUserId)
        {
            codeManager.SetMark(userCodeId, userCodeMark, userCodeTeachersComment, userCodeDTOUserId);
        }
    }
}