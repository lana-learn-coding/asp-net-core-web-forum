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
                    opt => opt.MapFrom(x => x.Threads.Sum(t => t.ViewsCount)))
                .ForMember(m => m.ThreadsCount,
                    opt => opt.MapFrom(x => x.Threads.Count))
                .ForMember(m => m.PostsCount,
                    opt => opt.MapFrom(x => x.Threads.Sum(t => t.Posts.Count)))
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
                        x.Posts.Where(p => p.Id.Equals(x.Id)).Select(p => p.Votes.Sum(v => (int?)v.Value ?? 0))
                            .FirstOrDefault()));
            CreateMap<Forum, ThreadForumView>();

            // Posts
            var authId = Guid.NewGuid(); // auto mapper Parameterization
            CreateMap<Post, PostView>()
                .ForMember(m => m.Vote,
                    opt => opt.MapFrom(x => x.Votes.Sum(v => (int?)v.Value) ?? 0))
                .ForMember(m => m.ThreadTitle,
                    opt => opt.MapFrom(x => x.Thread.Title))
                .ForMember(m => m.Voted,
                    opt => opt.MapFrom(x =>
                        x.Votes.Where(v => v.UserId.Equals(authId)).Select(v => v.Value).FirstOrDefault()));

            //Misc
            CreateMap<User, UserView>();
            CreateMap<CreateThreadUser, CreateThreadAdmin>();
        }
    }
}