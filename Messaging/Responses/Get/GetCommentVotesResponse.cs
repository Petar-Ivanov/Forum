using Messaging.Responses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.Get
{
    /// <summary>
    /// Get comment vote response object
    /// </summary>
    public class GetCommentVotesResponse : ServiceResponseBase
    {
        public List<CommentVoteViewModel>? Votes { get; set; }
    }
}
