using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Topic
{
    // Tags help categorizing Threads subject
    public class Tag : Entity
    {
        public override string RawSlug => Name;

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }
    }
}