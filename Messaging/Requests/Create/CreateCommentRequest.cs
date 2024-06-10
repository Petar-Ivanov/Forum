using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Create
{
    /// <summary>
    /// Create comment request object
    /// </summary>
    public class CreateCommentRequest : ServiceRequestBase
    {
        public CommentModel Comment { get; set; }

        public CreateCommentRequest(CommentModel comment)
        {
            Comment = comment;
        }
    }
}
