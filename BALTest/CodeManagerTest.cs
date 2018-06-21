//using AutoMapper;
//using BAL.IoC;
//using DAL.Interface;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Model.DB.Code;
//using NSubstitute;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;
//using BAL.Managers;
//using BAL.Interfaces;
//using Microsoft.AspNetCore.Identity;
//using Model.DB;

//namespace BALTest
//{
//    [TestClass]
//  public  class CodeManagerTest
//    {     
//           IMapper mapper;
//            IUnitOfWork sUoW;

//        private UserManager<User> userManager;
//        [TestInitialize]
//            public void Setup()
//            {
//                var config = new MapperConfiguration(cfg =>
//                {
//                    cfg.AddProfile(new AutoMapperProfileConfiguration());
//                });
//                mapper = config.CreateMapper();

//                var sCodeRepo = Substitute.For<IBaseRepository<UserCode>>();
//                sUoW = Substitute.For<IUnitOfWork>();


//                var moqNews = new List<UserCode>() { new UserCode() { CodeText="code",  EndTime=DateTime.Now } };
//            sCodeRepo.GetAll().Returns(moqNews);



//                sUoW.CodeRepo.Returns(sCodeRepo);
//            }


//        [TestMethod]
//        public void TestMethod2()
//        {
//            var exma = new ExerciseManager(sUoW, mapper);
//            var sandb = new SandboxManager("SandboxAPI");
//            var codeManager = new CodeManager(sUoW, mapper, exma, userManager , sandb);
//            var result = codeManager.Get(e=>e.CodeText == "code");
//            sUoW.Received(0).Save();
//            sUoW.ClearReceivedCalls();
          
//            Assert.AreEqual("code", result.First().CodeText);
  
//          //  Assert.AreEqual("Hello World", result.First().Text);
//        }
//    }

//}
