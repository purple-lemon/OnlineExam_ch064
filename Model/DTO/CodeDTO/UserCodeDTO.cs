using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Model.DB;
using Model.DB.Code;
using Model.Entity;

namespace Model.DTO.CodeDTO
{
    public class UserCodeDTO
    {
        public int Id { get; set; }
        public string CodeText { get; set; }
        
        public List<CodeHistoryDTO> CodeHistories { get; set; }
        
        [Required]
        public string UserId { get; set; }
        public UserDTO User { get; set; }

        [Required]
        public int ExerciseId { get; set; }
        public ExerciseDTO Exercise { get; set; }

        public CodeStatus CodeStatus { get; set; }
        public DateTime EndTime { get; set;  } 
        public int Mark { get; set; }
        public string TeachersComment { get; set; }
    }
}
