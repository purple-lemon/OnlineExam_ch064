using System.Collections.Generic;
using Model.DTO.CodeDTO;
using Model.Entity;
using System;

namespace Model.DB.Code
{
    public class UserCode
    {
        public int Id { get; set; }
        public string CodeText { get; set; }

        public virtual List<CodeHistory> CodeHistories { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public int ExerciseId { get; set; }
        public virtual Exercise Exercise { get; set; }

        public CodeStatus CodeStatus { get; set; }
        public DateTime EndTime { get; set; }
        public int Mark { get; set; }
        public string TeachersComment { get; set; }

    }

}
