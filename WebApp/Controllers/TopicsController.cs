using Messaging.Requests.Models;
using Messaging;
using Messaging.Responses.Get;
using Messaging.Responses.GetBy;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebApp.ViewModels.Topics;
using Messaging.Responses.Delete;
using WebApp.ActionFilters;
using Messaging.Responses.ViewModels;
using WebApp.ExtentionMethods;

namespace WebApp.Controllers
{
    public class TopicsController : Controller
    {
        private readonly Uri uri = new("https://localhost:7129/api/Topics/");

        public async Task<IActionResult> Index(IndexVM model)
        {
            //var token = GetAccessToken();
            GetTopicsResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                HttpResponseMessage response = await client.GetAsync(uri);
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetTopicsResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                //model.Items = responseData.Topics.ToList();
                model.Items = PageManager.GetTopics(responseData.Topics.OrderByDescending(x=>x.DiscussionCount).ToList(), model.Page ?? 1);
                model.PageCount = PageManager.TopicsCountPages(responseData.Topics.ToList().Count());
            }

            return View(model);
        }

        public async Task<IActionResult> Search(IndexVM model, string searchTerm)
        {
            //var token = GetAccessToken();
            if (searchTerm == null)
            {
                return RedirectToAction("Index", "Topics");
            }

            GetTopicsByNameResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                string encodedSearchTerm = Uri.EscapeDataString(searchTerm);
                HttpResponseMessage response = await client.GetAsync($"name/{encodedSearchTerm}");
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetTopicsByNameResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                //model.Items = responseData.Topics.ToList();
                model.Items = PageManager.GetTopics(responseData.Topics.OrderByDescending(x => x.DiscussionCount).ToList(), model.Page ?? 1);
                model.PageCount = PageManager.TopicsCountPages(responseData.Topics.ToList().Count());
            }

            model.SearchTerm = searchTerm;
            return View("Index", model);
        }

        [AuthenticationFilter]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateVM model = new CreateVM();
            return View(model);
        }

        [AuthenticationFilter]
        [HttpPost]
        public async Task<IActionResult> Create(CreateVM model)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            // Check if the model state is valid
            if (!ModelState.IsValid && (model.Name == null || model.Description == null))
            {
                // If the model state is not valid, return the view with validation errors
                return View(model);
            }

            // Construct the discussion model to send to the API
            var topicModel = new TopicModel
            {
                Name = model.Name,
                Description = model.Description,
                IsVisible = true,
                CreatedBy = user.Id
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(topicModel);

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
                if (response.IsSuccessStatusCode)
                {
                    // If successful, redirect to the discussion list
                    return RedirectToAction("Index", "Topics");
                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "Failed to create topic. Please try again later.");
                    model.UniqueNameError = true;
                    return View(model);
                }
            }
        }

        [AuthenticationFilter]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            EditVM model = new EditVM();
            GetTopicByIdResponse topicResponseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token); ;

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    topicResponseData = JsonSerializer.Deserialize<GetTopicByIdResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    //discussionResponseData.Discussion.Id = id;
                    model.Topic = topicResponseData.Topic;
                    IdTracker.ActiveTopicId = id;
                }
                else
                {
                    // Handle error responses
                    // You can customize the error handling as needed
                    ModelState.AddModelError(string.Empty, "Failed to find topic. Please try again later.");
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
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            // Check if the model state is valid
            if (!ModelState.IsValid && (model.Name == null || model.Description == null))
            {
                // If the model state is not valid, return the view with validation errors
                model.Topic = new() 
                { 
                    Name = model.Name,
                    Description = model.Description,
                    IsVisible = true
                };
                return View(model);
            }

            // Construct the discussion model to send to the API
            var topicModel = new TopicModel
            {
                Id = IdTracker.ActiveTopicId,
                Name = model.Name,
                Description = model.Description,
                IsVisible = true,
                UpdatedBy = user.Id

            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(topicModel);

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
                var response = await client.PutAsync("Topics", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // If successful, redirect to the discussion list
                    return RedirectToAction("Index", "Topics");
                }
                else
                {
                    // If not successful, handle the error response
                    //ModelState.AddModelError(string.Empty, "Failed to edit topic. Please try again later.");
                    //return RedirectToAction("Index", "Topics");

                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "The name must be unique. Try a different name!");
                    model.UniqueNameError = true;
                    model.Topic = new()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        IsVisible = true
                    };
                    //return RedirectToAction("Edit", "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    return View(model);
                }
            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            DeleteTopicResponse topicResponseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.DeleteAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    topicResponseData = JsonSerializer.Deserialize<DeleteTopicResponse>(jsonContent, new JsonSerializerOptions()
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
                    ModelState.AddModelError(string.Empty, "Failed to delete topic. Please try again later.");
                    return RedirectToAction("Index", "Topics");
                }
            }

            return RedirectToAction("Index", "Topics");
        }
    }
}
