using FileAnalysis.Core.Domain.Entities;

namespace FileAnalysis.Core.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с хранилищем отчётов
    /// </summary>
    public interface IReportRepository
    {
        /// <summary>
        /// Асинхронно добавляет отчёт в хранилище
        /// </summary>
        /// <param name="report">Отчёт, который нужно добавить</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        Task AddAsync(Report report, CancellationToken ct = default);
        
        /// <summary>
        /// Получение списка отчётов по работе
        /// </summary>
        /// <param name="workId">ID работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список отчётов</returns>
        Task<List<Report>> GetByWorkIdAsync(Guid workId, CancellationToken ct = default);
    }
}