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
    public class UserManagementService : BaseManagementService, IUserManagementService
    {
        //private readonly ForumDbContext _context;

        //public UserManagementService(ILogger<UserManagementService> logger, ForumDbContext context) : base(logger)
        //{
        //    _context = context;
        //}
        private readonly IUnitOfWork _unitOfWork;

        public UserManagementService(ILogger<UserManagementService> logger, IUnitOfWork unitOfWork) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            if (request.User.Username == null || request.User.Password == null || request.User.Email == null || request.User.Biography == null)
            {
                _logger.LogError("Username, password, email and biography must not be null.");
                throw new Exception("");
            }
            if (await _unitOfWork.Users.AnyAsync(x => x.Username == request.User.Username || x.Email == request.User.Email))
            {
                _logger.LogError("Username and email must be unique.");
                throw new Exception("");
            }

            //await _context.Users.AddAsync(new()
            _unitOfWork.Users.Insert(new()
            {
                Biography = request.User.Biography,
                BirthDay = request.User.BirthDay,
                Country = request.User.Country,
                Email = request.User.Email,
                IsVisible = true,
                Password = request.User.Password,
                Username = request.User.Username,
                CreatedOn = DateTime.UtcNow,

            });
            //await _context.SaveChangesAsync();
            await _unitOfWork.SaveChangesAsync();
            return new();
        }

        public async Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request)
        {
            //var user = _context.Users.Find(request.Id);
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);

            if (user == null)
            {
                _logger.LogError("User with identifier {request.Id} not found", request.Id);
                throw new Exception("");
            }

            foreach(var discussion in (await _unitOfWork.Discussions.GetAllAsync()).Where(x=>x.CreatedBy == user.Id))
            {
                _unitOfWork.Discussions.Delete(discussion);
            }

            foreach (var topic in (await _unitOfWork.Topics.GetAllAsync()).Where(x => x.CreatedBy == user.Id))
            {
                _unitOfWork.Topics.Delete(topic);
            }
            //_context.Users.Remove(user);
            //await _context.SaveChangesAsync();
            _unitOfWork.Users.Delete(user);
            await _unitOfWork.SaveChangesAsync();

            return new();
        }

        public async Task<GetUsersResponse> GetUsers()
        {
            GetUsersResponse response = new() { Users = new() };
            //var users = await _context.Users.ToListAsync();
            var users = await _unitOfWork.Users.GetAllAsync();

            if (users == null)
            {
                _logger.LogError("Users not found");
                throw new Exception("");
            }

            foreach (var user in users)
            {
                response.Users.Add(new()
                {
                    Id = user.Id,
                    Biography = user.Biography,
                    BirthDay = user.BirthDay,
                    Country = user.Country,
                    Email = user.Email,
                    IsVisible = user.IsVisible,
                    Password = user.Password,
                    Username = user.Username,
                    UpdatedOn = user.UpdatedOn,
                    CreatedOn = user.CreatedOn
                });
            }

            return response;
        }

        public async Task<GetUserByUsernamePasswordResponse> GetUserByUsernamePassword(GetUserByUsernamePasswordRequest request)
        {
            GetUserByUsernamePasswordResponse response = new();
            //var users = await _context.Users.ToListAsync();
            var user = (await _unitOfWork.Users.GetAllAsync()).Where(x => x.Username==request.Username && x.Password==request.Password).FirstOrDefault();

            if (user == null)
            {
                _logger.LogError("User not found");
                throw new Exception("");
            }

            List<int> discussionsUpVoted = (await _unitOfWork.DiscussionVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive==true).Select(x=>x.DiscussionId).ToList() ?? new List<int>();
            List<int> discussionsDownVoted = (await _unitOfWork.DiscussionVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == false).Select(x => x.DiscussionId).ToList() ?? new List<int>();
            List<int> commentsUpVoted = (await _unitOfWork.CommentVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive==true).Select(x => x.CommentId ?? 1).ToList() ?? new List<int>();
            List<int> commentsDownVoted = (await _unitOfWork.CommentVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == false).Select(x => x.CommentId ?? 1).ToList() ?? new List<int>();

            response.User = new()
            {
                Id = user.Id,
                Biography = user.Biography,
                BirthDay = user.BirthDay,
                Country = user.Country,
                Email = user.Email,
                IsVisible = user.IsVisible,
                Password = user.Password,
                Username = user.Username,
                UpdatedOn = user.UpdatedOn,
                CreatedOn = user.CreatedOn,
                DiscussionsUpVoted = discussionsUpVoted,
                DiscussionsDownVoted = discussionsDownVoted,
                CommentsUpVoted = commentsUpVoted,
                CommentsDownVoted = commentsDownVoted,
            };

            return response;
        }

        public async Task<GetUsersByUsernameResponse> GetUsersByUsername(GetUsersByUsernameRequest request)
        {
            GetUsersByUsernameResponse response = new() { Users = new() };
            //var topics = await _context.Topics.ToListAsync();
            var users = (await _unitOfWork.Users.GetAllAsync()).Where(x => x.Username.Trim().ToLower().Contains(request.Username.Trim().ToLower()));

            //List<int> discussionsUpVoted = (await _unitOfWork.DiscussionVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == true).Select(x => x.DiscussionId).ToList() ?? new List<int>();
            //List<int> discussionsDownVoted = (await _unitOfWork.DiscussionVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == false).Select(x => x.DiscussionId).ToList() ?? new List<int>();
            //List<int> commentsUpVoted = (await _unitOfWork.CommentVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == true).Select(x => x.CommentId ?? 1).ToList() ?? new List<int>();
            //List<int> commentsDownVoted = (await _unitOfWork.CommentVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == false).Select(x => x.CommentId ?? 1).ToList() ?? new List<int>();
            foreach (var user in users)
            {
                response.Users.Add(new()
                {
                    Id = user.Id,
                    Biography = user.Biography,
                    BirthDay = user.BirthDay,
                    Country = user.Country,
                    Email = user.Email,
                    IsVisible = user.IsVisible,
                    Password = user.Password,
                    Username = user.Username,
                    UpdatedOn = user.UpdatedOn,
                    CreatedOn = user.CreatedOn,
                    //DiscussionsUpVoted = discussionsUpVoted,
                    //DiscussionsDownVoted = discussionsDownVoted,
                    //CommentsUpVoted = commentsUpVoted,
                    //CommentsDownVoted = commentsDownVoted,
                });
            }
            return response;
        }

        public async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request)
        {
            GetUserByIdResponse response = new();
            //var users = await _context.Users.ToListAsync();
            var user = (await _unitOfWork.Users.GetAllAsync()).Where(x => x.Id == request.Id).FirstOrDefault();

            if (user == null)
            {
                _logger.LogError("User not found");
                throw new Exception("");
            }

            List<int> discussionsUpVoted = (await _unitOfWork.DiscussionVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == true).Select(x => x.DiscussionId).ToList() ?? new List<int>();
            List<int> discussionsDownVoted = (await _unitOfWork.DiscussionVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == false).Select(x => x.DiscussionId).ToList() ?? new List<int>();
            List<int> commentsUpVoted = (await _unitOfWork.CommentVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == true).Select(x => x.CommentId ?? 1).ToList() ?? new List<int>();
            List<int> commentsDownVoted = (await _unitOfWork.CommentVotes.GetAllAsync()).Where(x => x.UserId == user.Id && x.IsPositive == false).Select(x => x.CommentId ?? 1).ToList() ?? new List<int>();

            response.User = new()
            {
                Id = user.Id,
                Biography = user.Biography,
                BirthDay = user.BirthDay,
                Country = user.Country,
                Email = user.Email,
                IsVisible = user.IsVisible,
                Password = user.Password,
                Username = user.Username,
                UpdatedOn = user.UpdatedOn,
                CreatedOn = user.CreatedOn,
                DiscussionsUpVoted = discussionsUpVoted,
                DiscussionsDownVoted = discussionsDownVoted,
                CommentsUpVoted = commentsUpVoted,
                CommentsDownVoted = commentsDownVoted
            };

            return response;
        }

        public async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.User.Id ?? default);

            if (request.User.Id == null || user == null)
            {
                _logger.LogError("User with identifier {request.Id} not found", request.User.Id);
                throw new Exception("");
            }
            if (request.User.Username == null || request.User.Password == null || request.User.Email == null || request.User.Biography == null)
            {
                _logger.LogError("Username, password, email and biography must not be null.");
                throw new Exception("");
            }

            var updatedUser = request.User;

            user.Username = updatedUser.Username;
            user.Biography = updatedUser.Biography;
            user.Password = updatedUser.Password;
            user.BirthDay = updatedUser.BirthDay;
            user.Country = updatedUser.Country;
            user.Email = updatedUser.Email;
            user.IsVisible = updatedUser.IsVisible ?? user.IsVisible;
            user.UpdatedOn = DateTime.UtcNow;
            user.UpdatedBy = updatedUser.UpdatedBy;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return new();
        }
    }
}
