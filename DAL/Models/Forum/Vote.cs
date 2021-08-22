using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Auth;

namespace DAL.Models.Forum
{
    public class Vote : Entity
    {
        [Required(ErrorMessage = "Only user can perform this action")]
        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        [Required(ErrorMessage = "Select a post to vote")]
        public Guid PostId { get; set; }

        public virtual Post Post { get; set; }

        [Required(ErrorMessage = "Please select Up or Down vote")]
        public short Value { get; set; }

        [JsonIgnore]
        [NotMapped]
        public bool IsDownVote => Value < 0;
    }
}