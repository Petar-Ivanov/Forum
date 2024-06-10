using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.Extensions.Hosting;
using Messaging.Requests.Models;
using Messaging.Responses.ViewModels;

namespace WebApp.ViewModels.Discussions
{
    public class CreateVM
    {
        public List<TopicViewModel>? Topics { get; set; }
        public string? TopicsSerialized {  get; set; }
        public List<int>? SelectedTopicIds { get; set; }

        [DisplayName("Title: ")]
        [StringLength(80, ErrorMessage = "Maximum length is 80 characters. ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Title { get; set; }

        public bool? UniqueTitleError { get; set; }

        [DisplayName("Text: ")]
        [Required(ErrorMessage = "*This field is Required!")]
        public string Text { get; set; }

        [DisplayName("Image: ")]
        public IFormFile? Image { get; set; }


    }
}

//POST api/Discussions
//{
//    "id": null,
//    "title": "Some unique title",
//    "text": "Sample discussion text",
//    "isUpdated": false,
//    "isLocked": false,
//    "isVisible": true,
//    "image": null,
//    "createdBy": 7,
//    "updatedBy": 6
//}

//Id: int?
//Title: string
//Text: string
//IsUpdated: bool
//IsLocked: bool
//IsVisible: bool?
//Image: byte[]?
//CreatedBy: int?
//UpdatedBy: int?