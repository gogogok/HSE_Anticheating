using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Domain.Entities;

namespace FileAnalysis.Infrastructure.Persistence
{
    /// <summary>
    /// Хранилище отчётов
    /// </summary>
    public class ReportRepository : IReportRepository
    {
        /// <summary>
        /// База данных
        /// </summary>
        private readonly JsonDatabase _db;
        
        /// <summary>
        /// Список отчётов
        /// </summary>
        private List<Report> _cache;

        /// <summary>
        /// Конструктор ReportRepository
        /// </summary>
        /// <param name="db">База данных</param>
        public ReportRepository(JsonDatabase db)
        {
            _db = db;
            _cache = _db.LoadReports();
        }

        /// <summary>
        /// Асинхронно добавляет отчёт в хранилище
        /// </summary>
        /// <param name="report">Отчёт, который нужно добавить</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        public Task AddAsync(Report report, CancellationToken ct = default)
        {
            _cache.Add(report);
            _db.SaveReports(_cache);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Получение списка отчётов по работе
        /// </summary>
        /// <param name="workId">ID работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список отчётов</returns>
        public Task<List<Report>> GetByWorkIdAsync(Guid workId, CancellationToken ct = default)
        {
            List<Report> result = _cache
                .Where(r => r.WorkId == workId)
                .ToList();

            return Task.FromResult(result);
        }
    }
}