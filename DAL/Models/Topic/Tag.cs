using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Forum;
using DAL.Validation;

namespace DAL.Models.Topic
{
    // Tags help categorizing Threads subject
    public class Tag : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Name;

        [Standardized]
        [Unique]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(50)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(50)]
        public string Color { get; set; }

        [NotMapped]
        public bool IsSpecialty => Specialties.Count > 0;

        [JsonIgnore]
        public virtual ICollection<Specialty> Specialties { get; set; } = new List<Specialty>();

        [JsonIgnore]
        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();
    }
}