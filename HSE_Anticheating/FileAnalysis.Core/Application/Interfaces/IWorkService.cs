using FileAnalysis.Core.Domain.Entities;
using FileAnalysis.Core.Application.Models;

namespace FileAnalysis.Core.Application.Interfaces
{
    /// <summary>
    /// Интерфейс для работы с работы
    /// </summary>
    public interface IWorkService
    {
        /// <summary>
        /// Сохранение и анализ работ
        /// </summary>
        /// <param name="command">Команда для создания сущности работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Сущность работы</returns>
        Task<Work> CreateAndAnalyzeAsync(CreateWorkCommand command, CancellationToken ct = default);

        /// <summary>
        /// Получение всех работ по фильтру
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ</returns>
        Task<List<Work>> GetAllWorksAsync(WorkFilter filter, CancellationToken ct = default);
    }
}