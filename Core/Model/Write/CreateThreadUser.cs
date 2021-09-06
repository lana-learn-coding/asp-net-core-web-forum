using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DAL.Models;
using DAL.Models.Forum;
using DAL.Validation;

namespace Core.Model.Write
{
    public class CreateThreadUser : IIdentified
    {
        [Unique(Type = typeof(Thread))]
        [Required(ErrorMessage = "Please specify a name")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select a forum")]
        public Guid ForumId { get; set; }

        public virtual ICollection<Guid> TagIds { get; set; } = new List<Guid>();

        [Required(ErrorMessage = "Please write down something")]
        public string Content { get; set; }

        public Guid Id { get; set; }

        public string Slug { get; set; }
    }
}