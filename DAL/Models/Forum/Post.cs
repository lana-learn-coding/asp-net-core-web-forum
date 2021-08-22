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
        [Required(ErrorMessage = "Only user can perform this action")]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        [Column(TypeName = "NTEXT")]
        [MinLength(2, ErrorMessage = "Your content is too short (Require at least 2 character)")]
        public string Content { get; set; }

        [JsonIgnore]
        public bool IsOrigin = false;

        [Required(ErrorMessage = "Post must belong to some thread")]
        public Guid ThreadId;

        public virtual Thread Thread { get; set; }

        [JsonIgnore]
        public virtual ICollection<Vote> Votes { get; set; }

        // Sum of Votes. for fast accessing
        public int Vote { get; set; }
    }
}