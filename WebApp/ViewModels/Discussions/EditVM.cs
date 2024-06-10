using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Messaging.Responses.ViewModels;

namespace WebApp.ViewModels.Discussions
{
    public class EditVM
    {
        public List<TopicViewModel>? Topics { get; set; }
        public string? TopicsSerialized { get; set; }
        public bool? IsUpdated { get; set; }
        public List<int>? SelectedTopicIds { get; set; }
        public List<int>? InitiallySelectedTopicIds { get; set; }
        public DiscussionViewModel? Discussion { get; set; }

        [DisplayName("Title: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        [StringLength(80, ErrorMessage = "Maximum length is 80 characters. ")]
        public string Title { get; set; }

        public bool? UniqueTitleError { get; set; }

        [DisplayName("Text: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Text { get; set; }

        [DisplayName("Image: ")]
        public IFormFile? Image { get; set; }
    }
}
