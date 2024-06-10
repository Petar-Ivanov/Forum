using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.ViewModels
{
    /// <summary>
    /// Discussion view model
    /// </summary>
    public class DiscussionViewModel
    {
        public int? Id { get; set; }
        public string? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        required public bool IsVisible { get; set; }
        required public string Title { get; set; }
        required public string Text { get; set; }
        required public bool IsUpdated { get; set; }
        required public bool IsLocked { get; set; }
        public byte[] Image { get; set; }
        public int? UpVoteCount { get; set; }
        public int? DownVoteCount { get; set; }
        public int? CommentCount { get; set; }
        public int? ViewCount { get; set; }
        public double? TimeDifference { get; set; }
        public List<string>? Topics { get; set; }
    }
}
