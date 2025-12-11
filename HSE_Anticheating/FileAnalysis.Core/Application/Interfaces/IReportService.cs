using FileAnalysis.Core.Domain.Entities;

namespace FileAnalysis.Core.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для получения отчётов
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Метод получения отчётов
        /// </summary>
        /// <param name="workId">ID работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список отчётов оп работе</returns>
        Task<List<Report>> GetReportsForWorkAsync(Guid workId, CancellationToken ct = default);
    }
}