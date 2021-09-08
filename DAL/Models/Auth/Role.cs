using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Validation;

namespace DAL.Models.Auth
{
    public class Role : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Name;

        [Standardized]
        [Unique]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(20)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Icon { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}