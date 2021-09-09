using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Core.Services.Base;
using DAL.Models.Auth;

namespace Core.Services
{
    public class ExperienceService : SimpleCrudService<Experience>
    {
        public ExperienceService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "Level" };
        }

        protected override Experience FindForWrite(string slug,
            Func<IQueryable<Experience>, IQueryable<Experience>> query)
        {
            return base.FindForWrite(slug, q => q.Include("UserInfos.User"));
        }
    }

    public class PositionService : SimpleCrudService<Position>
    {
        public PositionService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "Name" };
        }

        protected override Position FindForWrite(string slug, Func<IQueryable<Position>, IQueryable<Position>> query)
        {
            return base.FindForWrite(slug, q => q.Include("UserInfos.User"));
        }
    }
}