using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.ViewModels
{
    /// <summary>
    /// Topic view model
    /// </summary>
    public class TopicViewModel
    {
        public int? Id { get; set; }
        public string? CreatedBy { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        required public bool IsVisible { get; set; }
        required public string Name { get; set; }
        required public string Description { get; set; }
        public double? TimeDifference { get; set; }
        public int DiscussionCount { get; set; }
    }
}
