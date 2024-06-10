using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Update
{
    /// <summary>
    /// Update discussions_topics request object
    /// </summary>
    public class UpdateDiscussions_TopicsRequest : ServiceRequestBase
    {
        public Discussions_TopicsModel Discussions_Topics { get; set; }

        public UpdateDiscussions_TopicsRequest(Discussions_TopicsModel discussions_topics)
        {
            Discussions_Topics = discussions_topics;
        }
    }
}
