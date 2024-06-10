using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Models
{
    /// <summary>
    /// Topic request model
    /// </summary>
    public class TopicModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the topic.
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }//
        /// <summary>
        /// Gets or sets the unique name of the topic.
        /// </summary>
        /// <example>Some Topic Name</example>
        required public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description of the topic.
        /// </summary>
        /// <example>Sample topic description</example>
        required public string Description { get; set; }
        /// <summary>
        /// Gets or sets wether the topic is visible.
        /// </summary>
        /// <example>true</example>
        required public bool IsVisible { get; set; }
        /// <summary>
        /// Gets or sets the id of the topic's creator.
        /// </summary>
        /// <example>12</example>
        public int? CreatedBy { get; set; }
        /// <summary>
        /// Gets or sets the datetime of the topic's creation.
        /// </summary>
        /// <example>null</example>
        public DateTime? CreatedOn { get; set; }
        /// <summary>
        /// Gets or sets the id of the topic's editor.
        /// </summary>
        /// <example>12</example>
        public int? UpdatedBy { get; set; }
    }
}
