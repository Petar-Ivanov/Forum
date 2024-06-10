using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        DbContext Context { get; }

        ICommentsRepository Comments { get; }
        ICommentVotesRepository CommentVotes { get; }
        IDiscussions_TopicsRepository Discussions_Topics { get; }
        IDiscussionsRepository Discussions { get; }
        IDiscussionVotesRepository DiscussionVotes { get; }
        ITopicsRepository Topics { get; }
        IUsersRepository Users { get; }
        IViewsRepository Views { get; }

        Task<int> SaveChangesAsync();
    }
}
