using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public static class IdTracker
    {
        public static int ActiveDiscussionId { get; set; }
        public static int ActiveCommentId { get; set; }
        public static int ActiveTopicId { get; set; }
    }
}
