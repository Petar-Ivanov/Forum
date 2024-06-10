using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Responses.ViewModels;

namespace Messaging.Responses.Get
{
    /// <summary>
    /// Get user response object
    /// </summary>
    public class GetUsersResponse : ServiceResponseBase
    {
        public List<UserViewModel>? Users { get; set; }
    }
}
