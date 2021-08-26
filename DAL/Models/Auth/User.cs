using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DAL.Models.Auth
{
    public class User : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Username;

        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        public string Avatar { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MinLength(3)]
        [StringLength(150)]
        public string FullName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MinLength(5)]
        [StringLength(80)]
        [Index(IsUnique = true)]
        [Required]
        public string Username { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        [Index(IsUnique = true)]
        [Required]
        public string Email { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string Phone { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MinLength(8)]
        [StringLength(120)]
        public string Password { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}