using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Topic: BaseEntity
    {
        [StringLength(50)]
        required public string Name { get; set; }
        [StringLength(100)]
        required public string Description { get; set; }
    }
}
