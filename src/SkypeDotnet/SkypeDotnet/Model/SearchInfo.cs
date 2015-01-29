using System.Web.Profile;

namespace SkypeDotnet.Model

    /*
     "ContactCards": {
		"Skype": {
			"DisplayName": "querty",
			"SkypeName": "preciouslove081",
			"Gender": null,
			"Age": null,
			"About": null,
			"Rank": "2",
			"Language": "fr"
		},
		"CurrentLocation": {
			"Country": "GB",
			"Province": null,
			"City": null
		}
	}
     */
{
    public class SearchInfo
    {
        ContactCards ContactCards { get; set; }
    }

    public class ContactCards
    {
        SkypeProfile Skype { get; set; }

        Location CurrentLocation { get; set; }
    }

    public class SkypeProfile : ProfileBase
    {
        public string SkypeName { get; set; }

        public string Gender { get; set; }

        public string Age { get; set; }

        public string About { get; set; }

        public string Rank { get; set; }

        public string Language { get; set; }
    }

    public class Location
    {
        public string Country { get; set; }

        public string Province { get; set; }

        public string City { get; set; }
    }
}