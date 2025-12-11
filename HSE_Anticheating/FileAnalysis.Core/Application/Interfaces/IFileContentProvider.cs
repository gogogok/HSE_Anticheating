namespace FileAnalysis.Core.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для получения текста файлов
    /// </summary>
    public interface IFileContentProvider
    {
        /// <summary>
        /// Метод для поучения текста файла
        /// </summary>
        /// <param name="fileId">ID работы в хранилище</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Текст работы</returns>
        Task<string> GetFileTextAsync(string fileId, CancellationToken ct = default);
    }
}