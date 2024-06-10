using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Delete
{
    /// <summary>
    /// Delete discussions_topics request object
    /// </summary>
    public class DeleteDiscussions_TopicsRequest : IntegerServiceRequestBase
    {
        public DeleteDiscussions_TopicsRequest(int id) : base(id)
        {
        }
    }
}
