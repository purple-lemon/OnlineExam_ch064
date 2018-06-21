using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApp.ViewModels.CoursesViewModels;
using BAL.Interfaces;
using AutoMapper;
using System.Threading.Tasks;

namespace WebApp.Controllers
{
    public class CourseManagementController : Controller
    {
        private readonly ICourseManager courseManager;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IExerciseManager exerciseManager;

        public CourseManagementController(ICourseManager courseManager, UserManager<User> userManager, IMapper mapper, IExerciseManager exerciseManager)
        {
            this.courseManager = courseManager;
            this.userManager = userManager;
            this.mapper = mapper;
            this.exerciseManager = exerciseManager;
        }

        public IActionResult Index()
        {
            var coursesList = courseManager.GetAll();
            return View(coursesList);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Create() => View();

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
            return RedirectToAction("Index", "CourseManagement");
        }

        public async Task<IActionResult> ViewCourses()
        {
            var currentTeacher = await userManager.GetUserAsync(HttpContext.User);
            var courseList = courseManager.Get(course => course.UserId == currentTeacher.Id);
            return View(courseList);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editingCourse = courseManager.GetById(id);
            return View(editingCourse);
        }

        [HttpPost]
        public IActionResult Edit(CourseDTO course)
        {
            if (ModelState.IsValid)
            {
                courseManager.Update(course);
            }
            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("ViewCourses", "CourseManagement");
            }
            return RedirectToAction("Index", "CourseManagement");
        }

        [HttpGet]
        [Authorize(Roles = "Teacher, Administrator")]
        public IActionResult Toggle(int id)
        {
            courseManager.ToggleCourseStatus(id);
            if (User.IsInRole("Teacher"))
            {
                return RedirectToAction("ViewCourses", "CourseManagement");
            }
            return RedirectToAction("Index", "CourseManagement");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult ChangeOwner(int id)
        {
            var teacherList = mapper.Map<List<UserDTO>>(userManager.GetUsersInRoleAsync("Teacher").Result);
            var course = courseManager.GetById(id);
            return View(new ChangeCourseOwnerViewModel()
            {
                TeacherList = teacherList,
                CourseId = course.Id,
                CourseName = course.Name,
                CurrentOwner = teacherList.Find(c => c.Id == course.UserId).UserName
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult ChangeOwner(ChangeCourseOwnerViewModel model)
        {
            courseManager.UpdateCourseOwner(model.CourseId, model.ResultTeacherId);
            return RedirectToAction("Index", "CourseManagement");
        }

        [HttpGet]
        public IActionResult ShowExercise(int id)
        {
            var taskList = exerciseManager.Get(x => x.CourseId == id);
            return View(taskList);
        }
    }
}