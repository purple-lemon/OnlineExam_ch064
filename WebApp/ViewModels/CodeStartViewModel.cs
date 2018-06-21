using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model.DTO.CodeDTO;

namespace WebApp.ViewModels
{
    public class CodeStartViewModel
    {
        public UserCodeDTO UserCodeDTO { get; set; }
        public string ExerciseTaskText { get; set; }
    }
}
