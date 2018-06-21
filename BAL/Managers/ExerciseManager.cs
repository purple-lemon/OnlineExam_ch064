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
    public class ExerciseManager : BaseManager, IExerciseManager 
    {
        public ExerciseManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public IEnumerable<ExerciseDTO> GetAll()
        {
            return mapper.Map<List<ExerciseDTO>>(unitOfWork.ExerciseRepo.GetAll());
        }

        public ExerciseDTO GetById(int id)
        {
            return mapper.Map<ExerciseDTO>(unitOfWork.ExerciseRepo.GetById(id));
        }

        public virtual IEnumerable<ExerciseDTO> Get(Expression<Func<Exercise, bool>> filter = null,
                                     Func<IQueryable<Exercise>,
                                     IOrderedQueryable<Exercise>> orderBy = null,
                                     string includeProperties = "")
        {
            return mapper.Map<List<ExerciseDTO>>(unitOfWork.ExerciseRepo.Get(filter, orderBy, includeProperties));
        }

        public void Insert(ExerciseDTO entity)
        {
            unitOfWork.ExerciseRepo.Insert(mapper.Map<Exercise>(entity));
            unitOfWork.Save();
        }

        public void Update(int id, string taskName, string taskTextField,
                           string taskBaseCodeField, int courseId, string course,
                           DateTime updateDateTime, string testCasesCode)
        {
            var task = unitOfWork.ExerciseRepo.GetById(id);
            task.Course = course;
            task.CourseId = courseId;
            task.TaskName = taskName;
            task.TaskTextField = taskTextField;
            task.TaskBaseCodeField = taskBaseCodeField;
            task.UpdateDateTime = updateDateTime;
            task.TestCasesCode = testCasesCode;
            unitOfWork.ExerciseRepo.Update(task);
            unitOfWork.Save();
        }

        public void DeleteOrRecover(int id)
        {
            var task = unitOfWork.ExerciseRepo.GetById(id);
            task.IsDeleted = !task.IsDeleted;
            unitOfWork.ExerciseRepo.Update(task);
            unitOfWork.Save();
        }

        public void UpdateRating(int id, double rating)
        {
            var task = unitOfWork.ExerciseRepo.GetById(id);
            task.Rating = rating;
            unitOfWork.ExerciseRepo.Update(task);
            unitOfWork.Save();
        }

        public void Delete(ExerciseDTO entityToDelete)
        {
            unitOfWork.ExerciseRepo.Delete(mapper.Map<Exercise>(entityToDelete));
            unitOfWork.Save();
        }
    }
}
