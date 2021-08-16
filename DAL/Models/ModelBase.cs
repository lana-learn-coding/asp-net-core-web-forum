using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DAL.Models
{
    
    // Base entity model with default entity and comparison method
    public abstract class Entity : IIdentified, IAuditable, IComparable, ISlugged
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [Index(IsUnique = true)]
        public string Slug { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual string RawSlug => Id.ToString();

        public int CompareTo(object obj)
        {
            return obj is null ? 1 : string.CompareOrdinal(ToString(), obj.ToString());
        }
    }

    public interface IIdentified
    {
        public Guid Id { get; set; }
    }

    // This interface to mark entities which Creation and Modification date 
    // will be tracked automatically
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }

    // This interface to mark entities that will be slugged when save of update
    // The RawSlug prop will determine which field to slug
    public interface ISlugged
    {
        string Slug { get; set; }

        string RawSlug { get; }
    }
}