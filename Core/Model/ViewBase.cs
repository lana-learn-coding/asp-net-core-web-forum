using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models;

namespace Core.Model
{
    public class ViewBase : IIdentified
    {
        [NotMapped]
        public Guid Uid => Id;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [JsonIgnore]
        public Guid Id { get; set; }

        [NotMapped]
        public string RawSlug => Id.ToString();

        public string Slug { get; set; }
    }
}