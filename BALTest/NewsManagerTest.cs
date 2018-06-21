using System;
using System.Collections.Generic;
using BAL.Managers;
using DAL.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.DB;
using NSubstitute;
using System.Linq;
using Model.DTO;

namespace BALTest
{
    [TestClass]
    public class NewsManagerTest : TestStartup
    {
        [TestInitialize]
        public void SetupForExerciseManagerTests()
        {
            Setup();
        }


        [TestMethod]
        public void NewsTestMethodGetAll()
        {
            var newsManager = new NewsManager(sUoW, mapper);
            var result = newsManager.GetAll();
            sUoW.Received(0).Save();
            sUoW.ClearReceivedCalls();

            Assert.AreEqual("Hello World", result.First().Text);
        }
    }
}
