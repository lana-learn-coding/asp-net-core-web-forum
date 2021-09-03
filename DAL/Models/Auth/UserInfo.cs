using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Validation;

namespace DAL.Models.Auth
{
    public class UserInfo
    {
        [ForeignKey("User")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column(TypeName = "NVARCHAR")]
        [MinLength(3)]
        [StringLength(150)]
        public string FullName { get; set; }

        [Unique]
        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        [Index(IsUnique = true)]
        public string Phone { get; set; }

        public virtual User User { get; set; }
    }
}