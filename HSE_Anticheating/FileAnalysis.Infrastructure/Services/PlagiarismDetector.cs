using FileAnalysis.Core.Domain.Entities;
using FileAnalysis.Core.Domain.Models;
using FileAnalysis.Core.Domain.Services;

namespace FileAnalysis.Infrastructure.Services
{
    /// <summary>
    /// Класс детектора плагиата
    /// </summary>
    public class PlagiarismDetector : IPlagiarismDetector
    {
        /// <summary>
        /// Метод для анализа на плагиат
        /// </summary>
        /// <param name="newWork">Работа, которая будет сравниваться</param>
        /// <param name="previousWorks">Список прошлых работ</param>
        /// <returns>Результат работы антиплагиата</returns>
        public Task<PlagiarismResult> AnalyzeAsync(
            Work newWork,
            IReadOnlyList<Work> previousWorks)
        {
            foreach (Work w in previousWorks)
            {
                if (w.TextHash == newWork.TextHash)
                {
                    return Task.FromResult(
                        new PlagiarismResult(true, 1.0, [w.Id])
                    );
                }
            }

            return Task.FromResult(PlagiarismResult.NoPlagiarism());
        }
    }
}