using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Responses.ViewModels;

namespace Messaging.Responses.Get
{
    /// <summary>
    /// Get discussion response object
    /// </summary>
    public class GetDiscussionsResponse : ServiceResponseBase
    {
        public List<DiscussionViewModel>? Discussions { get; set; }
    }
}
