using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Models
{
    /// <summary>
    /// View request model
    /// </summary>
    public class ViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the view.
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }//
        /// <summary>
        /// Gets or sets the source of the view.
        /// </summary>
        /// <example>Google Chrome</example>
        public string? Source { get; set; }
        /// <summary>
        /// Gets or sets the discussion id of the view.
        /// </summary>
        /// <example>7</example>
        public int DiscussionId { get; set; }
        /// <summary>
        /// Gets or sets the user identifier of the view.
        /// </summary>
        /// <example>15</example>
        public int UserId { get; set; }
        /// <summary>                                                                      
        /// Gets or sets wether the the view has been revisited..
        /// </summary>
        /// <example>true</example>
        public bool? Revisited { get; set; }
    }
}
