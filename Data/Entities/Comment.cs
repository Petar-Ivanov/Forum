using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Comment : BaseImpressions
    {
        //public DateTime? UpdatedOn { get; set; }
        required public bool IsUpdated { get; set; }

        [MaxLength]
        required public string Text { get; set; }
    }
}
