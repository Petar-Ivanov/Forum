using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Delete
{
    /// <summary>
    /// Delete user request object
    /// </summary>
    public class DeleteUserRequest : IntegerServiceRequestBase
    {
        public DeleteUserRequest(int id) : base(id)
        {
        }
    }
}
