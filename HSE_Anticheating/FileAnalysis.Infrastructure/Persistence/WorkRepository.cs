using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Application.Models;
using FileAnalysis.Core.Domain.Entities;

namespace FileAnalysis.Infrastructure.Persistence
{
    /// <summary>
    /// Хранилище работ
    /// </summary>
    public class WorkRepository : IWorkRepository
    {
        /// <summary>
        /// База данных
        /// </summary>
        private readonly JsonDatabase _db;
        
        /// <summary>
        /// Список работы в хранилище (кеш)
        /// </summary>
        private List<Work> _cache;

        /// <summary>
        /// Конструктор WorkRepository
        /// </summary>
        /// <param name="db">БД</param>
        public WorkRepository(JsonDatabase db)
        {
            _db = db;
            _cache = _db.LoadWorks();
        }

        /// <summary>
        /// Метод добавления работы в хранилище
        /// </summary>
        /// <param name="work">Работа, которую нужно добавить</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        public Task AddAsync(Work work, CancellationToken ct = default)
        {
            _cache.Add(work);
            _db.SaveWorks(_cache);
            return Task.CompletedTask;
        }
        

        /// <summary>
        /// Получение работ студента
        /// </summary>
        /// <param name="studentId">ID студента</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Работы нудного студента</returns>
        public Task<List<Work>> GetByStudentAsync(string studentId, CancellationToken ct = default)
        {
            List<Work> result = _cache
                .Where(w => w.StudentId == studentId)
                .ToList();

            return Task.FromResult(result);
        }

        /// <summary>
        /// Получение полного списка работ по фильтру
        /// </summary>
        /// <param name="filter">Фильтр работ</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ в хранилище</returns>
        public Task<List<Work>> GetAllAsync(WorkFilter filter, CancellationToken ct = default)
        {
            IEnumerable<Work> query = _cache;

            if (!string.IsNullOrWhiteSpace(filter.StudentId))
            {
                query = query.Where(w => w.StudentId == filter.StudentId);
            }

            if (!string.IsNullOrWhiteSpace(filter.AssignmentId))
            {
                query = query.Where(w => w.AssignmentId == filter.AssignmentId);
            }

            if (filter.OnlyPlagiarism == true || filter.OnlyPlagiarism == false)
            {
                query = query.Where(w => w.Reports.Any(r => r.IsPlagiarism));
            }

            return Task.FromResult(query.ToList());
        }

        /// <summary>
        /// Получаем список всх работ других студентов на эту тему
        /// </summary>
        /// <param name="assignmentId">ID темы</param>
        /// <param name="studentId">D студента, чью работу мы будем сверять</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ по теме</returns>
        public Task<List<Work>> GetPreviousForAssignmentAsync(
            string assignmentId,
            string studentId,
            CancellationToken ct = default)
        {
            List<Work> result = _cache
                .Where(w =>
                    w.AssignmentId == assignmentId &&
                    w.StudentId != studentId)
                .ToList();

            return Task.FromResult(result);
        }
    }
}