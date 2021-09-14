using System;
using System.Linq;
using AutoMapper;
using Core.Model.Write;
using DAL.Models.Auth;
using DAL.Models.Forum;

namespace Core.Model
{
    public class ServiceModelMappingProfile : Profile
    {
        public ServiceModelMappingProfile()
        {
            // Forums
            CreateMap<Forum, ForumView>()
                .ForMember(m => m.ViewsCount,
                    opt => opt.MapFrom(x => x.Threads.Sum(t => (int?)t.ViewsCount) ?? 0))
                .ForMember(m => m.ThreadsCount,
                    opt => opt.MapFrom(x => x.Threads.Count))
                .ForMember(m => m.PostsCount,
                    opt => opt.MapFrom(x => x.Threads.Sum(t => (int?)t.Posts.Count) ?? 0))
                .ForMember(m => m.LastThread,
                    opt => opt.MapFrom(x =>
                        x.Threads.OrderByDescending(t => t.LastActivityAt)
                            .FirstOrDefault(t => t.Status == ThreadStatus.Approved)));
            CreateMap<Thread, LastThread>().ForMember(m => m.User,
                opt => opt.MapFrom(x => x.Posts.FirstOrDefault(p => p.Id.Equals(x.Id)).User));

            // Threads
            CreateMap<Thread, ThreadView>()
                .ForMember(m => m.Post,
                    opt => opt.MapFrom(x => x.Posts.FirstOrDefault(p => p.Id.Equals(x.Id))))
                .ForMember(m => m.PostsCount,
                    opt => opt.MapFrom(x => x.Posts.Count))
                .ForMember(m => m.Vote,
                    opt => opt.MapFrom(x =>
                        x.Posts.Where(p => p.Id.Equals(x.Id)).Select(p => p.Votes.Sum(v => (int?)v.Value) ?? 0)
                            .FirstOrDefault()));
            CreateMap<Forum, ThreadForumView>();
            CreateMap<CreateThreadUser, CreateThreadAdmin>();

            // Posts
            var authId = Guid.NewGuid(); // auto mapper Parameterization
            CreateMap<Post, PostView>()
                .ForMember(m => m.Vote,
                    opt => opt.MapFrom(x => x.Votes.Sum(v => (int?)v.Value) ?? 0))
                .ForMember(m => m.ThreadTitle,
                    opt => opt.MapFrom(x => x.Thread.Title))
                .ForMember(m => m.ThreadSlug,
                    opt => opt.MapFrom(x => x.Thread.Slug))
                .ForMember(m => m.Voted,
                    opt => opt.MapFrom(x =>
                        x.Votes.Where(v => v.UserId.Equals(authId)).Select(v => v.Value).FirstOrDefault()));

            //User
            var showUserInfo = false;
            CreateMap<User, UserViewBase>()
                .ForMember(m => m.IsEmailConfirmed,
                    opt => opt.MapFrom(x => string.IsNullOrEmpty(x.EmailConfirmToken)));
            CreateMap<User, UserView>()
                .ForMember(m => m.VotesCount,
                    opt => opt.MapFrom(x => x.Votes.Count))
                .ForMember(m => m.ThreadsCount,
                    opt => opt.MapFrom(x => x.Posts.Count(p => p.Id.Equals(p.ThreadId))))
                .ForMember(m => m.PostsCount,
                    opt => opt.MapFrom(x => x.Posts.Count));

            CreateMap<UserInfo, UserInfoView>()
                .ForMember(m => m.ThreadsCount,
                    opt => opt.MapFrom(x => x.User.Posts.Count(p => p.Id.Equals(p.ThreadId))))
                .ForMember(m => m.Email,
                    opt => opt.MapFrom(x => x.ShowEmail || showUserInfo ? x.User.Email : null))
                .ForMember(m => m.Phone,
                    opt => opt.MapFrom(x => x.ShowPhone || showUserInfo ? x.Phone : null))
                .ForMember(m => m.WorkAddress,
                    opt => opt.MapFrom(x => x.ShowWorkAddress || showUserInfo ? x.WorkAddress : null))
                .ForMember(m => m.WorkCity,
                    opt => opt.MapFrom(x => x.ShowWorkAddress || showUserInfo ? x.WorkCity : null))
                .ForMember(m => m.WorkCityId,
                    opt => opt.MapFrom(x => x.ShowWorkAddress || showUserInfo ? x.WorkCityId : null))
                .ForMember(m => m.WorkCountryId,
                    opt => opt.MapFrom(x => x.ShowWorkAddress || showUserInfo ? x.WorkCountryId : null))
                .ForMember(m => m.WorkCountry,
                    opt => opt.MapFrom(x => x.ShowWorkAddress || showUserInfo ? x.WorkCountry : null))
                .ForMember(m => m.WorkPhone,
                    opt => opt.MapFrom(x => x.ShowWorkAddress || showUserInfo ? x.WorkPhone : null))
                .ForMember(m => m.WorkExperience,
                    opt => opt.MapFrom(x => x.ShowWorkExperience || showUserInfo ? x.WorkExperience : null))
                .ForMember(m => m.WorkExperienceId,
                    opt => opt.MapFrom(x => x.ShowWorkExperience || showUserInfo ? x.WorkExperienceId : null))
                .ForMember(m => m.WorkDescription,
                    opt => opt.MapFrom(x => x.ShowWorkExperience || showUserInfo ? x.WorkDescription : null))
                .ForMember(m => m.WorkPosition,
                    opt => opt.MapFrom(x => x.ShowWorkExperience || showUserInfo ? x.WorkPosition : null))
                .ForMember(m => m.WorkPositionId,
                    opt => opt.MapFrom(x => x.ShowWorkExperience || showUserInfo ? x.WorkPositionId : null));
        }
    }
}