using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public abstract class BaseVotes: BaseImpressions
    {
        required public bool IsPositive { get; set; }
    }
}
