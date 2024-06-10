using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetDiscussionVoteByIdsRequest: ServiceRequestBase
    {

        public int Discussion_Id { get; set; }
        public int User_Id { get; set; }

        public GetDiscussionVoteByIdsRequest(int discussion_Id, int user_Id)
        {
            Discussion_Id = discussion_Id;
            User_Id = user_Id;
        }
    }
}
