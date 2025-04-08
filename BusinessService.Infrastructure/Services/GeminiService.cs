using BusinessService.Aplication.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessService.Infrastructure.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        public GeminiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("GeminiClient");
        }
        public async Task<string> AskGeminiAsync(string userMessage)
        {
            try
            {
                var systemPrompt = "You are a helpful medical assistant. You only help users with general medical doubts, such as information about symptoms, health tips, medications, or medical terminology. Do not provide a diagnosis, treatment plans, or emergency medical advice. Always recommend consulting a licensed doctor for serious or specific issues.";

    var requestBody = new
{
    contents = new[]
    {
        new
        {
            parts = new[]
            {
                new { text = $"{systemPrompt} {userMessage}" }
            }
        }
    }
};


                var _apiKey = Environment.GetEnvironmentVariable("Gemini_APiKey");
                var response = await _httpClient.PostAsJsonAsync(
                       $"v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}",
                       requestBody);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<JsonElement>();

                var reply = result.GetProperty("candidates")[0]
                                  .GetProperty("content")
                                  .GetProperty("parts")[0]
                                  .GetProperty("text")
                                  .GetString();

                return reply ?? "I'm here to help with general medical doubts only.";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    } 
}
