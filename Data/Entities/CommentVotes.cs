using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class CommentVotes: BaseVotes
    {
        public int? CommentId { get; set; }
    }
}
