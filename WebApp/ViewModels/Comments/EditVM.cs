using Messaging.Responses.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApp.ViewModels.Comments
{
    public class EditVM
    {
        public CommentViewModel? Comment { get; set; }

        public int? DiscussionId { get; set; }

        public int? UserId { get; set; }

        public bool? IsUpdated { get; set; }

        [Required(ErrorMessage = "*This field is Required!")]
        public string Text { get; set; }

    }
}
