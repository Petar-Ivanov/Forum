using Messaging.Responses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.GetBy
{
    public class GetDiscussions_TopicsByDiscussionIdResponse : ServiceResponseBase
    {
        public List<Discussions_TopicsViewModel>? Discussions_Topics { get; set; }
    }
}
