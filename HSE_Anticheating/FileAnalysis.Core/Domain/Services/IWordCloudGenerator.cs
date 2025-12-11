namespace FileAnalysis.Core.Domain.Services
{
    /// <summary>
    /// Интерфейс генератора облаков
    /// </summary>
    public interface IWordCloudGenerator
    {
        /// <summary>
        /// Возвращает url по которой доступно облако
        /// </summary>
        /// <param name="text">Текст для облака</param>
        /// <returns>Ссылка на облако</returns>
        Task<string> GenerateWordCloudUrlAsync(string text);
        
    }
}