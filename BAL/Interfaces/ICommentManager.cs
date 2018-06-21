using System;
using System.Collections.Generic;
using System.Text;
using Model.DB;
using Model.DTO;
using System.Linq;
using System.Linq.Expressions;

namespace BAL.Interfaces
{
    public  interface ICommentManager
    {
        IEnumerable<CommentDTO> GetAll();
        CommentDTO GetById(int id);
        IEnumerable<CommentDTO> Get(Expression<Func<Comment, bool>> filter = null,
                                     Func<IQueryable<Comment>,
                                     IOrderedQueryable<Comment>> orderBy = null,
                                     string includeProperties = "");
        void Insert(CommentDTO item);
        void Update(CommentDTO item);
        void DeleteOrRecover(int id);
        void Delete(CommentDTO item);
    }
}
