using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Models
{
    /// <summary>
    /// Vote request model
    /// </summary>
    public class VoteModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the vote.
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }//
        /// <summary>
        /// Gets or sets wether the vote is positive.
        /// </summary>
        /// <example>true</example>
        required public bool IsPositive { get; set; }
        /// <summary>
        /// Gets or sets the the browser name of the user
        /// </summary>
        /// <example>Opera</example>
        public string? Source { get; set; }
        /// <summary>
        /// Gets or sets wether the vote is visible.
        /// </summary>
        /// <example>true</example>
        public bool? IsVisible { get; set; }
        /// <summary>
        /// Gets or sets the discussion id of the vote
        /// </summary>
        /// <example>15</example>
        public int DiscussionId { get; set; }
        /// <summary>
        /// Gets or sets the user Id.
        /// </summary>
        /// <example>6</example>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the comment id.
        /// </summary>
        /// <example>32</example>
        public int? CommentId { get; set; }
    }
}
