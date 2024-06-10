using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Delete
{
    public class DeleteDiscussions_TopicsByIdsRequest: ServiceRequestBase
    {
        public int DiscussionId { get; set; }
        public int TopicId { get; set; }

        public DeleteDiscussions_TopicsByIdsRequest(int discussionId, int topicId)
        {
            DiscussionId = discussionId;
            TopicId = topicId;
        }
    }
}
