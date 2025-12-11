using FileAnalysis.Core.Application.Interfaces;

namespace FileAnalysis.Infrastructure.FileAccess
{
    /// <summary>
    /// Класс, получающий доступ к файлам
    /// </summary>
    public class FileContentProvider : IFileContentProvider
    {
        /// <summary>
        /// Клиент, позволяющий получить файлы
        /// </summary>
        private readonly HttpClient _http;

        /// <summary>
        /// Конструктор FileContentProvider
        /// </summary>
        /// <param name="http">Клиент, позволяющий получить файлы</param>
        public FileContentProvider(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// Метод для поучения текста файла
        /// </summary>
        /// <param name="fileId">ID работы в хранилище</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Текст работы</returns>
        public async Task<string> GetFileTextAsync(string fileId, CancellationToken ct = default)
        {
            HttpResponseMessage response = await _http.GetAsync($"/files/{fileId}", ct);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Не удалось загрузить текстовые данные");
            }

            return await response.Content.ReadAsStringAsync(ct);
        }
    }
}