using AutoMapper;
using BAL.IoC;
using DAL.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BALTest
{
    [TestClass]
    public class TestStartup
    {
        protected IMapper mapper;
        protected IUnitOfWork sUoW;

        protected static MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfileConfiguration());
        });

        [ClassInitialize]
        public void Setup()
        {
            mapper = config.CreateMapper();
            sUoW = Substitute.For<IUnitOfWork>();
        }
    }
}
