using System.Web.Profile;

namespace SkypeDotnet.Model

{
    public class SkypeProfile : SkypeProfileBase
    {
        public string SkypeName { get; set; }

        public string Gender { get; set; }

        public string Age { get; set; }

        public string About { get; set; }

        public string Rank { get; set; }

        public string Language { get; set; }

        public Location CurrentLocation { get; set; }
    }

    public class Location
    {
        public string Country { get; set; }

        public string Province { get; set; }

        public string City { get; set; }
    }
}