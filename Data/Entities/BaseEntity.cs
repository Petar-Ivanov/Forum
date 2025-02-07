﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public abstract class BaseEntity: BaseIdentityEntity
    {
        //[Key]
        //public int Id { get; set; }
        //public int? CreatedBy {  get; set; }
        //public DateTime? CreatedOn { get; set; }
        //public int? UpdatedBy { get; set; }
        //public DateTime? UpdatedOn { get; set;}
        required public bool IsVisible { get; set; }
    }
}
