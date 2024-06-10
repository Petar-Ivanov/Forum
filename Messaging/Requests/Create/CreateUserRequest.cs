using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Create
{
    /// <summary>
    /// Create user request object
    /// </summary>
    public class CreateUserRequest : ServiceRequestBase
    {
        public UserModel User { get; set; }

        public CreateUserRequest(UserModel user)
        {
            User = user;
        }
    }
}
