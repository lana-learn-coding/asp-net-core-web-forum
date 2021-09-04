using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models;

namespace Core.Model
{
    public class ViewBase : IIdentified, IAuditable
    {
        [NotMapped]
        public Guid Uid => Id;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public Guid Id { get; set; }

        public string Slug { get; set; }
    }
}