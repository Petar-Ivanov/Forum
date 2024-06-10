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
    public class CommentManagementService : BaseManagementService, ICommentManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentManagementService(ILogger<CommentManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateCommentResponse> CreateComment(CreateCommentRequest request)
        {
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.Comment.UserId) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Discussions.AnyAsync(x => x.Id == request.Comment.DiscussionId) == false)
            {
                _logger.LogError("Discussion not found.");
                throw new Exception("");
            }
            if (request.Comment.Text == null)
            {
                _logger.LogError("Comment text cannot be null.");
                throw new Exception("");
            }

            _unitOfWork.Comments.Insert(new()
            {
                CreatedOn = DateTime.UtcNow,
                IsUpdated = false,
                Text = request.Comment.Text,
                DiscussionId = request.Comment.DiscussionId,
                Source = request.Comment.Source,
                UserId = request.Comment.UserId,

            });
            
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteCommentResponse> DeleteComment(DeleteCommentRequest request)
        {
            if (await _unitOfWork.Comments.AnyAsync(x => x.Id == request.Id) == false)
            {
                _logger.LogError("Comment not found.");
                throw new Exception("");
            }

            var comment = await _unitOfWork.Comments.GetByIdAsync(request.Id);

            if (comment == null)
            {
                _logger.LogError("Comment with identifier {request.Id} not found", request.Id);
                throw new Exception("");
            }

            _unitOfWork.Comments.Delete(comment);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetCommentsResponse> GetComments()
        {
            GetCommentsResponse response = new() { Comments = new() };
            var comments = await _unitOfWork.Comments.GetAllAsync();

            foreach (var comment in comments)
            {
                var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == comment.DiscussionId);
                var user = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == comment.UserId);

                var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == comment.UpdatedBy);
                string editorUsername = editor != null ? editor.Username : null;

                response.Comments.Add(new()
                {
                    Id = comment.Id,
                    CreatedOn = (DateTime)comment.CreatedOn,
                    CreatedBy = user.Id,
                    IsUpdated = comment.IsUpdated,
                    Source = comment.Source,
                    Text = comment.Text,
                    UpdatedOn = comment.UpdatedOn,
                    Discussion = discussion.Title,
                    User = user.Username,
                    UpdatedBy = editorUsername
                });
            }

            return response;
        }

        public async Task<GetCommentsByIdResponse> GetCommentsById(GetCommentsByIdRequest request)
        {
            GetCommentsByIdResponse response = new() { Comments = new() };
            //var comments = await _context.Comments.ToListAsync();
            var comments = (await _unitOfWork.Comments.GetAllAsync()).Where(x => x.DiscussionId == request.Id);

            foreach (var comment in comments)
            {
                var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == comment.DiscussionId);
                var user = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == comment.UserId);

                var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == comment.UpdatedBy);
                string editorUsername = editor != null ? editor.Username : null;

                response.Comments.Add(new()
                {
                    Id = comment.Id,
                    CreatedOn = (DateTime)comment.CreatedOn,
                    CreatedBy = user.Id,
                    IsUpdated = comment.IsUpdated,
                    Source = comment.Source,
                    Text = comment.Text,
                    UpdatedOn = comment.UpdatedOn,
                    Discussion = discussion.Title,
                    User = user.Username,
                    UpVoteCount = _unitOfWork.CommentVotes.GetAllAsync().Result.Where(x => x.CommentId == comment.Id && x.IsPositive == true).Count(),
                    DownVoteCount = _unitOfWork.CommentVotes.GetAllAsync().Result.Where(x => x.CommentId == comment.Id && x.IsPositive == false).Count(),
                    TimeDifference = (DateTime.UtcNow - comment.CreatedOn).Value.TotalHours,
                    DiscussionId = discussion.Id,
                    UserId = user.Id,
                    UpdatedBy = editorUsername
                }) ;
            }

            return response;
        }

        public async Task<GetCommentByIdResponse> GetCommentById(GetCommentByIdRequest request)
        {
            GetCommentByIdResponse response = new();
            //var comments = await _context.Comments.ToListAsync();
            var comment = (await _unitOfWork.Comments.GetAllAsync()).Where(x => x.Id == request.Id).FirstOrDefault();
            if (comment == null)
            {
                _logger.LogError("Comment not found.");
                throw new Exception("");
            }

            //var discussion = _context.Discussions.Where(x => x.Id == comment.DiscussionId).First();
            //var user = _context.Users.Where(x => x.Id == comment.UserId).First();
            var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == comment.DiscussionId);
            var user = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == comment.UserId);

            var editor = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == comment.UpdatedBy);
            string editorUsername = editor != null ? editor.Username : null;

            response.Comment = new()
            {
                Id = comment.Id,
                CreatedOn = (DateTime)comment.CreatedOn,
                CreatedBy = user.Id,
                IsUpdated = comment.IsUpdated,
                Source = comment.Source,
                Text = comment.Text,
                UpdatedOn = comment.UpdatedOn,
                Discussion = discussion.Title,
                User = user.Username,
                UpVoteCount = _unitOfWork.CommentVotes.GetAllAsync().Result.Where(x => x.CommentId == comment.Id && x.IsPositive == true).Count(),
                DownVoteCount = _unitOfWork.CommentVotes.GetAllAsync().Result.Where(x => x.CommentId == comment.Id && x.IsPositive == false).Count(),
                TimeDifference = (DateTime.UtcNow - comment.CreatedOn).Value.TotalHours,
                UpdatedBy = editorUsername
            };

            return response;
        }

        public async Task<UpdateCommentResponse> UpdateComment(UpdateCommentRequest request)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(request.Comment.Id ?? default);

            if (request.Comment.Id == null || comment == null)
            {
                _logger.LogError("Comment with identifier not found");
                throw new Exception("");
            }
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.Comment.UserId) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Discussions.AnyAsync(x => x.Id == request.Comment.DiscussionId) == false)
            {
                _logger.LogError("Discussion not found.");
                throw new Exception("");
            }
            if (request.Comment.Text == null)
            {
                _logger.LogError("Comment text cannot be null.");
                throw new Exception("");
            }


            var updatedComment = request.Comment;

            comment.DiscussionId = updatedComment.DiscussionId;
            comment.UserId = updatedComment.UserId;
            comment.Source = updatedComment.Source ?? comment.Source;
            comment.Text = updatedComment.Text;
            comment.IsUpdated = true;
            comment.UpdatedOn = DateTime.UtcNow;
            comment.UpdatedBy = updatedComment.UpdatedBy;

            _unitOfWork.Comments.Update(comment);
            await _unitOfWork.SaveChangesAsync();
            return new();
        }
    }
}
