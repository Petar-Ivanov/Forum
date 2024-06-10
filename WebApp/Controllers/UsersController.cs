using Messaging.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System;
using WebApp.ViewModels.Users;
using System.Reflection;
using WebApp.ExtentionMethods;
using Messaging.Responses.Get;
using Messaging.Responses.GetBy;
using Messaging.Responses.ViewModels;
using WebApp.ActionFilters;
using Messaging.Responses.Delete;
using Messaging;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.ViewModels.Users;

namespace WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly Uri uri = new("https://localhost:7129/api/Users/");

        public async Task<IActionResult> Index(IndexVM model)
        {
            GetUsersResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                HttpResponseMessage response = await client.GetAsync("");
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetUsersResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                //model.Items = responseData.Topics.ToList();

                model.Items = PageManager.GetUsers(responseData.Users.ToList(), model.Page ?? 1);
                model.PageCount = PageManager.UsersCountPages(responseData.Users.ToList().Count());
            }

            return View(model);
        }

        public async Task<IActionResult> Search(IndexVM model, string searchTerm)
        {
            //var token = GetAccessToken();
            if (searchTerm == null)
            {
                return RedirectToAction("Index", "Users");
            }

            GetUsersByUsernameResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                string encodedSearchTerm = Uri.EscapeDataString(searchTerm);
                HttpResponseMessage response = await client.GetAsync($"username/{encodedSearchTerm}");
                var jsonContent = await response.Content.ReadAsStringAsync();
                responseData = JsonSerializer.Deserialize<GetUsersByUsernameResponse>(jsonContent, new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                //ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                //model.Items = responseData.Topics.ToList();
                model.Items = PageManager.GetUsers(responseData.Users.ToList(), model.Page ?? 1);
                model.PageCount = PageManager.TopicsCountPages(responseData.Users.ToList().Count());
            }

            model.SearchTerm = searchTerm;
            return View("Index", model);
        }

        public IActionResult RegistrationSuccessfull()
        {
            return View();
        }

        public async Task<IActionResult> UpdateLoggedUser(string act, string ctrl, int page)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            GetUserByIdResponse userResponseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.GetAsync($"{user.Id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    userResponseData = JsonSerializer.Deserialize<GetUserByIdResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    HttpContext.Session.SetObject("loggedUser", userResponseData.User);

                    //return RedirectToAction(action, controller);
                    return RedirectToAction(act, ctrl, new { id = IdTracker.ActiveDiscussionId, page = page/*, SearchTerm = SearchTerm, Topic = Topic*/});
                    
                }
                else
                {
                    // Handle error responses
                    // You can customize the error handling as needed
                    ModelState.AddModelError(string.Empty, "Failed to find user. Please try again later.");
                    return RedirectToAction(act, ctrl, new { id = IdTracker.ActiveDiscussionId, page = page/*, SearchTerm = SearchTerm, Topic = Topic*/});
                }
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginVM model = new LoginVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!this.ModelState.IsValid)
                return View(model);

            GetUserByUsernamePasswordResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                string encodedUsername = Uri.EscapeDataString(model.Username);
                string encodedPassword = Uri.EscapeDataString(model.Password);
                HttpResponseMessage response = await client.GetAsync($"{encodedUsername}/{encodedPassword}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    responseData = JsonSerializer.Deserialize<GetUserByUsernamePasswordResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    HttpContext.Session.SetObject("loggedUser", responseData.User);
                    //var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
                    return RedirectToAction("Index", "Discussions");
                }
                else
                {
                    this.ModelState.AddModelError("authError", "Invalid username or password!");
                    return View(model);
                }
            }

        }

        [AuthenticationFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("loggedUser");

            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterVM model = new RegisterVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid && (model.Username == null || model.Password == null || model.Email == null || model.Biography == null))
            {
                // If the model state is not valid, return the view with validation errors
                return View(model);
            }

            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            // Construct the discussion model to send to the API
            var userModel = new UserModel
            {
                Username = model.Username,
                Password = model.Password,
                Email = model.Email,
                Biography = model.Biography,
                BirthDay = model.BirthDate,
                Country = model.Country,
                IsVisible = true
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(userModel);

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
                    return RedirectToAction("RegistrationSuccessfull", "Users");
                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "Failed to Sign Up. Please try again later.");
                    model.UniquePropertiesError = true;
                    return View(model);
                }
            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Profile()
        {
            int id = (int)HttpContext.Session.GetObject<UserViewModel>("loggedUser").Id;
            return RedirectToAction("Details", "Users", new {id = id});
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Details(int id)
        {
            GetUserByIdResponse userResponseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authentication", "Bearer" + token);

                //HttpResponseMessage response = await client.GetAsync(uri);
                HttpResponseMessage response = await client.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    userResponseData = JsonSerializer.Deserialize<GetUserByIdResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    DetailsVM model = new()
                    {
                        Id = userResponseData.User.Id,
                        Username = userResponseData.User.Username,
                        Email = userResponseData.User.Email,
                        Biography = userResponseData.User.Biography,
                        BirthDay = userResponseData.User.BirthDay,
                        Country = userResponseData.User.Country,
                        Password = userResponseData.User.Password
                    };

                    return View(model);
                }
                else
                {
                    // Handle error responses
                    // You can customize the error handling as needed
                    ModelState.AddModelError(string.Empty, "Failed to find user. Please try again later.");
                    return RedirectToAction("Index", "Discussions");
                }
            }
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            DeleteUserResponse userResponseData;

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
                    userResponseData = JsonSerializer.Deserialize<DeleteUserResponse>(jsonContent, new JsonSerializerOptions()
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
                    ModelState.AddModelError(string.Empty, "Failed to delete user. Please try again later.");
                    //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
                    return RedirectToAction("Index", "Discussions");
                }
            }

            //return RedirectToAction(ActivePage, "Discussions", new { id = IdTracker.ActiveDiscussionId });
            return RedirectToAction("Logout", "Users");
        }

        [AuthenticationFilter]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            EditVM model = new EditVM();
            GetUserByIdResponse userResponseData;

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
                    userResponseData = JsonSerializer.Deserialize<GetUserByIdResponse>(jsonContent, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    //discussionResponseData.Discussion.Id = id;
                    model.User = userResponseData.User;
                    model.UserId = id;
                }
                else
                {
                    // Handle error responses
                    // You can customize the error handling as needed
                    ModelState.AddModelError(string.Empty, "Failed to find user. Please try again later.");
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

            if (!ModelState.IsValid && (model.Username == null || model.Password == null || model.Email == null || model.Biography == null))
            {
                // If the model state is not valid, return the view with validation errors
                model.User = new() 
                { 
                    Username = model.Username,
                    Biography = model.Biography,
                    Country = model.Country,
                    BirthDay = model.BirthDate,
                    Email = model.Email,
                    IsVisible = true,
                    Password = model.Password
                };
                return View(model);
            }

            // Construct the discussion model to send to the API
            var userModel = new UserModel
            {
                Id = model.UserId,
                Username = model.Username,
                Biography = model.Biography,
                BirthDay = model.BirthDate,
                Country = model.Country,
                Email = model.Email,
                Password = model.Password,
                UpdatedBy = user.Id,
                IsVisible = true
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(userModel);

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
                var response = await client.PutAsync("", content);

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // If successful, redirect to the discussion list
                    user.Username = model.Username;
                    user.Password = model.Password;
                    HttpContext.Session.SetObject("loggedUser", user);

                    return RedirectToAction("Details", "Users", new {id = model.UserId});
                }
                else
                {
                    // If not successful, handle the error response
                    ModelState.AddModelError(string.Empty, "Failed to edit user. Please try again later.");
                    model.User = new()
                    {
                        Username = model.Username,
                        Biography = model.Biography,
                        Country = model.Country,
                        BirthDay = model.BirthDate,
                        Email = model.Email,
                        IsVisible = true,
                        Password = model.Password
                    };
                    model.UniquePropertiesError = true;
                    return View(model);
                }
            }
        }
    }
}
