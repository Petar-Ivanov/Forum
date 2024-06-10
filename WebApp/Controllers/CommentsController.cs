using Messaging.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System;
using UAParser;
using WebApp.ViewModels.Comments;
using WebApp.ViewModels.WrapperVMs;
using Messaging;
using Messaging.Responses.GetBy;
using Messaging.Responses.Delete;
using WebApp.ActionFilters;
using Messaging.Responses.ViewModels;
using WebApp.ExtentionMethods;
using Messaging.Responses;

namespace WebApp.Controllers
{
    public class CommentsController : Controller
    {
        private readonly Uri uri = new("https://localhost:7129/api/");

        public IActionResult Index()
        {
            return View();
        }

        [AuthenticationFilter]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Create(DetailsVM model)
        {
            model.CommentCreate.DiscussionId = IdTracker.ActiveDiscussionId;

            if(model.CommentCreate.DiscussionId == null || model.CommentCreate.Text == null)
            {
                return RedirectToAction("Details", "Discussions", new { id = model.CommentCreate.DiscussionId });
            }
            // Check if the model state is valid
            //if (!ModelState.IsValid)
            //{
            //    // If the model state is not valid, return the view with validation errors
            //    return RedirectToAction("Details", "Discussions", new { id = model.CommentCreate.DiscussionId });
            //}


            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            string userAgent = Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();

            ClientInfo clientInfo = parser.Parse(userAgent);
            string source = clientInfo.UA.Family + " - " + clientInfo.OS.Family;

            // Construct the discussion model to send to the API
            var commentModel = new CommentModel
            {
                DiscussionId = model.CommentCreate.DiscussionId,
                UserId = user.Id ?? 1, 
                IsUpdated = false,
                Source = source,
                Text = model.CommentCreate.Text,
                CreatedOn = DateTime.Now,
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(commentModel);

            // Send a POST request to create a new discussion
            using (var client = new HttpClient())
            {
                // Set the base address of the API
                client.BaseAddress = uri;

                // Set the headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                // Create the request content
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send the POST request
                var response = await client.PostAsync("Comments", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // If successful, redirect to the discussion list
                    return RedirectToAction("Details", "Discussions", new { id = model.CommentCreate.DiscussionId });
                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "Failed to create comment. Please try again later.");
                    return View(model);
                }
            }
        }

        [AuthenticationFilter]
        [HttpGet]
        public async Task<IActionResult> Edit(int discussion_id, int comment_id)
        {
            IdTracker.ActiveCommentId = comment_id;
            EditVM model = new EditVM();
            GetCommentsByIdResponse discussionResponseData;

            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.GetAsync($"Comments/{discussion_id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    discussionResponseData = JsonSerializer.Deserialize<GetCommentsByIdResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    //discussionResponseData.Discussion.Id = id;
                    model.Comment = discussionResponseData.Comments.Where(x=>x.Id == comment_id).FirstOrDefault();
                }
                else
                {
                    // Handle error responses
                    // You can customize the error handling as needed
                    ModelState.AddModelError(string.Empty, "Failed to find comment. Please try again later.");
                    return View(model);
                }
            }

            return View(model);
        }

        [AuthenticationFilter]
        [HttpPost]
        public async Task<IActionResult> Edit(EditVM model)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null)
            {
                token = await TokenManager.GetAccessToken(user.Username, user.Password);
                model.UserId = user.Id;
            }

            if (model.Text == null && !ModelState.IsValid)
            {
                model.Comment = new()
                {
                    Text = model.Text,
                    IsUpdated = model.IsUpdated ?? false,
                    DiscussionId = model.DiscussionId ?? 1
                };
                model.UserId = user.Id;
                //return RedirectToAction("Edit", "Discussions", new { id = IdTracker.ActiveDiscussionId });
                return View(model);
            }

            // Construct the discussion model to send to the API
            var commentModel = new CommentModel
            {
                Id = IdTracker.ActiveCommentId,
                DiscussionId = model.DiscussionId ?? 1,
                UserId = model.UserId ?? 1,
                IsUpdated = true,
                Text = model.Text,
                UpdatedBy = user.Id 
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(commentModel);

            // Send a POST request to create a new discussion
            using (var client = new HttpClient())
            {
                // Set the base address of the API
                //client.BaseAddress = uri;
                client.BaseAddress = new("https://localhost:7129/api/");

                // Set the headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // Create the request content
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send the PUT request
                var response = await client.PutAsync("Comments", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // If successful, redirect to the discussion list
                    return RedirectToAction("Details", "Discussions", new { id = IdTracker.ActiveDiscussionId });
                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "Failed to edit comment. Please try again later.");
                    return RedirectToAction("Details", "Discussions", new { id = IdTracker.ActiveDiscussionId });
                }
            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            DeleteCommentResponse discussionResponseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.DeleteAsync($"Comments/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    discussionResponseData = JsonSerializer.Deserialize<DeleteCommentResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    //discussionResponseData.Discussion.Id = id;
                    //model.Discussion = discussionResponseData.Discussion;
                }
                else
                {
                    // Handle error responses
                    // You can customize the error handling as needed
                    ModelState.AddModelError(string.Empty, "Failed to delete comment. Please try again later.");
                    
                }
            }

            return RedirectToAction("Details", "Discussions", new { id = IdTracker.ActiveDiscussionId });
        }

    }
}
