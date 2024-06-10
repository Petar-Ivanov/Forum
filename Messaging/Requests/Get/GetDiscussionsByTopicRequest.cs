using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetDiscussionsByTopicRequest: ServiceRequestBase
    {
        public string Topic { get; set; }

        public GetDiscussionsByTopicRequest(string topic)
        {
            Topic = topic;
        }
    }
}
