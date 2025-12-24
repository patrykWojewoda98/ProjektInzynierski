using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using ProjektInzynierski.Application.Interfaces;

namespace ProjektInzynierski.Infrastructure.Services
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly HttpClient _httpClient;

        public ChatGPTService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetResponseAsync(string message)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "user", content = message }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"OpenAI API error: {responseString}");

            using var doc = JsonDocument.Parse(responseString);
            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString() ?? "";
        }
    }
}