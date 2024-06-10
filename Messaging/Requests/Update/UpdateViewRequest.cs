using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Update
{
    /// <summary>
    /// Update view request object
    /// </summary>
    public class UpdateViewRequest : ServiceRequestBase
    {
        public ViewModel View { get; set; }

        public UpdateViewRequest(ViewModel view)
        {
            View = view;
        }
    }
}
