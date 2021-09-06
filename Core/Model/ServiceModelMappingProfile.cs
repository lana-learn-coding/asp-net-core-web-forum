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
                    opt => opt.MapFrom(x => x.Posts.Count));
            CreateMap<Forum, ThreadForumView>();

            //Misc
            CreateMap<Post, PostView>()
                .ForMember(m => m.ThreadTitle,
                    opt => opt.MapFrom(x => x.Thread.Title));
            CreateMap<User, UserView>();
            CreateMap<CreateThreadUser, CreateThreadAdmin>();
        }
    }
}