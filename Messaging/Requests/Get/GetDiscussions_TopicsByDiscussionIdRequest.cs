using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetDiscussions_TopicsByDiscussionIdRequest: IntegerServiceRequestBase
    {
        public GetDiscussions_TopicsByDiscussionIdRequest(int id) : base(id)
        {
        }
    }
}
