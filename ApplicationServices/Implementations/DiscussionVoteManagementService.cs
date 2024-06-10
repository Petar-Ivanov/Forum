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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging.Responses.GetBy;
using Messaging.Requests.Get;
using System.Net;

namespace ApplicationServices.Implementations
{
    public class DiscussionVoteManagementService : BaseManagementService, IDiscussionVoteManagementService
    {
        //private readonly ForumDbContext _context;

        //public DiscussionVoteManagementService(ILogger<DiscussionVoteManagementService> logger, ForumDbContext context) : base(logger)
        //{
        //    _context = context;
        //}
        private readonly IUnitOfWork _unitOfWork;

        public DiscussionVoteManagementService(ILogger<DiscussionVoteManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
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
            if (await _unitOfWork.DiscussionVotes.AnyAsync(x => x.UserId == request.Vote.UserId && x.DiscussionId == request.Vote.DiscussionId && x.IsPositive == request.Vote.IsPositive))
            {
                _logger.LogError("Vote must be unique.");
                throw new Exception("");
            }

            //await _context.DiscussionVotes.AddAsync(new()
            _unitOfWork.DiscussionVotes.Insert(new()
            {
                IsPositive = request.Vote.IsPositive,
                Source = request.Vote.Source,
                UserId = request.Vote.UserId,
                DiscussionId = request.Vote.DiscussionId,
                CreatedOn = DateTime.UtcNow,
                //Discussion
                //User
            });
            //await _context.SaveChangesAsync();
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteVoteResponse> DeleteVote(DeleteVoteRequest request)
        {
            //var vote = _context.DiscussionVotes.Find(request.Id);
            var vote = await _unitOfWork.DiscussionVotes.GetByIdAsync(request.Id);

            if (vote == null)
            {
                _logger.LogError("Discussion vote with identifier {request.Id} not found", request.Id);
                throw new Exception("");
            }

            //_context.DiscussionVotes.Remove(vote);
            //await _context.SaveChangesAsync();
            _unitOfWork.DiscussionVotes.Delete(vote);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetDiscussionVotesResponse> GetVotes()
        {
            GetDiscussionVotesResponse response = new() { Votes = new() };
            //var votes = await _context.DiscussionVotes.ToListAsync();
            var votes = await _unitOfWork.DiscussionVotes.GetAllAsync();

            foreach (var vote in votes)
            {
                //var discussion = _context.Discussions.Where(x => x.Id == vote.DiscussionId).First();
                //var user = _context.Users.Where(x => x.Id == vote.UserId).First();
                var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == vote.DiscussionId);
                var user = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == vote.UserId);

                response.Votes.Add(new()
                {
                    Id = vote.Id,
                    CreatedOn = (DateTime)vote.CreatedOn,
                    IsPositive = vote.IsPositive,
                    Source = vote.Source,
                    Discussion = discussion.Title,
                    User = user.Username
                });
            }

            return response;
        }

        public async Task<GetDiscussionVoteByIdsResponse> GetVoteByIds(GetDiscussionVoteByIdsRequest request)
        {
            GetDiscussionVoteByIdsResponse response = new();
            //var votes = await _context.DiscussionVotes.ToListAsync();
            var vote = (await _unitOfWork.DiscussionVotes.GetAllAsync()).Where(x=>x.DiscussionId == request.Discussion_Id && x.UserId==request.User_Id).FirstOrDefault();
            if(vote == null)
            {
                _logger.LogError("Discussion vote not found");
                throw new Exception("");
            }

            //var discussion = _context.Discussions.Where(x => x.Id == vote.DiscussionId).First();
            //var user = _context.Users.Where(x => x.Id == vote.UserId).First();
            var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == vote.DiscussionId);
            var user = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == vote.UserId);

            response.DiscussionVote = new()
            {
                Id = vote.Id,
                CreatedOn = (DateTime)vote.CreatedOn,
                IsPositive = vote.IsPositive,
                Source = vote.Source,
                Discussion = discussion.Title,
                User = user.Username
            };

            return response;
        }

        public async Task<UpdateDiscussionVoteResponse> UpdateVote(UpdateVoteRequest request)
        {
            var discussionVote = await _unitOfWork.DiscussionVotes.GetByIdAsync(request.Vote.Id ?? default);

            if (request.Vote.Id == null || discussionVote == null)
            {
                _logger.LogError("Discussion vote with identifier {request.Id} not found", request.Vote.Id);
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
            if (await _unitOfWork.DiscussionVotes.AnyAsync(x => x.UserId == request.Vote.UserId && x.DiscussionId == request.Vote.DiscussionId && x.IsPositive == request.Vote.IsPositive))
            {
                _logger.LogError("Vote must be unique.");
                throw new Exception("");
            }


            var updatedDiscussionVote = request.Vote;

            discussionVote.DiscussionId = updatedDiscussionVote.DiscussionId;
            discussionVote.UserId = updatedDiscussionVote.UserId;
            discussionVote.IsPositive = updatedDiscussionVote.IsPositive;
            discussionVote.Source = updatedDiscussionVote.Source;

            _unitOfWork.DiscussionVotes.Update(discussionVote);
            await _unitOfWork.SaveChangesAsync();
            return new();
        }
    }
}
