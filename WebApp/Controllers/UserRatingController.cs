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
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Controllers
{
    public class UserRatingController : Controller
    {
       
        private readonly IUserRatingManager userRatingManager;
        private readonly IMapper mapper;

        private readonly UserManager<User> userManager;

        public UserRatingController(IUserRatingManager userRatingManager, IMapper mapper, UserManager<User> userManager)
        {
            this.userRatingManager = userRatingManager;
            this.mapper = mapper;
            this.userManager = userManager;
        }

       
        public IActionResult Index()
        {
            var userList = userRatingManager.GetAll(userManager);
            return View(userList);
        }
    }
}