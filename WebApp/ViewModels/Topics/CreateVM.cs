using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApp.ViewModels.Topics
{
    public class CreateVM
    {
        [DisplayName("Name: ")]
        [StringLength(50, ErrorMessage = "Maximum length is 50 characters. ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Name { get; set; }

        [DisplayName("Description: ")]
        [StringLength(100, ErrorMessage = "Maximum length is 100 characters. ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Description { get; set; }

        public bool? UniqueNameError { get; set; }
    }
}
