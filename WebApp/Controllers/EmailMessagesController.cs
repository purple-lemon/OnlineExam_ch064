using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BAL;
using BAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DB;
using Model.DTO;
using Model.Entity;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class EmailMessagesController : BaseWebController
	{
        private readonly IMessagesManager messagesManager;
        private readonly IMapper mapper;

        public EmailMessagesController(IMessagesManager messagesManager, IMapper mapper)
        {
            this.messagesManager = messagesManager;
            this.mapper = mapper;
        }
        [Authorize(Roles = "Teacher, Administrator")]
        public IActionResult Index(int page = 1)
        {
            int pageSize = 8;   // количество элементов на странице

            var InBoxmessages = messagesManager.Get().Where(e => e.InboxText != null).Reverse().ToList();
            var OutBoxmessages = messagesManager.Get().Where(e => e.OutboxText != null).Reverse().ToList();
            var InBoxcount = InBoxmessages.Count();
            var OutBoxcount = OutBoxmessages.Count();
            var InBoxItems = InBoxmessages.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var IOutBoxItems = OutBoxmessages.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel InBoxMessagesPageViewModel = new PageViewModel(InBoxcount, page, pageSize);
            PageViewModel IOutBoxMessagesPageViewModel = new PageViewModel(OutBoxcount, page, pageSize);

            IndexViewModel viewModel = new IndexViewModel
            {
                InBoxMessagesPageViewModel = InBoxMessagesPageViewModel,
                OutBoxMessagesPageViewModel = IOutBoxMessagesPageViewModel,
                InBoxMessages = InBoxItems,
                OutBoxMessages = IOutBoxItems
            };
            return View(viewModel);
        }
        public IActionResult GetMessage(string Email, string Message)
        {
            try
            {
                var OutBoxmessages = messagesManager.Get().Where(e => e.OutboxText != null).LastOrDefault();
                OutBoxmessages.IsInBox = true;
                messagesManager.Update(OutBoxmessages);
            }
            catch (Exception ex)
            {

            }
           
            messagesManager.Insert(new MessagesDTO() { FromEmail = Email, InboxText = Message, Date = DateTime.Now, IsNew = true, IsInBox = true });
            return RedirectToAction("GetEmail");
        }
        public IActionResult SendEmail()
        {
            return View();
        }
        public IActionResult GetEmail()
        {
            return View();
        }
        public IActionResult Open(bool flag)
        {
            try
            {
                bool isInbox = flag;

                var InBoxmessages = messagesManager.Get().Where(e => e.InboxText != null).LastOrDefault();
                var OutBoxmessages = messagesManager.Get().Where(e => e.OutboxText != null).LastOrDefault();
                InBoxmessages.IsInBox = isInbox;
                OutBoxmessages.IsInBox = isInbox;
                var NewMessages = messagesManager.Get().Where(e => e.IsNew == true).ToList();
                messagesManager.Update(InBoxmessages);
                foreach (var n in NewMessages)
                {
                    n.IsNew = false;
                    messagesManager.Update(n);
                }
                messagesManager.Update(OutBoxmessages);
            }
            catch(Exception ex)
            {

            }
            return RedirectToAction("Index");
        }
        public IActionResult Send(string Email, string Message, string Subject)
        {
            try
            {
                var InBoxmessages = messagesManager.Get().Where(e => e.InboxText != null).LastOrDefault();
                InBoxmessages.IsInBox = false;
                messagesManager.Update(InBoxmessages);
            }
            catch (Exception ex)
            {

            }
            
            EmailService emailService = new EmailService();
            emailService.SendEmail(Email, Message, Subject);
            messagesManager.Insert(new MessagesDTO() { ToEmail = Email, OutboxText = Message, Date = DateTime.Now, IsNew = false, IsInBox = true });
            return RedirectToAction("Index");
        }
    }
}