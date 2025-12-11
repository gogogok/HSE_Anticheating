using Gateway.Api.Models;

namespace Gateway.Api.Services
{
    /// <summary>
    /// Интерфейс для анализа текстовых файлов
    /// </summary>
    public interface IFileAnalysisApiClient
    {
        /// <summary>
        /// Возвращает Dto с информацией о работе
        /// </summary>
        /// <param name="studentId">ID студента, загрузившего работу</param>
        /// <param name="assignmentId">ID задания</param>
        /// <param name="fileId">ID по которому файл можно будет достать из хранилища</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Dto работы</returns>
        Task<WorkResponseDto> CreateAndAnalyzeAsync(
            string studentId,
            string assignmentId,
            string fileId,
            CancellationToken ct);

        /// <summary>
        /// Метод для получения всех работ студента
        /// </summary>
        /// <param name="studentId">ID студента, анализ на чьи работы мы хотим получить</param>
        /// <param name="assignmentId">ID темы, на которую мы хотим получить анализ</param>
        /// <param name="onlyPlagiarism">Параметр наличия/отсутствия плагиата</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ</returns>
        Task<List<WorkResponseDto>> GetWorksAsync(
            string? studentId,
            string? assignmentId,
            bool? onlyPlagiarism,
            CancellationToken ct);

        /// <summary>
        /// Метод для получения анализов одной работы
        /// </summary>
        /// <param name="workId">ID работы студента, по которой нужно получить анализы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список анализов</returns>
        Task<List<ReportResponseDto>> GetReportsAsync(
            Guid workId,
            CancellationToken ct);
    }
}