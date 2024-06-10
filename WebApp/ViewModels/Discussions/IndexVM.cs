using Messaging.Responses.ViewModels;

namespace WebApp.ViewModels.Discussions
{
    public class IndexVM
    {
        public string? SearchTerm { get; set; }
        public string? Topic { get; set; }
        public int? Page { get; set; }
        public int? PageCount { get; set; }
        public List<DiscussionViewModel> Items { get; set; }
    }
}
