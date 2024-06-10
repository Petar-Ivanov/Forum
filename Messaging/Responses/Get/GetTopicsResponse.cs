using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Responses.ViewModels;

namespace Messaging.Responses.Get
{
    /// <summary>
    /// Get topic response object
    /// </summary>
    public class GetTopicsResponse : ServiceResponseBase
    {
        public List<TopicViewModel>? Topics { get; set; }
    }
}
