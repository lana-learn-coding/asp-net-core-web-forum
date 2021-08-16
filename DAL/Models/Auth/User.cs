using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.Auth
{
    public class User : Entity
    {
        public override string RawSlug => Username;

        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Avatar { get; set; }

        public string FullName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        [Index(IsUnique = true)]
        [Required]
        public string Username { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        [Index(IsUnique = true)]
        [Required]
        public string Email { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        [Index(IsUnique = true)]
        public string Phone { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MinLength(6)]
        [StringLength(120)]
        public string Password { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}