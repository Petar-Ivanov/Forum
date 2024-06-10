using Messaging.Responses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.GetBy
{
    public class GetCommentByIdResponse: ServiceResponseBase
    {
        public CommentViewModel? Comment { get; set; }
    }
}
