using Messaging.Responses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.GetBy
{
    public class GetDiscussionByIdResponse: ServiceResponseBase
    {
        public DiscussionViewModel? Discussion {  get; set; }
    }
}
