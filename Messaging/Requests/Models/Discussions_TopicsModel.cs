using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Models
{
    /// <summary>
    /// Discussions_topics request model
    /// </summary>
    public class Discussions_TopicsModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the discussions_topics.
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }//
        /// <summary>
        /// Gets or sets the source browser of the discussions_topics.
        /// </summary>
        /// <example>Firefox</example>
        public string? Source { get; set; }
        //required public bool IsVisible { get; set; }
        /// <summary>
        /// Gets or sets the id of the discussion of the discussions_topics.
        /// </summary>
        /// <example>11</example>
        public int DiscussionId { get; set; }
        /// <summary>
        /// Gets or sets the id of the topic of the discussions_topics.
        /// </summary>
        /// <example>11</example>
        public int TopicId { get; set; }
        /// <summary>
        /// Gets or sets wether the discussions_topics is visible.
        /// </summary>
        /// <example>true</example>
        public bool? IsVisible { get; set; }

    }
}
