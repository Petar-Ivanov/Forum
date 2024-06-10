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

namespace ApplicationServices.Implementations
{
    public class TopicManagementService : BaseManagementService, ITopicManagementService
    {
        //private readonly ForumDbContext _context;

        //public TopicManagementService(ILogger<TopicManagementService> logger, ForumDbContext context) : base(logger)
        //{
        //    _context = context;
        //}
        private readonly IUnitOfWork _unitOfWork;

        public TopicManagementService(ILogger<TopicManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateTopicResponse> CreateTopic(CreateTopicRequest request)
        {
            if (request.Topic.Name == null || request.Topic.Description == null)
            {
                _logger.LogError("Topic and description must not be null.");
                throw new Exception("");
            }
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.Topic.CreatedBy) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }

            //await _context.Topics.AddAsync(new()
            _unitOfWork.Topics.Insert(new()
            {
                CreatedBy = request.Topic.CreatedBy,
                CreatedOn = DateTime.UtcNow,
                IsVisible = true,
                Description = request.Topic.Description,
                Name = request.Topic.Name
                
            });
            //await _context.SaveChangesAsync();
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteTopicResponse> DeleteTopic(DeleteTopicRequest request)
        {
            //var topic = _context.Topics.Find(request.Id);
            var topic = await _unitOfWork.Topics.GetByIdAsync(request.Id);

            if (topic == null)
            {
                _logger.LogError("Topic with identifier {request.Id} not found", request.Id);
                throw new Exception("");
            }

            //_context.Topics.Remove(topic);
            //await _context.SaveChangesAsync();
            _unitOfWork.Topics.Delete(topic);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetTopicsResponse> GetTopics()
        {
            GetTopicsResponse response = new() { Topics = new() };
            //var topics = await _context.Topics.ToListAsync();
            var topics = await _unitOfWork.Topics.GetAllAsync();

            foreach (var topic in topics)
            {
                //string creatorUsername = _context.Users.Where(x => x.Id == topic.CreatedBy).First().Username;
                var creatorUsername = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == topic.CreatedBy).Username;
                //string editorUsername = null;
                
                //editorUsername = _context.Users.Where(x => x.Id == topic.UpdatedBy).First().Username;
                var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == topic.UpdatedBy);
                string editorUsername = editor != null ? editor.Username : null;

                int discussionCount = (await _unitOfWork.Discussions_Topics.GetAllAsync()).Where(x => x.TopicId == topic.Id).Count();

                response.Topics.Add(new()
                {
                    Id = topic.Id,
                    CreatedBy = creatorUsername,
                    CreatedById = topic.CreatedBy,
                    CreatedOn = topic.CreatedOn,
                    Description = topic.Description,
                    IsVisible = topic.IsVisible,
                    Name = topic.Name,
                    UpdatedBy = editorUsername,
                    UpdatedOn = topic.UpdatedOn,
                    TimeDifference = (DateTime.UtcNow - topic.CreatedOn).Value.TotalHours,
                    DiscussionCount = discussionCount
                });
            }

            return response;
        }

        public async Task<GetTopicByIdResponse> GetTopicById(GetTopicByIdRequest request)
        {
            GetTopicByIdResponse response = new();
            //var topics = await _context.Topics.ToListAsync();
            var topic = (await _unitOfWork.Topics.GetAllAsync()).Where(x => x.Id == request.Id).FirstOrDefault();

            if (topic == null)
            {
                _logger.LogError("Topic not found.");
                throw new Exception("");
            }

            //string creatorUsername = _context.Users.Where(x => x.Id == topic.CreatedBy).First().Username;
            var creatorUsername = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == topic.CreatedBy).Username;
            //string editorUsername = null;

            //editorUsername = _context.Users.Where(x => x.Id == topic.UpdatedBy).First().Username;
            var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == topic.UpdatedBy);
            string editorUsername = editor != null ? editor.Username : null;

            int discussionCount = (await _unitOfWork.Discussions_Topics.GetAllAsync()).Where(x => x.TopicId == topic.Id).Count();

            response.Topic = new()
            {
                Id = topic.Id,
                CreatedBy = creatorUsername,
                CreatedById = topic.CreatedBy,
                CreatedOn = topic.CreatedOn,
                Description = topic.Description,
                IsVisible = topic.IsVisible,
                Name = topic.Name,
                UpdatedBy = editorUsername,
                UpdatedOn = topic.UpdatedOn,
                TimeDifference = (DateTime.UtcNow - topic.CreatedOn).Value.TotalHours,
                DiscussionCount = discussionCount
            };

            return response;
        }

        

        public async Task<GetTopicsByNameResponse> GetTopicsByName(GetTopicsByNameRequest request)
        {
            GetTopicsByNameResponse response = new() { Topics = new() };
            //var topics = await _context.Topics.ToListAsync();
            var topics = (await _unitOfWork.Topics.GetAllAsync()).Where(x=>x.Name.Trim().ToLower().Contains(request.SearchTerm.Trim().ToLower()));

            foreach (var topic in topics)
            {
                //string creatorUsername = _context.Users.Where(x => x.Id == topic.CreatedBy).First().Username;
                var creatorUsername = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == topic.CreatedBy).Username;
                //string editorUsername = null;

                //editorUsername = _context.Users.Where(x => x.Id == topic.UpdatedBy).First().Username;
                var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == topic.UpdatedBy);
                string editorUsername = editor != null ? editor.Username : null;

                int discussionCount = (await _unitOfWork.Discussions_Topics.GetAllAsync()).Where(x => x.TopicId == topic.Id).Count();

                response.Topics.Add(new()
                {
                    Id = topic.Id,
                    CreatedBy = creatorUsername,
                    CreatedById = topic.CreatedBy,
                    CreatedOn = topic.CreatedOn,
                    Description = topic.Description,
                    IsVisible = topic.IsVisible,
                    Name = topic.Name,
                    UpdatedBy = editorUsername,
                    UpdatedOn = topic.UpdatedOn,
                    TimeDifference = (DateTime.UtcNow - topic.CreatedOn).Value.TotalHours,
                    DiscussionCount = discussionCount
                });
            }

            return response;
        }

        public async Task<UpdateTopicResponse> UpdateTopic(UpdateTopicRequest request)
        {
            var topic = await _unitOfWork.Topics.GetByIdAsync(request.Topic.Id ?? default);

            if (request.Topic.Id == null || topic == null)
            {
                _logger.LogError("Topic with identifier {request.Id} not found", request.Topic.Id);
                throw new Exception("");
            }
            if (request.Topic.Name == null || request.Topic.Description == null)
            {
                _logger.LogError("Topic and description must not be null.");
                throw new Exception("");
            }
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.Topic.UpdatedBy) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }

            var updatedTopic = request.Topic;

            topic.Name = updatedTopic.Name;
            topic.Description = updatedTopic.Description;
            topic.IsVisible = updatedTopic.IsVisible;
            topic.UpdatedOn = DateTime.UtcNow;
            topic.UpdatedBy = updatedTopic.UpdatedBy;

            _unitOfWork.Topics.Update(topic);
            await _unitOfWork.SaveChangesAsync();
            return new();
        }
    }
}
