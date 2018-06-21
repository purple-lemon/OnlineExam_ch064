using AutoMapper;
using BAL.Interfaces;
using Common;
using DAL.Interface;
using Microsoft.AspNetCore.Identity;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BAL.Managers
{
    public class NewsManager : INewsManager
    {
        private IUnitOfWork unitOfWork;
        private IMapper mapper;

        public NewsManager(IUnitOfWork unitOfWork, IMapper mapper){
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<NewsDTO> GetAll()
        {
            return mapper.Map<List<NewsDTO>>(unitOfWork.MessageRepo.GetAll());
        }

        public NewsDTO GetById(int id)
        {
            return mapper.Map<NewsDTO>(unitOfWork.MessageRepo.GetById(id));
        }

        public virtual IEnumerable<NewsDTO> Get(Expression<Func<News, bool>> filter = null,
                                     Func<IQueryable<News>,
                                     IOrderedQueryable<News>> orderBy = null,
                                     string includeProperties = "")
        {
            return mapper.Map<List<NewsDTO>>(unitOfWork.MessageRepo.Get(filter, orderBy, includeProperties));
        }

        public void Insert(NewsDTO entity)
        {
            unitOfWork.MessageRepo.Insert(mapper.Map<News>(entity));
            unitOfWork.Save();
        }

        public void Update(NewsDTO entity)
        {
            var article = unitOfWork.MessageRepo.GetById(entity.Id);
            article.CourseId = entity.CourseId;
            article.ImagePath = entity.ImagePath;
            article.Title = entity.Title;
            article.Text = entity.Text;
            article.Day = DateTime.Today.Day;
            article.Month = Enum.GetName(typeof(MonthEnum), DateTime.Today.Month-1);
            unitOfWork.MessageRepo.Update(article);
            unitOfWork.Save();
        }

        public void DeleteOrRecover(int id)
        {
            var article = unitOfWork.MessageRepo.GetById(id);
            article.IsDeleted = !article.IsDeleted;
            unitOfWork.MessageRepo.Update(article);
            unitOfWork.Save();
        }

    }
}
