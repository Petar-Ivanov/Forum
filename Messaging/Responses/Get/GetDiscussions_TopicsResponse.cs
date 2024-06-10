using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Responses.ViewModels;

namespace Messaging.Responses.Get
{
    /// <summary>
    /// Get discussions-topics response object
    /// </summary>
    public class GetDiscussions_TopicsResponse : ServiceResponseBase
    {
        public List<Discussions_TopicsViewModel>? Discussions_Topics { get; set; }
    }
}
