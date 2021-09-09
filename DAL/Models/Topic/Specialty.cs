using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Auth;
using DAL.Validation;

namespace DAL.Models.Topic
{
    public class Specialty : Entity
    {
        [NotMapped]
        public string Name => Tag?.Name;

        [Unique]
        [Required]
        public Guid TagId { get; set; }

        [JsonIgnore]
        public Tag Tag { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
    }
}