using Messaging.Responses.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Responses.GetBy
{
    public class GetUserByIdResponse: ServiceResponseBase
    {
        public UserViewModel? User { get; set; }
    }
}
