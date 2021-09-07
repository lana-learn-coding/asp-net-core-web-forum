using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            // set deleted forum category to Uncategorized
            foreach (var forum in entity.Forums)
            {
                forum.CategoryId = Guid.Empty;
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

    public class SpecialtyService : SimpleCrudService<Specialty>
    {
        public SpecialtyService(DbContext context) : base(context)
        {
        }

        protected override IQueryable<Specialty> Query(IQueryable<Specialty> queryable)
        {
            return queryable.Include("Tag");
        }
    }

    public class LanguageService : SimpleCrudService<Language>
    {
        public LanguageService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "CreatedAt" };
        }

        protected override void Delete(Language entity)
        {
            // set deleted forum category to Uncategorized
            foreach (var forum in entity.Forums)
            {
                forum.LanguageId = Guid.Empty;
            }

            base.Delete(entity);
        }
    }
}