using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Create
{
    /// <summary>
    /// Create discussions_topics request object
    /// </summary>
    public class CreateDiscussions_TopicsRequest : ServiceRequestBase
    {
        public Discussions_TopicsModel Discussions_Topics { get; set; }

        public CreateDiscussions_TopicsRequest(Discussions_TopicsModel discussions_Topics)
        {
            Discussions_Topics = discussions_Topics;
        }
    }
}
