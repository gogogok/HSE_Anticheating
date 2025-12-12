using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Application.Models;
using FileAnalysis.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysis.Infrastructure.Persistence
{
    /// <summary>
    ///  Класс для работы с БД работ
    /// </summary>
    public class EfWorkRepository : IWorkRepository
    {
        /// <summary>
        ///БД
        /// </summary>
        private readonly FileAnalysisDbContext _db;

        /// <summary>
        /// Конструктор EfWorkRepository
        /// </summary>
        /// <param name="db">БД</param>
        public EfWorkRepository(FileAnalysisDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Добавление работы в БД
        /// </summary>
        /// <param name="work">Работа, которую нужно добавить</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        public async Task AddAsync(Work work, CancellationToken ct = default)
        {
            _db.Works.Add(work);
            await _db.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Получение списка работ студента по его ID
        /// </summary>
        /// <param name="studentId">ID студента</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ студента</returns>
        public Task<List<Work>> GetByStudentAsync(string studentId, CancellationToken ct = default)
        {
            return _db.Works
                .Include(w => w.Reports)
                .Where(w => w.StudentId == studentId)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Получение списка работ по фильтру
        /// </summary>
        /// <param name="filter">Фильтр для отбора работ</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ по фильтру</returns>
        public Task<List<Work>> GetAllAsync(WorkFilter filter, CancellationToken ct = default)
        {
            IQueryable<Work> query = _db.Works.Include(w => w.Reports);

            if (!string.IsNullOrWhiteSpace(filter.StudentId))
            {
                query = query.Where(w => w.StudentId == filter.StudentId);
            }

            if (!string.IsNullOrWhiteSpace(filter.AssignmentId))
            {
                query = query.Where(w => w.AssignmentId == filter.AssignmentId);
            }

            if (filter.OnlyPlagiarism == true)
            {
                query = query.Where(w => w.Reports.Any(r => r.IsPlagiarism));
            }

            return query.ToListAsync(ct);
        }

        /// <summary>
        /// Получение работ по заданной теме других студентов
        /// </summary>
        /// <param name="assignmentId">ID темы</param>
        /// <param name="studentId">ID студента</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ</returns>
        public Task<List<Work>> GetPreviousForAssignmentAsync(
            string assignmentId,
            string studentId,
            CancellationToken ct = default)
        {
            return _db.Works
                .Include(w => w.Reports)
                .Where(w =>
                    w.AssignmentId == assignmentId &&
                    w.StudentId != studentId)
                .ToListAsync(ct);
        }
    }
}
