using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Delete
{
    /// <summary>
    /// Delete topic request object
    /// </summary>
    public class DeleteTopicRequest : IntegerServiceRequestBase
    {
        public DeleteTopicRequest(int id) : base(id)
        {
        }
    }
}
