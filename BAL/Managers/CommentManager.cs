using System;
using DAL.Interface;
using BAL.Interfaces;
using System.Collections.Generic;
using Model.DTO;
using System.Linq.Expressions;
using System.Linq;
using Model.DB;
using AutoMapper;

namespace BAL.Managers
{
    public class CommentManager : BaseManager, ICommentManager
    {
        public CommentManager(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public IEnumerable<CommentDTO> GetAll()
        {
            return mapper.Map<List<CommentDTO>>(unitOfWork.CommentRepo.GetAll());
        }

        public CommentDTO GetById(int id)
        {
            return mapper.Map<CommentDTO>(unitOfWork.CommentRepo.GetById(id));
        }

        public virtual IEnumerable<CommentDTO> Get(Expression<Func<Comment, bool>> filter = null,
                                     Func<IQueryable<Comment>,
                                     IOrderedQueryable<Comment>> orderBy = null,
                                     string includeProperties = "")
        {
            return mapper.Map<List<CommentDTO>>(unitOfWork.CommentRepo.Get(filter, orderBy, includeProperties));
        }

        public void Insert(CommentDTO entity)
        {
            unitOfWork.CommentRepo.Insert(mapper.Map<Comment>(entity));
            unitOfWork.Save();
        }

        public void Update(CommentDTO entityToUpdate)
        {
            unitOfWork.CommentRepo.Update(mapper.Map<Comment>(entityToUpdate));
            unitOfWork.Save();
        }

        public void DeleteOrRecover(int id)
        {
            var task = unitOfWork.ExerciseRepo.GetById(id);
            task.IsDeleted = !task.IsDeleted;
            unitOfWork.ExerciseRepo.Update(task);
            unitOfWork.Save();
        }

        public void Delete(CommentDTO entityToDelete)
        {
            unitOfWork.CommentRepo.Delete(mapper.Map<Comment>(entityToDelete));
            unitOfWork.Save();
        }

    }
}
