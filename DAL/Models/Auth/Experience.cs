using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Validation;

namespace DAL.Models.Auth
{
    public class Experience : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Level.ToString();

        [Standardized]
        [Unique]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a measurement")]
        public string Measurement { get; set; }

        [Unique]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify level of this experience")]
        public short Level { get; set; }
    }

    public class Position : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Name;

        [Standardized]
        [Unique]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(120)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }
    }
}