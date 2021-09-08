using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Topic;
using DAL.Validation;

namespace DAL.Models.Auth
{
    public class UserInfo : Entity
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => User?.Username;

        [Column(TypeName = "NVARCHAR")]
        [MinLength(3)]
        [StringLength(150)]
        public string FirstName { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MinLength(3)]
        [StringLength(150)]
        public string LastName { get; set; }

        [Unique]
        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        public string Phone { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(500)]
        public string Bio { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? DateOfBirth { get; set; }

        public bool? Gender { get; set; }

        public User User { get; set; }

        [Phone]
        [Column(TypeName = "VARCHAR")]
        [StringLength(30)]
        public string WorkPhone { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(200)]
        public string WorkAddress { get; set; }

        public Guid? WorkCityId { get; set; }
        public City WorkCity { get; set; }

        public Guid? WorkCountryId { get; set; }
        public Country WorkCountry { get; set; }

        public Guid? WorkPositionId { get; set; }
        public Position WorkPosition { get; set; }

        public Guid? WorkExperienceId { get; set; }
        public Experience WorkExperience { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(500)]
        public string WorkDescription { get; set; }

        public virtual ICollection<Specialty> WorkSpecialities { get; set; } = new List<Specialty>();

        [NotMapped]
        public virtual ICollection<Guid> WorkSpecialitiesIds { get; set; } = new List<Guid>();

        public bool ShowPhone { get; set; } = false;
        public bool ShowEmail { get; set; } = true;
        public bool ShowWorkAddress { get; set; } = true;
        public bool ShowWorkExperience { get; set; } = true;
    }
}