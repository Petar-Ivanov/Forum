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
    public class Discussions_TopicsRepository : Repository<Discussions_Topics>, IDiscussions_TopicsRepository 
    {
        public Discussions_TopicsRepository(DbContext context) : base(context) { }
    }
}
