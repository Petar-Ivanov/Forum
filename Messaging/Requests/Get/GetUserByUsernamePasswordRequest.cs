using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetUserByUsernamePasswordRequest: ServiceRequestBase
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public GetUserByUsernamePasswordRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
