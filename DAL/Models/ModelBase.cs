using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DAL.Models
{
    public abstract class Entity : IIdentified<Guid>, IAuditable, IComparable, ISlugged
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

    public interface IIdentified<T>
    {
        public T Id { get; set; }
    }

    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }

    public interface ISlugged
    {
        string Slug { get; set; }

        string RawSlug { get; }
    }
}