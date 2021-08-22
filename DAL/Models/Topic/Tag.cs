using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Forum;

namespace DAL.Models.Topic
{
    // Tags help categorizing Threads subject
    public class Tag : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Name;

        [Column(TypeName = "NVARCHAR")]
        [StringLength(50)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Thread> Threads { get; set; }
    }
}