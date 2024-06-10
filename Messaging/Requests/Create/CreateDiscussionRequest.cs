using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Create
{
    /// <summary>
    /// Create discussion request object
    /// </summary>
    public class CreateDiscussionRequest : ServiceRequestBase
    {
        public DiscussionModel Discussion { get; set; }

        public CreateDiscussionRequest(DiscussionModel discussion)
        {
            Discussion = discussion;
        }
    }
}
