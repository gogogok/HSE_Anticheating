using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Domain.Entities;

namespace FileAnalysis.Core.Application.Services
{
    /// <summary>
    /// Сервис для доступа к отчётам
    /// </summary>
    public class ReportService : IReportService
    {
        /// <summary>
        /// Хранилище отчётов
        /// </summary>
        private readonly IReportRepository _repo;

        /// <summary>
        /// Конструктор ReportService
        /// </summary>
        /// <param name="repo"> Хранилище отчётов</param>
        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Метод получения отчётов
        /// </summary>
        /// <param name="workId">ID работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список отчётов оп работе</returns>
        public Task<List<Report>> GetReportsForWorkAsync(Guid workId, CancellationToken ct = default)
        {
            return _repo.GetByWorkIdAsync(workId, ct);
        }
    }
}