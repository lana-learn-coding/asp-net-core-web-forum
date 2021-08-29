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
                );
        }
    }
}