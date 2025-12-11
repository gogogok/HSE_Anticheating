namespace FileStoring.Api.Services
{
    /// <summary>
    /// Интерфейс для сервисов по работе с хранением файлов
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Метод для сохранения в файле и возвращения его ID
        /// </summary>
        /// <param name="file">Файл, который нужно сохранить</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>fileId работы</returns>
        Task<string> SaveFileAsync(IFormFile file, CancellationToken ct = default);

        /// <summary>
        /// Метод для получения текста файла по ID
        /// </summary>
        /// <param name="fileId">fileId работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Текст файла по ID</returns>
        Task<Stream?> GetFileStreamAsync(string fileId, CancellationToken ct = default);
    }
}
