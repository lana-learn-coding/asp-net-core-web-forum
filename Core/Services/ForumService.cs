using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Services.Base;
using DAL.Models.Forum;

namespace Core.Services
{
    public class ForumService : SimpleCrudService<Forum>
    {
        public ForumService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "Priority", "CreatedAt" };
        }

        protected override IQueryable<Forum> Query(DbSet<Forum> dbSet)
        {
            return dbSet.Include("Category");
        }
    }
}