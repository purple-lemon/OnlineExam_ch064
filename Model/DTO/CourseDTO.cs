using System;
using System.ComponentModel.DataAnnotations;
using Model.DB;

namespace Model.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }

        public string UserId { get; set; }
        public UserDTO User { get; set; }    
    }
}
