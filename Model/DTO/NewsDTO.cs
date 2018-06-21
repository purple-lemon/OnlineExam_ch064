using Model.DB;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTO
{
    public class NewsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImagePath { get; set; }
        public int Day { get; set; }
        public string Month { get; set; }
        public bool IsDeleted { get; set; }

        public int CourseId { get; set; }
        public CourseDTO Course { get; set; }
    }
}
