using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetUsersByUsernameRequest: ServiceRequestBase
    {
        public string Username { get; set; }

        public GetUsersByUsernameRequest(string username)
        {
            Username = username;
        }
    }
}
