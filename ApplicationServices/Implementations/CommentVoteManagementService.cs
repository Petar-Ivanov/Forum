using ApplicationServices.Interfaces;
using Messaging.Requests.Create;
using Messaging.Requests.Delete;
using Messaging.Requests.Update;
using Messaging.Responses.Create;
using Messaging.Responses.Delete;
using Messaging.Responses.Get;
using Messaging.Responses.Update;
using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Requests.Get;
using Messaging.Responses.GetBy;

namespace ApplicationServices.Implementations
{
    public class CommentVoteManagementService : BaseManagementService, ICommentVoteManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentVoteManagementService(ILogger<CommentVoteManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateVoteResponse> CreateVote(CreateVoteRequest request)
        {
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.Vote.UserId) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Discussions.AnyAsync(x => x.Id == request.Vote.DiscussionId) == false)
            {
                _logger.LogError("Discussion not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Comments.AnyAsync(x => x.Id == request.Vote.CommentId) == false)
            {
                _logger.LogError("Comment not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.CommentVotes.AnyAsync(x => x.UserId == request.Vote.UserId && x.DiscussionId == request.Vote.DiscussionId && x.CommentId == request.Vote.CommentId && x.IsPositive == request.Vote.IsPositive))
            {
                _logger.LogError("Vote must be unique.");
                throw new Exception("");
            }

            //await _context.CommentsVotes.AddAsync(new()
            _unitOfWork.CommentVotes.Insert(new()
            {
                IsPositive = request.Vote.IsPositive,
                CreatedOn = DateTime.UtcNow,
                Source = request.Vote.Source,
                DiscussionId = request.Vote.DiscussionId,
                UserId = request.Vote.UserId,
                CommentId = request.Vote.CommentId
                
                //Discussion
                //User
            });
            //await _context.SaveChangesAsync();
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteVoteResponse> DeleteVote(DeleteVoteRequest request)
        {
            //var vote = _context.CommentsVotes.Find(request.Id);
            var vote = await _unitOfWork.CommentVotes.GetByIdAsync(request.Id);

            if(vote == null)
            {
                _logger.LogError("Vote not found.");
                throw new Exception("");
            }

            //_context.CommentsVotes.Remove(vote);
            //await _context.SaveChangesAsync();
            _unitOfWork.CommentVotes.Delete(vote);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetCommentVotesResponse> GetVotes()
        {
            GetCommentVotesResponse response = new() { Votes = new() };
            //var votes = await _context.CommentsVotes.ToListAsync();
            var votes = await _unitOfWork.CommentVotes.GetAllAsync();

            foreach (var vote in votes)
            {
                var comment = (await _unitOfWork.Comments.GetAllAsync()).FirstOrDefault(x => x.Id == vote.CommentId);
                var user = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == vote.UserId);

                response.Votes.Add(new()
                {
                    Id = vote.Id,
                    CreatedOn = (DateTime)vote.CreatedOn,
                    IsPositive = vote.IsPositive,
                    Source = vote.Source,
                    //Discussion = discussion.Title,
                    Comment = comment.Id.ToString(),
                    User = user.Username
                });
            }

            return response;
        }

        public async Task<GetCommentVoteByIdsResponse> GetVoteByIds(GetCommentVoteByIdsRequest request)
        {
            GetCommentVoteByIdsResponse response = new();
            //var votes = await _context.DiscussionVotes.ToListAsync();
            var vote = (await _unitOfWork.CommentVotes.GetAllAsync()).Where(x => x.DiscussionId == request.Discussion_Id && x.UserId == request.User_Id && x.CommentId == request.Comment_Id).FirstOrDefault();
            if (vote == null)
            {
                _logger.LogError("Comment vote not found");
                throw new Exception("");
            }

            var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == vote.DiscussionId);
            var user = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == vote.UserId);

            response.CommentVote = new()
            {
                Id = vote.Id,
                CreatedOn = (DateTime)vote.CreatedOn,
                IsPositive = vote.IsPositive,
                Source = vote.Source,
                User = user.Username,
                Comment = vote.Id.ToString(),

            };

            return response;
        }

        public async Task<UpdateCommentVoteResponse> UpdateVote(UpdateVoteRequest request)
        {
            var commentVote = await _unitOfWork.CommentVotes.GetByIdAsync(request.Vote.Id ?? default);

            if (request.Vote.Id == null || commentVote == null)
            {
                _logger.LogError("Comment vote with identifier {request.Id} not found", request.Vote.Id);
                throw new Exception("");
            }
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.Vote.UserId) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Discussions.AnyAsync(x => x.Id == request.Vote.DiscussionId) == false)
            {
                _logger.LogError("Discussion not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Comments.AnyAsync(x => x.Id == request.Vote.CommentId) == false)
            {
                _logger.LogError("Comment not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.CommentVotes.AnyAsync(x => x.UserId == request.Vote.UserId && x.DiscussionId == request.Vote.DiscussionId && x.CommentId == request.Vote.CommentId && x.IsPositive == request.Vote.IsPositive))
            {
                _logger.LogError("Vote must be unique.");
                throw new Exception("");
            }

            var updatedCommentVote = request.Vote;

            commentVote.DiscussionId = updatedCommentVote.DiscussionId;
            commentVote.UserId = updatedCommentVote.UserId;
            commentVote.CommentId = updatedCommentVote.CommentId;
            commentVote.Source = updatedCommentVote.Source;
            commentVote.IsPositive = updatedCommentVote.IsPositive;

            _unitOfWork.CommentVotes.Update(commentVote);
            await _unitOfWork.SaveChangesAsync();
            return new();
        }
    }
}
