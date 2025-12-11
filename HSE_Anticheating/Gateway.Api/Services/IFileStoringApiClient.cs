namespace Gateway.Api.Services
{
    /// <summary>
    /// Интерфейс, с помощью которого на сервер будут отправляться запросы с текстами работ
    /// </summary>
    public interface IFileStoringApiClient
    {
        /// <summary>
        /// Метод, делающий пост запрос для загрузки работы
        /// </summary>
        /// <param name="file">IFormFile модель файла, которую понимает Swagger и сам ASP.NET</param>
        /// <param name="cancellationToken">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Возвращает уникальный id файла</returns>
        Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
    }
}