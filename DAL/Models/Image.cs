using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    // This entity help store image on server.
    // Its has no relation, as others entity will store the image links instead
    // which make it easier to migrate to 3rd service in the future
    public class Image : Entity
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(50)]
        public string Extension { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(500)]
        public string FileName { get; set; }

        public long Length { get; set; }

        public Guid OwnerId { get; set; }
    }
}