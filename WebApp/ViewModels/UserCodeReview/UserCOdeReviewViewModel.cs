using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.DTO.CodeDTO;

namespace WebApp.ViewModels.UserCodeReview
{
    public class UserCodeReviewViewModel
    {
        public UserCodeDTO UserCodeDTO { get; set; }
        
        public string ExerciseName { get; set; }
        public string ExerciseTask { get; set; }

    }
}
