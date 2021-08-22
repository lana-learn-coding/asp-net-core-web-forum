using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Topic;

namespace DAL.Models.Forum
{
    public class Thread : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Title;

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
        public ushort Priority { get; set; } = (ushort)Models.Priority.Normal;

        public bool Pinned => Priority > (ushort)Models.Priority.High;

        [JsonIgnore]
        public virtual ICollection<Post> Posts { get; set; }

        [JsonIgnore]
        public Guid OriginPostId { get; set; }

        // The origin (first) post of thread
        public virtual Post OriginPost { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}