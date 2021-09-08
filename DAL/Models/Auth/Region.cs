using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Validation;

namespace DAL.Models.Auth
{
    public class Country : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Name;

        [TitleCase]
        [Unique]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<City> Cities { get; set; } = new List<City>();
    }

    public class City : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Name;

        [TitleCase]
        [Unique]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a name")]
        public string Name { get; set; }

        public Guid CountryId { get; set; }

        public Country Country { get; set; }
    }
}