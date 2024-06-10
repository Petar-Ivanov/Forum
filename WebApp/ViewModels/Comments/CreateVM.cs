using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApp.ViewModels.Comments
{
    public class CreateVM
    {
        public int DiscussionId { get; set; }

        [DisplayName("Text: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Text {  get; set; }
    }
}
