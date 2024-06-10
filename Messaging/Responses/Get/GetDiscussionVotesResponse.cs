using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Responses.ViewModels;

namespace Messaging.Responses.Get
{
    /// <summary>
    /// Get discussion vote response object
    /// </summary>
    public class GetDiscussionVotesResponse : ServiceResponseBase
    {
        public List<DiscussionVoteViewModel>? Votes { get; set; }
    }
}
