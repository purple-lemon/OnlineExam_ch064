using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<NewsDTO> News { get; set; }
        public IEnumerable<CourseDTO> Courses { get; set; }
        public IEnumerable<MessagesDTO> InBoxMessages { get; set; }
        public IEnumerable<MessagesDTO> OutBoxMessages { get; set; }
        public PageViewModel NewsPageViewModel { get; set; }
        public PageViewModel InBoxMessagesPageViewModel { get; set; }
        public PageViewModel OutBoxMessagesPageViewModel { get; set; }


    }
}
