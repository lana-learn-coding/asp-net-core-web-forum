using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public abstract class Entity : IIdentified<Guid>, IAuditable, IComparable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

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
}