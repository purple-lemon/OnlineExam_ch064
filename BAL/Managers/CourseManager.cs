using System;
using DAL.Interface;
using BAL.Interfaces;
using System.Collections.Generic;
using Model.DTO;
using System.Linq.Expressions;
using System.Linq;
using Model.DB;
using AutoMapper;

namespace BAL.Managers
{
    public class CourseManager : BaseManager, ICourseManager
    {           
        public CourseManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public IEnumerable<CourseDTO> GetAll()
        {
            return mapper.Map<List<CourseDTO>>(unitOfWork.CourseRepo.GetAll());
        }
       
        public CourseDTO GetById(int id)
        {
            return mapper.Map<CourseDTO>(unitOfWork.CourseRepo.GetById(id));
        }

        public virtual IEnumerable<CourseDTO> Get(
            Expression<Func<Course, bool>> filter = null,
            Func<IQueryable<Course>,
            IOrderedQueryable<Course>> orderBy = null,
            string includeProperties = "")
        {
            return mapper.Map<List<CourseDTO>>(unitOfWork.CourseRepo.Get(filter, orderBy, includeProperties));
        }

        public void Insert(CourseDTO entity)
        {
            unitOfWork.CourseRepo.Insert(mapper.Map<Course>(entity));
            unitOfWork.Save();
        }

        public void Update(CourseDTO entityToUpdate)
        {
            unitOfWork.CourseRepo.Update(mapper.Map<Course>(entityToUpdate));
            unitOfWork.Save();
        }

        public void ToggleCourseStatus(int id)
        {
            var cource = unitOfWork.CourseRepo.GetById(id);
            cource.IsActive = !cource.IsActive;
            unitOfWork.CourseRepo.Update(cource);
            unitOfWork.Save();
        }

        public void UpdateCourseOwner(int courseId, string teacherId)
        {
            var cource = unitOfWork.CourseRepo.GetById(courseId);
            cource.UserId = teacherId;
            unitOfWork.CourseRepo.Update(cource);
            unitOfWork.Save();
        }

        public void Delete(CourseDTO entityToDelete)
        {
            unitOfWork.CourseRepo.Delete(mapper.Map<Course>(entityToDelete));
            unitOfWork.Save();
        }
    }
}
