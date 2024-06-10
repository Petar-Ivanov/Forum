using Messaging.Responses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.Get
{
    /// <summary>
    /// Get comment response object
    /// </summary>
    public class GetCommentsResponse : ServiceResponseBase
    {
        public List<CommentViewModel>? Comments { get; set; }
    }
}
