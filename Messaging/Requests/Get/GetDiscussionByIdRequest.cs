using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetDiscussionByIdRequest:IntegerServiceRequestBase
    {
        public GetDiscussionByIdRequest(int id) : base(id)
        {
        }
    }
}
