using Messaging.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text;
using System;
using UAParser;
using Messaging.Responses.Delete;
using WebApp.ActionFilters;
using Messaging.Responses.ViewModels;
using WebApp.ExtentionMethods;

namespace WebApp.Controllers
{
    public class Discussions_TopicsController : Controller
    {
        private readonly Uri uri = new("https://localhost:7129/api/Discussions_Topics/");

        public IActionResult Index()
        {
            return View();
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Edit(int discussionId)
        {
            //List<int> ToBeDeleted = (TempData["ToBeDeleted"] as int[]).ToList();
            //List<int> ToBeCreated = (TempData["ToBeCreated"] as int[]).ToList();

            //if (ToBeDeleted != null)
            //{
            //    TempData["DeletedTopics"] = ToBeDeleted;
            //    return RedirectToAction("Delete", "Discussions_Topics", new { discussionId = discussionId });
            //}
            //if (ToBeCreated != null) 
            //{
            //    TempData["Topics"] = ToBeCreated;
            //    return RedirectToAction("Create", "Discussions_Topics", new { discussionId = discussionId});
            //}

            if (TempData["ToBeDeleted"] != null)
            {
                TempData["DeletedTopics"] = TempData["ToBeDeleted"];
                return RedirectToAction("Delete", "Discussions_Topics", new { discussionId = discussionId });
            }
            if (TempData["ToBeCreated"] != null)
            {
                TempData["Topics"] = TempData["ToBeCreated"];
                return RedirectToAction("Create", "Discussions_Topics", new { discussionId = discussionId });
            }

            return RedirectToAction("Details", "Discussions", new { id = discussionId});
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Create(int discussionId)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            List<int> TopicIds = (TempData["Topics"] as int[]).ToList();
            // Check if the model state is valid
            if (TopicIds.Count == 0)
            {
                // If the model state is not valid, return the view with validation errors
                return RedirectToAction("Index", "Discussions");
            }

            string userAgent = Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();

            ClientInfo clientInfo = parser.Parse(userAgent);
            string source = clientInfo.UA.Family + " - " + clientInfo.OS.Family;

            foreach (var topicId in TopicIds)
            {
                // Construct the discussion model to send to the API
                var discussions_topicsModel = new Discussions_TopicsModel
                {
                    DiscussionId = discussionId,
                    TopicId = topicId,
                    IsVisible = true,
                    Source = source
                };

                // Serialize the discussion model to JSON
                var jsonPayload = JsonSerializer.Serialize(discussions_topicsModel);

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
                    var response = await client.PostAsync("", content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode == false)
                    {
                        // If successful, redirect to the discussion list
                        ModelState.AddModelError(string.Empty, "Failed to create discussions_topics. Please try again later.");
                        return RedirectToAction("Index", "Discussions");
                    }
                }
            }

            return RedirectToAction("Index", "Discussions");

        }

        [AuthenticationFilter]
        public async Task<IActionResult> Delete(int discussionId)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            List<int> TopicIds = (TempData["DeletedTopics"] as int[]).ToList();

            if (TopicIds.Count == 0)
            {
                // If the model state is not valid, return the view with validation errors
                return RedirectToAction("Details", "Discussions", new { id = discussionId });
            }

            string userAgent = Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();

            ClientInfo clientInfo = parser.Parse(userAgent);
            string source = clientInfo.UA.Family + " - " + clientInfo.OS.Family;

            foreach (var topicId in TopicIds)
            {
                DeleteDiscussions_TopicsByIdsResponse topicResponseData;

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = uri;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    //HttpResponseMessage response = await client.GetAsync(uri);
                    HttpResponseMessage response = await client.DeleteAsync($"{discussionId}/{topicId}");

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode == false)
                    {
                        // If successful, redirect to the discussion list
                        ModelState.AddModelError(string.Empty, "Failed to delete discussions_topics. Please try again later.");
                        return RedirectToAction("Details", "Discussions", new { id = discussionId });
                    }
                }

            }
            TempData["ToBeDeleted"] = null;
            return RedirectToAction("Edit", "Discussions_Topics", new { discussionId = discussionId });
        }
    }
}
