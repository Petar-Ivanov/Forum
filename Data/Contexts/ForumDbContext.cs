using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contexts
{
    public class ForumDbContext: DbContext
    {
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<DiscussionVote> DiscussionVotes { get; set; }
        public DbSet<CommentVotes> CommentsVotes { get; set; }
        public DbSet<Discussions_Topics> Discussions_Topics { get; set; }

        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            builder.Entity<Discussion>().HasIndex(d => d.Title).IsUnique();
            builder.Entity<Topic>().HasIndex(d => d.Name).IsUnique();
            builder.Entity<View>().HasIndex(v => new { v.DiscussionId, v.UserId }).IsUnique();
            builder.Entity<CommentVotes>().HasIndex(cv => new { cv.CommentId, cv.UserId }).IsUnique();
            builder.Entity<DiscussionVote>().HasIndex(dv => new { dv.DiscussionId, dv.UserId }).IsUnique();
            //builder.Entity<Comment>().HasIndex(c => new { c.DiscussionId, c.UserId }).IsUnique();
            builder.Entity<Discussions_Topics>().HasIndex(dt => new { dt.DiscussionId, dt.TopicId }).IsUnique();
        }
    }
}
