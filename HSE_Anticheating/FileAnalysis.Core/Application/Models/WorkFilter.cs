namespace FileAnalysis.Core.Application.Models
{
    /// <summary>
    /// Фильтр для поиска работ
    /// </summary>
    public class WorkFilter
    {
        /// <summary>
        /// ID студента
        /// </summary>
        public string? StudentId { get; init; }
        
        /// <summary>
        /// ID темы
        /// </summary>
        public string? AssignmentId { get; init; }
        
        /// <summary>
        /// наличие плагиата
        /// </summary>
        public bool? OnlyPlagiarism { get; init; }
    }
}