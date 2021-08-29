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
        private readonly IConfigurationProvider _mapperConfig;
        public ForumService(DbContext context, IConfigurationProvider mapperConfig) : base(context)
        {
            DefaultSort = new List<string> { "Priority", "CreatedAt" };
            _mapperConfig = mapperConfig;
        }

        protected override IQueryable<ForumView> Query(IQueryable<Forum> queryable)
        {
            return queryable
                .Include("Category")
                .ProjectTo<ForumView>(_mapperConfig);
        }
    }
}