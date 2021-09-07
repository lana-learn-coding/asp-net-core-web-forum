using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Validation;

namespace DAL.Models.Topic
{
    public class Language : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Name;

        [Unique]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(50)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Forum.Forum> Forums { get; set; } = new List<Forum.Forum>();
    }
}