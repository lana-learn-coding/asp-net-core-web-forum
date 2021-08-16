using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DAL.Models.Auth
{
    public class Role : Entity
    {
        public override string RawSlug => Name;

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Icon { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}