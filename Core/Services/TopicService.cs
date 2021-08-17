using System.Data.Entity;
using Core.Services.Base;
using DAL.Models.Topic;

namespace Core.Services
{
    public class CategoryService : SimpleCrudService<Category>
    {
        public CategoryService(DbContext context) : base(context)
        {
        }
    }

    public class TagService : SimpleCrudService<Tag>
    {
        public TagService(DbContext context) : base(context)
        {
        }
    }
}