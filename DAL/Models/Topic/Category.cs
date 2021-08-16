using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Topic
{
    // Categories help categorizing Forums and SubForums main subject 
    public class Category : Entity
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
    }
}