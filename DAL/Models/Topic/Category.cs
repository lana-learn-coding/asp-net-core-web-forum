using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DAL.Models.Topic
{
    // Categories help categorizing Forums and SubForums main subject 
    public class Category : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Name;

        [Column(TypeName = "NVARCHAR")]
        [StringLength(50)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        public string Icon { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Description { get; set; }

        // Control order of categories
        // Top important categories are show on front page
        public short Priority { get; set; } = (short)Models.Priority.Normal;

        [JsonIgnore]
        public virtual ICollection<Forum.Forum> Forums { get; set; }
    }
}