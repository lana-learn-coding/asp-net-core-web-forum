using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using DAL.Models.Auth;
using DAL.Models.Topic;

namespace Core.Model
{
    public class UserInfoView : UserViewBase
    {
        [JsonIgnore]
        public UserViewBase User { get; set; }

        public override string Avatar => User.Avatar;
        public override string Username => User.Username;
        public override string Email { get; set; }

        public bool? Gender { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Bio { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string WorkPhone { get; set; }

        public string WorkAddress { get; set; }

        public Guid? WorkCityId { get; set; }
        public City WorkCity { get; set; }

        public Guid? WorkCountryId { get; set; }
        public Country WorkCountry { get; set; }

        public Guid? WorkPositionId { get; set; }
        public Position WorkPosition { get; set; }

        public Guid? WorkExperienceId { get; set; }
        public Experience WorkExperience { get; set; }

        public string WorkDescription { get; set; }

        [JsonIgnore]
        public virtual ICollection<Specialty> WorkSpecialities { get; set; }

        public virtual IEnumerable<Guid> WorkSpecialitiesIds => WorkSpecialities.Select(x => x.Id) ?? new List<Guid>();

        public bool ShowPhone { get; set; } = false;
        public bool ShowEmail { get; set; } = true;
        public bool ShowWorkAddress { get; set; } = true;
        public bool ShowWorkExperience { get; set; } = true;
    }
}