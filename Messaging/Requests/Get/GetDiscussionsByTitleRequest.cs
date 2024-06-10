using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Get
{
    public class GetDiscussionsByTitleRequest : TermSearchRequestBase
    {
        public GetDiscussionsByTitleRequest(string searchTerm) : base(searchTerm)
        {
        }
    }
}
