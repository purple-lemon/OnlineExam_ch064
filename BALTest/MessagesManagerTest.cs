using BAL.Managers;
using DAL.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.DB;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;

namespace BALTest
{
    [TestClass]
    public class MessagesManagerTest : TestStartup
    {
        [TestInitialize]
        public void SetupForMessagesManager()
        {
            Setup();
            var sMessagesRepo = Substitute.For<IBaseRepository<Messages>>();
        }

        [TestMethod]
        public void MessageTestMethodInsert()
        {
            var messageManager = new MessagesManager(sUoW, mapper);
            messageManager.Insert(new Model.DTO.MessagesDTO() { IsDeleted = false });
            sUoW.Received(1).Save();
            sUoW.ClearReceivedCalls();
        }

        [TestMethod]
        public void MessageTestMethodUpdate()
        {
            var messageManager = new MessagesManager(sUoW, mapper);
            messageManager.Update(new Model.DTO.MessagesDTO() { IsDeleted = false, OutboxText = "Hello WOrld" });
            sUoW.Received(1).Save();
            sUoW.ClearReceivedCalls();
        }

        [TestMethod]
        public void MessageTestMethodDelete()
        {
            var messageManager = new MessagesManager(sUoW, mapper);
            messageManager.Insert(new Model.DTO.MessagesDTO() { IsDeleted = false });
            messageManager.Delete(new Model.DTO.MessagesDTO() { IsDeleted = false });
            sUoW.Received(2).Save();
            sUoW.ClearReceivedCalls();
        }
        [TestMethod]
        public void MessageTestMethodGetAll()
        {
            var messageManager = new MessagesManager(sUoW, mapper);
            sUoW.MessagesRepo.GetAll().Returns(new List<Messages>() { new Messages() { FromEmail = "dada", ToEmail = "aaa" } });
            Assert.AreEqual("aaa", messageManager.GetAll().FirstOrDefault().ToEmail);
        }

        [TestMethod]
        public void MessageTestMethodGetById()
        {
            var messageManager = new MessagesManager(sUoW, mapper);
            sUoW.MessagesRepo.GetById(1).Returns(new Messages() { FromEmail = "dada", Id = 1 });
            Assert.AreEqual(1, messageManager.GetById(1).Id);

        }

        [TestMethod]
        public void MessageTestMethodGet()
        {
            var messageManager = new MessagesManager(sUoW, mapper);
            sUoW.MessagesRepo.Get(c => c.Id == 1).ReturnsForAnyArgs(new List<Messages> { new Messages { IsDeleted = true, FromEmail = "dad", Id = 1 } });
            Assert.AreEqual(1, messageManager.Get(c => c.Id == 1).FirstOrDefault().Id);
        }

    }
}
