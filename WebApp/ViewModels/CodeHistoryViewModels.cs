using Model.DB;
using Model.DB.Code;
using Model.DTO;
using Model.DTO.CodeDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class CodeHistoryViewModels
    {
        public List<CodeHistory> CodeHistory;
        public List<ExerciseDTO> Exercises;
        public List<UserCodeDTO> UserCode;
        public string UserName;
    }
}
