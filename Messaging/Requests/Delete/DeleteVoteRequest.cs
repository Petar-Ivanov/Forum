using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Delete
{
    /// <summary>
    /// Delete vote request object
    /// </summary>
    public class DeleteVoteRequest : IntegerServiceRequestBase
    {
        public DeleteVoteRequest(int id) : base(id)
        {
        }
    }
}
