using Messaging.Responses.ViewModels;

namespace WebApp.ViewModels.Users
{
    public class IndexVM
    {
        public string? SearchTerm { get; set; }
        public int? Page { get; set; }
        public int? PageCount { get; set; }
        public List<UserViewModel>? Items { get; set; }
    }
}
