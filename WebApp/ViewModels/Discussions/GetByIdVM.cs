using Messaging.Responses.ViewModels;

namespace WebApp.ViewModels.Discussions
{
    public class GetByIdVM
    {
        public DiscussionViewModel Discussion { get; set; }

        public List<CommentViewModel>? Comments { get; set; }
    }
}
