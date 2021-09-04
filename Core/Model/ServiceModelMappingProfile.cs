using System.Linq;
using AutoMapper;
using DAL.Models.Forum;

namespace Core.Model
{
    public class ServiceModelMappingProfile : Profile
    {
        public ServiceModelMappingProfile()
        {
            CreateMap<Forum, ForumView>()
                .ForMember(
                    m => m.ThreadCounts,
                    opt => opt.MapFrom(x => x.Threads.Count)
                )
                .ForMember(m => m.PostCounts,
                    opt => opt.MapFrom(x => x.Threads.Sum(t => t.Posts.Count)));

            CreateMap<Thread, ThreadView>()
                .ForMember(m => m.Post,
                    opt => opt.MapFrom(x => x.Posts.FirstOrDefault(p => p.Id.Equals(x.Id)))
                )
                .ForMember(
                    m => m.PostCounts,
                    opt => opt.MapFrom(x => x.Posts.Count)
                );

            CreateMap<Post, PostView>()
                .ForMember(
                    m => m.Vote,
                    opt => opt.MapFrom(x => x.Votes.Sum(v => v.Value))
                );
        }
    }
}