using Messaging.Requests.Models;
using Messaging.Responses.Get;
using Messaging.Responses.GetBy;
using Messaging.Responses.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using UAParser;
using WebApp.ActionFilters;
using WebApp.ExtentionMethods;

namespace WebApp.Controllers
{
    public class ViewsController : Controller
    {
        private readonly Uri uri = new("https://localhost:7129/api/Views/");

        public IActionResult Index()
        {
            return View();
        }

        [AuthenticationFilter]
        public async Task<IActionResult> View(int id)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            //[HttpGet("{discussion_id}/{user_id}")]
            //var token = GetAccessToken();
            GetViewByIdsResponse responseData;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                if (user != null) client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);


                HttpResponseMessage response = await client.GetAsync($"{id}/{user.Id}"); 
                if (response.IsSuccessStatusCode == false) // Change Existing
                {
                    //var jsonContent = await response.Content.ReadAsStringAsync();
                    //responseData = JsonSerializer.Deserialize<GetViewByIdsResponse>(jsonContent, new JsonSerializerOptions()
                    //{
                    //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    //});
                    return RedirectToAction("Create", "Views", new { discussionId = id });
                }
                return RedirectToAction("Details", "Discussions", new { id = id });
                //var jsonContent = await response.Content.ReadAsStringAsync();
                //responseData = JsonSerializer.Deserialize<GetViewByIdsResponse>(jsonContent, new JsonSerializerOptions()
                //{
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                //});

                ////ViewBag.data = responseData.Discussions.FirstOrDefault().Title.ToString();
                //model.Items = responseData.Discussions.ToList();
            }

            //return View(model);
        }

        [AuthenticationFilter]
        public async Task<IActionResult> Create(int discussionId)
        {
            var user = HttpContext.Session.GetObject<UserViewModel>("loggedUser");
            string token = null;
            if (user != null) token = await TokenManager.GetAccessToken(user.Username, user.Password);

            string userAgent = Request.Headers["User-Agent"].ToString();
            var parser = Parser.GetDefault();

            ClientInfo clientInfo = parser.Parse(userAgent);
            string source = clientInfo.UA.Family + " - " + clientInfo.OS.Family;

            // Construct the discussion model to send to the API
            var viewModel = new ViewModel
            {
                DiscussionId = discussionId,
                UserId = user.Id ?? 1,
                Revisited = false,
                Source = source
            };

            // Serialize the discussion model to JSON
            var jsonPayload = JsonSerializer.Serialize(viewModel);

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
                    ModelState.AddModelError(string.Empty, "Failed to create topic. Please try again later.");
                    
                }
                return RedirectToAction("Details", "Discussions", new { id = discussionId });
            }
        }
    }
}
