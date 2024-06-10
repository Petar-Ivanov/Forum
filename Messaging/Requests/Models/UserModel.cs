using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Models
{
    /// <summary>
    /// User request model
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }//
        /// <summary>
        /// Gets or sets the username of the user.
        /// </summary>
        /// <example>testUsername</example>
        required public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password of the user.
        /// </summary>
        /// <example>pass123</example>
        required public string Password { get; set; }
        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        /// <example>user@gmail.com</example>
        required public string Email { get; set; }
        /// <summary>
        /// Gets or sets the country of the user.
        /// </summary>
        /// <example>Bulgaria</example>
        required public string Country { get; set; }
        /// <summary>
        /// Gets or sets the biography of the user.
        /// </summary>
        /// <example>Some sample biographical text.</example>
        required public string Biography { get; set; }
        /// <summary>
        /// Gets or sets the birthday of the user
        /// </summary>
        /// <example>null</example>
        required public DateTime BirthDay { get; set; }
        /// <summary>
        /// Gets or sets wether the user is visible.
        /// </summary>
        /// <example>false</example>
        public bool? IsVisible { get; set; }
        /// <summary>
        /// Gets or sets who is the editor.
        /// </summary>
        /// <example>1</example>
        public int? UpdatedBy { get; set; }
    }
}
