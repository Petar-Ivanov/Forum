using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Update
{
    /// <summary>
    /// Update discussion request object
    /// </summary>
    public class UpdateDiscussionRequest : ServiceRequestBase
    {
        public DiscussionModel Discussion { get; set; }

        public UpdateDiscussionRequest(DiscussionModel discussion)
        {
            Discussion = discussion;
        }
    }
}
