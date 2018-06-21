using Model.DTO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.CoursesViewModels
{
    public class ChangeCourseOwnerViewModel
    {
        public IEnumerable<UserDTO> TeacherList { get; set; }
        [Required]
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public string ResultTeacherId { get; set; }
        public string CurrentOwner { get; set; }
    }
}
