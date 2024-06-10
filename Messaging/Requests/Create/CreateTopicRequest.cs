using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Create
{
    /// <summary>
    /// Create topic request object
    /// </summary>
    public class CreateTopicRequest : ServiceRequestBase
    {
        public TopicModel Topic { get; set; }

        public CreateTopicRequest(TopicModel topic)
        {
            Topic = topic;
        }
    }
}
