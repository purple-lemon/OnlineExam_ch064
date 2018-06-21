using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.DB
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
