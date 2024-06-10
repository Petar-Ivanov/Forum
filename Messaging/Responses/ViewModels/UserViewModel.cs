using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.ViewModels
{
    /// <summary>
    /// User view model
    /// </summary>
    public class UserViewModel
    {
        public int? Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        required public bool IsVisible { get; set; }
        required public string Username { get; set; }
        required public string Password { get; set; }
        required public string Email { get; set; }
        required public string Country { get; set; }
        required public string Biography { get; set; }
        required public DateTime BirthDay { get; set; }
        public List<int>? DiscussionsUpVoted { get; set; }
        public List<int>? DiscussionsDownVoted { get; set; }
        public List<int>? CommentsUpVoted { get; set; }
        public List<int>? CommentsDownVoted { get; set; }
    }
}
