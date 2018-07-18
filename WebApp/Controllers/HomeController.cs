using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BAL.Interfaces;
using DAL;
using DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Model.DTO;
using Model.DTO.CodeDTO;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class HomeController : BaseWebController
	{
        private readonly INewsManager newsManager;
        public HomeController(INewsManager newsManager)
        {
            this.newsManager = newsManager;
        }
        public IActionResult Index(int page = 1, int id = 0)
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

            PageViewModel NewsPageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                NewsPageViewModel = NewsPageViewModel,
                News = items
            };
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ShowNews(int id)
        {
            var news = newsManager.Get().Where(e => e.Id == id).FirstOrDefault();
            return View(news);
        }
    }
}
