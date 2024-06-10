using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public class IntegerServiceRequestBase : ServiceRequestBase
    {
        public int Id { get; set; }
        public IntegerServiceRequestBase(int id)
        {
            Id = id;
        }
    }
}
