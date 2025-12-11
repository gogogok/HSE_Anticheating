namespace FileAnalysis.Core.Application.Models
{
    /// <summary>
    /// Класс команды
    /// </summary>
    public class CreateWorkCommand
    {
        /// <summary>
        /// ID студента
        /// </summary>
        public string StudentId { get; init; } = default!;
        
        /// <summary>
        /// ID темы
        /// </summary>
        public string AssignmentId { get; init; } = default!;
        
        /// <summary>
        /// ID работы в хранилище
        /// </summary>
        public string FileId { get; init; } = default!;
    }
}