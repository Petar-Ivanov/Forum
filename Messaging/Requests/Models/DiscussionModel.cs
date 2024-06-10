using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Models
{
    /// <summary>
    /// Discussion request model
    /// </summary>
    public class DiscussionModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the discussion.
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }//
        /// <summary>
        /// Gets or sets the unique title of the discussion.
        /// </summary>
        /// <example>Some Title</example>
        required public string Title { get; set; }
        /// <summary>
        /// Gets or sets the text of the discussion.
        /// </summary>
        /// <example>Some sample text.</example>
        required public string Text { get; set; }
        /// <summary>
        /// Gets or sets wether the discussion has been updated.
        /// </summary>
        /// <example>true</example>
        required public bool IsUpdated { get; set; }
        /// <summary>
        /// Gets or sets wether the discussion is locked.
        /// </summary>
        /// <example>false</example>
        required public bool IsLocked { get; set; }
        /// <summary>
        /// Gets or sets wether the discussion is visible.
        /// </summary>
        /// <example>true</example>
        public bool? IsVisible { get; set; }
        /// <summary>
        /// Gets or sets the image of a discussion.
        /// </summary>
        /// <example>null</example>
        public byte[]? Image { get; set; }
        /// <summary>
        /// Gets or sets the id of the discussion's creator.
        /// </summary>
        /// <example>12</example>
        public int? CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the id of the discussion's editor.
        /// </summary>
        /// <example>67</example>
        public int? UpdatedBy { get; set; }
        //required public bool IsVisible { get; set; }
    }
}
