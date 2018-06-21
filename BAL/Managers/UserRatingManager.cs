using System;
using DAL.Interface;
using BAL.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Model.DTO;
using System.Linq.Expressions;
using System.Linq;
using Model.DB;
using AutoMapper;


namespace BAL.Managers
{
    public class UserRatingManager : BaseManager, IUserRatingManager
    {
        public UserRatingManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public List<User> GetAll(UserManager<User> userManager)
        {
            var alluserList = userManager.GetUsersInRoleAsync("Student").Result.ToList();
        

            return alluserList;
        }
    }
}
