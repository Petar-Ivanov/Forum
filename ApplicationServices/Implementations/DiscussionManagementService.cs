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
using Messaging.Requests.Get;
using Messaging.Responses.GetBy;

namespace ApplicationServices.Implementations
{
    public class DiscussionManagementService : BaseManagementService, IDiscussionManagementService
    {
        //private readonly ForumDbContext _context;

        //public DiscussionManagementService(ILogger<DiscussionManagementService> logger, ForumDbContext context) : base(logger)
        //{
        //    _context = context;
        //}
        private readonly IUnitOfWork _unitOfWork;

        public DiscussionManagementService(ILogger<DiscussionManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateDiscussionResponse> CreateDiscussion(CreateDiscussionRequest request)
        {
            if (request.Discussion.Title == null || request.Discussion.Text == null)
            {
                _logger.LogError("Title and text must not be null.");
                throw new Exception("");
            }
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.Discussion.CreatedBy) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }

            //await _context.Discussions.AddAsync(new()
            _unitOfWork.Discussions.Insert(new()
            {
                IsLocked = false,
                IsUpdated = false,
                IsVisible = true,
                Text = request.Discussion.Text,
                Title = request.Discussion.Title,
                CreatedBy = request.Discussion.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                Image = request.Discussion.Image,
            });
            //await _context.SaveChangesAsync();
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteDiscussionResponse> DeleteDiscussion(DeleteDiscussionRequest request)
        {
            //var discussion = _context.Discussions.Find(request.Id);
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(request.Id);

            if (discussion == null)
            {
                _logger.LogError("Discussion with identifier {request.Id} not found", request.Id);
                throw new Exception("");
            }

            //_context.Discussions.Remove(discussion);
            //await _context.SaveChangesAsync();
            _unitOfWork.Discussions.Delete(discussion);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetDiscussionsResponse> GetDiscussions()
        {
            GetDiscussionsResponse response = new() { Discussions = new() };
            //var discussions = await _context.Discussions.ToListAsync();
            var discussions = await _unitOfWork.Discussions.GetAllAsync();

            foreach (var discussion in discussions)
            {
                //string creatorUsername = _context.Users.Where(x => x.Id == discussion.CreatedBy).First().Username;
                var creator = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == discussion.CreatedBy);
                //string editorUsername = null;

                //editorUsername = _context.Users.Where(x => x.Id == discussion.UpdatedBy).First().Username;

                var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == discussion.UpdatedBy);
                string editorUsername = editor != null ? editor.Username : null;

                var topicIds = _unitOfWork.Discussions_Topics.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Select(x => x.TopicId).ToList();
                var topicNames = _unitOfWork.Topics.GetAllAsync().Result.Where(x => topicIds.Contains(x.Id)).Select(x => x.Name).ToList();

                response.Discussions.Add(new()
                {
                    Id = discussion.Id,
                    CreatedBy = creator.Username,
                    CreatedOn = discussion.CreatedOn,
                    Image = discussion.Image,
                    IsLocked = discussion.IsLocked,
                    IsUpdated = discussion.IsUpdated,
                    IsVisible = discussion.IsVisible,
                    Text = discussion.Text,
                    Title = discussion.Title,
                    UpdatedBy = editorUsername,
                    UpdatedOn = discussion.UpdatedOn,
                    ViewCount = _unitOfWork.Views.GetAllAsync().Result.Where(x=>x.DiscussionId == discussion.Id).Count(),
                    CommentCount = _unitOfWork.Comments.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Count(),
                    UpVoteCount = _unitOfWork.DiscussionVotes.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id && x.IsPositive == true).Count(),
                    DownVoteCount = _unitOfWork.DiscussionVotes.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id && x.IsPositive == false).Count(),
                    TimeDifference = (DateTime.UtcNow - discussion.CreatedOn).Value.TotalHours,
                    Topics = topicNames,
                    CreatedById = creator.Id
                });
            }

            return response;
        }

        public async Task<GetDiscussionByIdResponse> GetDiscussionById(GetDiscussionByIdRequest request)
        {
            GetDiscussionByIdResponse response = new();
            //var discussions = await _context.Discussions.ToListAsync();
            var discussion = (await _unitOfWork.Discussions.GetAllAsync()).Where(x => x.Id == request.Id).FirstOrDefault();
            if (discussion == null)
            {
                _logger.LogError("Discussion with identifier {request.Id} not found", request.Id);
                throw new Exception("");
            }

            //string creatorUsername = _context.Users.Where(x => x.Id == discussion.CreatedBy).First().Username;
            var creator = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == discussion.CreatedBy);
            //string editorUsername = null;

            //editorUsername = _context.Users.Where(x => x.Id == discussion.UpdatedBy).First().Username;

            var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == discussion.UpdatedBy);
            string editorUsername = editor != null ? editor.Username : null;

            var topicIds = _unitOfWork.Discussions_Topics.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Select(x => x.TopicId).ToList();
            var topicNames = _unitOfWork.Topics.GetAllAsync().Result.Where(x => topicIds.Contains(x.Id)).Select(x => x.Name).ToList();

            response.Discussion = new()
            {
                Id = discussion.Id,
                CreatedBy = creator.Username,
                CreatedOn = discussion.CreatedOn,
                Image = discussion.Image,
                IsLocked = discussion.IsLocked,
                IsUpdated = discussion.IsUpdated,
                IsVisible = discussion.IsVisible,
                Text = discussion.Text,
                Title = discussion.Title,
                UpdatedBy = editorUsername,
                UpdatedOn = discussion.UpdatedOn,
                ViewCount = _unitOfWork.Views.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Count(),
                CommentCount = _unitOfWork.Comments.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Count(),
                UpVoteCount = _unitOfWork.DiscussionVotes.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id && x.IsPositive == true).Count(),
                DownVoteCount = _unitOfWork.DiscussionVotes.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id && x.IsPositive == false).Count(),
                TimeDifference = (DateTime.UtcNow - discussion.CreatedOn).Value.TotalHours,
                Topics = topicNames,
                CreatedById = creator.Id
            };

            return response;
        }

        public async Task<GetDiscussionsByTitleResponse> GetDiscussionsByTitle(GetDiscussionsByTitleRequest request)
        {
            GetDiscussionsByTitleResponse response = new() { Discussions = new() };
            //var discussions = await _context.Discussions.ToListAsync();
            var discussions = (await _unitOfWork.Discussions.GetAllAsync()).Where(x=>x.Title.Trim().ToLower().Contains(request.SearchTerm.Trim().ToLower()));

            foreach (var discussion in discussions)
            {
                //string creatorUsername = _context.Users.Where(x => x.Id == discussion.CreatedBy).First().Username;
                var creator = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == discussion.CreatedBy);
                //string editorUsername = null;

                //editorUsername = _context.Users.Where(x => x.Id == discussion.UpdatedBy).First().Username;

                var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == discussion.UpdatedBy);
                string editorUsername = editor != null ? editor.Username : null;

                var topicIds = _unitOfWork.Discussions_Topics.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Select(x => x.TopicId).ToList();
                var topicNames = _unitOfWork.Topics.GetAllAsync().Result.Where(x => topicIds.Contains(x.Id)).Select(x => x.Name).ToList();

                response.Discussions.Add(new()
                {
                    Id = discussion.Id,
                    CreatedBy = creator.Username,
                    CreatedOn = discussion.CreatedOn,
                    Image = discussion.Image,
                    IsLocked = discussion.IsLocked,
                    IsUpdated = discussion.IsUpdated,
                    IsVisible = discussion.IsVisible,
                    Text = discussion.Text,
                    Title = discussion.Title,
                    UpdatedBy = editorUsername,
                    UpdatedOn = discussion.UpdatedOn,
                    ViewCount = _unitOfWork.Views.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Count(),
                    CommentCount = _unitOfWork.Comments.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Count(),
                    UpVoteCount = _unitOfWork.DiscussionVotes.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id && x.IsPositive == true).Count(),
                    DownVoteCount = _unitOfWork.DiscussionVotes.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id && x.IsPositive == false).Count(),
                    TimeDifference = (DateTime.UtcNow - discussion.CreatedOn).Value.TotalHours,
                    Topics = topicNames,
                    CreatedById = creator.Id
                });
            }

            return response;
        }

        public async Task<GetDiscussionsByTopicResponse> GetDiscussionsByTopic(GetDiscussionsByTopicRequest request)
        {
            GetDiscussionsByTopicResponse response = new() { Discussions = new() };
            //var discussions = await _context.Discussions.ToListAsync();
            var discussions = await _unitOfWork.Discussions.GetAllAsync();

            foreach (var discussion in discussions)
            {
                //string creatorUsername = _context.Users.Where(x => x.Id == discussion.CreatedBy).First().Username;
                var creator = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == discussion.CreatedBy);
                //string editorUsername = null;

                //editorUsername = _context.Users.Where(x => x.Id == discussion.UpdatedBy).First().Username;

                var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == discussion.UpdatedBy);
                string editorUsername = editor != null ? editor.Username : null;

                var topicIds = _unitOfWork.Discussions_Topics.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Select(x => x.TopicId).ToList();
                var topicNames = _unitOfWork.Topics.GetAllAsync().Result.Where(x => topicIds.Contains(x.Id)).Select(x => x.Name).ToList();

                if(topicNames != null && topicNames.Contains(request.Topic))
                {
                    response.Discussions.Add(new()
                    {
                        Id = discussion.Id,
                        CreatedBy = creator.Username,
                        CreatedOn = discussion.CreatedOn,
                        Image = discussion.Image,
                        IsLocked = discussion.IsLocked,
                        IsUpdated = discussion.IsUpdated,
                        IsVisible = discussion.IsVisible,
                        Text = discussion.Text,
                        Title = discussion.Title,
                        UpdatedBy = editorUsername,
                        UpdatedOn = discussion.UpdatedOn,
                        ViewCount = _unitOfWork.Views.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Count(),
                        CommentCount = _unitOfWork.Comments.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id).Count(),
                        UpVoteCount = _unitOfWork.DiscussionVotes.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id && x.IsPositive == true).Count(),
                        DownVoteCount = _unitOfWork.DiscussionVotes.GetAllAsync().Result.Where(x => x.DiscussionId == discussion.Id && x.IsPositive == false).Count(),
                        TimeDifference = (DateTime.UtcNow - discussion.CreatedOn).Value.TotalHours,
                        Topics = topicNames,
                        CreatedById = creator.Id
                    });
                }
                
            }

            return response;
        }

        public async Task<UpdateDiscussionResponse> UpdateDiscussion(UpdateDiscussionRequest request)
        {
            var discussion = await _unitOfWork.Discussions.GetByIdAsync(request.Discussion.Id ?? default);

            if (request.Discussion.Id == null || discussion == null)
            {
                _logger.LogError("Discussion with identifier {request.Id} not found", request.Discussion.Id);
                throw new Exception("");
            }
            if (request.Discussion.Title == null || request.Discussion.Text == null)
            {
                _logger.LogError("Title and text must not be null.");
                throw new Exception("");
            }
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.Discussion.UpdatedBy) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }

            var updatedDiscussion = request.Discussion;

            discussion.Title = updatedDiscussion.Title;
            discussion.Image = updatedDiscussion.Image ?? discussion.Image;
            discussion.Text = updatedDiscussion.Text;
            discussion.IsVisible = updatedDiscussion.IsVisible ?? discussion.IsVisible;
            discussion.IsLocked = updatedDiscussion.IsLocked;
            discussion.IsUpdated = true;
            discussion.UpdatedOn = DateTime.UtcNow;
            discussion.UpdatedBy = updatedDiscussion?.UpdatedBy ?? default;

            _unitOfWork.Discussions.Update(discussion);
            await _unitOfWork.SaveChangesAsync();
            return new();
        }
    }
}
