using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BAL.Interfaces;
using Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class AddNewsController : Controller
    {
        IHostingEnvironment appEnvironment;
        private readonly ICourseManager courseManager;
        private readonly INewsManager newsManager;

        public AddNewsController(ICourseManager courseManager, INewsManager newsManager,
            IHostingEnvironment appEnvironment)
        {
            this.newsManager = newsManager;
            this.courseManager = courseManager;
            this.appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult News(int page = 1, int id = 0)
        {
            int pageSize = 8; 
            List<NewsDTO> news;
            if (id == 0)
            {
                news = newsManager.GetAll().Reverse().ToList();
            }
            else
            {
                news = newsManager.Get().Where(e => e.CourseId == id).Reverse().ToList();
            }
            var count = news.Count();
            var items = news.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var courses = courseManager.GetAll().ToList();

            PageViewModel NewsPageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                NewsPageViewModel = NewsPageViewModel,
                News = items,
                Courses = courses
            };
            return View(viewModel);
        }
        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(IFormFile files, string Text, string Title, string Course)
        {
            // full path to file in temp location
            var filePath = "/images/" + files.FileName;
            

            using (var fileStream = new FileStream(appEnvironment.WebRootPath + filePath, FileMode.Create))
            {
                await files.CopyToAsync(fileStream);
            }
            if (filePath == null)
                filePath = " ";
            var course = courseManager.Get().Where(e => e.Name == Course).FirstOrDefault();

            NewsDTO newsDTO = new NewsDTO()
            {
                Text = Text,
                Title = Title,
                ImagePath = filePath,
                Day = DateTime.Today.Day,
                Month = Enum.GetName(typeof(MonthEnum),
                DateTime.Today.Month - 1),
                CourseId = course.Id
            };

            newsManager.Insert(newsDTO);

            var news = newsManager.GetAll().ToList();
            var courses = courseManager.GetAll().ToList();
            return RedirectToAction("News");

        }
        public void AddArticle()
        {


        }

        public void DeleteOrRecoverArticle(NewsDTO newsDTO)
        {
            newsManager.DeleteOrRecover(newsDTO.Id);
        }

        public void UpdateArticle(NewsDTO newsDTO)
        {
            newsManager.Update(newsDTO);
        }
        public IActionResult ShowNews(int id)
        {
            var news = newsManager.Get().Where(e => e.Id == id).FirstOrDefault();
            return View(news);
        }
    }
}