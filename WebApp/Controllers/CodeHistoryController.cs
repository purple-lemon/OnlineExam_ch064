using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BAL.Interfaces;
using BAL.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Model.DB;
using Model.DB.Code;
using Model.DTO.CodeDTO;
using Model.Entity;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CodeHistoryController : Controller
    {
        private readonly ICodeManager codeManager;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly IExerciseManager exerciseManager;
        private readonly ICourseManager courseManager;

        public CodeHistoryController(ICodeManager codeManager, IMapper mapper, UserManager<User> userManager, IExerciseManager exerciseManager, ICourseManager courseManager)
        {
            this.userManager = userManager;
            this.codeManager = codeManager;
            this.mapper = mapper;
            this.exerciseManager = exerciseManager;
            this.courseManager = courseManager;
        }

        public IActionResult History()
        {
            try
            {
                var userName = User.Identity.Name;
                var user = userManager.Users.Where(e => e.UserName == userName).FirstOrDefault();

                var codelst = codeManager.Get().Where(e => e.UserId == user.Id).ToList();
                var history = codeManager.GetAll().ToList();
                var exercise = exerciseManager.GetAll().ToList();

                return View(new CodeHistoryViewModels
                {
                    CodeHistory = history,
                    Exercises = exercise,
                    UserCode = codelst,
                    UserName = user.UserName
                });
            }
            catch(Exception ex)
            {

            }
            return View();
        }
        [HttpPost]
        public CodeModel EditCode(CodeModel codeModel)
        {
            codeManager.EditCode(codeModel);
            return codeModel;
        }
        [HttpPost]
        public IActionResult SendCodeToExecute(UserCodeDTO codeModel)
        {
            return RedirectToAction("Index", "Code", codeModel);
        }
        [HttpPost]
        public SetFav SetFav(SetFav model)
        {
            codeManager.SetFavouriteCode(model);
            return model;
        }
        
    }
}