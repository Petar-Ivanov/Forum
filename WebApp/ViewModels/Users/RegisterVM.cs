using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApp.ViewModels.Users
{
    public class RegisterVM
    {
        [DisplayName("Username: ")]
        [StringLength(25, ErrorMessage = "Maximum length is 80 characters. ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Username { get; set; }

        [DisplayName("Password: ")]
        [StringLength(15, ErrorMessage = "Maximum length is 80 characters. ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Password { get; set; }

        [DisplayName("Email: ")]
        [StringLength(80, ErrorMessage = "Maximum length is 80 characters. ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Email { get; set; }

        [DisplayName("Country: ")]
        [StringLength(80, ErrorMessage = "Maximum length is 80 characters. ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Country { get; set; }

        [DisplayName("Bio: ")]
        [StringLength(1000, ErrorMessage = "Maximum length is 80 characters. ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Biography { get; set; }

        [DisplayName("Birth Date: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public DateTime BirthDate { get; set; }

        public bool? UniquePropertiesError { get; set; }
    }
}
