using System.Text.Json;

namespace carGooBackend.Services
{
    public class ImageUploadService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ImageUploadService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(bool Success, string? Url, string? ErrorMessage)> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, null, "No file uploaded.");

            var apiKey = "970620cae606996fc4cabf114bbac83c";
            var apiUrl = "https://api.imghippo.com/v1/upload";

            if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiUrl))
                return (false, null, "API key or URL is missing.");

            using var content = new MultipartFormDataContent();
            await using var stream = file.OpenReadStream();
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(streamContent, "file", file.FileName);
            content.Add(new StringContent(apiKey), "api_key");

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
                return (false, null, $"Failed to upload image. Status code: {(int)response.StatusCode}");

            var responseData = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseData);

            if (jsonDoc.RootElement.TryGetProperty("data", out var dataElement) &&
                dataElement.TryGetProperty("url", out var urlElement))
            {
                return (true, urlElement.GetString(), null);
            }

            return (false, null, "Response did not contain a valid URL.");
        }
    }
}