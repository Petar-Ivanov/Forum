using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetTopicByIdRequest: IntegerServiceRequestBase
    {
        public GetTopicByIdRequest(int id) : base(id)
        {
        }
    }
}
