using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Update
{
    /// <summary>
    /// Update user request object
    /// </summary>
    public class UpdateUserRequest : ServiceRequestBase
    {
        public UserModel User { get; set; }

        public UpdateUserRequest(UserModel user)
        {
            User = user;
        }
    }
}
