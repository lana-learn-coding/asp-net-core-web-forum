using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Auth;

namespace DAL.Models.Forum
{
    public class Post : Entity
    {
        // The origin (first) post of thread
        // Note that the origin post will have same id with thread
        [JsonIgnore]
        [Index]
        public bool IsOrigin { get; set; }

        [Required(ErrorMessage = "Only user can perform this action")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }

        [Column(TypeName = "NTEXT")]
        [MinLength(2, ErrorMessage = "Your content is too short (Require at least 2 character)")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Post must belong to some thread")]
        public Guid ThreadId { get; set; }

        public virtual Thread Thread { get; set; }

        [JsonIgnore]
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();
    }
}