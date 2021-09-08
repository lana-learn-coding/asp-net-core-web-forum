using System.Collections.Generic;
using System.Data.Entity;
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
    }

    public class PositionService : SimpleCrudService<Position>
    {
        public PositionService(DbContext context) : base(context)
        {
            DefaultSort = new List<string> { "Name" };
        }
    }
}