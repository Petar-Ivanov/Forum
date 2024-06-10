using Messaging.Responses;
using System.Text.Json;

namespace WebApp
{
    public static class TokenManager
    {
        public static async Task<string> GetAccessToken(string username, string password)
        {
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
