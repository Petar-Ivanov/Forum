using Messaging.Responses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.GetBy
{
    public class GetCommentVoteByIdsResponse:ServiceResponseBase
    {
        public CommentVoteViewModel? CommentVote { get; set; }
    }
}
