using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Update
{
    /// <summary>
    /// Update vote request object
    /// </summary>
    public class UpdateVoteRequest : ServiceRequestBase
    {
        public VoteModel Vote { get; set; }

        public UpdateVoteRequest(VoteModel vote)
        {
            Vote = vote;
        }
    }
}
