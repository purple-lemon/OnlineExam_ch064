using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Model.DB.Code;
using Model.DTO.CodeDTO;
using Model.Entity;

namespace BAL.Interfaces
{
    public interface ICodeManager
    {
        IEnumerable<UserCodeDTO> Get(Expression<Func<UserCode, bool>> filter = null,
                             Func<IQueryable<UserCode>,
                             IOrderedQueryable<UserCode>> orderBy = null,
                             string includeProperties = "");
        UserCodeDTO GetUserCodeById(string id);
        UserCodeDTO UserCodeByExId(string userId, int exerciseId);
        bool FindUserCode(string userId, int exerciseId);
        void AddHistory(int codeId, string text, DateTime date, string error = null, string result = null);
        string ExecuteCode(UserCodeDTO model);
        string ExecutionResult(string code, int exId, string userId, CodeStatus codeStatus);
        UserCodeDTO BuildCodeModel(UserCodeDTO model);
        List<CodeHistory> GetHistoryLst(int codeId);
        bool DeleteHistoryLst(UserCodeDTO code);
        bool DeleteCode(UserCodeDTO code);
        List<UserCodeDTO> UserCodeListByExId(int exerciseId);
        void SetCodeStatus(int id, string userId);
        void SetMark(int id, int mark, string comment, string userId);
        SetFav SetFavouriteCode(SetFav model);
        CodeModel EditCode(CodeModel codeModel);
        IEnumerable<CodeHistory> GetAll();

    }
}
