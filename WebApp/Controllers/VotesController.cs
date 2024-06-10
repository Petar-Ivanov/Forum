using Messaging;
using Messaging.Requests.Models;
using Messaging.Responses.Delete;
using Messaging.Responses.Get;
using Messaging.Responses.GetBy;
using Messaging.Responses.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using UAParser;
using WebApp.ActionFilters;
using WebApp.ExtentionMethods;

namespace WebApp.Controllers
{
    public class VotesController : Controller
    {
        private readonly Uri uri = new("https://localhost:7129/api/");
        private static string ActivePage = "Index";
        private static int Page = 1;
        //private static string SearchTerm;
        //private static string Topic;

        public IActionResult Index()
        {
            return View();
        }

        [AuthenticationFilter]
        public async Task<IActionResult> VoteDiscussion(int discussion_id, string voteType, string activePage, int page = 1/*, string searchTerm = "", string topic = ""*/)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            Page = page;
            //SearchTerm = searchTerm;
            //Topic = topic;

            ActivePage = activePage ?? "Index";
            //[HttpGet("{discussion_id}/{user_id}")]
            //var token = GetAccessToken();
            GetDiscussionVoteByIdsResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await client.GetAsync($"DiscussionVotes/{discussion_id}/{user.Id}"); // Needs to change user
                if (response.IsSuccessStatusCode) // Change Existing
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    responseData = JsonSerializer.Deserialize<GetDiscussionVoteByIdsResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    if(responseData.DiscussionVote.IsPositive && voteType == "up" || !responseData.DiscussionVote.IsPositive && voteType == "down") // Delete Vote
                    {
                        return RedirectToAction("DeleteDV", "Votes", new { id = responseData.DiscussionVote.Id });
                    }
                    else // Change Vote
                    {
                        return RedirectToAction("EditDV", "Votes", new { isPositive = voteType == "up", discussion = discussion_id, voteId = responseData.DiscussionVote.Id });
                    }
                }
                else // Create New
                {
                    return RedirectToAction("CreateDV", "Votes", new { isPositive = voteType == "up", discussion = discussion_id});
                }
                
            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> VoteComment(int discussion_id, string voteType, int comment_id, int page = 1/*, string searchTerm = "", string topic = ""*/)
        {

            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            //[HttpGet("{discussion_id}/{user_id}")]
            //var token = GetAccessToken();
            Page = page;
            //SearchTerm = searchTerm;
            //Topic = topic;


            ActivePage = "Details";

            GetCommentVoteByIdsResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //[HttpGet("{discussion_id}/{user_id}/{comment_id}")]
                HttpResponseMessage response = await client.GetAsync($"CommentVotes/{discussion_id}/{user.Id}/{comment_id}"); // Needs to change user
                if (response.IsSuccessStatusCode) // Change Existing
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    responseData = JsonSerializer.Deserialize<GetCommentVoteByIdsResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    if (responseData.CommentVote.IsPositive && voteType == "up" || !responseData.CommentVote.IsPositive && voteType == "down") // Delete Vote
                    {
                        return RedirectToAction("DeleteCV", "Votes", new { id = responseData.CommentVote.Id });
                    }
                    else // Change Vote
                    {
                        return RedirectToAction("EditCV", "Votes", new { isPositive = voteType == "up", discussion = discussion_id, voteId = responseData.CommentVote.Id, comment = comment_id });
                    }
                }
                else // Create New
                {
                    return RedirectToAction("CreateCV", "Votes", new { isPositive = voteType == "up", discussion = discussion_id, comment = comment_id });
                }

            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> CreateDV(bool isPositive, int discussion)
        {

            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            string userAgent = Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();

            ClientInfo clientInfo = parser.Parse(userAgent);
            string source = clientInfo.UA.Family + " - " + clientInfo.OS.Family;

            // Construct the discussion model to send to the API
            var voteModel = new VoteModel
            {
                DiscussionId = discussion,
                UserId = user.Id ?? 1,
                IsPositive = isPositive,
                IsVisible = true,
                Source = source
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(voteModel);

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
                var response = await client.PostAsync("DiscussionVotes", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // If successful, redirect to the discussion list

                    // new { Page = i, SearchTerm = Model.SearchTerm, Topic = Model.Topic }

                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });

                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/});
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });

                    //if (ActivePage == "Discussions") return RedirectToAction(ActivePage, "Discussions");
                    //else return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });

                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "Failed to create a vote. Please try again later.");
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
                }
            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> CreateCV(bool isPositive, int discussion, int comment)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            string userAgent = Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();

            ClientInfo clientInfo = parser.Parse(userAgent);
            string source = clientInfo.UA.Family + " - " + clientInfo.OS.Family;

            // Construct the discussion model to send to the API
            var voteModel = new VoteModel
            {
                DiscussionId = discussion,
                UserId = user.Id ?? 1,
                CommentId = comment,
                IsPositive = isPositive,
                IsVisible = true,
                Source = source
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(voteModel);

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
                var response = await client.PostAsync("CommentVotes", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // If successful, redirect to the discussion list

                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
                    //if (ActivePage == "Discussions") return RedirectToAction(ActivePage, "Discussions");
                    //else return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });

                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "Failed to create a vote. Please try again later.");
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
                }
            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> EditDV(bool isPositive, int discussion, int voteId)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            string userAgent = Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();

            ClientInfo clientInfo = parser.Parse(userAgent);
            string source = clientInfo.UA.Family + " - " + clientInfo.OS.Family;

            // Construct the discussion model to send to the API
            var voteModel = new VoteModel
            {
                Id = voteId,
                IsPositive = isPositive,
                IsVisible = true,
                DiscussionId = discussion,
                UserId = user.Id ?? 1,
                Source = source
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(voteModel);

            // Send a POST request to create a new discussion
            using (var client = new HttpClient())
            {
                // Set the base address of the API
                //client.BaseAddress = uri;
                client.BaseAddress = uri;

                // Set the headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // Create the request content
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send the PUT request
                var response = await client.PutAsync("DiscussionVotes", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // If successful, redirect to the discussion list
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "Failed to edit vote. Please try again later.");
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
                }
            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> EditCV(bool isPositive, int discussion, int voteId, int comment)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            string userAgent = Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();

            ClientInfo clientInfo = parser.Parse(userAgent);
            string source = clientInfo.UA.Family + " - " + clientInfo.OS.Family;

            // Construct the discussion model to send to the API
            var voteModel = new VoteModel
            {
                Id = voteId,
                IsPositive = isPositive,
                IsVisible = true,
                DiscussionId = discussion,
                UserId = user.Id ?? 1,
                CommentId = comment,
                Source = source
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(voteModel);

            // Send a POST request to create a new discussion
            using (var client = new HttpClient())
            {
                // Set the base address of the API
                //client.BaseAddress = uri;
                client.BaseAddress = uri;

                // Set the headers
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // Create the request content
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Send the PUT request
                var response = await client.PutAsync("CommentVotes", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // If successful, redirect to the discussion list
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "Failed to edit vote. Please try again later.");
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
                }
            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> DeleteDV(int id)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            DeleteVoteResponse voteResponseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.DeleteAsync($"DiscussionVotes/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    voteResponseData = JsonSerializer.Deserialize<DeleteVoteResponse>(jsonContent, new JsonSerializerOptions()
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
                    ModelState.AddModelError(string.Empty, "Failed to delete vote. Please try again later.");
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
                }
            }

            //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
            //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
            return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
        }

        [AuthenticationFilter]
        public async Task<IActionResult> DeleteCV(int id)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            DeleteVoteResponse voteResponseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.DeleteAsync($"CommentVotes/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    voteResponseData = JsonSerializer.Deserialize<DeleteVoteResponse>(jsonContent, new JsonSerializerOptions()
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
                    ModelState.AddModelError(string.Empty, "Failed to delete vote. Please try again later.");
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
                    return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
                }
            }

            //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
            //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId, page = Page/*, SearchTerm = SearchTerm, Topic = Topic*/ });
            return RedirectToAction("UpdateLoggedUser", "Users", new { act = ActivePage, ctrl = "Discussions", page = Page });
        }

    }
}
