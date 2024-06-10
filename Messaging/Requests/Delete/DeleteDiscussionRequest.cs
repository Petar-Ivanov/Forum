using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Delete
{
    /// <summary>
    /// Delete discussion request object
    /// </summary>
    public class DeleteDiscussionRequest : IntegerServiceRequestBase
    {
        public DeleteDiscussionRequest(int id) : base(id)
        {
        }
    }
}
