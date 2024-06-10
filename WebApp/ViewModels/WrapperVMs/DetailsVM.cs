using WebApp.ViewModels.Comments;
using WebApp.ViewModels.Discussions;

namespace WebApp.ViewModels.WrapperVMs
{
    public class DetailsVM
    {
        public Comments.CreateVM CommentCreate {  get; set; }
        public GetByIdVM DiscussionGet { get; set; }
        public int? Page { get; set; }
        public int? PageCount { get; set; }

        public DetailsVM()
        {
            CommentCreate = new Comments.CreateVM();
            DiscussionGet = new GetByIdVM();
        }

    }
}
