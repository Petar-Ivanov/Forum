using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Update
{
    /// <summary>
    /// Update comment request object
    /// </summary>
    public class UpdateCommentRequest : ServiceRequestBase
    {
        public CommentModel Comment { get; set; }

        public UpdateCommentRequest(CommentModel comment)
        {
            Comment = comment;
        }
    }
}
