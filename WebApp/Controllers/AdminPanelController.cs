using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebApp.Models;
using WebApp.ViewModels;
using Model.DB;
using Model.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BAL.Interfaces;
using BAL.Managers;
using Model.DB.Code;
using Model.DTO.CodeDTO;
using Model.Entity;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminPanelController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IExerciseManager exerciseManager;
        private readonly ICourseManager courseManager;
        private readonly ICodeManager codeManager;

        IMapper mapper;

        public AdminPanelController(ICodeManager codeManager, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IExerciseManager exerciseManager, ICourseManager courseManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.codeManager = codeManager;
            this.mapper = mapper;
            this.exerciseManager = exerciseManager;
            this.courseManager = courseManager;
        }
        

        public IActionResult Users()
        {
            var t = this.mapper.Map<List<UserDTO>>(this.userManager.Users.ToList());
            return View(t);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await this.userManager.FindByIdAsync(id);
            var code = codeManager.Get().Where(e => e.UserId == user.Id).FirstOrDefault();
            
            if (user != null)
            {
                codeManager.DeleteHistoryLst(code);
                codeManager.DeleteCode(code);
                await this.userManager.DeleteAsync(user);
            }
            return RedirectToAction("Users");
        }


        
       
        public IActionResult Roles() => View(this.roleManager.Roles.ToList());

        

        public IActionResult CreateRole() => View();
        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await this.roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }
        
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await this.roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await this.roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Roles");
        }

        public async Task<IActionResult> DeleteRoleFromUser(string userId, List<string> roles)
        {
            User user = await this.userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await this.userManager.GetRolesAsync(user);
                var addedRoles = roles.Except(userRoles);

                await this.userManager.RemoveFromRolesAsync(user, addedRoles);

                return RedirectToAction("EditRole");
            }
            return NotFound();
        }

        public async Task<IActionResult> EditRole(string userId)
        {
            User user = await this.userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await this.userManager.GetRolesAsync(user);
                var allRoles = this.roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(string userId, List<string> roles)
        {
            User user = await this.userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await this.userManager.GetRolesAsync(user);
                var allRoles = this.roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                await this.userManager.AddToRolesAsync(user, addedRoles);
                await this.userManager.RemoveFromRolesAsync(user, removedRoles);
                return RedirectToAction("Users");
            }

            return NotFound();
        }

    }
}