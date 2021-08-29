using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Model;
using Core.Services.Base;
using DAL.Models.Forum;

namespace Core.Services
{
    public class ForumService : CrudService<Forum, ForumView>
    {
        private static readonly MapperConfiguration Configuration = new(cfg =>
        {
            cfg.CreateMap<Forum, ForumView>()
                .ForMember(
                    m => m.ThreadCounts,
                    opt => opt.MapFrom(x => x.Threads.Count)
                );
        });

        public ForumService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "Priority", "CreatedAt" };
        }

        protected override IQueryable<ForumView> Query(IQueryable<Forum> queryable)
        {
            return queryable
                .Include("Category")
                .ProjectTo<ForumView>(Configuration);
        }
    }
}