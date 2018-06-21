using System;
using System.Collections.Generic;
using System.Text;
using Model.DB;
using Model.DTO;
using System.Linq;
using System.Linq.Expressions;


namespace BAL.Interfaces
{
    public interface IExerciseManager
    {
        IEnumerable<ExerciseDTO> GetAll();
        ExerciseDTO GetById(int id);
        IEnumerable<ExerciseDTO> Get(Expression<Func<Exercise, bool>> filter = null,
                                     Func<IQueryable<Exercise>,
                                     IOrderedQueryable<Exercise>> orderBy = null,
                                     string includeProperties = "");
        void Insert(ExerciseDTO item);
        void Update(int id, string taskName, string taskTextField,
                           string taskBaseCodeField, int courseId, string course,
                           DateTime updateDateTime, string testCasesCode);
        void DeleteOrRecover(int id);
        void UpdateRating(int id, double rating);
        void Delete(ExerciseDTO item);
    }
}
