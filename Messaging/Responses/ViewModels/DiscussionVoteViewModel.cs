using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.ViewModels
{
    /// <summary>
    /// Discussion vote view model
    /// </summary>
    public class DiscussionVoteViewModel
    {
        public int? Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Source { get; set; }
        public string Discussion { get; set; }
        public string User { get; set; }
        required public bool IsPositive { get; set; }
        //public int? CommentId { get; set; }
    }
}
