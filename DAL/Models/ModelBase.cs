using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Humanizer;

namespace DAL.Models
{
    // Base entity model with default entity and comparison method
    public abstract class Entity : IAuditable, IComparable, IIdentified, ISlugged
    {
        [NotMapped]
        public Guid Uid => Id;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int CompareTo(object obj)
        {
            return obj is null ? 1 : string.CompareOrdinal(ToString(), obj.ToString());
        }

        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(TypeName = "NVARCHAR")]
        [Index(IsUnique = true)]
        public string Slug { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual string RawSlug => Id.ToString();
    }

    // This interface to mark entities which Creation and Modification date 
    // will be tracked automatically
    public interface IAuditable
    {
        DateTime CreatedAt { get; set; }

        DateTime UpdatedAt { get; set; }
    }

    // This interface to mark entities last modification or interaction by user
    public interface ITracked
    {
        DateTime LastActivityAt { get; set; }
    }

    // This interface to mark entities that will be slugged when save of update
    // The RawSlug prop will determine which field to slug
    public interface IIdentified
    {
        Guid Id { get; set; }

        string Slug { get; set; }
    }

    public interface ISlugged
    {
        string Slug { get; set; }
        string RawSlug { get; }
    }

    public enum Priority : short
    {
        VeryHigh = 10,
        High = 20,
        Normal = 30,
        Low = 40
    }

    public enum AccessMode
    {
        Public = 0,
        Protected = 1,
        Internal = 2,
        Private = 3
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class TitleCase : Attribute
    {
        public virtual ICulturedStringTransformer To { get; } = Humanizer.To.TitleCase;
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LowerCase : TitleCase
    {
        public override ICulturedStringTransformer To { get; } = Humanizer.To.TitleCase;
    }
}