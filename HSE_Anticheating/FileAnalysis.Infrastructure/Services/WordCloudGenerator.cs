using System.Globalization;
using System.Net;
using System.Text;
using FileAnalysis.Core.Domain.Services;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace FileAnalysis.Infrastructure.Services
{
    /// <summary>
    /// Сервис для создания облака слов
    /// </summary>
    public class WordCloudGenerator : IWordCloudGenerator
    {
        //http - клиент
        private readonly HttpClient _http;

        /// <summary>
        /// Конструкор WordCloudGenerator
        /// </summary>
        /// <param name="httpClient"></param>
        public WordCloudGenerator(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task<string> GenerateWordCloudUrlAsync(
            string text )
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException("Пустой текст для облака слов.");
            }

            var request = new
            {
                format = "png",
                width = 800,
                height = 600,
                fontScale = 15,
                scale = "linear",
                text
            };

            //отправляем запрос
            HttpResponseMessage response =
                await _http.PostAsJsonAsync("https://quickchart.io/wordcloud", request);
            
            byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
            
            string? contentType = response.Content.Headers.ContentType?.MediaType;

            if (!response.IsSuccessStatusCode &&
                (contentType == null || !contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)))
            {
                string errorText;
                try
                {
                    errorText = Encoding.UTF8.GetString(imageBytes);
                }
                catch
                {
                    errorText = "<binary>";
                }

                throw new InvalidOperationException(
                    $"QuickChart wordcloud error ({(int)response.StatusCode}): {errorText}");
            }

            
            string id = Guid.NewGuid().ToString("N") + ".png";
            string folder = Path.Combine(AppContext.BaseDirectory, "WordClouds");

            Directory.CreateDirectory(folder);
            string fullPath = Path.Combine(folder, id);

            await File.WriteAllBytesAsync(fullPath, imageBytes);

            //отдаём URL
            return "http://localhost:5002/wordclouds/" + id;
        }
    }
}
