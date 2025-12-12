using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysis.Infrastructure.Persistence
{
    /// <summary>
    /// Класс для работы с БД отчёттов
    /// </summary>
    public class EfReportRepository : IReportRepository
    {
        /// <summary>
        /// БД
        /// </summary>
        private readonly FileAnalysisDbContext _db;

        /// <summary>
        /// Конструктор EfReportRepository
        /// </summary>
        /// <param name="db">БД</param>
        public EfReportRepository(FileAnalysisDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Добавление отчёта в БД
        /// </summary>
        /// <param name="report">Отчёт, который нужно добавить</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        public async Task AddAsync(Report report, CancellationToken ct = default)
        {
            _db.Reports.Add(report);
            await _db.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Получение списка отчётов по ID работы
        /// </summary>
        /// <param name="workId">ID работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список отчётов по работе</returns>
        public Task<List<Report>> GetByWorkIdAsync(Guid workId, CancellationToken ct = default)
        {
            return _db.Reports
                .Where(r => r.WorkId == workId)
                .ToListAsync(ct);
        }
    }
}