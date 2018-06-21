using AutoMapper;
using Model.DB;
using Model.DTO;
using System.Collections.Generic;
using Model.DB.Code;
using Model.DTO.CodeDTO;

namespace BAL.IoC
{
   public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
              : this("MyProfile")
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<User, UserDTO>().ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, options => options.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, options => options.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserRating, options => options.MapFrom(src => src.UserRating))
                .ForMember(dest => dest.DoneTaskNumber, options => options.MapFrom(src => src.DoneTaskNumber));
                cfg.CreateMap<List<User>, List<UserDTO>>();

                cfg.CreateMap<Exercise, ExerciseDTO>().ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.TaskName, options => options.MapFrom(src => src.TaskName))
                .ForMember(dest => dest.TaskTextField, options => options.MapFrom(src => src.TaskTextField))
                .ForMember(dest => dest.TaskBaseCodeField, options => options.MapFrom(src => src.TaskBaseCodeField))
                .ForMember(dest => dest.TeacherId, options => options.MapFrom(src => src.TeacherId))
                .ForMember(dest => dest.CourseId, options => options.MapFrom(src => src.CourseId))
                .ForMember(dest => dest.Course, options => options.MapFrom(src => src.Course))
                .ForMember(dest => dest.IsDeleted, options => options.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.Rating, options => options.MapFrom(src => src.Rating))
                .ForMember(dest => dest.CreateDateTime, options => options.MapFrom(src => src.CreateDateTime))
                .ForMember(dest => dest.UpdateDateTime, options => options.MapFrom(src => src.UpdateDateTime));
                cfg.CreateMap<List<Exercise>, List<ExerciseDTO>>();

                cfg.CreateMap<Comment, CommentDTO>().ForMember(dest => dest.Id, option => option.MapFrom(src => src.Id))
                .ForMember(dest => dest.ExerciseId, options => options.MapFrom(src => src.ExerciseId))
                .ForMember(dest => dest.CommentText, options => options.MapFrom(src => src.CommentText))
                .ForMember(dest => dest.CreationDateTime, options => options.MapFrom(src => src.CreationDateTime))
                .ForMember(dest => dest.Rating, options => options.MapFrom(src => src.Rating));
                cfg.CreateMap<List<Comment>, List<CommentDTO>>();


                cfg.CreateMap<Course, CourseDTO>().ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, options => options.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, options => options.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsActive, options => options.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.CreationDate, options => options.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.UserId));
                cfg.CreateMap<List<Course>, List<CourseDTO>>();

                cfg.CreateMap<UserCode, UserCodeDTO>()
                .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                    .ForMember(dest => dest.CodeText, options => options.MapFrom(src => src.CodeText))
                    .ForMember(dest => dest.ExerciseId, options => options.MapFrom(src => src.ExerciseId))
                    .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.CodeStatus, options => options.MapFrom(src => src.CodeStatus))
                    .ForMember(dest => dest.Mark, options => options.MapFrom(src => src.Mark))
                    .ForMember(dest => dest.TeachersComment, options => options.MapFrom(src => src.TeachersComment))
                    .ForMember(dest => dest.EndTime, options => options.MapFrom(src => src.EndTime))
                    .ForMember(dest => dest.UserId, options => options.MapFrom(src => src.UserId));
 
                cfg.CreateMap<List<UserCode>, List<UserCodeDTO>>();


                cfg.CreateMap<CodeHistory, CodeHistoryDTO>()
                    .ForMember(dest => dest.Id, options => options.MapFrom(src => src.Id))
                    .ForMember(dest => dest.CodeText, options => options.MapFrom(src => src.CodeText))
                    .ForMember(dest => dest.Error, options => options.MapFrom(src => src.Error))
                    .ForMember(dest => dest.Result, options => options.MapFrom(src => src.Result))
                    .ForMember(dest => dest.IsFavouriteCode, options => options.MapFrom(src => src.IsFavouriteCode))
                    .ForMember(dest => dest.UserCodeId, options => options.MapFrom(src => src.UserCodeId));
                cfg.CreateMap<List<CodeHistory>, List<CodeHistoryDTO>>();

                cfg.CreateMap<News, NewsDTO>();
                cfg.CreateMap<List<News>, List<NewsDTO>>();

                cfg.CreateMap<Messages, MessagesDTO>();
                cfg.CreateMap<List<Messages>, List<MessagesDTO>>();

            });
        }

        protected AutoMapperProfileConfiguration(string profileName)
            : base(profileName)
        {

        }
    }
}
