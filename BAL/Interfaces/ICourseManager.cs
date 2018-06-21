using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BAL.Interfaces
{
    public interface ICourseManager
    {
        /// <summary>
        /// Returns list of all courses
        /// </summary>
        /// <returns></returns>
        IEnumerable<CourseDTO> GetAll();
        /// <summary>
        /// Returns a course with a given Id
        /// </summary>
        /// <param name="id">Id of course</param>
        /// <returns></returns>
        CourseDTO GetById(int id);
        /// <summary>
        /// Returns courses with given filters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<CourseDTO> Get(
            Expression<Func<Course, bool>> filter = null,
            Func<IQueryable<Course>,
            IOrderedQueryable<Course>> orderBy = null,
            string includeProperties = "");
        /// <summary>
        /// Insert a new course into DB
        /// </summary>
        /// <param name="item"></param>
        void Insert(CourseDTO item);
        /// <summary>
        /// Edit an existing course
        /// </summary>
        /// <param name="item"></param>
        void Update(CourseDTO item);
        /// <summary>
        /// Change course owner
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        void UpdateCourseOwner(int courseId, string teacherId);
        /// <summary>
        /// Change course status
        /// </summary>
        /// <param name="id"></param>
        void ToggleCourseStatus(int id);
        /// <summary>
        /// Delete an existing from DB
        /// </summary>
        /// <param name="item"></param>
        void Delete(CourseDTO item);
    }
}
