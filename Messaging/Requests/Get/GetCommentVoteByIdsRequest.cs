using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetCommentVoteByIdsRequest : ServiceRequestBase
    {
        public int Discussion_Id { get; set; }
        public int User_Id { get; set; }
        public int Comment_Id { get; set; }

        public GetCommentVoteByIdsRequest(int discussion_Id, int user_Id, int comment_id)
        {
            Discussion_Id = discussion_Id;
            User_Id = user_Id;
            Comment_Id = comment_id;
        }
    }
}
