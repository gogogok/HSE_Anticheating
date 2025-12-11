using System.Net.Http.Headers;
using System.Text.Json;

namespace Gateway.Api.Services
{
    /// <summary>
    /// Класс, с помощью которого на сервер будут отправляться запросы с текстами работ
    /// </summary>
    public class FileStoringApiClient : IFileStoringApiClient
    {
        /// <summary>
        /// HTTP-клиент, на который будут делаться запросы
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Конструктор FileStoringApiClient
        /// </summary>
        /// <param name="httpClient">HTTP-клиент, на который будут делаться запросы</param>
        public FileStoringApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Метод, делающий пост запрос для загрузки работы
        /// </summary>
        /// <param name="file">IFormFile модель файла, которую понимает Swagger и сам ASP.NET</param>
        /// <param name="cancellationToken">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Возвращает уникальный id файла</returns>
        public async Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
            {
                throw new InvalidOperationException("Файл пуст");
            }
            
            //объект, который отправит файл как при загрузке в браузере
            MultipartFormDataContent content = new MultipartFormDataContent();

            await using Stream fileStream = file.OpenReadStream();
            StreamContent fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType ?? "application/octet-stream");
            content.Add(fileContent, "file", file.FileName);

            //запрос для загрузки работы и получение ID
            HttpResponseMessage response = await _httpClient.PostAsync("files", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                string errorText = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidOperationException("FileStoring.Api: " + errorText);
            }

            string responseText = await response.Content.ReadAsStringAsync(cancellationToken);

            using JsonDocument document = JsonDocument.Parse(responseText);
            if (!document.RootElement.TryGetProperty("fileId", out JsonElement fileIdElement))
            {
                throw new InvalidOperationException("FileStoring.Api не содержит fileId.");
            }

            string fileId = fileIdElement.GetString();
            return fileId;
        }
    }
}
