using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using BAL.Interfaces;
using DAL.Interface;
using Microsoft.AspNetCore.Identity;
using Model.DB;
using Model.DB.Code;
using Model.DTO.CodeDTO;
using Model.Entity;

namespace BAL.Managers
{
    public class CodeManager : ICodeManager
    {
        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        private IExerciseManager exerciseManager;
        private UserManager<User> userManager;
        private ISandboxManager sandboxManager;
        public CodeManager(IUnitOfWork unitOfWork, IMapper mapper, IExerciseManager exerciseManager, UserManager<User> userManager, ISandboxManager sandboxManager)
        {
            this.sandboxManager = sandboxManager;
            this.userManager = userManager;
            this.exerciseManager = exerciseManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public virtual IEnumerable<UserCodeDTO> Get(Expression<Func<UserCode, bool>> filter = null,
                             Func<IQueryable<UserCode>,
                             IOrderedQueryable<UserCode>> orderBy = null,
                             string includeProperties = "")
        {
            return mapper.Map<List<UserCodeDTO>>(unitOfWork.CodeRepo.Get(filter, orderBy, includeProperties));
        }

        public UserCodeDTO GetUserCodeById(string id)
        {
            UserCodeDTO code = mapper.Map<UserCodeDTO>(unitOfWork.CodeRepo.Get(c => c.UserId == id).FirstOrDefault());
            return code;
        }

        public UserCodeDTO UserCodeByExId(string userId, int exerciseId)
        {
            UserCodeDTO code = mapper.Map<UserCodeDTO>(unitOfWork.CodeRepo.Get(c => c.UserId == userId && c.ExerciseId == exerciseId).FirstOrDefault());
            return code;
        }

        public List<UserCodeDTO> UserCodeListByExId(int exerciseId)
        {
            var code = mapper.Map<List<UserCodeDTO>>(unitOfWork.CodeRepo.Get(c => c.ExerciseId == exerciseId));
            return code;
        }

        public bool FindUserCode(string userId, int exerciseId)
        {
            return unitOfWork.CodeRepo.Get(c => c.ExerciseId == exerciseId && c.UserId == userId).FirstOrDefault() != null;
        }


        public void AddHistory(int codeId, string text, DateTime date, string error = null, string result = null)
        {
            unitOfWork.CodeHistoryRepo.Insert(new CodeHistory
            {
                CodeText = text,
                UserCodeId = codeId,
                Error = error,
                Result = result,
                time = date
            });
            unitOfWork.Save();

        }

        public string ExecuteCode(UserCodeDTO model)
        {
            if (model.CodeStatus == CodeStatus.InProgress)
            {
                if (FindUserCode(model.UserId, model.ExerciseId))
                {
               
                    var code = unitOfWork.CodeRepo.Get(c => c.ExerciseId == model.ExerciseId && c.UserId == model.UserId).FirstOrDefault();
                    if (code != null)
                    {
                        code.CodeText = model.CodeText;
                        unitOfWork.CodeRepo.Update(code);
                    }
                }
                else
                {
                    UserCode code = new UserCode
                    {
                        CodeText = model.CodeText,
                        UserId = model.UserId,
                        ExerciseId = model.ExerciseId
                    };

                    unitOfWork.CodeRepo.Insert(code);
                }
                unitOfWork.Save();
            }
            return ExecutionResult(model.CodeText, model.ExerciseId, model.UserId, model.CodeStatus);
        }

        public string ExecutionResult(string code, int exId, string userId, CodeStatus codeStatus)
        {
            var codeId = unitOfWork.CodeRepo.Get(c => c.ExerciseId == exId && c.UserId == userId).First().Id;
            var testCasesCode = unitOfWork.ExerciseRepo.GetById(exId).TestCasesCode;
            var finalCode = string.Concat(testCasesCode, code);
            var res = sandboxManager.Execute(finalCode);
            if (res.Success)
            {
                string result = res.Result;
                if (codeStatus == CodeStatus.InProgress)
                {
                    AddHistory(codeId, code, DateTime.Now, null, result);
                }
                return result;
            }

            string errors = res.Result;
            if (codeStatus == CodeStatus.InProgress) 
            {
                AddHistory(codeId, code, DateTime.Now, errors, null);
            }
            return errors;
        }


        public string GetOnFlyCode(UserCodeDTO model)
        {
            UserCode code = new UserCode
            {
                CodeText = model.CodeText
            };
            var testCasesCode = unitOfWork.ExerciseRepo.GetById(model.ExerciseId).TestCasesCode;
            var finalCode = string.Concat(testCasesCode, code.CodeText);
            return ExecuteOnFlyCode(finalCode);
        }

        public string ExecuteOnFlyCode(string code)
        {
            var res = sandboxManager.Execute(code);
            return res.Result;
        }

        public void SetCodeStatus(int id, string userId)
        {
            var code = unitOfWork.CodeRepo.GetById(id);
            if (code.CodeStatus != CodeStatus.Done && code.CodeStatus != CodeStatus.Appreciated)
            {
                code.CodeStatus = CodeStatus.Done;
                code.EndTime = DateTime.Now;
                unitOfWork.CodeRepo.Update(code);
                unitOfWork.Save();
                var user = userManager.FindByIdAsync(userId).Result;
                user.DoneTaskNumber += 1;
                unitOfWork.UserRepo.Update(user);
                unitOfWork.Save();
            }
        }

        public void SetMark(int id, int mark, string comment, string userId)
        {
            var code = unitOfWork.CodeRepo.GetById(id);
            code.Mark = mark;
            code.TeachersComment = comment;
            code.CodeStatus = CodeStatus.Appreciated;
            unitOfWork.CodeRepo.Update(code);
            unitOfWork.Save();
            var user = userManager.FindByIdAsync(userId).Result;
            var codes = unitOfWork.CodeRepo.Get(c => c.UserId == userId && c.CodeStatus == CodeStatus.Appreciated && c.Mark != 0);
            double MarkSum = 0;
            foreach (var elem in codes)
            {
                MarkSum += elem.Mark;
            }
            user.UserRating = MarkSum / codes.Count();
            unitOfWork.UserRepo.Update(user);
            unitOfWork.Save();
        }

        public UserCodeDTO BuildCodeModel(UserCodeDTO model)
        {
            var exercise = exerciseManager.GetById(model.ExerciseId);
            model.Exercise = exercise;
            var user = userManager.FindByNameAsync(model.UserId).Result;
            model.UserId = user.Id;
            if (model.CodeText != null)
            {
                model.CodeStatus = CodeStatus.FromHistory; 
                return model;
            }
            else
            {
                var code = unitOfWork.CodeRepo.Get(c => c.ExerciseId == model.ExerciseId && c.UserId == model.UserId).FirstOrDefault();
                if (code != null)
                {
                    model.CodeText = code.CodeText;
                    model.CodeStatus = code.CodeStatus;
                    model.Mark = code.Mark;
                    model.TeachersComment = code.TeachersComment;
                }
                else
                {
                    model.CodeStatus = CodeStatus.InProgress;
                    model.CodeText = exercise.TaskBaseCodeField;
                }
            }
            return model;
        }

        public List<CodeHistory> GetHistoryLst(int codeId)
        {
            var codeHistories = unitOfWork.CodeHistoryRepo.Get().Where(e => e.UserCodeId == codeId).ToList();
            return codeHistories;
        }

        public bool DeleteHistoryLst(UserCodeDTO code)
        {
            try
            {
                var codeHistories = unitOfWork.CodeHistoryRepo.Get().Where(e => e.UserCodeId == code.Id).ToList();
                foreach (var ch in codeHistories)
                {
                    unitOfWork.CodeHistoryRepo.Delete(ch);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public bool DeleteCode(UserCodeDTO code)
        {
            var usercode = unitOfWork.CodeRepo.Get(c=>c.UserId==code.UserId);
            try
            {
                foreach(var elem in usercode) {
                    unitOfWork.CodeRepo.Delete(elem);

                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public SetFav SetFavouriteCode(SetFav model)
        {
            var codeHistoryEntity = unitOfWork.CodeHistoryRepo.Get().Where(e => e.Id == model.codeId).FirstOrDefault();
            codeHistoryEntity.IsFavouriteCode = !model.flag;
            model.flag = !model.flag;
            unitOfWork.Save();
            return model;
        }
        public CodeModel EditCode(CodeModel codeModel)
        {
            var code = unitOfWork.CodeHistoryRepo.Get().Where(e => e.Id == codeModel.codeTextId).FirstOrDefault();
            code.CodeText = codeModel.codeText;
            unitOfWork.Save();
            return codeModel;
        }
        public IEnumerable<CodeHistory> GetAll()
        {
            return unitOfWork.CodeHistoryRepo.GetAll();
        }
    }
}