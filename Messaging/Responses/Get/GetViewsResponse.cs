using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Responses.ViewModels;

namespace Messaging.Responses.Get
{
    /// <summary>
    /// Get vote response object
    /// </summary>
    public class GetViewsResponse : ServiceResponseBase
    {
        public List<ViewViewModel>? Views { get; set; }
    }
}
