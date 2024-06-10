using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.ViewModels
{
    /// <summary>
    /// Comment view model
    /// </summary>
    public class CommentViewModel
    {
        public int? Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string? Source { get; set; }
        public string Discussion { get; set; }
        public int? DiscussionId { get; set; }
        public string User { get; set; }
        public int? UserId { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        required public bool IsUpdated { get; set; }
        required public string Text { get; set; }
        public int? UpVoteCount { get; set; }
        public int? DownVoteCount { get; set; }
        public double? TimeDifference { get; set; }

    }
}
