using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Update
{
    /// <summary>
    /// Update topic request object
    /// </summary>
    public class UpdateTopicRequest : ServiceRequestBase
    {
        public TopicModel Topic { get; set; }

        public UpdateTopicRequest(TopicModel topic)
        {
            Topic = topic;
        }
    }
}
