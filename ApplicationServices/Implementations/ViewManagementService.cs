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
    public class ViewManagementService : BaseManagementService, IViewManagementService
    {
        //private readonly ForumDbContext _context;

        //public ViewManagementService(ILogger<ViewManagementService> logger, ForumDbContext context) : base(logger)
        //{
        //    _context = context;
        //}
        private readonly IUnitOfWork _unitOfWork;

        public ViewManagementService(ILogger<ViewManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateViewResponse> CreateView(CreateViewRequest request)
        {
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.View.UserId) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Discussions.AnyAsync(x => x.Id == request.View.DiscussionId) == false)
            {
                _logger.LogError("Discussion not found.");
                throw new Exception("");
            }

            //await _context.Views.AddAsync(new()
            _unitOfWork.Views.Insert(new()
            {
                CreatedOn = DateTime.UtcNow,
                Revisited = false,
                Source = request.View.Source,
                UserId = request.View.UserId,
                DiscussionId = request.View.DiscussionId    
            });
            //await _context.SaveChangesAsync();
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteViewResponse> DeleteView(DeleteViewRequest request)
        {
            //var view = _context.Views.Find(request.Id);
            var view = await _unitOfWork.Views.GetByIdAsync(request.Id);

            if (view == null)
            {
                _logger.LogError("View with identifier {request.Id} not found", request.Id);
                throw new Exception("");
            }

            //_context.Views.Remove(view);
            //await _context.SaveChangesAsync();
            _unitOfWork.Views.Delete(view);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetViewsResponse> GetViews()
        {
            GetViewsResponse response = new() { Views = new() };
            //var views = await _context.Views.ToListAsync();
            var views = await _unitOfWork.Views.GetAllAsync();

            foreach (var view in views)
            {
                //var discussion = _context.Discussions.Where(x => x.Id == view.DiscussionId).First();
                //var user = _context.Users.Where(x => x.Id == view.UserId).First();
                var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == view.DiscussionId);
                var user = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == view.UserId);

                response.Views.Add(new()
                {
                    Id = view.Id,
                    CreatedOn = (DateTime)view.CreatedOn,
                    Discussion = discussion.Title,
                    User = user.Username,
                    Revisited = view.Revisited,
                    Source = view.Source
                });
            }

            return response;
        }

        public async Task<GetViewByIdsResponse> GetViewByIds(GetViewByIdsRequest request)
        {
            GetViewByIdsResponse response = new();
            //var views = await _context.Views.ToListAsync();
            var view = (await _unitOfWork.Views.GetAllAsync()).Where(x=>x.DiscussionId==request.Discussion_Id && x.UserId == request.User_Id).FirstOrDefault();
            if (view == null)
            {
                _logger.LogError("View not found");
                throw new Exception("");
            }

            //var discussion = _context.Discussions.Where(x => x.Id == view.DiscussionId).First();
            //var user = _context.Users.Where(x => x.Id == view.UserId).First();
            var discussion = (await _unitOfWork.Discussions.GetAllAsync()).FirstOrDefault(x => x.Id == view.DiscussionId);
            var user = (await _unitOfWork.Users.GetAllAsync()).FirstOrDefault(x => x.Id == view.UserId);

            response.View = new()
            {
                Id = view.Id,
                CreatedOn = (DateTime)view.CreatedOn,
                Discussion = discussion.Title,
                User = user.Username,
                Revisited = view.Revisited,
                Source = view.Source
            };

            return response;
        }

        public async Task<UpdateViewResponse> UpdateView(UpdateViewRequest request)
        {
            var view = await _unitOfWork.Views.GetByIdAsync(request.View.Id ?? default);

            if (request.View.Id == null || view == null)
            {
                _logger.LogError("View with identifier {request.Id} not found", request.View.Id);
                throw new Exception("");
            }
            if (await _unitOfWork.Users.AnyAsync(x => x.Id == request.View.UserId) == false)
            {
                _logger.LogError("User not found.");
                throw new Exception("");
            }
            if (await _unitOfWork.Discussions.AnyAsync(x => x.Id == request.View.DiscussionId) == false)
            {
                _logger.LogError("Discussion not found.");
                throw new Exception("");
            }

            var updatedView = request.View;

            view.DiscussionId = updatedView.DiscussionId;
            view.UserId = updatedView.UserId;
            view.Source = updatedView.Source;
            view.Revisited = updatedView.Revisited;

            _unitOfWork.Views.Update(view);
            await _unitOfWork.SaveChangesAsync();
            return new();
        }
    }
}
