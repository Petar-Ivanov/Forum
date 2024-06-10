using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.ViewModels
{
    /// <summary>
    /// Discussions_Topics view model
    /// </summary>
    public class Discussions_TopicsViewModel
    {
        public int? Id { get; set; }
        public string? Source { get; set; }
        public DateTime? CreatedOn { get; set; }
        required public bool IsVisible { get; set; }
        public string Discussion { get; set; }
        public int? DiscussionId { get; set; }
        public string Topic { get; set; }
        public int? TopicId { get; set; }
    }
}
