using AutoMapper;
using BAL.Interfaces;
using DAL.Interface;
using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BAL.Managers
{
    public class MessagesManager : IMessagesManager
    {
        private IUnitOfWork unitOfWork;
        private IMapper mapper;

        public MessagesManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<MessagesDTO> GetAll()
        {
            return mapper.Map<List<MessagesDTO>>(unitOfWork.MessagesRepo.GetAll());
        }

        public MessagesDTO GetById(int id)
        {
            return mapper.Map<MessagesDTO>(unitOfWork.MessagesRepo.GetById(id));
        }

        public virtual IEnumerable<MessagesDTO> Get(Expression<Func<Messages, bool>> filter = null,
                                     Func<IQueryable<Messages>,
                                     IOrderedQueryable<Messages>> orderBy = null,
                                     string includeProperties = "")
        {
            return mapper.Map<List<MessagesDTO>>(unitOfWork.MessagesRepo.Get(filter, orderBy, includeProperties));
        }

        public void Insert(MessagesDTO entity)
        {
            unitOfWork.MessagesRepo.Insert(mapper.Map<Messages>(entity));
            unitOfWork.Save();
        }
        public void Update(MessagesDTO entity)
        {
            unitOfWork.MessagesRepo.Update(mapper.Map<Messages>(entity));
            unitOfWork.Save();
        }

        public void Delete(MessagesDTO entityToDelete)
        {
            unitOfWork.MessagesRepo.Delete(mapper.Map<Messages>(entityToDelete));
            unitOfWork.Save();
        }
    }
}
