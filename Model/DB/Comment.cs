using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.DB
{
    public class Comment
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string CommentText { get; set; }

        public DateTime CreationDateTime { get; set; }

        public int ExerciseId { get; set; }

        public int? Rating { get; set; } 
    }
}
