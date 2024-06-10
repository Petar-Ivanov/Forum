using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class TopicsRepository : Repository<Topic>, ITopicsRepository
    {
        public TopicsRepository(DbContext context) : base(context) { }
    }
}
