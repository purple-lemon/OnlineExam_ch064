using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Model.DTO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class UpdateExerciseViewModel
    {
        public int Id { get; set; }

        [Required]
        public string TaskName { get; set; }

        [Required]
        public int CourseId { get; set; }

        public string TaskTextField { get; set; }

        public string TaskBaseCodeField { get; set; }

        public string TestCases { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime UpdateDateTime { get; set; }

        public IEnumerable<CourseDTO> CourseList { get; set; }

    }
}
