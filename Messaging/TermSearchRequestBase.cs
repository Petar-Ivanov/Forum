using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public class TermSearchRequestBase: ServiceRequestBase
    {
        public string SearchTerm { get; set; }
        public TermSearchRequestBase(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
