using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DB
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }

        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
