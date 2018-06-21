using Model.DB;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BAL.Interfaces
{
    public interface INewsManager
    {
        IEnumerable<NewsDTO> GetAll();

        NewsDTO GetById(int id);

        IEnumerable<NewsDTO> Get(Expression<Func<News, bool>> filter = null,
                                     Func<IQueryable<News>,
                                     IOrderedQueryable<News>> orderBy = null,
                                     string includeProperties = "");

        void Insert(NewsDTO entity);

        void Update(NewsDTO entity);

        void DeleteOrRecover(int id);
        
    }
}
