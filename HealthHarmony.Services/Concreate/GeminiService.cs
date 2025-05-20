using HealthHarmony.Services.Contracts;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthHarmony.Services.Concreate
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "your_api_keys";
        private readonly string _model = "gemini-2.0-flash";

        public GeminiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AskGeminiAsync(string prompt)
        {
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return $"Hata: {response.StatusCode}. Detay: {errorMessage}";
            }

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw Gemini API Response: {json}"); // Keep this for now

            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(json);

            if (geminiResponse == null)
            {
                return "Hata: Gemini yanıtı başarıyla deserial edilemedi.";
            }

            if (geminiResponse.Candidates == null || geminiResponse.Candidates.Count == 0)
            {
                return "Hata: Gemini yanıtında aday bulunamadı.";
            }

            if (geminiResponse.Candidates[0].Content == null || geminiResponse.Candidates[0].Content.Parts == null || geminiResponse.Candidates[0].Content.Parts.Count == 0)
            {
                return "Hata: Gemini yanıtının içeriği veya parçaları eksik.";
            }

            return geminiResponse.Candidates[0].Content.Parts[0].Text;
        }
    }
}
