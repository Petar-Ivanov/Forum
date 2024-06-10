using Messaging.Responses.ViewModels;

namespace WebApp.ViewModels.Topics
{
    public class IndexVM
    {
        public string? SearchTerm { get; set; }
        public int? Page {  get; set; }
        public int? PageCount { get; set; }
        public List<TopicViewModel> Items { get; set; }
    }
}
