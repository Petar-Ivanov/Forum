using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public abstract class BaseImpressions: BaseIdentityEntity
    {
        //[Key]
        //public int Id { get; set; }
        //public DateTime CreatedOn { get; set; }
        [StringLength(40)]
        public string? Source { get; set; }
        public int DiscussionId { get; set; }
        public virtual Discussion Discussion { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        

    }
}
