using System;
using System.Collections.Generic;
using System.Data.Entity;
using Core.Services.Base;
using DAL.Models.Topic;

namespace Core.Services
{
    public class CategoryService : SimpleCrudService<Category>
    {
        public CategoryService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "Priority", "CreatedAt" };
        }

        protected override void Delete(Category entity)
        {
            if (entity.Id.Equals(Guid.Empty))
            {
                throw new ConflictException("Cannot delete base category: Uncategorized");
            }

            base.Delete(entity);
        }
    }

    public class TagService : SimpleCrudService<Tag>
    {
        public TagService(DbContext context) : base(context)
        {
        }
    }
}