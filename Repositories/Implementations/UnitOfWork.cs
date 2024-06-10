using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implementations
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly DbContext context;

        public ICommentsRepository Comments { get; set; }
        public ICommentVotesRepository CommentVotes { get; set; }
        public IDiscussions_TopicsRepository Discussions_Topics { get; set; }
        public IDiscussionsRepository Discussions { get; set; }
        public IDiscussionVotesRepository DiscussionVotes { get; set; }
        public ITopicsRepository Topics { get; set; }
        public IUsersRepository Users { get; set; }
        public IViewsRepository Views { get; set; }

        public DbContext Context { get { return context; } }

        public UnitOfWork(DbContext context)
        {
            this.context = context;
            Comments = new CommentsRepository(context);
            CommentVotes = new CommentVotesRepository(context);
            Discussions_Topics = new Discussions_TopicsRepository(context);
            Discussions = new DiscussionsRepository(context);
            DiscussionVotes = new DiscussionVotesRepository(context);
            Topics = new TopicsRepository(context);
            Users = new UsersRepository(context);
            Views = new ViewsRepository(context);
        }

        public void Dispose() => this.Dispose(true);

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.context?.Dispose();
            }
        }

        public async Task<int> SaveChangesAsync() => await this.context.SaveChangesAsync();
    }
}
