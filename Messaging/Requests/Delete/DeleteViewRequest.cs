using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Delete
{
    /// <summary>
    /// Delete view request object
    /// </summary>
    public class DeleteViewRequest : IntegerServiceRequestBase
    {
        public DeleteViewRequest(int id) : base(id)
        {
        }
    }
}
