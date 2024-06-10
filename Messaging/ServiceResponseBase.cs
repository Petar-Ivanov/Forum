using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public abstract class ServiceResponseBase
    {
        public BusinessStatusCodeEnum StatusCode { get; set; }

        public ServiceResponseBase()
        {
            StatusCode = BusinessStatusCodeEnum.Success;
        }

        public ServiceResponseBase(BusinessStatusCodeEnum statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
