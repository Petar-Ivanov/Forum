using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Models
{
    /// <summary>
    /// Comment request model
    /// </summary>
    public class CommentModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the comment.
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }//

        /// <summary>
        /// Gets or sets the source browser of the comment.
        /// </summary>
        /// <example>Firefox</example>
        public string? Source { get; set; }

        /// <summary>
        /// Gets or sets the id of the discussion in which the comment has been written.
        /// </summary>
        /// <example>11</example>
        public int DiscussionId { get; set; }

        /// <summary>
        /// Gets or sets the id of the user who wrote the comment.
        /// </summary>
        /// <example>5</example>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets wether the comment has been updated.
        /// </summary>
        /// <example>false</example>
        required public bool IsUpdated { get; set; }

        /// <summary>
        /// Gets or sets the text content of the comment.
        /// </summary>
        /// <example>This is an example comment.</example>
        required public string Text { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
