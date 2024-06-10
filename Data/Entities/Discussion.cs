using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Discussion: BaseEntity
    {
        [StringLength(80)]
        required public string Title { get; set; }
        [MaxLength]
        required public string Text { get; set; }
        required public bool IsUpdated { get; set; }
        required public bool IsLocked { get; set; }
        public byte[]? Image { get; set; }
        public virtual ICollection<View> Views { get; set; }
        public virtual ICollection<DiscussionVote> DiscussionVote { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
