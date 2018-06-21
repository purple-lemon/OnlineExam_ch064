using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels.CommentViewModel
{
    public class CreateCommentViewModel
    {
        public int ExerciseId { get; set; }

        public string UserName { get; set; }

        public int? Rating { get; set; }

        public string CommentText { get; set; }
    }
}
