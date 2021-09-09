using Core.Dto;

namespace Web.Dto.Home
{
    public class AdvancedSearch : PageQuery
    {
        public string Search { get; set; }
        public string Forum { get; set; }
        public string Thread { get; set; }

        public string Position { get; set; }
        public string Experience { get; set; }

        public string Country { get; set; }
        public string City { get; set; }


        public string User { get; set; }
        public string Speciality { get; set; }

        public string Tag { get; set; }

        public string Type { get; set; }
    }
}