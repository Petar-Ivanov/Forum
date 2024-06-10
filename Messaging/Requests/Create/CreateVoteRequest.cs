using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Create
{
    /// <summary>
    /// Create vote request object
    /// </summary>
    public class CreateVoteRequest : ServiceRequestBase
    {
        public VoteModel Vote { get; set; }

        public CreateVoteRequest(VoteModel vote)
        {
            Vote = vote;
        }
    }
}
