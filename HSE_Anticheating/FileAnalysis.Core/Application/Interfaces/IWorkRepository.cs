using FileAnalysis.Core.Domain.Entities;
using FileAnalysis.Core.Application.Models;

namespace FileAnalysis.Core.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с хранилищем работ
    /// </summary>
    public interface IWorkRepository
    {
        /// <summary>
        /// Метод добавления работы в хранилище
        /// </summary>
        /// <param name="work">Работа, которую нужно добавить</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        Task AddAsync(Work work, CancellationToken ct = default);
        
        /// <summary>
        /// Получение работ студента
        /// </summary>
        /// <param name="studentId">ID студента</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Работы нудного студента</returns>
        Task<List<Work>> GetByStudentAsync(string studentId, CancellationToken ct = default);
        
        /// <summary>
        /// Получение полного списка работ по фильтру
        /// </summary>
        /// <param name="filter">Фильтр работ</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ в хранилище</returns>
        Task<List<Work>> GetAllAsync(WorkFilter filter, CancellationToken ct = default);

        /// <summary>
        /// Получаем список всх работ других студентов на эту тему
        /// </summary>
        /// <param name="assignmentId">ID темы</param>
        /// <param name="studentId">D студента, чью работу мы будем сверять</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ по теме</returns>
        Task<List<Work>> GetPreviousForAssignmentAsync(
            string assignmentId,
            string studentId,
            CancellationToken ct = default);
    }
}