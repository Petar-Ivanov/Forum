using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Delete
{
    /// <summary>
    /// Delete comment request object
    /// </summary>
    public class DeleteCommentRequest : IntegerServiceRequestBase
    {
        public DeleteCommentRequest(int id) : base(id)
        {
        }
    }
}
