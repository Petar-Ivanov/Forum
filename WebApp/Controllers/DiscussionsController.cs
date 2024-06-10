using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Messaging.Responses.Get;
using WebApp.ViewModels.Discussions;
using Messaging.Responses.Create;
using System.Net.Http;
using System.Text;
using WebApp.Models;
using Messaging.Requests.Models;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Messaging.Requests.Get;
using Messaging.Responses.GetBy;
using WebApp.ViewModels.WrapperVMs;
using Messaging;
using Messaging.Responses.ViewModels;
using Messaging.Responses.Delete;
using System.Reflection;
using WebApp.ActionFilters;
using WebApp.ExtentionMethods;
using Messaging.Responses;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApp.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly Uri uri = new("https://localhost:7129/api/Discussions/");
        // https://localhost:7129/api/Discussions
        public async Task<IActionResult> Index(IndexVM model)
        {
            //var token = GetAccessToken();
            GetDiscussionsResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                HttpResponseMessage response = await client.GetAsync(uri);
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetDiscussionsResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                model.Items = PageManager.GetDiscussions(responseData.Discussions.OrderByDescending(x => x.CreatedOn).ToList(), model.Page ?? 1);
                model.PageCount = PageManager.DiscussionsCountPages(responseData.Discussions.ToList().Count());
            }

            return View(model);
        }

        public async Task<IActionResult> Search(IndexVM model, string searchTerm, string topic)
        {

            //var token = GetAccessToken();
            if (searchTerm == null)
            {
                return RedirectToAction("Index", "Discussions");
            }

            GetDiscussionsByTitleResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                string encodedSearchTerm = Uri.EscapeDataString(searchTerm);
                HttpResponseMessage response = await client.GetAsync($"title/{encodedSearchTerm}");
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetDiscussionsByTitleResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                if (topic != null) responseData.Discussions = responseData.Discussions.Where(x => x.Topics.Contains(topic)).ToList();

                model.Items = PageManager.GetDiscussions(responseData.Discussions.OrderByDescending(x => x.CreatedOn).ToList(), model.Page ?? 1);
                model.PageCount = PageManager.DiscussionsCountPages(responseData.Discussions.ToList().Count());
            }

            model.SearchTerm = searchTerm;
            return View("Index", model);
        }

        public async Task<IActionResult> GetByTopic(IndexVM model, string topic)
        {
            //var token = GetAccessToken();
            if (topic == null)
            {
                return RedirectToAction("Index", "Topics");
            }

            GetDiscussionsByTopicResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                string encodedTopic = Uri.EscapeDataString(topic);
                HttpResponseMessage response = await client.GetAsync($"topic/{encodedTopic}");
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetDiscussionsByTopicResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                model.Items = PageManager.GetDiscussions(responseData.Discussions.OrderByDescending(x => x.CreatedOn).ToList(), model.Page ?? 1);
                model.PageCount = PageManager.DiscussionsCountPages(responseData.Discussions.ToList().Count());
            }

            model.Topic = topic;
            return View("Index", model);
        }

        public async Task<IActionResult> Details(int id, int page = 1)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await GetAccessToken(user.Username, user.Password);

            DetailsVM model = new DetailsVM();
            GetDiscussionByIdResponse discussionResponseData;
            GetCommentsByIdResponse commentResponseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    discussionResponseData = JsonSerializer.Deserialize<GetDiscussionByIdResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    discussionResponseData.Discussion.Id = id;
                    model.DiscussionGet.Discussion = discussionResponseData.Discussion;
                }
                else
                {
                    // Handle error responses
                    // You can customize the error handling as needed
                    ModelState.AddModelError(string.Empty, "Failed to find discussion. Please try again later.");
                    return View(model);
                }
            }
            using (HttpClient client = new HttpClient())
            {
                // Getting comments
                client.BaseAddress = new("https://localhost:7129/api/Comments/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    commentResponseData = JsonSerializer.Deserialize<GetCommentsByIdResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    //model.DiscussionGet.Comments = commentResponseData.Comments;
                    model.DiscussionGet.Comments = PageManager.GetComments(commentResponseData.Comments.OrderByDescending(x => x.CreatedOn).ToList(), page);
                    model.PageCount = PageManager.CommentsCountPages(commentResponseData.Comments.ToList().Count());
                    model.Page = page;
                }
                else
                {
                    // Handle error responses
                    // You can customize the error handling as needed
                    ModelState.AddModelError(string.Empty, "Failed to find comments. Please try again later.");
                    return View(model);
                }
            }

            IdTracker.ActiveDiscussionId = id;
            return View(model);
        }

        [AuthenticationFilter]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await GetAccessToken(user.Username, user.Password);

            CreateVM model = new CreateVM();
            //var token = GetAccessToken();
            GetTopicsResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new("https://localhost:7129/api/Topics/"); ;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await client.GetAsync("");
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetTopicsResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                model.Topics = responseData.Topics.ToList();
            }


            return View(model);
        }

        [AuthenticationFilter]
        [HttpPost]
        public async Task<IActionResult> Create(CreateVM model)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await GetAccessToken(user.Username, user.Password);
            // Check if the model state is valid
            //if (model.Text == null || model.Title == null)
            if (!ModelState.IsValid)
            {
                var topics = JsonSerializer.Deserialize<List<TopicViewModel>>(model.TopicsSerialized);
                model.Topics = topics ?? new List<TopicViewModel> { };
                return View(model);
            }
            long fileSize = model.Image != null ? model.Image.Length : 0;
            //string fileType = model.Image.ContentType;
            byte[] bytes = null;

            if (fileSize > 0)
            {
                using (var stream = new MemoryStream())
                {
                    model.Image.CopyTo(stream);
                    bytes = stream.ToArray();
                }
            }

            //int LoggedUserId = HttpContext.Session.GetObject<UserViewModel>("loggedUser").Id ?? 1;

            // Construct the discussion model to send to the API
            var discussionModel = new DiscussionModel
            {
                Title = model.Title,
                Text = model.Text,
                IsUpdated = false,
                IsLocked = false,
                IsVisible = true,
                Image = bytes,
                CreatedBy = user.Id
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(discussionModel);

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
                    //return RedirectToAction("Index", "Discussions");
                    if (model.SelectedTopicIds == null)
                    {
                        return RedirectToAction("Index", "Discussions");
                    }
                    TempData["Topics"] = model.SelectedTopicIds;
                    return RedirectToAction("GetIdByTitle", "Discussions", new { discussionTitle = model.Title });
                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "The title must be unique. Try a different title!");
                    model.UniqueTitleError = true;
                    var topics = JsonSerializer.Deserialize<List<TopicViewModel>>(model.TopicsSerialized);
                    model.Topics = topics ?? new List<TopicViewModel> { };

                    return View(model);
                }
            }
        }

        public async Task<IActionResult> GetIdByTitle(string discussionTitle)
        {
            //var token = GetAccessToken();
            if (discussionTitle == null)
            {
                return RedirectToAction("Index", "Discussions");
            }

            GetDiscussionsByTitleResponse responseData;
            int discussionId = 0;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                string encodedDiscussionTitle = Uri.EscapeDataString(discussionTitle);
                HttpResponseMessage response = await client.GetAsync($"title/{encodedDiscussionTitle}");
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetDiscussionsByTitleResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                var searchId = responseData.Discussions.ToList().Where(x => x.Title == discussionTitle).FirstOrDefault().Id;
                if (searchId != null)
                {
                    discussionId = (int)searchId;
                }
                else return RedirectToAction("Index", "Discussions");
            }

            return RedirectToAction("Create", "Discussions_Topics", new { discussionId = discussionId });
        }

        [AuthenticationFilter]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            TempData["InitiallySelectedTopicIds"] = null;
            TempData["ToBeDeleted"] = null;
            TempData["ToBeCreated"] = null;
            TempData["DeletedTopics"] = null;

            EditVM model = new EditVM();
            GetDiscussionByIdResponse discussionResponseData;

            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await GetAccessToken(user.Username, user.Password);

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    discussionResponseData = JsonSerializer.Deserialize<GetDiscussionByIdResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    //discussionResponseData.Discussion.Id = id;
                    model.Discussion = discussionResponseData.Discussion;
                }
                else
                {
                    // Handle error responses
                    // You can customize the error handling as needed
                    ModelState.AddModelError(string.Empty, "Failed to find discussion. Please try again later.");
                    return View(model);
                }
            }

            // Getting the topics
            //var token = GetAccessToken();
            GetTopicsResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new("https://localhost:7129/api/Topics/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await client.GetAsync("");
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetTopicsResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                model.Topics = responseData.Topics.ToList();
            }

            // Getting the selected topics
            GetDiscussions_TopicsByDiscussionIdResponse dtResponseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new("https://localhost:7129/api/Discussions_Topics/");
                //client.BaseAddress = new("https://localhost:7129/api/Topics/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await client.GetAsync($"{id}");
                var jsonContent = await response.Content.ReadAsStringAsync();
                dtResponseData = JsonSerializer.Deserialize<GetDiscussions_TopicsByDiscussionIdResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                model.InitiallySelectedTopicIds = dtResponseData.Discussions_Topics.Select(x => (int)x.TopicId).ToList();
                TempData["InitiallySelectedTopicIds"] = model.InitiallySelectedTopicIds;
            }

            return View(model);
        }

        [AuthenticationFilter]
        [HttpPost]
        public async Task<IActionResult> Edit(EditVM model)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await GetAccessToken(user.Username, user.Password);

            if (!ModelState.IsValid && (model.Title == null || model.Text == null))
            {
                var topics = JsonSerializer.Deserialize<List<TopicViewModel>>(model.TopicsSerialized);
                model.Topics = topics ?? new List<TopicViewModel> { };
                model.InitiallySelectedTopicIds = model.SelectedTopicIds ?? new List<int>();
                model.Discussion = new()
                {
                    Title = model.Title ?? "",
                    Text = model.Text ?? "",
                    IsLocked = false,
                    IsUpdated = model.IsUpdated ?? false,
                    IsVisible = true
                };
                //return RedirectToAction("Edit", "Discussions", new { id = IdTracker.ActiveDiscussionId });
                return View(model);
            }

            long fileSize = 0;
            if (model.Image != null) fileSize = model.Image.Length;
            //string fileType = model.Image.ContentType;
            byte[] bytes = null;

            if (fileSize > 0)
            {
                using (var stream = new MemoryStream())
                {
                    model.Image.CopyTo(stream);
                    bytes = stream.ToArray();
                }
            }

            // Construct the discussion model to send to the API
            var discussionModel = new DiscussionModel
            {
                Id = IdTracker.ActiveDiscussionId,
                Title = model.Title,
                Text = model.Text,
                IsUpdated = true,
                IsLocked = false,
                IsVisible = true,
                Image = bytes,
                UpdatedBy = user.Id
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(discussionModel);

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
                var response = await client.PutAsync("Discussions", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    if (TempData["InitiallySelectedTopicIds"] != null && model.SelectedTopicIds != null)
                    {
                        model.InitiallySelectedTopicIds = (TempData["InitiallySelectedTopicIds"] as int[]).ToList();
                        TempData["ToBeDeleted"] = model.InitiallySelectedTopicIds.Where(x => model.SelectedTopicIds.Contains(x) == false).ToList();
                        TempData["ToBeCreated"] = model.SelectedTopicIds.Where(x => model.InitiallySelectedTopicIds.Contains(x) == false).ToList();
                        return RedirectToAction("Edit", "Discussions_Topics", new { discussionId = IdTracker.ActiveDiscussionId });
                    }
                    else if (model.SelectedTopicIds != null)
                    {
                        TempData["Topics"] = model.SelectedTopicIds;
                        return RedirectToAction("GetIdByTitle", "Discussions", new { discussionTitle = model.Title });
                    }
                    else if (TempData["InitiallySelectedTopicIds"] != null)
                    {
                        TempData["ToBeDeleted"] = TempData["InitiallySelectedTopicIds"];
                        return RedirectToAction("Edit", "Discussions_Topics", new { discussionId = IdTracker.ActiveDiscussionId });
                    }
                    else
                    {
                        return RedirectToAction("Details", "Discussions", new { id = IdTracker.ActiveDiscussionId });
                        //return RedirectToAction("Details", "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    }

                    // If successful, redirect to the discussion list
                    //return RedirectToAction("Details", "Discussions", new { id = IdTracker.ActiveDiscussionId });

                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "The title must be unique. Try a different title!");
                    model.UniqueTitleError = true;
                    var topics = JsonSerializer.Deserialize<List<TopicViewModel>>(model.TopicsSerialized);
                    model.Topics = topics ?? new List<TopicViewModel> { };
                    model.InitiallySelectedTopicIds = model.SelectedTopicIds ?? new List<int>();
                    model.Discussion = new()
                    {
                        Title = model.Title ?? "",
                        Text = model.Text ?? "",
                        IsLocked = false,
                        IsUpdated = false,
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
            if (user != null) token = await GetAccessToken(user.Username, user.Password);

            DeleteDiscussionResponse discussionResponseData;

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
                    discussionResponseData = JsonSerializer.Deserialize<DeleteDiscussionResponse>(jsonContent, new JsonSerializerOptions()
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
                    ModelState.AddModelError(string.Empty, "Failed to delete discussion. Please try again later.");
                    return RedirectToAction("Details", "Discussions", new { id = IdTracker.ActiveDiscussionId });
                }
            }

            return RedirectToAction("Index", "Discussions");
        }

        private static async Task<string> GetAccessToken(string username, string password)
        {


            //using (HttpClient client = new())
            //{
            //    //client.BaseAddress = new($"https://localhost:7129/api/authorization?username={username}&password={password}");
            //    client.BaseAddress = new($"https://localhost:7129/api/Authorization/token/{username}/{password}");

            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Add("Accept", "application/json");

            //    HttpResponseMessage response = await client.GetAsync("");
            //    var jsonContent = await response.Content.ReadAsStringAsync();
            //    var responseData = JsonSerializer.Deserialize<AuthenticationResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            //    return responseData.Token;
            //}
            using (HttpClient client = new HttpClient())
            {
                //client.BaseAddress = new($"https://localhost:7129/api/authorization?username={username}&password={password}");
                client.BaseAddress = new Uri($"https://localhost:7129/api/Authorization/token/{username}/{password}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                HttpResponseMessage response = await client.GetAsync("");
                var jsonContent = await response.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<AuthenticationResponse>(jsonContent, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

                return responseData?.Token;
            }
        }
    }
}
