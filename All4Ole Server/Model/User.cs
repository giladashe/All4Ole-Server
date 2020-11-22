using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;


namespace All4Ole_Server.Model
{

	// User for the db - can be converted to/from jsons
    public class User
    {
        public User()
        {

        }
        public User(string userName, string password, string firstName, string lastName, string phone, int hobbies, int help,
			string email, string previousCountry, string language, string residentialArea, int maritalStatus, int hasChildrenUnder18)
        {
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Hobbies = hobbies;
            Help = help;
            Email = email;
            PreviousCountry = previousCountry;
            Language = language;
            ResidentialArea = residentialArea;
            MaritalStatus = maritalStatus;
            HasChildrenUnder18 = hasChildrenUnder18;
        }

        [Key]
		[JsonProperty("user_name")]
		public string UserName { get; set; }
		[JsonProperty("password")]
		public string Password { get; set; }

		[JsonProperty("first_name")]
		[Required(ErrorMessage = "Must have a name")]
		public string FirstName { get; set; }

		[JsonProperty("last_name")]
		[Required(ErrorMessage = "Must have a last name")]
		public string LastName { get; set; }

		[JsonProperty("cell")]
		[Required(ErrorMessage = "Must have phone to communicate")]
		public string Phone { get; set; }

		[JsonProperty("hobbies")]
		[Range(1, int.MaxValue, ErrorMessage = "Must have at least one hobby")]
		public int Hobbies { get; set; }

		[JsonProperty("help")]
		[Range(0, int.MaxValue, ErrorMessage = "Must be non negative number")]
		public int Help { get; set; }

		[JsonProperty("email")]
		public string Email { get; set; }

		[JsonProperty("origin_country")]
		[Required(ErrorMessage = "Must have origin country")]
		public string PreviousCountry { get; set; }

		[JsonProperty("language")]
		[Required(ErrorMessage = "Must have language")]
		public string Language { get; set; }

		[JsonProperty("residential_area")]
		[Required(ErrorMessage = "Must have residential area")]
		public string ResidentialArea { get; set; }

		[JsonProperty("marital_status")]
		[Required(ErrorMessage = "Must have marital status")]
		public int MaritalStatus { get; set; }

		[JsonProperty("has_little_children")]
		[Required(ErrorMessage = "Must say if has children")]
		public int HasChildrenUnder18 { get; set; }
	}
}
