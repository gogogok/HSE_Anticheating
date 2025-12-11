using FileAnalysis.Core.Domain.Entities;
using FileAnalysis.Core.Domain.Models;

namespace FileAnalysis.Core.Domain.Services
{
    /// <summary>
    /// Интерфейс антиплагиата
    /// </summary>
    public interface IPlagiarismDetector
    {
        /// <summary>
        /// Метод для анализа на плагиат
        /// </summary>
        /// <param name="newWork">Работа, которая будет сравниваться</param>
        /// <param name="previousWorks">Список прошлых работ</param>
        /// <returns>Результат работы антиплагиата</returns>
        Task<PlagiarismResult> AnalyzeAsync(Work newWork, IReadOnlyList<Work> previousWorks);
    }
}