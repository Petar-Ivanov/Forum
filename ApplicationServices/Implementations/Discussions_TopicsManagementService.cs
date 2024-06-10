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
    public class Discussions_TopicsManagementService : BaseManagementService, IDiscussions_TopicsManagementService
    {
        //private readonly ForumDbContext _context;

        //public Discussions_TopicsManagementService(ILogger<Discussions_TopicsManagementService> logger, ForumDbContext context) : base(logger)
        //{
        //    _context = context;
        //}
        private readonly IUnitOfWork _unitOfWork;

        public Discussions_TopicsManagementService(ILogger<Discussions_TopicsManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateDiscussions_TopicsResponse> CreateDiscussions_Topics(CreateDiscussions_TopicsRequest request)
        {
            if (await _unitOfWork.Discussions.AnyAsync(x => x.Id == request.Discussions_Topics.DiscussionId) == false)
            {
                _logger.LogError("Discussion not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Topics.AnyAsync(x => x.Id == request.Discussions_Topics.TopicId) == false)
            {
                _logger.LogError("Topic not found.");
                throw new Exception("");
            }

            //await _context.Discussions_Topics.AddAsync(new()
            _unitOfWork.Discussions_Topics.Insert(new()
            {
                IsVisible = true,
                CreatedOn = DateTime.UtcNow,
                TopicId = request.Discussions_Topics.TopicId,
                DiscussionId = request.Discussions_Topics.DiscussionId,
                Source = request.Discussions_Topics.Source,
                //Discussion
                //Topic
            });
            //await _context.SaveChangesAsync();
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteDiscussions_TopicsResponse> DeleteDiscussions_Topics(DeleteDiscussions_TopicsRequest request)
        {
            //var discussions_topics = _context.Discussions_Topics.Find(request.Id);
            var discussions_topics = await _unitOfWork.Discussions_Topics.GetByIdAsync(request.Id);

            if (discussions_topics == null)
            {
                _logger.LogError("Discussions_Topics with identifier {request.Id} not found", request.Id);
                throw new Exception("");
            }

            //_context.Discussions_Topics.Remove(discussions_topics);
            //await _context.SaveChangesAsync();
            _unitOfWork.Discussions_Topics.Delete(discussions_topics);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<DeleteDiscussions_TopicsByIdsResponse> DeleteDiscussions_TopicsByIds(DeleteDiscussions_TopicsByIdsRequest request)
        {
            //var discussions_topics = _context.Discussions_Topics.Find(request.Id);
            var discussions_topics = (await _unitOfWork.Discussions_Topics.GetAllAsync()).Where(x => x.DiscussionId == request.DiscussionId && x.TopicId == request.TopicId).FirstOrDefault();

            if (discussions_topics == null)
            {
                _logger.LogError("Discussions_Topics with identifiers not found");
                throw new Exception("");
            }

            //_context.Discussions_Topics.Remove(discussions_topics);
            //await _context.SaveChangesAsync();
            _unitOfWork.Discussions_Topics.Delete(discussions_topics);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetDiscussions_TopicsResponse> GetDiscussions_Topics()
        {
            GetDiscussions_TopicsResponse response = new() { Discussions_Topics = new() };
            //var discussions_topics = await _context.Discussions_Topics.ToListAsync();
            var discussions_topics = await _unitOfWork.Discussions_Topics.GetAllAsync();

            foreach (var discussion_topic in discussions_topics)
            {
                //var discussion = _context.Discussions.Where(x => x.Id == discussion_topic.DiscussionId).First();
                //var topic = _context.Topics.Where(x => x.Id == discussion_topic.TopicId).First();
                var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == discussion_topic.DiscussionId);
                var topic = (await _unitOfWork.Topics.GetAllAsync()).FirstOrDefault(x => x.Id == discussion_topic.TopicId);

                response.Discussions_Topics.Add(new()
                {
                    Id = discussion_topic.Id,
                    CreatedOn = discussion_topic.CreatedOn,
                    Discussion = discussion.Title,
                    IsVisible = discussion_topic.IsVisible,
                    Source = discussion_topic.Source,
                    Topic = topic.Name,
                    DiscussionId = discussion_topic.DiscussionId,
                    TopicId = discussion_topic.TopicId
                });
            }

            return response;
        }

        public async Task<GetDiscussions_TopicsByDiscussionIdResponse> GetDiscussions_TopicsByDiscussionId(GetDiscussions_TopicsByDiscussionIdRequest request)
        {
            GetDiscussions_TopicsByDiscussionIdResponse response = new() { Discussions_Topics = new() };
            //var discussions_topics = await _context.Discussions_Topics.ToListAsync();
            var discussions_topics = (await _unitOfWork.Discussions_Topics.GetAllAsync()).Where(x => x.DiscussionId == request.Id);

            if (discussions_topics == null)
            {
                _logger.LogError("Discussion topics with identifier not found");
                throw new Exception("");
            }

            foreach (var discussion_topic in discussions_topics)
            {
                //var discussion = _context.Discussions.Where(x => x.Id == discussion_topic.DiscussionId).First();
                //var topic = _context.Topics.Where(x => x.Id == discussion_topic.TopicId).First();
                var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == discussion_topic.DiscussionId);
                var topic = (await _unitOfWork.Topics.GetAllAsync()).FirstOrDefault(x => x.Id == discussion_topic.TopicId);

                response.Discussions_Topics.Add(new()
                {
                    Id = discussion_topic.Id,
                    CreatedOn = discussion_topic.CreatedOn,
                    Discussion = discussion.Title,
                    IsVisible = discussion_topic.IsVisible,
                    Source = discussion_topic.Source,
                    Topic = topic.Name,
                    DiscussionId = discussion_topic.DiscussionId,
                    TopicId = discussion_topic.TopicId
                });
            }

            return response;
        }

        public async Task<UpdateDiscussions_TopicsResponse> UpdateDiscussions_Topics(UpdateDiscussions_TopicsRequest request)
        {
            var discussions_topics = await _unitOfWork.Discussions_Topics.GetByIdAsync(request.Discussions_Topics.Id ?? default);

            if (request.Discussions_Topics.Id == null || discussions_topics == null)
            {
                _logger.LogError("Discussion topics with identifier {request.Id} not found", request.Discussions_Topics.Id);
                throw new Exception("");
            }
            if (await _unitOfWork.Discussions.AnyAsync(x => x.Id == request.Discussions_Topics.DiscussionId) == false)
            {
                _logger.LogError("Discussion not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Topics.AnyAsync(x => x.Id == request.Discussions_Topics.TopicId) == false)
            {
                _logger.LogError("Topic not found.");
                throw new Exception("");
            }

            var updatedDiscussions_Topics = request.Discussions_Topics;

            discussions_topics.DiscussionId = updatedDiscussions_Topics.DiscussionId;
            discussions_topics.TopicId = updatedDiscussions_Topics.TopicId;
            discussions_topics.Source = updatedDiscussions_Topics?.Source;
            discussions_topics.IsVisible = updatedDiscussions_Topics?.IsVisible ?? discussions_topics.IsVisible;

            _unitOfWork.Discussions_Topics.Update(discussions_topics);
            await _unitOfWork.SaveChangesAsync();
            return new();
        }
    }
}
