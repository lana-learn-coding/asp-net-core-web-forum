using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Topic;
using DAL.Validation;

namespace DAL.Models.Forum
{
    public class Thread : Entity, ITracked
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Title;

        [Unique]
        [TitleCase]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please select a forum")]
        public Guid ForumId { get; set; }

        public Forum Forum { get; set; }

        // Control order of threads
        // If priority > High then thread will be pinned 
        public short Priority { get; set; } = (short)Models.Priority.Normal;

        [JsonIgnore]
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        [JsonIgnore]
        public int ViewsCount { get; set; }

        [Required(ErrorMessage = "Please select a status")]
        public ThreadStatus Status { get; set; } = ThreadStatus.Approved;

        [JsonIgnore]
        public DateTime LastActivityAt { get; set; }
    }

    public enum ThreadStatus
    {
        Approved = 0,
        Pending = 1,
        Closed = 2
    }
}