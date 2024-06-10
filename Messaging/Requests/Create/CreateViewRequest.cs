using Messaging.Requests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Requests.Create
{
    /// <summary>
    /// Create view request object
    /// </summary>
    public class CreateViewRequest : ServiceRequestBase
    {
        public ViewModel View { get; set; }

        public CreateViewRequest(ViewModel view)
        {
            View = view;
        }
    }
}
