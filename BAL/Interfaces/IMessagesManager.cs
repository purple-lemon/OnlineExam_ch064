using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Model.DB;
using Model.DTO;

namespace BAL.Interfaces
{
    public interface IMessagesManager
    {
        /// <summary>
        /// Delete existing message 
        /// </summary>
        /// <param name="entityToDelete"></param>
        void Delete(MessagesDTO entityToDelete);
        IEnumerable<MessagesDTO> Get(Expression<Func<Messages, bool>> filter = null, Func<IQueryable<Messages>, IOrderedQueryable<Messages>> orderBy = null, string includeProperties = "");
        /// <summary>
        /// Returns lists of all messages
        /// </summary>
        /// <returns></returns>
        IEnumerable<MessagesDTO> GetAll();
        /// <summary>
        /// Returns a message by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MessagesDTO GetById(int id);
        /// <summary>
        /// Insert a new message
        /// </summary>
        /// <param name="entity"></param>
        void Insert(MessagesDTO entity);
        /// <summary>
        /// Edit an existing message
        /// </summary>
        /// <param name="entity"></param>
        void Update(MessagesDTO entity);
    }
}